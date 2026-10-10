using LiteDB;
using Phony.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Phony.Data
{
    public static class InvoiceService
    {
        public static decimal Change(decimal paid, decimal total) => Math.Round(Math.Max(0, paid - total), 2);

        public static long Save(ConnectionString connection, long clientId, decimal discount, decimal paid, string notes, User creator, IEnumerable<BillItemMove> items, IEnumerable<BillServiceMove> services)
        {
            var itemMoves = items.ToList();
            var serviceMoves = services.ToList();

            if (itemMoves.Count + serviceMoves.Count == 0 || paid < 0 || discount < 0 || discount > 100)
            {
                throw new InvalidOperationException("Invalid invoice values.");
            }

            using var db = new LiteDatabase(connection);
            db.BeginTrans();

            try
            {
                var client = db.GetCollection<Client>(DBCollections.Clients).FindById(clientId)
                    ?? throw new InvalidOperationException("Client no longer exists.");
                var store = db.GetCollection<Store>(DBCollections.Stores).FindById(1)
                    ?? throw new InvalidOperationException("Store no longer exists.");
                var treasury = db.GetCollection<Treasury>(DBCollections.Treasuries).FindById(1)
                    ?? throw new InvalidOperationException("Treasury no longer exists.");

                foreach (var move in itemMoves)
                {
                    if (move.Item == null || move.QTY <= 0 || move.ItemPrice < 0 || move.Discount < 0 || move.Discount > 100)
                    {
                        throw new InvalidOperationException("Invalid item movement.");
                    }
                }

                foreach (var move in serviceMoves)
                {
                    if (move.Service == null || move.Balance < 0 || move.Cost <= 0 || move.Discount < 0 || move.Discount > 100)
                    {
                        throw new InvalidOperationException("Invalid service movement.");
                    }
                }

                // Read current balances inside the transaction, aggregating repeated lines.
                foreach (var group in itemMoves.GroupBy(x => x.Item.Id))
                {
                    var item = db.GetCollection<Item>(DBCollections.Items).FindById(group.Key)
                        ?? throw new InvalidOperationException("Item no longer exists.");

                    var quantity = group.Sum(x => x.QTY);

                    if (item.QTY < quantity)
                    {
                        throw new InvalidOperationException("Insufficient item stock.");
                    }

                    item.QTY -= quantity;
                    db.GetCollection<Item>(DBCollections.Items).Update(item);
                }

                foreach (var group in serviceMoves.GroupBy(x => x.Service.Id))
                {
                    var service = db.GetCollection<Service>(DBCollections.Services).FindById(group.Key)
                        ?? throw new InvalidOperationException("Service no longer exists.");
                    var balance = group.Sum(x => x.Balance);

                    if (service.Balance < balance)
                    {
                        throw new InvalidOperationException("Insufficient service balance.");
                    }

                    service.Balance -= balance;
                    db.GetCollection<Service>(DBCollections.Services).Update(service);
                }

                var subtotal = itemMoves.Sum(x => Math.Round(x.ItemPrice * x.QTY * (1 - x.Discount / 100), 2)) + serviceMoves.Sum(x => Math.Round(x.Cost * (1 - x.Discount / 100), 2));
                var total = Math.Round(subtotal * (1 - discount / 100), 2);

                if (clientId == 1 && paid < total)
                {
                    throw new InvalidOperationException("Cash invoices must be fully paid.");
                }

                var bill = new Bill
                {
                    Client = client,
                    Store = store,
                    Discount = discount,
                    TotalAfterDiscounts = total,
                    TotalPayed = paid,
                    Notes = notes,
                    CreateDate = DateTime.Now,
                    Creator = creator
                };

                db.GetCollection<Bill>(DBCollections.Bills).Insert(bill);

                // Insert fresh movements so rollback does not leave generated IDs in the draft.
                foreach (var move in itemMoves)
                {
                    db.GetCollection<BillItemMove>(DBCollections.BillsItemsMoves).Insert(new BillItemMove
                    {
                        Bill = bill,
                        Item = move.Item,
                        QTY = move.QTY,
                        ItemPrice = move.ItemPrice,
                        Discount = move.Discount,
                        Notes = move.Notes,
                        Creator = creator,
                        CreateDate = bill.CreateDate
                    });
                }

                foreach (var move in serviceMoves)
                {
                    db.GetCollection<BillServiceMove>(DBCollections.BillsServicesMoves).Insert(new BillServiceMove
                    {
                        Bill = bill,
                        Service = move.Service,
                        Balance = move.Balance,
                        Cost = move.Cost,
                        Discount = move.Discount,
                        Notes = move.Notes,
                        Creator = creator,
                        CreateDate = bill.CreateDate
                    });
                }

                client.Balance += Math.Max(0, total - paid);

                db.GetCollection<Client>(DBCollections.Clients).Update(client);
                db.GetCollection<TreasuryMove>(DBCollections.TreasuriesMoves).Insert(new TreasuryMove
                {
                    Treasury = treasury,
                    Debit = paid,
                    Credit = Change(paid, total),
                    Notes = $"فاتورة رقم {bill.Id}",
                    Creator = creator,
                    CreateDate = bill.CreateDate
                });
                db.Commit();

                return bill.Id;
            }
            catch { db.Rollback(); throw; }
        }

        public static void Return(ConnectionString connection, long billId, User editor)
        {
            using var db = new LiteDatabase(connection);
            db.BeginTrans();
            try
            {
                var bill = db.GetCollection<Bill>(DBCollections.Bills).FindById(billId)
                    ?? throw new InvalidOperationException("Invoice no longer exists.");

                if (bill.IsReturned)
                {
                    throw new InvalidOperationException("Invoice was already returned.");
                }

                foreach (var group in db.GetCollection<BillItemMove>(DBCollections.BillsItemsMoves).Find(x => x.Bill.Id == billId).ToList().GroupBy(x => x.Item.Id))
                {
                    var item = db.GetCollection<Item>(DBCollections.Items).FindById(group.Key)
                        ?? throw new InvalidOperationException("Cannot return a deleted item.");

                    item.QTY += group.Sum(x => x.QTY);
                    db.GetCollection<Item>(DBCollections.Items).Update(item);
                }

                foreach (var group in db.GetCollection<BillServiceMove>(DBCollections.BillsServicesMoves).Find(x => x.Bill.Id == billId).ToList().GroupBy(x => x.Service.Id))
                {
                    var service = db.GetCollection<Service>(DBCollections.Services).FindById(group.Key)
                        ?? throw new InvalidOperationException("Cannot return a deleted service.");

                    service.Balance += group.Sum(x => x.Balance);
                    db.GetCollection<Service>(DBCollections.Services).Update(service);
                }

                var client = db.GetCollection<Client>(DBCollections.Clients).FindById(bill.Client.Id)
                    ?? throw new InvalidOperationException("Cannot return an invoice for a deleted client.");

                client.Balance -= Math.Max(0, bill.TotalAfterDiscounts - bill.TotalPayed);
                db.GetCollection<Client>(DBCollections.Clients).Update(client);

                var treasury = db.GetCollection<Treasury>(DBCollections.Treasuries).FindById(1)
                    ?? throw new InvalidOperationException("Treasury no longer exists.");

                db.GetCollection<TreasuryMove>(DBCollections.TreasuriesMoves).Insert(new TreasuryMove
                {
                    Treasury = treasury,
                    Credit = Math.Min(bill.TotalPayed, bill.TotalAfterDiscounts),
                    Notes = $"مرتجع فاتورة رقم {billId}",
                    Creator = editor,
                    CreateDate = DateTime.Now
                });

                bill.IsReturned = true;
                bill.Editor = editor;
                bill.EditDate = DateTime.Now;

                db.GetCollection<Bill>(DBCollections.Bills).Update(bill);
                db.Commit();
            }
            catch { db.Rollback(); throw; }
        }
    }
}

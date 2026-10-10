using LiteDB;
using Phony.Data;
using Phony.DTOs;
using Phony.Mappers;
using Phony.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Phony.ViewModels;
using Xunit;

namespace Phony.Tests
{
    public sealed class InvoiceTests : DatabaseTestBase
    {
        [Fact]
        public void SaveWritesStockDebtAndNetCashTogether()
        {
            var id = Save(70);
            using var db = new LiteDatabase(_connection);
            Assert.Equal(8, db.GetCollection<Item>(DBCollections.Items).FindById(1).QTY);
            Assert.Equal(80, db.GetCollection<Service>(DBCollections.Services).FindById(1).Balance);
            Assert.Equal(30, db.GetCollection<Client>(DBCollections.Clients).FindById(2).Balance);
            Assert.Equal(100, db.GetCollection<Bill>(DBCollections.Bills).FindById(id).TotalAfterDiscounts);
            Assert.Equal(70, db.GetCollection<TreasuryMove>(DBCollections.TreasuriesMoves).FindAll().Sum(x => x.Debit - x.Credit));
        }

        [Fact]
        public void FailedSaveRollsBackEarlierStockUpdatesAndDraftIds()
        {
            var item = Item();
            Assert.Throws<InvalidOperationException>(() => Save(items: new[] { item }, services: new[] { Service(101) }));
            using (var db = new LiteDatabase(_connection))
            {
                Assert.Equal(10, db.GetCollection<Item>(DBCollections.Items).FindById(1).QTY);
                Assert.Equal(0, db.GetCollection<Bill>(DBCollections.Bills).Count());
                Assert.Equal(0, db.GetCollection<BillItemMove>(DBCollections.BillsItemsMoves).Count());
                Assert.Equal(0, db.GetCollection<TreasuryMove>(DBCollections.TreasuriesMoves).Count());
            }
            Assert.Equal(0, item.Id);
            Save(items: new[] { item });
        }

        [Fact]
        public void RepeatedLinesCannotOversellStock()
        {
            Assert.Throws<InvalidOperationException>(() => Save(items: new[] { Item(6), Item(6) }));
        }

        [Fact]
        public void FailureAfterInvoiceInsertionRollsBackAllWrites()
        {
            using (var db = new LiteDatabase(_connection))
            {
                db.GetCollection<BillItemMove>(DBCollections.BillsItemsMoves).EnsureIndex(x => x.Notes, true);
            }

            var first = Item();
            var second = Item();
            first.Notes = second.Notes = "duplicate";
            Assert.ThrowsAny<Exception>(() => Save(items: new[] { first, second }));
            using var verify = new LiteDatabase(_connection);
            Assert.Equal(10, verify.GetCollection<Item>(DBCollections.Items).FindById(1).QTY);
            Assert.Equal(100, verify.GetCollection<Service>(DBCollections.Services).FindById(1).Balance);
            Assert.Equal(0, verify.GetCollection<Bill>(DBCollections.Bills).Count());
            Assert.Equal(0, verify.GetCollection<BillItemMove>(DBCollections.BillsItemsMoves).Count());
            Assert.Equal(0, verify.GetCollection<TreasuryMove>(DBCollections.TreasuriesMoves).Count());
            Assert.Equal(0, first.Id);
            Assert.Equal(0, second.Id);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(70)]
        [InlineData(120)]
        public void ReturnRestoresStockDebtAndRefundsOnlyRetainedCash(decimal paid)
        {
            var id = Save(paid);
            InvoiceService.Return(_connection, id, _user);
            using var db = new LiteDatabase(_connection);
            Assert.Equal(10, db.GetCollection<Item>(DBCollections.Items).FindById(1).QTY);
            Assert.Equal(100, db.GetCollection<Service>(DBCollections.Services).FindById(1).Balance);
            Assert.Equal(0, db.GetCollection<Client>(DBCollections.Clients).FindById(2).Balance);
            Assert.Equal(0, db.GetCollection<TreasuryMove>(DBCollections.TreasuriesMoves).FindAll().Sum(x => x.Debit - x.Credit));
            Assert.True(db.GetCollection<Bill>(DBCollections.Bills).FindById(id).IsReturned);
        }

        [Fact]
        public void ReturnCannotBeAppliedTwice()
        {
            var id = Save();
            InvoiceService.Return(_connection, id, _user);
            Assert.Throws<InvalidOperationException>(() => InvoiceService.Return(_connection, id, _user));
            using var db = new LiteDatabase(_connection);
            Assert.Equal(10, db.GetCollection<Item>(DBCollections.Items).FindById(1).QTY);
            Assert.Equal(2, db.GetCollection<TreasuryMove>(DBCollections.TreasuriesMoves).Count());
        }

        [Fact]
        public void FailedReturnRollsBackInventoryRestoration()
        {
            var id = Save();
            using (var db = new LiteDatabase(_connection))
            {
                db.GetCollection<Service>(DBCollections.Services).Delete(1);
            }

            Assert.Throws<InvalidOperationException>(() => InvoiceService.Return(_connection, id, _user));
            using var verify = new LiteDatabase(_connection);
            Assert.Equal(8, verify.GetCollection<Item>(DBCollections.Items).FindById(1).QTY);
            Assert.False(verify.GetCollection<Bill>(DBCollections.Bills).FindById(id).IsReturned);
            Assert.Equal(1, verify.GetCollection<TreasuryMove>(DBCollections.TreasuriesMoves).Count());
        }

        [Fact]
        public void ReportsUseDiscountedTotalsAndActualChange()
        {
            Assert.Equal(90, new BillItemMoveDto { ItemPrice = 100, QTY = 1, Discount = 10 }.Total);
            Assert.Equal(90, new BillServiceMoveDto { Cost = 100, Discount = 10 }.Total);
            Assert.Equal(10, AppMapper.ToDto(new Bill { TotalPayed = 100, TotalAfterDiscounts = 90 }).Change);
            Assert.Equal(0, AppMapper.ToDto(new Bill { TotalPayed = 70, TotalAfterDiscounts = 90 }).Change);
        }

        [Fact]
        public void LaterInvoiceRechecksStockRatherThanTrustingTheDraft()
        {
            var draft = new[] { Item(6) };
            Save(items: draft);
            Assert.Throws<InvalidOperationException>(() => Save(items: draft));
            using var db = new LiteDatabase(_connection);
            Assert.Equal(4, db.GetCollection<Item>(DBCollections.Items).FindById(1).QTY);
            Assert.Equal(1, db.GetCollection<Bill>(DBCollections.Bills).Count());
        }

    }
}

using Caliburn.Micro;
using LiteDB;
using MahApps.Metro.Controls.Dialogs;
using Phony.WPF.Data;
using Phony.WPF.Models;
using Phony.WPF.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Phony.WPF.ViewModels
{
    public class SalesBillsViewerViewModel : Screen
    {
        long _clientSelectedValue;
        long _billSelectedValue;
        DateTime _firstDate;
        DateTime _secondDate;
        bool _byBillNo;
        bool _by2Dates;
        bool _byClientName;
        bool _byUserName;
        bool _isReturned;

        Visibility _isReturnedVisible;

        ObservableCollection<Bill> _bills;

        ObservableCollection<Client> _clients;

        ObservableCollection<User> _users;

        string _report;

        public long ClientSelectedValue
        {
            get => _clientSelectedValue;
            set
            {
                _clientSelectedValue = value;
                NotifyOfPropertyChange(() => ClientSelectedValue);
            }
        }

        public long BillSelectedValue
        {
            get => _billSelectedValue;
            set
            {
                _billSelectedValue = value;
                NotifyOfPropertyChange(() => BillSelectedValue);
            }
        }

        public DateTime FirstDate
        {
            get => _firstDate;
            set
            {
                _firstDate = value;
                NotifyOfPropertyChange(() => FirstDate);
            }
        }

        public DateTime SecondDate
        {
            get => _secondDate;
            set
            {
                _secondDate = value;
                NotifyOfPropertyChange(() => SecondDate);
            }
        }

        public bool ByBillNo
        {
            get => _byBillNo;
            set
            {
                _byBillNo = value;
                NotifyOfPropertyChange(() => ByBillNo);
            }
        }

        public bool By2Dates
        {
            get => _by2Dates;
            set
            {
                _by2Dates = value;
                NotifyOfPropertyChange(() => By2Dates);
            }
        }

        public bool ByClientName
        {
            get => _byClientName;
            set
            {
                _byClientName = value;
                NotifyOfPropertyChange(() => ByClientName);
            }
        }

        public bool ByUserName
        {
            get => _byUserName;
            set
            {
                _byUserName = value;
                NotifyOfPropertyChange(() => ByUserName);
            }
        }

        public bool IsReturned
        {
            get => _isReturned;
            set
            {
                _isReturned = value;
                NotifyOfPropertyChange(() => IsReturned);
            }
        }

        public Visibility IsReturnedVisible
        {
            get => _isReturnedVisible;
            set
            {
                _isReturnedVisible = value;
                NotifyOfPropertyChange(() => IsReturnedVisible);
            }
        }

        public ObservableCollection<Bill> Bills
        {
            get => _bills;
            set
            {
                _bills = value;
                NotifyOfPropertyChange(() => Bills);
            }
        }

        public ObservableCollection<Client> Clients
        {
            get => _clients;
            set
            {
                _clients = value;
                NotifyOfPropertyChange(() => Clients);
            }
        }

        public ObservableCollection<User> Users
        {
            get => _users;
            set
            {
                _users = value;
                NotifyOfPropertyChange(() => Users);
            }
        }

        public string Report
        {
            get => _report;
            set
            {
                _report = value;
                NotifyOfPropertyChange(() => Report);
            }
        }

        public SalesBillsViewerViewModel()
        {
            ByBillNo = true;
            IsReturnedVisible = Visibility.Collapsed;
            FirstDate = SecondDate = DateTime.Now;
            using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
            {
                Bills = new ObservableCollection<Bill>(db.GetCollection<Bill>(DBCollections.Bills).FindAll());
            }
        }

        public SalesBillsViewerViewModel(long id) : this()
        {
            BillSelectedValue = id;
            //LoadReport(id);
        }


        SalesBillsViewer Message = Application.Current.Windows.OfType<SalesBillsViewer>().FirstOrDefault();


        void BillReturnedStatues(long id)
        {
            using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
            {
                IsReturned = db.GetCollection<Bill>(DBCollections.Bills).FindById(id).IsReturned;
            }
        }
        /*
        async void LoadReport(long id)
        {
            using (var ds = new DataSet1())
            {
                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                {
                    var bill = db.GetCollection<Bill>(DBCollections.Bills).FindById(id);
                    var store = db.GetCollection<Store>(DBCollections.Stores).FindById(bill.Store.Id);
                    DataRow billdr = ds.Tables["Bill"].NewRow();
                    billdr["BillId"] = bill.Id;
                    billdr["BillDiscount"] = bill.Discount;
                    billdr["BillTotalAfterDiscount"] = bill.TotalAfterDiscounts;
                    billdr["BillTotalPayed"] = bill.TotalPayed;
                    billdr["BillNotes"] = bill.Notes;
                    billdr["ClientName"] = bill.Client.Name;
                    if (bill.Editor == null)
                    {
                        billdr["UserName"] = db.GetCollection<User>(DBCollections.Users).FindById(bill.Creator.Id).Name;
                    }
                    else
                    {
                        billdr["UserName"] = db.GetCollection<User>(DBCollections.Users).FindById(bill.Editor.Id).Name;
                    }
                    billdr["StoreName"] = store.Name;
                    billdr["StoreImage"] = store.Image;
                    billdr["Address1"] = store.Address1;
                    billdr["Address2"] = store.Address2;
                    billdr["Tel1"] = store.Tel1;
                    billdr["Tel2"] = store.Tel2;
                    billdr["Phone1"] = store.Phone1;
                    billdr["Phone2"] = store.Phone2;
                    billdr["Email1"] = store.Email1;
                    billdr["Email2"] = store.Email2;
                    billdr["Site"] = store.Site;
                    billdr["StoreNotes"] = store.Notes;
                    billdr["BillCreateDate"] = bill.CreateDate;
                    billdr["Motto"] = store.Motto;
                    ds.Tables["Bill"].Rows.Add(billdr);
                    var Items = db.GetCollection<BillItemMove>(DBCollections.BillsItemsMoves).Find(x => x.Bill.Id == id);
                    DataRow itemdr = ds.Tables["Items"].NewRow();
                    foreach (var item in Items)
                    {
                        itemdr["ItemName"] = db.GetCollection<Item>(DBCollections.Items).FindById(item.Item.Id).Name;
                        itemdr["ItemQTY"] = item.QTY;
                        itemdr["ItemDiscount"] = item.Discount;
                        itemdr["ItemSalePrice"] = item.ItemPrice;
                        itemdr["ItemNotes"] = item.Notes;
                        ds.Tables["Items"].Rows.Add(itemdr);
                    }
                    var Services = db.GetCollection<BillServiceMove>(DBCollections.BillsServicesMoves).Find(x => x.Bill.Id == id);
                    DataRow servicedr = ds.Tables["Services"].NewRow();
                    foreach (var service in Services)
                    {
                        servicedr["ServiceName"] = db.GetCollection<Service>(DBCollections.Services).FindById(service.Service.Id).Name;
                        servicedr["ServiceBalance"] = service.Balance;
                        servicedr["ServicePayment"] = service.Cost;
                        servicedr["ServiceDiscount"] = service.Discount;
                        servicedr["ServiceNotes"] = service.Notes;
                        ds.Tables["Services"].Rows.Add(servicedr);
                    }
                }
                string r = "";
                if (Properties.Settings.Default.SalesBillsPaperSize == "A4")
                {
                    //load report
                    await Task.Run(() =>
                    {
                        Report = r;
                    });
                }
                else
                {
                    await Task.Run(() =>
                    {
                        Report = r;
                    });
                }
            }
            BillReturnedStatues(id);
            IsReturnedVisible = Visibility.Visible;
        }

        async Task LoadReportAsync(long id)
        {
            using (var ds = new DataSet1())
            {
                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                {
                    var bill = db.GetCollection<Bill>(DBCollections.Bills).FindById(id);
                    var store = db.GetCollection<Store>(DBCollections.Stores).FindById(bill.Store.Id);
                    DataRow billdr = ds.Tables["Bill"].NewRow();
                    billdr["BillId"] = bill.Id;
                    billdr["BillDiscount"] = bill.Discount;
                    billdr["BillTotalAfterDiscount"] = bill.TotalAfterDiscounts;
                    billdr["BillTotalPayed"] = bill.TotalPayed;
                    billdr["BillNotes"] = bill.Notes;
                    billdr["ClientName"] = bill.Client.Name;
                    if (bill.Editor == null)
                    {
                        billdr["UserName"] = db.GetCollection<User>(DBCollections.Users).FindById(bill.Creator.Id).Name;
                    }
                    else
                    {
                        billdr["UserName"] = db.GetCollection<User>(DBCollections.Users).FindById(bill.Editor.Id).Name;
                    }
                    billdr["StoreName"] = store.Name;
                    billdr["StoreImage"] = store.Image;
                    billdr["Address1"] = store.Address1;
                    billdr["Address2"] = store.Address2;
                    billdr["Tel1"] = store.Tel1;
                    billdr["Tel2"] = store.Tel2;
                    billdr["Phone1"] = store.Phone1;
                    billdr["Phone2"] = store.Phone2;
                    billdr["Email1"] = store.Email1;
                    billdr["Email2"] = store.Email2;
                    billdr["Site"] = store.Site;
                    billdr["StoreNotes"] = store.Notes;
                    billdr["BillCreateDate"] = bill.CreateDate;
                    billdr["Motto"] = store.Motto;
                    ds.Tables["Bill"].Rows.Add(billdr);
                    var Items = db.GetCollection<BillItemMove>(DBCollections.BillsItemsMoves).Find(x => x.Bill.Id == id);
                    DataRow itemdr = ds.Tables["Items"].NewRow();
                    foreach (var item in Items)
                    {
                        itemdr["ItemName"] = db.GetCollection<Item>(DBCollections.Items).FindById(item.Item.Id).Name;
                        itemdr["ItemQTY"] = item.QTY;
                        itemdr["ItemDiscount"] = item.Discount;
                        itemdr["ItemSalePrice"] = item.ItemPrice;
                        itemdr["ItemNotes"] = item.Notes;
                        ds.Tables["Items"].Rows.Add(itemdr);
                    }
                    var Services = db.GetCollection<BillServiceMove>(DBCollections.BillsServicesMoves).Find(x => x.Bill.Id == id);
                    DataRow servicedr = ds.Tables["Services"].NewRow();
                    foreach (var service in Services)
                    {
                        servicedr["ServiceName"] = db.GetCollection<Service>(DBCollections.Services).FindById(service.Service.Id).Name;
                        servicedr["ServiceBalance"] = service.Balance;
                        servicedr["ServicePayment"] = service.Cost;
                        servicedr["ServiceDiscount"] = service.Discount;
                        servicedr["ServiceNotes"] = service.Notes;
                        ds.Tables["Services"].Rows.Add(servicedr);
                    }
                }
                string r = "";
                if (Properties.Settings.Default.SalesBillsPaperSize == "A4")
                {
                    //load report
                    await Task.Run(() =>
                    {
                        Report = r;
                    });
                }
                else
                {
                    await Task.Run(() =>
                    {
                        Report = r;
                    });
                }
            }
            BillReturnedStatues(id);
            IsReturnedVisible = Visibility.Visible;
        }*/
        /*
        private bool CanShow()
        {
            if (BillSelectedValue > 0)
            {
                return true;
            }
            return false;
        }

        private async void DoShow()
        {
            await LoadReportAsync(BillSelectedValue);
        }
        */
        private bool CanSaveReturned()
        {
            if (BillSelectedValue > 0)
            {
                return true;
            }
            return false;
        }

        private async void DoSaveReturned()
        {
            using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
            {
                var b = db.GetCollection<Bill>(DBCollections.Bills).FindById(BillSelectedValue);
                if (!b.IsReturned)
                {
                    b.IsReturned = IsReturned;
                    //b.Editor = Core.ReadUserSession();
                    b.EditDate = DateTime.Now;
                    if (b.TotalPayed < b.TotalAfterDiscounts)
                    {
                        if (IsReturned)
                        {
                            var c = db.GetCollection<Client>(DBCollections.Clients).FindById(b.Client.Id);
                            c.Balance -= b.TotalAfterDiscounts - b.TotalPayed;
                            db.GetCollection<Client>(DBCollections.Clients).Update(c);
                        }
                    }
                    db.GetCollection<Bill>(DBCollections.Bills).Update(b);
                    await Message.ShowMessageAsync("نجاح العملية", "تم ارجاع الفاتورة بنجاح");
                }
                else
                {
                    IsReturned = true;
                    await Message.ShowMessageAsync("خطأ", "لا يمكن اعاده مرتجع مرة اخرى قم بانشاء فاتورة جديدة");
                }
            }
        }

        private bool CanGetBills()
        {
            return true;
        }

        private void DoGetBills()
        {
            if (ByClientName && By2Dates)
            {
                if (ClientSelectedValue > 0 && FirstDate.Year > 2000 && SecondDate.Year > 2000)
                {
                    using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                    {
                        Bills = new ObservableCollection<Bill>(db.GetCollection<Bill>(DBCollections.Bills).Find(b => b.Client.Id == ClientSelectedValue && b.CreateDate >= FirstDate && b.CreateDate <= SecondDate));
                    }
                }
            }
            else if (ByClientName)
            {
                if (ClientSelectedValue > 0)
                {
                    using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                    {
                        Bills = new ObservableCollection<Bill>(db.GetCollection<Bill>(DBCollections.Bills).Find(b => b.Client.Id == ClientSelectedValue));
                    }
                }
            }
            else if (By2Dates)
            {
                if (FirstDate.Year > 2000 && SecondDate.Year > 2000)
                {
                    using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                    {
                        Bills = new ObservableCollection<Bill>(db.GetCollection<Bill>(DBCollections.Bills).Find(b => b.CreateDate >= FirstDate && b.CreateDate <= SecondDate));
                    }
                }
            }
            else
            {
                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                {
                    Bills = new ObservableCollection<Bill>(db.GetCollection<Bill>(DBCollections.Bills).FindAll());
                }
            }
        }
    }
}
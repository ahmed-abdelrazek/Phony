using LiteDB;
using Phony.Data;
using Phony.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Phony.ViewModels
{
    public class SalesBillsViewerViewModel : BindableBase
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

        CrystalDecisions.CrystalReports.Engine.ReportDocument _report;

        public long ClientSelectedValue
        {
            get => _clientSelectedValue;
            set => SetProperty(ref _clientSelectedValue, value);
        }

        public long BillSelectedValue
        {
            get => _billSelectedValue;
            set => SetProperty(ref _billSelectedValue, value);
        }

        public DateTime FirstDate
        {
            get => _firstDate;
            set => SetProperty(ref _firstDate, value);
        }

        public DateTime SecondDate
        {
            get => _secondDate;
            set => SetProperty(ref _secondDate, value);
        }

        public bool ByBillNo
        {
            get => _byBillNo;
            set => SetProperty(ref _byBillNo, value);
        }

        public bool By2Dates
        {
            get => _by2Dates;
            set => SetProperty(ref _by2Dates, value);
        }

        public bool ByClientName
        {
            get => _byClientName;
            set => SetProperty(ref _byClientName, value);
        }

        public bool ByUserName
        {
            get => _byUserName;
            set => SetProperty(ref _byUserName, value);
        }

        public bool IsReturned
        {
            get => _isReturned;
            set => SetProperty(ref _isReturned, value);
        }

        public Visibility IsReturnedVisible
        {
            get => _isReturnedVisible;
            set => SetProperty(ref _isReturnedVisible, value);
        }

        public ObservableCollection<Bill> Bills
        {
            get => _bills;
            set => SetProperty(ref _bills, value);
        }

        public ObservableCollection<Client> Clients
        {
            get => _clients;
            set => SetProperty(ref _clients, value);
        }

        public ObservableCollection<User> Users
        {
            get => _users;
            set => SetProperty(ref _users, value);
        }

        public CrystalDecisions.CrystalReports.Engine.ReportDocument Report
        {
            get => _report;
            set => SetProperty(ref _report, value);
        }

        public ICommand GetBills { get; set; }
        public ICommand Show { get; set; }
        public ICommand SaveReturned { get; set; }

        public SalesBillsViewerViewModel()
        {
            ByBillNo = true;
            IsReturnedVisible = Visibility.Collapsed;
            FirstDate = SecondDate = DateTime.Now;
            using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
            {
                Bills = new ObservableCollection<Bill>(db.GetCollection<Bill>(DBCollections.Bills).FindAll());
            }
            LoadCommands();
        }

        public SalesBillsViewerViewModel(long id) : this()
        {
            BillSelectedValue = id;
            LoadReport(id);
        }

        public void LoadCommands()
        {
            GetBills = new DelegateCommand(DoGetBills, CanGetBills);
            Show = new DelegateCommand(DoShow, CanShow).ObservesProperty(() => BillSelectedValue);
            SaveReturned = new DelegateCommand(DoSaveReturned, CanSaveReturned).ObservesProperty(() => BillSelectedValue);
        }

        void BillReturnedStatues(long id)
        {
            using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
            {
                IsReturned = db.GetCollection<Bill>(DBCollections.Bills).FindById(id).IsReturned;
            }
        }

        async void LoadReport(long id)
        {
            using (var ds = new DataSet1())
            {
                using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
                {
                    var bill = db.GetCollection<Bill>(DBCollections.Bills).FindById(id);
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
                    billdr["StoreName"] = bill.Store.Name;
                    billdr["StoreImage"] = bill.Store.Image;
                    billdr["Address1"] = bill.Store.Address1;
                    billdr["Address2"] = bill.Store.Address2;
                    billdr["Tel1"] = bill.Store.Tel1;
                    billdr["Tel2"] = bill.Store.Tel2;
                    billdr["Phone1"] = bill.Store.Phone1;
                    billdr["Phone2"] = bill.Store.Phone2;
                    billdr["Email1"] = bill.Store.Email1;
                    billdr["Email2"] = bill.Store.Email2;
                    billdr["Site"] = bill.Store.Site;
                    billdr["StoreNotes"] = bill.Store.Notes;
                    billdr["BillCreateDate"] = bill.CreateDate;
                    billdr["Motto"] = bill.Store.Motto;
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
                if (Properties.Settings.Default.SalesBillsPaperSize == "A4")
                {
                    Reports.SalesBillA4 r = new Reports.SalesBillA4();
                    r.SetDataSource(ds);
                    await Task.Run(() =>
                    {
                        Report = r;
                    });
                }
                else
                {
                    Reports.SalesBillA8 r = new Reports.SalesBillA8();
                    r.SetDataSource(ds);
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
                using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
                {
                    var bill = db.GetCollection<Bill>(DBCollections.Bills).FindById(id);
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
                    billdr["StoreName"] = bill.Store.Name;
                    billdr["StoreImage"] = bill.Store.Image;
                    billdr["Address1"] = bill.Store.Address1;
                    billdr["Address2"] = bill.Store.Address2;
                    billdr["Tel1"] = bill.Store.Tel1;
                    billdr["Tel2"] = bill.Store.Tel2;
                    billdr["Phone1"] = bill.Store.Phone1;
                    billdr["Phone2"] = bill.Store.Phone2;
                    billdr["Email1"] = bill.Store.Email1;
                    billdr["Email2"] = bill.Store.Email2;
                    billdr["Site"] = bill.Store.Site;
                    billdr["StoreNotes"] = bill.Store.Notes;
                    billdr["BillCreateDate"] = bill.CreateDate;
                    billdr["Motto"] = bill.Store.Motto;
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
                if (Properties.Settings.Default.SalesBillsPaperSize == "A4")
                {
                    Reports.SalesBillA4 r = new Reports.SalesBillA4();
                    r.SetDataSource(ds);
                    await Task.Run(() =>
                    {
                        Report = r;
                    });
                }
                else
                {
                    Reports.SalesBillA8 r = new Reports.SalesBillA8();
                    r.SetDataSource(ds);
                    await Task.Run(() =>
                    {
                        Report = r;
                    });
                }
            }
            BillReturnedStatues(id);
            IsReturnedVisible = Visibility.Visible;
        }

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

        private bool CanSaveReturned()
        {
            if (BillSelectedValue > 0)
            {
                return true;
            }
            return false;
        }

        private void DoSaveReturned()
        {
            using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
            {
                var b = db.GetCollection<Bill>(DBCollections.Bills).FindById(BillSelectedValue);
                if (!b.IsReturned)
                {
                    b.IsReturned = IsReturned;
                    b.Editor = Core.ReadUserSession();
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
                    BespokeFusion.MaterialMessageBox.Show("تم ارجاع الفاتورة بنجاح");
                }
                else
                {
                    IsReturned = true;
                    BespokeFusion.MaterialMessageBox.Show("لا يمكن اعاده مرتجع مرة اخرى قم بانشاء فاتورة جديدة");
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
                    using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
                    {
                        Bills = new ObservableCollection<Bill>(db.GetCollection<Bill>(DBCollections.Bills).Find(b => b.Client.Id == ClientSelectedValue && b.CreateDate >= FirstDate && b.CreateDate <= SecondDate));
                    }
                }
            }
            else if (ByClientName)
            {
                if (ClientSelectedValue > 0)
                {
                    using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
                    {
                        Bills = new ObservableCollection<Bill>(db.GetCollection<Bill>(DBCollections.Bills).Find(b => b.Client.Id == ClientSelectedValue));
                    }
                }
            }
            else if (By2Dates)
            {
                if (FirstDate.Year > 2000 && SecondDate.Year > 2000)
                {
                    using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
                    {
                        Bills = new ObservableCollection<Bill>(db.GetCollection<Bill>(DBCollections.Bills).Find(b => b.CreateDate >= FirstDate && b.CreateDate <= SecondDate));
                    }
                }
            }
            else
            {
                using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
                {
                    Bills = new ObservableCollection<Bill>(db.GetCollection<Bill>(DBCollections.Bills).FindAll());
                }
            }
        }
    }
}
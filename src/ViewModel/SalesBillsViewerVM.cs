using LiteDB;
using Phony.Kernel;
using Phony.Model;
using Phony.Utility;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Phony.ViewModel
{
    public class SalesBillsViewerVM : CommonBase
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
            set
            {
                if (value != _clientSelectedValue)
                {
                    _clientSelectedValue = value;
                    RaisePropertyChanged();
                }
            }
        }

        public long BillSelectedValue
        {
            get => _billSelectedValue;
            set
            {
                if (value != _billSelectedValue)
                {
                    _billSelectedValue = value;
                    RaisePropertyChanged();
                }
            }
        }

        public DateTime FirstDate
        {
            get => _firstDate;
            set
            {
                if (value != _firstDate)
                {
                    _firstDate = value;
                    RaisePropertyChanged();
                }
            }
        }

        public DateTime SecondDate
        {
            get => _secondDate;
            set
            {
                if (value != _secondDate)
                {
                    _secondDate = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool ByBillNo
        {
            get => _byBillNo;
            set
            {
                if (value != _byBillNo)
                {
                    _byBillNo = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool By2Dates
        {
            get => _by2Dates;
            set
            {
                if (value != _by2Dates)
                {
                    _by2Dates = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool ByClientName
        {
            get => _byClientName;
            set
            {
                if (value != _byClientName)
                {
                    _byClientName = value;
                    if (_byClientName)
                    {
                        using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
                        {
                            ClientSelectedValue = 0;
                            Clients = new ObservableCollection<Client>(db.GetCollection<Client>(DBCollections.Clients.ToString()).FindAll());
                        }
                    }
                    RaisePropertyChanged();
                }
            }
        }

        public bool ByUserName
        {
            get => _byUserName;
            set
            {
                if (value != _byUserName)
                {
                    _byUserName = value;
                    if (_byUserName)
                    {
                        using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
                        {
                            Users = new ObservableCollection<User>(db.GetCollection<User>(DBCollections.Users.ToString()).FindAll());
                        }
                    }
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsReturned
        {
            get => _isReturned;
            set
            {
                if (value != _isReturned)
                {
                    _isReturned = value;
                    RaisePropertyChanged();
                }
            }
        }

        public Visibility IsReturnedVisible
        {
            get => _isReturnedVisible;
            set
            {
                if (value != _isReturnedVisible)
                {
                    _isReturnedVisible = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<Bill> Bills
        {
            get => _bills;
            set
            {
                if (value != _bills)
                {
                    _bills = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<Client> Clients
        {
            get => _clients;
            set
            {
                if (value != _clients)
                {
                    _clients = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<User> Users
        {
            get => _users;
            set
            {
                if (value != _users)
                {
                    _users = value;
                    RaisePropertyChanged();
                }
            }
        }

        public CrystalDecisions.CrystalReports.Engine.ReportDocument Report
        {
            get => _report;
            set
            {
                if (value != _report)
                {
                    _report = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ICommand GetBills { get; set; }
        public ICommand Show { get; set; }
        public ICommand SaveReturned { get; set; }

        Users.LoginVM CurrentUser = new Users.LoginVM();

        public SalesBillsViewerVM()
        {
            ByBillNo = true;
            IsReturnedVisible = Visibility.Collapsed;
            FirstDate = SecondDate = DateTime.Now;
            using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
            {
                Bills = new ObservableCollection<Bill>(db.GetCollection<Bill>(DBCollections.Bills.ToString()).FindAll());
            }
            LoadCommands();
        }

        public SalesBillsViewerVM(long id) : this()
        {
            BillSelectedValue = id;
            LoadReport(id);
        }

        public void LoadCommands()
        {
            GetBills = new CustomCommand(DoGetBills, CanGetBills);
            Show = new CustomCommand(DoShow, CanShow);
            SaveReturned = new CustomCommand(DoSaveReturned, CanSaveReturned);
        }

        void BillReturnedStatues(long id)
        {
            using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
            {
                IsReturned = db.GetCollection<Bill>(DBCollections.Bills.ToString()).FindById(id).IsReturned;
            }
        }

        async void LoadReport(long id)
        {
            using (var ds = new DataSet1())
            {
                using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
                {
                    var bill = db.GetCollection<Bill>(DBCollections.Bills.ToString()).FindById(id);
                    DataRow billdr = ds.Tables["Bill"].NewRow();
                    billdr["BillId"] = bill.Id;
                    billdr["BillDiscount"] = bill.Discount;
                    billdr["BillTotalAfterDiscount"] = bill.TotalAfterDiscounts;
                    billdr["BillTotalPayed"] = bill.TotalPayed;
                    billdr["BillNotes"] = bill.Notes;
                    billdr["ClientName"] = bill.Client.Name;
                    if (bill.Editor == null)
                    {
                        billdr["UserName"] = db.GetCollection<User>(DBCollections.Users.ToString()).FindById(bill.Creator.Id).Name;
                    }
                    else
                    {
                        billdr["UserName"] = db.GetCollection<User>(DBCollections.Users.ToString()).FindById(bill.Editor.Id).Name;
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
                    var Items = db.GetCollection<BillItemMove>(DBCollections.BillsItemsMoves.ToString()).Find(x => x.Bill.Id == id);
                    DataRow itemdr = ds.Tables["Items"].NewRow();
                    foreach (var item in Items)
                    {
                        itemdr["ItemName"] = db.GetCollection<Item>(DBCollections.Items.ToString()).FindById(item.Item.Id).Name;
                        itemdr["ItemQTY"] = item.QTY;
                        itemdr["ItemDiscount"] = item.Discount;
                        itemdr["ItemSalePrice"] = item.ItemPrice;
                        itemdr["ItemNotes"] = item.Notes;
                        ds.Tables["Items"].Rows.Add(itemdr);
                    }
                    var Services = db.GetCollection<BillServiceMove>(DBCollections.BillsServicesMoves.ToString()).Find(x => x.Bill.Id == id);
                    DataRow servicedr = ds.Tables["Services"].NewRow();
                    foreach (var service in Services)
                    {
                        servicedr["ServiceName"] = db.GetCollection<Service>(DBCollections.Services.ToString()).FindById(service.Service.Id).Name;
                        servicedr["ServicePayment"] = service.ServicePayment;
                        servicedr["ServiceDiscount"] = service.Discount;
                        servicedr["ServiceNotes"] = service.Notes;
                        ds.Tables["Services"].Rows.Add(itemdr);
                    }
                }
                if (Properties.Settings.Default.SalesBillsPaperSize == "A4")
                {
                    Reports.SalesBillA4 r = new Reports.SalesBillA4();
                    await Task.Run(() =>
                    {
                        r.SetDataSource(ds);
                        Report = r;
                    });
                }
                else
                {
                    Reports.SalesBillA8 r = new Reports.SalesBillA8();
                    await Task.Run(() =>
                    {
                        r.SetDataSource(ds);
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
                    var bill = db.GetCollection<Bill>(DBCollections.Bills.ToString()).FindById(id);
                    DataRow billdr = ds.Tables["Bill"].NewRow();
                    billdr["BillId"] = bill.Id;
                    billdr["BillDiscount"] = bill.Discount;
                    billdr["BillTotalAfterDiscount"] = bill.TotalAfterDiscounts;
                    billdr["BillTotalPayed"] = bill.TotalPayed;
                    billdr["BillNotes"] = bill.Notes;
                    billdr["ClientName"] = bill.Client.Name;
                    if (bill.Editor == null)
                    {
                        billdr["UserName"] = db.GetCollection<User>(DBCollections.Users.ToString()).FindById(bill.Creator.Id).Name;
                    }
                    else
                    {
                        billdr["UserName"] = db.GetCollection<User>(DBCollections.Users.ToString()).FindById(bill.Editor.Id).Name;
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
                    var Items = db.GetCollection<BillItemMove>(DBCollections.BillsItemsMoves.ToString()).Find(x => x.Bill.Id == id);
                    DataRow itemdr = ds.Tables["Items"].NewRow();
                    foreach (var item in Items)
                    {
                        itemdr["ItemName"] = db.GetCollection<Item>(DBCollections.Items.ToString()).FindById(item.Item.Id).Name;
                        itemdr["ItemQTY"] = item.QTY;
                        itemdr["ItemDiscount"] = item.Discount;
                        itemdr["ItemSalePrice"] = item.ItemPrice;
                        itemdr["ItemNotes"] = item.Notes;
                        ds.Tables["Items"].Rows.Add(itemdr);
                    }
                    var Services = db.GetCollection<BillServiceMove>(DBCollections.BillsServicesMoves.ToString()).Find(x => x.Bill.Id == id);
                    DataRow servicedr = ds.Tables["Services"].NewRow();
                    foreach (var service in Services)
                    {
                        servicedr["ServiceName"] = db.GetCollection<Service>(DBCollections.Services.ToString()).FindById(service.Service.Id).Name;
                        servicedr["ServicePayment"] = service.ServicePayment;
                        servicedr["ServiceDiscount"] = service.Discount;
                        servicedr["ServiceNotes"] = service.Notes;
                        ds.Tables["Services"].Rows.Add(itemdr);
                    }
                }
                if (Properties.Settings.Default.SalesBillsPaperSize == "A4")
                {
                    Reports.SalesBillA4 r = new Reports.SalesBillA4();
                    await Task.Run(() =>
                    {
                        r.SetDataSource(ds);
                        Report = r;
                    });
                }
                else
                {
                    Reports.SalesBillA8 r = new Reports.SalesBillA8();
                    await Task.Run(() =>
                    {
                        r.SetDataSource(ds);
                        Report = r;
                    });
                }
            }
            BillReturnedStatues(id);
            IsReturnedVisible = Visibility.Visible;
        }

        private bool CanShow(object obj)
        {
            if (BillSelectedValue > 0)
            {
                return true;
            }
            return false;
        }

        private async void DoShow(object obj)
        {
            await LoadReportAsync(BillSelectedValue);
        }

        private bool CanSaveReturned(object obj)
        {
            if (BillSelectedValue > 0)
            {
                return true;
            }
            return false;
        }

        private void DoSaveReturned(object obj)
        {
            using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
            {
                var b = db.GetCollection<Bill>(DBCollections.Bills.ToString()).FindById(BillSelectedValue);
                if (!b.IsReturned)
                {
                    b.IsReturned = IsReturned;
                    if (b.TotalPayed < b.TotalAfterDiscounts)
                    {
                        if (IsReturned)
                        {
                            var c = db.GetCollection<Client>(DBCollections.Clients.ToString()).FindById(b.Client.Id);
                            c.Balance -= b.TotalAfterDiscounts - b.TotalPayed;
                            db.GetCollection<Client>(DBCollections.Clients.ToString()).Update(c);
                        }
                    }
                    db.GetCollection<Bill>(DBCollections.Bills.ToString()).Update(b);
                    BespokeFusion.MaterialMessageBox.Show("تم ارجاع الفاتورة بنجاح");
                }
                else
                {
                    IsReturned = true;
                    BespokeFusion.MaterialMessageBox.Show("لا يمكن اعاده مرتجع مرة اخرى قم بانشاء فاتورة جديدة");
                }
            }
        }

        private bool CanGetBills(object obj)
        {
            return true;
        }

        private void DoGetBills(object obj)
        {
            if (ByClientName && By2Dates)
            {
                if (ClientSelectedValue > 0 && FirstDate.Year > 2000 && SecondDate.Year > 2000)
                {
                    using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
                    {
                        Bills = new ObservableCollection<Bill>(db.GetCollection<Bill>(DBCollections.Bills.ToString()).Find(b => b.Client.Id == ClientSelectedValue && b.CreateDate >= FirstDate && b.CreateDate <= SecondDate));
                    }
                }
            }
            else if (ByClientName)
            {
                if (ClientSelectedValue > 0)
                {
                    using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
                    {
                        Bills = new ObservableCollection<Bill>(db.GetCollection<Bill>(DBCollections.Bills.ToString()).Find(b => b.Client.Id == ClientSelectedValue));
                    }
                }
            }
            else if (By2Dates)
            {
                if (FirstDate.Year > 2000 && SecondDate.Year > 2000)
                {
                    using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
                    {
                        Bills = new ObservableCollection<Bill>(db.GetCollection<Bill>(DBCollections.Bills.ToString()).Find(b => b.CreateDate >= FirstDate && b.CreateDate <= SecondDate));
                    }
                }
            }
            else
            {
                using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
                {
                    Bills = new ObservableCollection<Bill>(db.GetCollection<Bill>(DBCollections.Bills.ToString()).FindAll());
                }
            }
        }
    }
}
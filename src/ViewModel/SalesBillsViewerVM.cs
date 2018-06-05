using Phony.Kernel;
using Phony.Model;
using Phony.Persistence;
using Phony.Utility;
using Phony.View;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Phony.ViewModel
{
    public class SalesBillsViewerVM : CommonBase
    {
        int _clientSelectedValue;
        int _billSelectedValue;
        string billCommand = "SELECT Bills.Id AS BillId, Bills.Discount AS BillDiscount, Bills.TotalAfterDiscounts AS BillTotalAfterDiscount, Bills.TotalPayed AS BillTotalPayed, Bills.Notes AS BillNotes, Bills.CreateDate AS BillCreateDate, Clients.[Name] AS ClientName, Users.[Name] AS UserName, [Stores].[Name] AS StoreName, CAST(Stores.[Image] AS VARBINARY(MAX)) AS StoreImage, Stores.Address1, Stores.Address2, Stores.Tel1, Stores.Tel2, Stores.Phone1, Stores.Phone2, Stores.Email1, Stores.Email2, Stores.[Site], Stores.Notes AS StoreNotes FROM [dbo].[Bills] JOIN [Clients] on [Clients].[Id] = [Bills].[ClientId] JOIN [Stores] on [Bills].[StoreId] = [dbo].[Stores].[Id] JOIN [Users] on [Users].[Id] = (CASE WHEN([Bills].[EditById] > 0) THEN [Bills].[EditById] ELSE([Bills].[CreatedById]) END) WHERE [Bills].[Id] = {0}";
        string billItemsCommand = "SELECT BillItemMoves.QTY AS ItemQTY, BillItemMoves.Discount AS ItemDiscount, BillItemMoves.Notes AS ItemNotes, Items.[Name] AS ItemName, Items.SalePrice AS ItemSalePrice FROM [dbo].[BillItemMoves] JOIN [Items] on [BillItemMoves].[ItemId] = [dbo].[Items].[Id] WHERE [dbo].[BillItemMoves].[BillId] = {0}";
        string billServicesCommand = "SELECT BillServiceMoves.ServicePayment, BillServiceMoves.Discount AS ServiceDiscount, BillServiceMoves.Notes AS ServiceNotes, [Services].[Name] AS ServiceName FROM [dbo].[BillServiceMoves] JOIN [Services] on [BillServiceMoves].ServiceId = [dbo].[Services].[Id] WHERE [dbo].[BillServiceMoves].[BillId] = {0}";
        DateTime _firstDate;
        DateTime _secondDate;
        bool _byBillNo;
        bool _by2Dates;
        bool _byClientName;
        bool _byUserName;
        bool _isReturned;

        Visibility _isReturnedVisiable;

        ObservableCollection<Bill> _bills;

        ObservableCollection<Client> _clients;

        ObservableCollection<User> _users;

        CrystalDecisions.CrystalReports.Engine.ReportDocument _report;

        public int ClientSelectedValue
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

        public int BillSelectedValue
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
                        using (var db = new PhonyDbContext())
                        {
                            ClientSelectedValue = 0;
                            Clients = new ObservableCollection<Client>(db.Clients);
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
                        using (var db = new PhonyDbContext())
                        {
                            Users = new ObservableCollection<User>(db.Users);
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

        public Visibility IsReturnedVisiable
        {
            get => _isReturnedVisiable;
            set
            {
                if (value != _isReturnedVisiable)
                {
                    _isReturnedVisiable = value;
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
            IsReturnedVisiable = Visibility.Collapsed;
            FirstDate = SecondDate = DateTime.Now;
            using (var db = new PhonyDbContext())
            {
                Bills = new ObservableCollection<Bill>(db.Bills);
            }
            LoadCommands();
        }

        public SalesBillsViewerVM(int id) : this()
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

        void BillReturnedStatues(int id)
        {
            using (var db = new UnitOfWork(new PhonyDbContext()))
            {
                IsReturned = db.Bills.Get(id).IsReturned;
            }
        }

        async void LoadReport(int id)
        {
            using (var ds = new DataSet1())
            {
                var billno = id;
                SqlDataAdapter adp = new SqlDataAdapter(string.Format(billCommand, billno), Properties.Settings.Default.ConnectionString);
                adp.Fill(ds, "Bill");
                adp = new SqlDataAdapter(string.Format(billItemsCommand, billno), Properties.Settings.Default.ConnectionString);
                adp.Fill(ds, "Items");
                adp = new SqlDataAdapter(string.Format(billServicesCommand, billno), Properties.Settings.Default.ConnectionString);
                adp.Fill(ds, "Services");
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
                    Reports.SalesBillA7 r = new Reports.SalesBillA7();
                    await Task.Run(() =>
                    {
                        r.SetDataSource(ds);
                        Report = r;
                    });
                }
            }
            BillReturnedStatues(id);
            IsReturnedVisiable = Visibility.Visible;
        }

        async Task LoadReportAsync(int id)
        {
            using (var ds = new DataSet1())
            {
                var billno = id;
                SqlDataAdapter adp = new SqlDataAdapter(string.Format(billCommand, billno), Properties.Settings.Default.ConnectionString);
                adp.Fill(ds, "Bill");
                adp = new SqlDataAdapter(string.Format(billItemsCommand, billno), Properties.Settings.Default.ConnectionString);
                adp.Fill(ds, "Items");
                adp = new SqlDataAdapter(string.Format(billServicesCommand, billno), Properties.Settings.Default.ConnectionString);
                adp.Fill(ds, "Services");
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
                    Reports.SalesBillA7 r = new Reports.SalesBillA7();
                    await Task.Run(() =>
                    {
                        r.SetDataSource(ds);
                        Report = r;
                    });
                }
            }
            BillReturnedStatues(id);
            IsReturnedVisiable = Visibility.Visible;
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
            using (var db = new UnitOfWork(new PhonyDbContext()))
            {
                var b = db.Bills.Get(BillSelectedValue);
                if (!b.IsReturned)
                {
                    b.IsReturned = IsReturned;
                    b.EditById = CurrentUser.Id;
                    b.EditDate = DateTime.Now;
                    if (b.TotalPayed < b.TotalAfterDiscounts)
                    {
                        if (IsReturned)
                        {
                            var c = db.Clients.Get(b.ClientId);
                            c.Balance -= b.TotalAfterDiscounts - b.TotalPayed;
                        }
                    }
                    db.Complete();
                }
                else
                {
                    IsReturned = true;
                    MessageBox.Show("لا يمكن اعاده مرتجع مرة اخرى قم بانشاء فاتورة جديدة");
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
                    using (var db = new PhonyDbContext())
                    {
                        Bills = new ObservableCollection<Bill>(db.Bills.Where(b => b.ClientId == ClientSelectedValue && b.CreateDate >= FirstDate && b.CreateDate <= SecondDate));
                    }
                }
            }
            else if (ByClientName)
            {
                if (ClientSelectedValue > 0)
                {
                    using (var db = new PhonyDbContext())
                    {
                        Bills = new ObservableCollection<Bill>(db.Bills.Where(b => b.ClientId == ClientSelectedValue));
                    }
                }
            }
            else if (By2Dates)
            {
                if (FirstDate.Year > 2000 && SecondDate.Year > 2000)
                {
                    using (var db = new PhonyDbContext())
                    {
                        Bills = new ObservableCollection<Bill>(db.Bills.Where(b => b.CreateDate >= FirstDate && b.CreateDate <= SecondDate));
                    }
                }
            }
            else
            {
                using (var db = new PhonyDbContext())
                {
                    Bills = new ObservableCollection<Bill>(db.Bills);
                }
            }
        }
    }
}
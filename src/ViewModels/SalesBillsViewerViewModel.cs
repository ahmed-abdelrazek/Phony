using LiteDB;
using MahApps.Metro.Controls.Dialogs;
using Phony.Data;
using Phony.DTOs;
using Phony.Mappers;
using Phony.Models;
using Phony.Views;
using Prism.Commands;
using Prism.Mvvm;
using RazorEngineCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
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

        ObservableCollection<Bill> _bills = [];

        ObservableCollection<Client> _clients = [];

        ObservableCollection<User> _users = [];

        string? _report;

        private long _loadedBillId;
        private bool _isReturning;
        private bool _loadedBillWasReturned;
        public long LoadedBillId
        {
            get => _loadedBillId;
            private set => SetProperty(ref _loadedBillId, value);
        }
        public bool IsReturning
        {
            get => _isReturning;
            private set => SetProperty(ref _isReturning, value);
        }
        public long ClientSelectedValue
        {
            get => _clientSelectedValue;
            set => SetProperty(ref _clientSelectedValue, value);
        }

        public long BillSelectedValue
        {
            get => _billSelectedValue;
            set
            {
                if (SetProperty(ref _billSelectedValue, value))
                {
                    LoadedBillId = 0;
                    IsReturned = false;
                    IsReturnedVisible = Visibility.Collapsed;
                }
            }
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

        public string? Report
        {
            get => _report;
            set => SetProperty(ref _report, value);
        }

        public ICommand? GetBills { get; set; }
        public ICommand? Show { get; set; }
        public ICommand? SaveReturned { get; set; }

        public SalesBillsViewerViewModel()
        {
            ByBillNo = true;
            IsReturnedVisible = Visibility.Collapsed;
            FirstDate = SecondDate = DateTime.Now;
            using (var db = new LiteDatabase(LiteDbContext.ConnectionString))
            {
                Bills = new ObservableCollection<Bill>(db.GetCollection<Bill>(DBCollections.Bills).FindAll());
            }
            LoadCommands();
        }

        public SalesBillsViewerViewModel(long id) : this()
        {
            BillSelectedValue = id;
        }

        private SalesBillsViewer Message => Application.Current.Windows.OfType<SalesBillsViewer>().FirstOrDefault() ?? throw new InvalidOperationException("An open view is required for this UI action.");

        public void LoadCommands()
        {
            GetBills = new DelegateCommand(DoGetBills, CanGetBills);
            Show = new DelegateCommand(DoShow, CanShow).ObservesProperty(() => BillSelectedValue);
            SaveReturned = new DelegateCommand(DoSaveReturned, CanSaveReturned).ObservesProperty(() => BillSelectedValue).ObservesProperty(() => LoadedBillId).ObservesProperty(() => IsReturned).ObservesProperty(() => IsReturning).ObservesProperty(() => Bills);
        }

        void BillReturnedStatues(long id)
        {
            using var db = new LiteDatabase(LiteDbContext.ConnectionString);
            IsReturned = db.GetCollection<Bill>(DBCollections.Bills).FindById(id).IsReturned;
        }

        public async Task LoadReportAsync(long id)
        {
            LoadedBillId = 0;
            Report = null;
            IsReturnedVisible = Visibility.Collapsed;
            BillDto? bill = null;

            using (var db = new LiteDatabase(LiteDbContext.ConnectionString))
            {
                bill = AppMapper.ToDto(db.GetCollection<Bill>(DBCollections.Bills)
                    .Include(x => x.Store)
                    .Include(x => x.Client)
                    .Include(x => x.Creator).FindById(id));

                if (bill is null)
                {
                    return;
                }

                var services = AppMapper.ToBillServiceMoveDtos(db.GetCollection<BillServiceMove>(DBCollections.BillsServicesMoves).Include(x => x.Service).Find(x => x.Bill != null && x.Bill.Id == bill.Id));
                bill.ServicesMoves = services;

                var Items = AppMapper.ToBillItemMoveDtos(db.GetCollection<BillItemMove>(DBCollections.BillsItemsMoves).Include(x => x.Item).Find(x => x.Bill != null && x.Bill.Id == bill.Id));
                bill.ItemsMoves = Items;

            }
            if (Properties.Settings.Default.SalesBillsPaperSize == "A4")
            {
                IRazorEngine razorEngine = new RazorEngine();
                var templateString = System.IO.File.ReadAllText(System.IO.Path.Combine(AppContext.BaseDirectory, "Reports", "SalesInvoiceReportA4.html"));
                IRazorEngineCompiledTemplate template = await razorEngine.CompileAsync(templateString);

                var report = template.Run(bill);
                if (BillSelectedValue != id)
                {
                    return;
                }

                Report = report;
            }
            else
            {
                IRazorEngine razorEngine = new RazorEngine();
                var templateString = System.IO.File.ReadAllText(System.IO.Path.Combine(AppContext.BaseDirectory, "Reports", "SalesInvoiceReportA8.html"));
                IRazorEngineCompiledTemplate template = await razorEngine.CompileAsync(templateString);

                var report = template.Run(bill);
                if (BillSelectedValue != id)
                {
                    return;
                }

                Report = report;
            }

            if (BillSelectedValue != id)
            {
                return;
            }

            BillReturnedStatues(id);
            _loadedBillWasReturned = IsReturned;
            LoadedBillId = id;
            IsReturnedVisible = Visibility.Visible;
        }

        private bool CanShow()
        {
            return BillSelectedValue > 0;
        }

        private async void DoShow()
        {
            await LoadReportAsync(BillSelectedValue);
        }

        private bool CanSaveReturned()
        {
            return !IsReturning && !_loadedBillWasReturned && IsReturned && LoadedBillId > 0
                && LoadedBillId == BillSelectedValue && Bills.Any(b => b.Id == BillSelectedValue);
        }

        private async void DoSaveReturned()
        {
            if (!CanSaveReturned())
            {
                return;
            }

            var id = LoadedBillId;
            IsReturning = true;

            try
            {
                InvoiceService.Return(LiteDbContext.ConnectionString, id, Core.ReadUserSession());
                LoadedBillId = 0;
                IsReturnedVisible = Visibility.Collapsed;
                await Message.ShowMessageAsync("نجاح العملية", "تم ارجاع الفاتورة بنجاح");
            }
            catch (Exception ex)
            {
                await Core.SaveExceptionAsync(ex);
                await Message.ShowMessageAsync("خطأ", "تعذر إرجاع الفاتورة. تحقق من بياناتها أو أنها لم ترجع سابقاً.");
            }
            finally { IsReturning = false; }
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
                    using var db = new LiteDatabase(LiteDbContext.ConnectionString);
                    Bills = new ObservableCollection<Bill>(db.GetCollection<Bill>(DBCollections.Bills).Find(b => b.Client != null && b.Client.Id == ClientSelectedValue && b.CreateDate >= FirstDate && b.CreateDate <= SecondDate));
                }
            }
            else if (ByClientName)
            {
                if (ClientSelectedValue > 0)
                {
                    using var db = new LiteDatabase(LiteDbContext.ConnectionString);
                    Bills = new ObservableCollection<Bill>(db.GetCollection<Bill>(DBCollections.Bills).Find(b => b.Client != null && b.Client.Id == ClientSelectedValue));
                }
            }
            else if (By2Dates)
            {
                if (FirstDate.Year > 2000 && SecondDate.Year > 2000)
                {
                    using var db = new LiteDatabase(LiteDbContext.ConnectionString);
                    Bills = new ObservableCollection<Bill>(db.GetCollection<Bill>(DBCollections.Bills).Find(b => b.CreateDate >= FirstDate && b.CreateDate <= SecondDate));
                }
            }
            else
            {
                using var db = new LiteDatabase(LiteDbContext.ConnectionString);
                Bills = new ObservableCollection<Bill>(db.GetCollection<Bill>(DBCollections.Bills).FindAll());
            }
        }
    }
}

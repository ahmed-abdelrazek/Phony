using MahApps.Metro.Controls.Dialogs;
using Phony.Kernel;
using Phony.Model;
using Phony.Persistence;
using Phony.Utility;
using Phony.View;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace Phony.ViewModel
{
    public class ClientVM : CommonBase
    {
        int _clientsId;
        string _name;
        string _notes;
        string _searchText;
        string _childName;
        string _childPrice;
        static string _clientsCount;
        static string _clientsPurchasePrice;
        static string _clientsSalePrice;
        static string _clientsProfit;
        decimal _balance;
        bool _fastResult;
        bool _openFastResult;
        bool _isAddClientFlyoutOpen;
        Client _dataGridSelectedClient;

        ObservableCollection<Client> _clients;

        public int ClientId
        {
            get => _clientsId;
            set
            {
                if (value != _clientsId)
                {
                    _clientsId = value;
                    RaisePropertyChanged(nameof(ClientId));
                }
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (value != _name)
                {
                    _name = value;
                    RaisePropertyChanged(nameof(Name));
                }
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (value != _searchText)
                {
                    _searchText = value;
                    RaisePropertyChanged(nameof(SearchText));
                }
            }
        }

        public string Notes
        {
            get => _notes;
            set
            {
                if (value != _notes)
                {
                    _notes = value;
                    RaisePropertyChanged(nameof(Notes));
                }
            }
        }

        public string ChildName
        {
            get => _childName;
            set
            {
                if (value != _childName)
                {
                    _childName = value;
                    RaisePropertyChanged(nameof(ChildName));
                }
            }
        }

        public string ChildPrice
        {
            get => _childPrice;
            set
            {
                if (value != _childPrice)
                {
                    _childPrice = value;
                    RaisePropertyChanged(nameof(ChildPrice));
                }
            }
        }

        public string ClientCount
        {
            get => _clientsCount;
            set
            {
                if (value != _clientsCount)
                {
                    _clientsCount = value;
                    RaisePropertyChanged(nameof(ClientCount));
                }
            }
        }

        public string ClientCredits
        {
            get => _clientsPurchasePrice;
            set
            {
                if (value != _clientsPurchasePrice)
                {
                    _clientsPurchasePrice = value;
                    RaisePropertyChanged(nameof(ClientCredits));
                }
            }
        }

        public string ClientDebits
        {
            get => _clientsSalePrice;
            set
            {
                if (value != _clientsSalePrice)
                {
                    _clientsSalePrice = value;
                    RaisePropertyChanged(nameof(ClientDebits));
                }
            }
        }

        public string ClientProfit
        {
            get => _clientsProfit;
            set
            {
                if (value != _clientsProfit)
                {
                    _clientsProfit = value;
                    RaisePropertyChanged(nameof(ClientProfit));
                }
            }
        }

        public decimal Balance
        {
            get => _balance;
            set
            {
                if (value != _balance)
                {
                    _balance = value;
                    RaisePropertyChanged(nameof(Balance));
                }
            }
        }

        public bool FastResult
        {
            get => _fastResult;
            set
            {
                if (value != _fastResult)
                {
                    _fastResult = value;
                    RaisePropertyChanged(nameof(FastResult));
                }
            }
        }

        public bool OpenFastResult
        {
            get => _openFastResult;
            set
            {
                if (value != _openFastResult)
                {
                    _openFastResult = value;
                    RaisePropertyChanged(nameof(OpenFastResult));
                }
            }
        }

        public Client DataGridSelectedClient
        {
            get => _dataGridSelectedClient;
            set
            {
                if (value != _dataGridSelectedClient)
                {
                    _dataGridSelectedClient = value;
                    RaisePropertyChanged(nameof(DataGridSelectedClient));
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
                    RaisePropertyChanged(nameof(Clients));
                }
            }
        }

        public ObservableCollection<User> Users { get; set; }

        public bool IsAddClientFlyoutOpen
        {
            get => _isAddClientFlyoutOpen;
            set
            {
                if (value != _isAddClientFlyoutOpen)
                {
                    _isAddClientFlyoutOpen = value;
                    RaisePropertyChanged(nameof(IsAddClientFlyoutOpen));
                }
            }
        }

        public ICommand OpenAddClientFlyout { get; set; }
        public ICommand FillUI { get; set; }
        public ICommand DeleteClient { get; set; }
        public ICommand ReloadAllClients { get; set; }
        public ICommand Search { get; set; }
        public ICommand AddClient { get; set; }
        public ICommand ClientPay { get; set; }
        public ICommand EditClient { get; set; }

        Users.LoginVM CurrentUser = new Users.LoginVM();

        Clients ClientsMassage = Application.Current.Windows.OfType<Clients>().FirstOrDefault();

        public ClientVM()
        {
            LoadCommands();
            using (var db = new PhonyDbContext())
            {
                Clients = new ObservableCollection<Client>(db.Clients);
                Users = new ObservableCollection<User>(db.Users);
            }
            new Thread(() =>
            {
                ClientCount = $"مجموع العملاء: {Clients.Count().ToString()}";
                ClientDebits = $"اجمالى لينا: {decimal.Round(Clients.Where(c => c.Balance > 0).Sum(i => i.Balance), 2).ToString()}";
                ClientCredits = $"اجمالى علينا: {decimal.Round(Clients.Where(c => c.Balance < 0).Sum(i => i.Balance), 2).ToString()}";
                ClientProfit = $"تقدير لصافى لينا: {decimal.Round((Clients.Where(c => c.Balance > 0).Sum(i => i.Balance) + Clients.Where(c => c.Balance < 0).Sum(i => i.Balance)), 2).ToString()}";
                Thread.CurrentThread.Abort();
            }).Start();
        }

        public void LoadCommands()
        {
            OpenAddClientFlyout = new CustomCommand(DoOpenAddClientFlyout, CanOpenAddClientFlyout);
            FillUI = new CustomCommand(DoFillUI, CanFillUI);
            DeleteClient = new CustomCommand(DoDeleteClient, CanDeleteClient);
            ReloadAllClients = new CustomCommand(DoReloadAllClients, CanReloadAllClients);
            Search = new CustomCommand(DoSearch, CanSearch);
            AddClient = new CustomCommand(DoAddClient, CanAddClient);
            ClientPay = new CustomCommand(DoClientPayAsync, CanClientPay);
            EditClient = new CustomCommand(DoEditClient, CanEditClient);
        }

        private bool CanClientPay(object obj)
        {
            if (DataGridSelectedClient == null)
            {
                return false;
            }
            return true;
        }

        private async void DoClientPayAsync(object obj)
        {
            var result = await ClientsMassage.ShowInputAsync("تدفيع", $"ادخل المبلغ الذى تريد تدفيعه للعميل {DataGridSelectedClient.Name}");
            if (string.IsNullOrWhiteSpace(result))
            {
                await ClientsMassage.ShowMessageAsync("ادخل مبلغ", "لم تقم بادخال اى مبلغ لتدفيعه");
            }
            else
            {
                decimal clientpaymentamount;
                bool isvalidmoney =  decimal.TryParse(result, out clientpaymentamount);
                if (isvalidmoney)
                {
                    using (var db = new UnitOfWork(new PhonyDbContext()))
                    {
                        var c = db.Clients.Get(DataGridSelectedClient.Id);
                        c.Balance -= clientpaymentamount;
                        c.EditDate = DateTime.Now;
                        c.EditById = CurrentUser.Id;
                        db.Complete();
                        ClientId = 0;
                        Clients.Remove(DataGridSelectedClient);
                        Clients.Add(c);
                        DataGridSelectedClient = null;
                        await ClientsMassage.ShowMessageAsync("تمت العملية", "تم تعديل الصنف بنجاح");
                    }
                }
                else
                {
                    await ClientsMassage.ShowMessageAsync("خطاء فى المبلغ", "ادخل مبلغ صحيح بعلامه عشرية واحدة");
                }
            }
        }

        private bool CanEditClient(object obj)
        {
            if (string.IsNullOrWhiteSpace(Name) || ClientId == 0 || DataGridSelectedClient == null)
            {
                return false;
            }
            return true;
        }

        private void DoEditClient(object obj)
        {
            using (var db = new UnitOfWork(new PhonyDbContext()))
            {
                var c = db.Clients.Get(DataGridSelectedClient.Id);
                c.Name = Name;
                c.Balance = Balance;
                c.Notes = Notes;
                c.EditDate = DateTime.Now;
                c.EditById = CurrentUser.Id;
                db.Complete();
                ClientId = 0;
                Clients.Remove(DataGridSelectedClient);
                Clients.Add(c);
                DataGridSelectedClient = null;
                ClientsMassage.ShowMessageAsync("تمت العملية", "تم تعديل الصنف بنجاح");
            }
        }

        private bool CanAddClient(object obj)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return false;
            }
            return true;
        }

        private void DoAddClient(object obj)
        {
            using (var db = new UnitOfWork(new PhonyDbContext()))
            {
                var c = new Client
                {
                    Name = Name,
                    Balance = Balance,
                    Notes = Notes,
                    CreateDate = DateTime.Now,
                    CreatedById = CurrentUser.Id,
                    EditDate = null,
                    EditById = null
                };
                db.Clients.Add(c);
                db.Complete();
                Clients.Add(c);
                ClientsMassage.ShowMessageAsync("تمت العملية", "تم اضافة الصنف بنجاح");
            }
        }

        private bool CanSearch(object obj)
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                return false;
            }
            return true;
        }

        private void DoSearch(object obj)
        {
            using (var db = new PhonyDbContext())
            {
                Clients = new ObservableCollection<Client>(db.Clients.Where(i => i.Name.Contains(SearchText)));
                if (FastResult)
                {
                    ChildName = Clients.FirstOrDefault().Name;
                    ChildPrice = Clients.FirstOrDefault().Balance.ToString();
                    OpenFastResult = true;
                }
            }
        }

        private bool CanReloadAllClients(object obj)
        {
            return true;
        }

        private void DoReloadAllClients(object obj)
        {
            using (var db = new PhonyDbContext())
            {
                Clients = new ObservableCollection<Client>(db.Clients);
            }
        }

        private bool CanDeleteClient(object obj)
        {
            if (DataGridSelectedClient == null)
            {
                return false;
            }
            return true;
        }

        private async void DoDeleteClient(object obj)
        {
            var result = await ClientsMassage.ShowMessageAsync("حذف الصنف", $"هل انت متاكد من حذف العميل {DataGridSelectedClient.Name}", MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Affirmative)
            {
                using (var db = new UnitOfWork(new PhonyDbContext()))
                {
                    db.Clients.Remove(db.Clients.Get(DataGridSelectedClient.Id));
                    db.Complete();
                    Clients.Remove(DataGridSelectedClient);
                }
                DataGridSelectedClient = null;
                await ClientsMassage.ShowMessageAsync("تمت العملية", "تم حذف الصنف بنجاح");
            }
        }

        private bool CanFillUI(object obj)
        {
            if (DataGridSelectedClient == null)
            {
                return false;
            }
            return true;
        }

        private void DoFillUI(object obj)
        {
            ClientId = DataGridSelectedClient.Id;
            Name = DataGridSelectedClient.Name;
            Balance = DataGridSelectedClient.Balance;
            Notes = DataGridSelectedClient.Notes;
            IsAddClientFlyoutOpen = true;
        }

        private bool CanOpenAddClientFlyout(object obj)
        {
            return true;
        }

        private void DoOpenAddClientFlyout(object obj)
        {
            if (IsAddClientFlyoutOpen)
            {
                IsAddClientFlyoutOpen = false;
            }
            else
            {
                IsAddClientFlyoutOpen = true;
            }
        }
    }
}

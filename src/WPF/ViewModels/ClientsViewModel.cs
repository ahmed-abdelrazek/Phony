using LiteDB;
using Phony.WPF.Data;
using Phony.WPF.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Phony.WPF.ViewModels
{
    public class ClientsViewModel : BaseViewModelWithAnnotationValidation
    {
        long _clientsId;
        string _name;
        string _site;
        string _email;
        string _phone;
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

        public long ClientId
        {
            get => _clientsId;
            set
            {
                _clientsId = value;
                NotifyOfPropertyChange(() => ClientId);
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                NotifyOfPropertyChange(() => SearchText);
            }
        }

        public string Site
        {
            get => _site;
            set
            {
                _site = value;
                NotifyOfPropertyChange(() => Site);
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                NotifyOfPropertyChange(() => Email);
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                NotifyOfPropertyChange(() => Phone);
            }
        }

        public string Notes
        {
            get => _notes;
            set
            {
                _notes = value;
                NotifyOfPropertyChange(() => Notes);
            }
        }

        public string ChildName
        {
            get => _childName;
            set
            {
                _childName = value;
                NotifyOfPropertyChange(() => ChildName);
            }
        }

        public string ChildPrice
        {
            get => _childPrice;
            set
            {
                _childPrice = value;
                NotifyOfPropertyChange(() => ChildPrice);
            }
        }

        public string ClientCount
        {
            get => _clientsCount;
            set
            {
                _clientsCount = value;
                NotifyOfPropertyChange(() => ClientCount);
            }
        }

        public string ClientCredits
        {
            get => _clientsPurchasePrice;
            set
            {
                _clientsPurchasePrice = value;
                NotifyOfPropertyChange(() => ClientCredits);
            }
        }

        public string ClientDebits
        {
            get => _clientsSalePrice;
            set
            {
                _clientsSalePrice = value;
                NotifyOfPropertyChange(() => ClientDebits);
            }
        }

        public string ClientProfit
        {
            get => _clientsProfit;
            set
            {
                _clientsProfit = value;
                NotifyOfPropertyChange(() => ClientProfit);
            }
        }

        public decimal Balance
        {
            get => _balance;
            set
            {
                _balance = value;
                NotifyOfPropertyChange(() => Balance);
            }
        }

        public bool FastResult
        {
            get => _fastResult;
            set
            {
                _fastResult = value;
                NotifyOfPropertyChange(() => FastResult);
            }
        }

        public bool OpenFastResult
        {
            get => _openFastResult;
            set
            {
                _openFastResult = value;
                NotifyOfPropertyChange(() => OpenFastResult);
            }
        }

        public bool IsAddClientFlyoutOpen
        {
            get => _isAddClientFlyoutOpen;
            set
            {
                _isAddClientFlyoutOpen = value;
                NotifyOfPropertyChange(() => IsAddClientFlyoutOpen);
            }
        }

        public Client DataGridSelectedClient
        {
            get => _dataGridSelectedClient;
            set
            {
                _dataGridSelectedClient = value;
                NotifyOfPropertyChange(() => DataGridSelectedClient);
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

        public ObservableCollection<User> Users { get; set; }

        public ClientsViewModel()
        {
            Title = "العملاء";
            using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
            {
                Clients = new ObservableCollection<Client>(db.GetCollection<Client>(DBCollections.Clients).FindAll());
                Users = new ObservableCollection<User>(db.GetCollection<User>(DBCollections.Users).FindAll());
            }
            DebitCredit();
        }

        async void DebitCredit()
        {
            decimal Debit = decimal.Round(Clients.Where(c => c.Balance > 0).Sum(i => i.Balance), 2);
            decimal Credit = decimal.Round(Clients.Where(c => c.Balance < 0).Sum(i => i.Balance), 2);
            await Task.Run(() =>
            {
                ClientCount = $"مجموع العملاء: {Clients.Count().ToString()}";
            });
            await Task.Run(() =>
            {
                ClientDebits = $"اجمالى لينا: {Math.Abs(Debit).ToString()}";
            });
            await Task.Run(() =>
            {
                ClientCredits = $"اجمالى علينا: {Math.Abs(Credit).ToString()}";
            });
            await Task.Run(() =>
            {
                ClientProfit = $"تقدير لصافى لينا: {(Math.Abs(Debit) - Math.Abs(Credit)).ToString()}";
            });
        }

        private bool CanClientPay()
        {
            return DataGridSelectedClient != null;
        }

        private void DoClientPayAsync()
        {
            var result = MessageBox.MaterialInputBox.Show($"ادخل المبلغ الذى تريد خصمه من حساب العميل {DataGridSelectedClient.Name}", "تدفيع", true);
            if (string.IsNullOrWhiteSpace(result))
            {
                MessageBox.MaterialMessageBox.ShowWarning("لم تقم بادخال اى مبلغ لتدفيعه", "ادخل مبلغ", true);
            }
            else
            {
                bool isvalidmoney = decimal.TryParse(result, out decimal clientpaymentamount);
                if (isvalidmoney)
                {
                    using var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString);
                    var c = db.GetCollection<Client>(DBCollections.Clients).FindById(DataGridSelectedClient.Id);
                    c.Balance -= clientpaymentamount;
                    db.GetCollection<ClientMove>(DBCollections.ClientsMoves).Insert(new ClientMove
                    {
                        Client = db.GetCollection<Client>(DBCollections.Clients).FindById(DataGridSelectedClient.Id),
                        Credit = clientpaymentamount,
                        CreateDate = DateTime.Now,
                        Creator = CurrentUser,
                        EditDate = null,
                        Editor = null
                    });
                    if (clientpaymentamount > 0)
                    {
                        db.GetCollection<TreasuryMove>(DBCollections.TreasuriesMoves).Insert(new TreasuryMove
                        {
                            Treasury = db.GetCollection<Treasury>(DBCollections.Treasuries).FindById(1),
                            Debit = clientpaymentamount,
                            Notes = $"استلام من العميل بكود {DataGridSelectedClient.Id} باسم {DataGridSelectedClient.Name}",
                            CreateDate = DateTime.Now,
                            Creator = CurrentUser,
                            EditDate = null,
                            Editor = null
                        });
                    }
                    MessageBox.MaterialMessageBox.Show($"تم تدفيع {DataGridSelectedClient.Name} مبلغ {clientpaymentamount} جنية بنجاح", "تمت العملية", true);
                    Clients[Clients.IndexOf(DataGridSelectedClient)] = c;
                    DebitCredit();
                    ClientId = 0;
                    DataGridSelectedClient = null;
                }
                else
                {
                    MessageBox.MaterialMessageBox.ShowWarning("ادخل مبلغ صحيح بعلامه عشرية واحدة", "خطاء فى المبلغ", true);
                }
            }
        }

        private bool CanPayClient()
        {
            return DataGridSelectedClient == null ? false : true;
        }

        private void DoPayClientAsync()
        {
            var result = MessageBox.MaterialInputBox.Show($"ادخل المبلغ الذى تريد اضافته لحساب للعميل {DataGridSelectedClient.Name}", "تدفيع", true);
            if (string.IsNullOrWhiteSpace(result))
            {
                MessageBox.MaterialMessageBox.ShowWarning("لم تقم بادخال اى مبلغ لتدفيعه", "ادخل مبلغ", true);
            }
            else
            {
                bool isvalidmoney = decimal.TryParse(result, out decimal clientpaymentamount);
                if (isvalidmoney)
                {
                    using var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString);
                    var c = db.GetCollection<Client>(DBCollections.Clients).FindById(DataGridSelectedClient.Id);
                    c.Balance += clientpaymentamount;
                    db.GetCollection<ClientMove>(DBCollections.ClientsMoves).Insert(new ClientMove
                    {
                        Client = db.GetCollection<Client>(DBCollections.Clients).FindById(DataGridSelectedClient.Id),
                        Debit = clientpaymentamount,
                        CreateDate = DateTime.Now,
                        Creator = CurrentUser,
                        EditDate = null,
                        Editor = null
                    });
                    db.GetCollection<TreasuryMove>(DBCollections.TreasuriesMoves).Insert(new TreasuryMove
                    {
                        Treasury = db.GetCollection<Treasury>(DBCollections.Treasuries).FindById(1),
                        Credit = clientpaymentamount,
                        Notes = $"تسليم المبلغ للعميل بكود {DataGridSelectedClient.Id} باسم {DataGridSelectedClient.Name}",
                        CreateDate = DateTime.Now,
                        Creator = CurrentUser,
                        EditDate = null,
                        Editor = null
                    });
                    MessageBox.MaterialMessageBox.Show($"تم دفع {DataGridSelectedClient.Name} مبلغ {clientpaymentamount} جنية بنجاح", "تمت العملية", true);
                    Clients[Clients.IndexOf(DataGridSelectedClient)] = c;
                    DebitCredit();
                    ClientId = 0;
                    DataGridSelectedClient = null;
                }
                else
                {
                    MessageBox.MaterialMessageBox.ShowWarning("ادخل مبلغ صحيح بعلامه عشرية واحدة", "خطاء فى المبلغ", true);
                }
            }
        }

        private bool CanAddClient()
        {
            return string.IsNullOrWhiteSpace(Name) ? false : true;
        }

        private void DoAddClient()
        {
            using var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString);
            var exist = db.GetCollection<Client>(DBCollections.Clients).Find(x => x.Name == Name).FirstOrDefault();
            if (exist == null)
            {
                var c = new Client
                {
                    Name = Name,
                    Balance = Balance,
                    Site = Site,
                    Email = Email,
                    Phone = Phone,
                    Notes = Notes,
                    CreateDate = DateTime.Now,
                    Creator = CurrentUser,
                    EditDate = null,
                    Editor = null
                };
                db.GetCollection<Client>(DBCollections.Clients).Insert(c);
                Clients.Add(c);
                MessageBox.MaterialMessageBox.Show("تم اضافة العميل بنجاح", "تمت العملية", true);
                DebitCredit();
            }
            else
            {
                MessageBox.MaterialMessageBox.ShowWarning("هناك عميل بنفس الاسم بالفعل", "موجود", true);
            }
        }

        private bool CanEditClient()
        {
            return string.IsNullOrWhiteSpace(Name) || ClientId == 0 || DataGridSelectedClient == null ? false : true;
        }

        private void DoEditClient()
        {
            using var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString);
            var c = db.GetCollection<Client>(DBCollections.Clients).FindById(DataGridSelectedClient.Id);
            c.Name = Name;
            c.Balance = Balance;
            c.Site = Site;
            c.Email = Email;
            c.Phone = Phone;
            c.Notes = Notes;
            c.Editor = CurrentUser;
            c.EditDate = DateTime.Now;
            db.GetCollection<Client>(DBCollections.Clients).Update(c);
            MessageBox.MaterialMessageBox.ShowWarning("تم تعديل العميل بنجاح", "تمت العملية", true);
            Clients[Clients.IndexOf(DataGridSelectedClient)] = c;
            DebitCredit();
            DataGridSelectedClient = null;
            ClientId = 0;
        }

        private bool CanDeleteClient()
        {
            return DataGridSelectedClient == null || DataGridSelectedClient.Id == 1 ? false : true;
        }

        private void DoDeleteClient()
        {
            var result = MessageBox.MaterialMessageBox.ShowWithCancel($"هل انت متاكد من حذف العميل {DataGridSelectedClient.Name}", "حذف الصنف", true);
            if (result == System.Windows.MessageBoxResult.OK)
            {
                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                {
                    db.GetCollection<Client>(DBCollections.Clients).Delete(DataGridSelectedClient.Id);
                    Clients.Remove(DataGridSelectedClient);
                }
                MessageBox.MaterialMessageBox.Show("تم حذف العميل بنجاح", "تمت العملية", true);
                DebitCredit();
                DataGridSelectedClient = null;
            }
        }

        private bool CanSearch()
        {
            return string.IsNullOrWhiteSpace(SearchText) ? false : true;
        }

        private void DoSearch()
        {
            try
            {
                using var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString);
                Clients = new ObservableCollection<Client>(db.GetCollection<Client>(DBCollections.Clients).Find(x => x.Name.Contains(SearchText)));
                if (Clients.Count > 0)
                {
                    if (FastResult)
                    {
                        ChildName = Clients.FirstOrDefault().Name;
                        ChildPrice = Clients.FirstOrDefault().Balance.ToString();
                        OpenFastResult = true;
                    }
                }
                else
                {
                    MessageBox.MaterialMessageBox.ShowWarning("لم يتم العثور على شئ", "غير موجود", true);
                }
            }
            catch (Exception ex)
            {
                Core.SaveException(ex);
                MessageBox.MaterialMessageBox.ShowError("لم يستطع ايجاد ما تبحث عنه تاكد من صحه البيانات المدخله", "خطأ", true);
            }
        }

        private bool CanReloadAllClients()
        {
            return true;
        }

        private void DoReloadAllClients()
        {
            using var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString);
            Clients = new ObservableCollection<Client>(db.GetCollection<Client>(DBCollections.Clients).FindAll());
        }

        private bool CanFillUI()
        {
            return DataGridSelectedClient == null ? false : true;
        }

        private void DoFillUI()
        {
            ClientId = DataGridSelectedClient.Id;
            Name = DataGridSelectedClient.Name;
            Balance = DataGridSelectedClient.Balance;
            Site = DataGridSelectedClient.Site;
            Email = DataGridSelectedClient.Email;
            Phone = DataGridSelectedClient.Phone;
            Notes = DataGridSelectedClient.Notes;
            IsAddClientFlyoutOpen = true;
        }

        private bool CanOpenAddClientFlyout()
        {
            return true;

        }

        private void DoOpenAddClientFlyout()
        {
            IsAddClientFlyoutOpen = IsAddClientFlyoutOpen ? false : true;
        }
    }
}
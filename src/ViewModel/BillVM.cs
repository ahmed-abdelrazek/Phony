using Phony.Kernel;
using Phony.Model;
using Phony.Persistence;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Phony.ViewModel
{

    public class BillVM : CommonBase
    {
        string _currentBillNo;
        Client _selectedClient;

        public string CurrentBillNo
        {
            get => _currentBillNo;
            set
            {
                if (value != _currentBillNo)
                {
                    _currentBillNo = value;
                    RaisePropertyChanged();
                }
            }
        }

        public Client SelectedClient
        {
            get => _selectedClient;
            set
            {
                if (value != _selectedClient)
                {
                    _selectedClient = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<Client> Clients { get; set; }

        public ObservableCollection<User> Users { get; set; }

        public BillVM()
        {
            using (var db = new PhonyDbContext())
            {
                Clients = new ObservableCollection<Client>(db.Clients);
                Users = new ObservableCollection<User>(db.Users);
            }
            CurrentBillNo = NewBillNo();
        }

        string NewBillNo()
        {
            try
            {
                using (var db = new PhonyDbContext())
                {
                    return $"رقم الفاتورة الحاليه: {db.Bills.OrderByDescending(p => p.Id).FirstOrDefault().Id + 1}";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return $"رقم الفاتورة الحاليه: 1";
            }
        }
    }
}
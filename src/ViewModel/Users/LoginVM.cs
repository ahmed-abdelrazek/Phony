using Phony.Kernel;
using Phony.Utility;
using System;
using System.Windows.Input;

namespace Phony.ViewModel.Users
{
    public class LoginVM : CommonBase
    {
        string _name;
        string _pass;

        public string Name
        {
            get { return _name; }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    RaisePropertyChanged(nameof(Name));
                }
            }
        }

        public string Pass
        {
            get { return _pass; }
            set
            {
                if (value != _pass)
                {
                    _pass = value;
                    RaisePropertyChanged(nameof(Pass));
                }
            }
        }

        public ICommand LogIn { get; set; }

        public LoginVM()
        {
            LoadCommands();
        }

        private void LoadCommands()
        {
            LogIn = new CustomCommand(DoLogIn, CanDoLogIn);
        }

        private void DoLogIn(object obj)
        {
            try
            {
                //ToDo add logins
                MainWindowVM.PageName = "Main";
            }
            catch (Exception ex)
            {
                Core.SaveException(ex);
            }
        }

        private bool CanDoLogIn(object obj)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return false;
            }
            return true;
        }
    }
}
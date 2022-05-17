using App1.Database;
using App1.Interfaces;
using App1.Services;
using App1.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class StartViewModel : BaseViewModel
    {
        private bool _isIncorrect;

        private string _login;
        private string _password;

        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged(nameof(Login));
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public bool IsIncorrect 
        {
            get => _isIncorrect;
            set {
                _isIncorrect = value;
                OnPropertyChanged(nameof(IsIncorrect)); 
            }
        }

        public ICommand StartGameCommand { get; }

        public StartViewModel()
        {
            StartGameCommand = new Command(OnStartGameClicked);
        }

        private async void OnStartGameClicked(object obj)
        {
            try
            {
                if (AuthenticateUser().Result)
                {
                    MainDataStore.Username = Login;
                    ClearEntries();
                    await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
                }
                else
                {
                    ShowIncorrectDataError();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                return;
            }
        }

        private void ClearEntries()
        {
            Login = "";
            Password = "";
        }

        private async Task<bool> AuthenticateUser()
        {
            try
            {
                string dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.DBFILENAME);
                using (var context = new ApplicationContext(dbPath))
                {
                    var user = await context.Users.FirstOrDefaultAsync(u => u.Username.Equals(Login) && u.Password.Equals(Password));
                    if (user is null)
                    {
                        return false;
                    }
                    IsIncorrect = false;
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                IsIncorrect = false;

                return false;
            }
        }

        private void ShowIncorrectDataError()
        {
            IsIncorrect = true;
        }
    }
}

using App1.Database;
using App1.Interfaces;
using App1.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        private string _profileName;
        private string _newLogin;
        private string _newPassword;
        private string _confirmPassword;

        public string Username
        {
            get => _profileName;
            set
            {
                _profileName = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        public string NewLogin
        {
            get => _newLogin;
            set
            {
                _newLogin = value;
                OnPropertyChanged(nameof(NewLogin));
            }
        }
        public string NewPassword
        {
            get => _newPassword;
            set
            {
                _newPassword = value;
                OnPropertyChanged(nameof(NewPassword));
            }
        }
        public string ConfirmNewPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmNewPassword));
            }
        }

        public ICommand SaveNewDataCommand { get; }

        public ProfileViewModel()
        {
            Username = MainDataStore.Username;
            SaveNewDataCommand = new Command(SaveUserData);
        }

        private async void SaveUserData(object obj)
        {
            if (string.IsNullOrWhiteSpace(NewLogin))
            {
                if (string.IsNullOrWhiteSpace(NewPassword))
                {
                    return;
                }
            }
            if (!string.IsNullOrWhiteSpace(NewPassword))
            {
                if (!ConfirmNewPassword.Equals(NewPassword))
                {
                    return;
                }
            }

            await SaveToDatabase();
            ClearEntries();
        }

        private async Task<bool> SaveToDatabase()
        {
            string dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.DBFILENAME);
            using (var context = new ApplicationContext(dbPath))
            {
                var user = await context.Users.FirstOrDefaultAsync(u => u.Username.Equals(Username));
                if (user is null)
                {
                    return false;
                }

                if (!string.IsNullOrWhiteSpace(NewLogin))
                {
                    user.Username = NewLogin;
                    Username = NewLogin;
                }
                if (!string.IsNullOrWhiteSpace(NewPassword))
                {
                    user.Password = NewPassword;
                }

                context.Users.Update(user);
                await context.SaveChangesAsync();

                return true;
            }
        }

        private void ClearEntries()
        {
            NewLogin = "";
            NewPassword = "";
            ConfirmNewPassword = "";
        }

    }
}

using App1.Database;
using App1.Interfaces;
using App1.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        private bool _isIncorrect;

        private string _profileName;
        private string _newLogin;
        private string _newPassword;
        private string _confirmPassword;
        private string _bestScore = "None";

        public string BestScore
        {
            get => _bestScore;
            set
            {
                _bestScore = value;
                OnPropertyChanged(nameof(BestScore));
            }
        }

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

        public bool IsIncorrect
        {
            get => _isIncorrect;
            set
            {
                _isIncorrect = value;
                OnPropertyChanged(nameof(IsIncorrect));
            }
        }

        public ICommand SaveNewDataCommand { get; }

        public ProfileViewModel()
        {
            Username = MainDataStore.Username;
            CalculateBestScore();
            SaveNewDataCommand = new Command(SaveUserData);
        }

        private async void SaveUserData(object obj)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NewLogin))
                {
                    if (string.IsNullOrWhiteSpace(NewPassword))
                    {
                        IsIncorrect = true;
                        return;
                    }
                }
                if (!string.IsNullOrWhiteSpace(NewPassword))
                {
                    if (!ConfirmNewPassword.Equals(NewPassword))
                    {
                        IsIncorrect = true;
                        return;
                    }
                }

                await SaveToDatabase();
                ClearEntries();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                IsIncorrect = true;

                return;
            }
        }

        private async void CalculateBestScore()
        {
            try
            {
                string dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.DBFILENAME);
                using (var context = new ApplicationContext(dbPath))
                {
                    _bestScore = (await context.Results
                        .Include(r => r.User)
                        .Where(r => r.User.Username.Equals(Username))
                        .MinAsync(r => r.Steps)).ToString();
                    if (int.Parse(_bestScore) == 0)
                    {
                        _bestScore = "None";
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                return;
            }
        }

        private async Task<bool> SaveToDatabase()
        {
            try
            {
                string dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.DBFILENAME);
                using (var context = new ApplicationContext(dbPath))
                {
                    var user = await context.Users.FirstOrDefaultAsync(u => u.Username.Equals(Username));
                    if (user is null)
                    {
                        IsIncorrect = true;
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
                    IsIncorrect = false;

                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                IsIncorrect = true;

                return false;
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

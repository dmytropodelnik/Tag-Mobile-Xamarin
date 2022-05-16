using App1.Database;
using App1.Interfaces;
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
        public string Username { get; set; }
        public string NewLogin { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }

        public ICommand SaveNewDataCommand { get; }

        public ProfileViewModel()
        {
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

    }
}

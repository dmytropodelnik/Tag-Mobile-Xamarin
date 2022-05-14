using App1.Database;
using App1.Interfaces;
using App1.Services;
using App1.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class StartViewModel : BaseViewModel
    {
        private ApplicationContext _context;

        public string Login { get; set; }
        public string Password { get; set; }
        public bool IsIncorrect { get; set; } = false;

        public ICommand StartGameCommand { get; }

        public StartViewModel()
        {
            StartGameCommand = new Command(OnStartGameClicked);
        }

        private async void OnStartGameClicked(object obj)
        {
            if (AuthenticateUser().Result)
            {
                MainDataStore.Username = Login;
                await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
            }
            else
            {
                ShowIncorrectDataError();
            }
        }

        private async Task<bool> AuthenticateUser()
        {
            string dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.DBFILENAME);
            using (var db = new ApplicationContext(dbPath))
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username.Equals(Login) && u.Password.Equals(Password));
                if (user is null)
                {
                    return false;
                }
                return true;
            }
        }

        private void ShowIncorrectDataError()
        {
            IsIncorrect = true;
        }
    }
}

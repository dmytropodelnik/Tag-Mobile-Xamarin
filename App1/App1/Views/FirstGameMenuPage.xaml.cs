using App1.Database;
using App1.Interfaces;
using App1.ViewModels;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;

namespace App1.Views
{
    public partial class FirstGameMenuPage : ContentPage
    {
        public FirstGameMenuPage()
        {
            InitializeComponent();
            BindingContext = new FirstGameMenuViewModel();
        }
        protected override void OnAppearing()
        {
            string dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.DBFILENAME);
            using (ApplicationContext db = new ApplicationContext(dbPath))
            {
                //friendsList.ItemsSource = db.Users.ToList();
            }
            base.OnAppearing();
        }
    }
}
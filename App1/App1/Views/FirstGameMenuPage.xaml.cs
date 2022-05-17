using App1.Database;
using App1.Interfaces;
using App1.Services;
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
        protected async override void OnAppearing()
        {
            if (string.IsNullOrEmpty(MainDataStore.Username))
            {
                await Shell.Current.GoToAsync("//StartPage");
            }

            base.OnAppearing();
        }
    }
}
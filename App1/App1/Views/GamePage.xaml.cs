using App1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GamePage : ContentPage
    {
        public GamePage()
        {
            InitializeComponent();
            BindingContext = GameViewModel.Create(playField);
        }

        protected override void OnAppearing()
        {
            if (!GameViewModel.IsInitialized)
            {
                GameViewModel.FillGrid();
            }

            base.OnAppearing();
        }
    }
}
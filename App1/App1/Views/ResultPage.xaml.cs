using App1.Services;
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
    public partial class ResultPage : ContentPage
    {
        public ResultPage()
        {
            InitializeComponent();
            this.BindingContext = ResultViewModel.Create();
        }

        protected override void OnAppearing()
        {
            ResultViewModel.Create().ResultScore = MainDataStore.PreviousScore;

            base.OnAppearing();
        }
    }
}
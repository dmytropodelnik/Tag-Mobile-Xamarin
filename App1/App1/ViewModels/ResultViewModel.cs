using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class ResultViewModel : BaseViewModel
    {
        public ICommand ContinueCommand { get; }
        public ResultViewModel()
        {
            ContinueCommand = new Command(OnContinueClicked);
        }

        private async void OnContinueClicked(object obj)
        {
            await Shell.Current.GoToAsync("//FirstGameMenuPage");
        }
    }
}

using App1.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class StartViewModel : BaseViewModel
    {
        public Command StartGameCommand { get; }

        public StartViewModel()
        {
            StartGameCommand = new Command(OnStartGameClicked);
        }

        private async void OnStartGameClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"{nameof(FirstGameMenuPage)}");
        }
    }
}

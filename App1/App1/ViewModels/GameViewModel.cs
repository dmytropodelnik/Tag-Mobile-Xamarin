using App1.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class GameViewModel : BaseViewModel
    {
        public ICommand StartGameCommand { get; }

        public GameViewModel()
        {
            StartGameCommand = new Command(OnStartGameClicked);
        }

        private async void OnStartGameClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            //await Shell.Current.GoToAsync($"//{nameof(FirstGameMenuPage)}");
            // await Shell.Current.GoToAsync($"//{nameof(GamePage)}");

        }
    }
}

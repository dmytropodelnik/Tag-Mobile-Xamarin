using App1.Models;
using App1.Views;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class FirstGameMenuViewModel : BaseViewModel
    {
        private string _rules = "After the “tags” are shuffled randomly, you can start the game.\n To move the \"stone\" from the \"tags\" to an empty cell, you need to \"click\" on it with the mouse.\n You can move in this way only the “stone”, which is located to the left / right or top / bottom of an empty cell.\n The goal of the game of fifteen is to collect all the numbers in order from 1 to 15 in this way, as quickly as possible and in the least number of moves.;";

        private string description = "test";

        public ICommand StartGameCommand { get; }

        public FirstGameMenuViewModel()
        {
            StartGameCommand = new Command(OnStartGameClicked);
        }

        private async void OnStartGameClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            //await Shell.Current.GoToAsync($"//{nameof(FirstGameMenuPage)}");
            await Shell.Current.GoToAsync($"//{nameof(GamePage)}");

        }

        public string Rules
        {
            get => _rules;
            set => SetProperty(ref _rules, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }
    }
}

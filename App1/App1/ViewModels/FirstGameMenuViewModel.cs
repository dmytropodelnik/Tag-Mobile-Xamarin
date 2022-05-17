using App1.Database;
using App1.Interfaces;
using App1.Models;
using App1.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class FirstGameMenuViewModel : BaseViewModel
    {
        private string _rules = "After the “tags” are shuffled randomly, you can start the game.\n To move the \"stone\" from the \"tags\" to an empty cell, you need to \"click\" on it with the mouse.\n You can move in this way only the “stone”, which is located to the left / right or top / bottom of an empty cell.\n The goal of the game of fifteen is to collect all the numbers in order from 1 to 15 in this way, as quickly as possible and in the least number of moves.;";

        private string description = "test";
        private string _bestScore = "None";

        public ICommand StartGameCommand { get; }

        public string BestScore
        {
            get => _bestScore;
            set
            {
                _bestScore = value;
                OnPropertyChanged(nameof(BestScore));
            }
        }

        public FirstGameMenuViewModel()
        {
            CalculateBestScore();
            StartGameCommand = new Command(OnStartGameClicked);
        }

        private async void CalculateBestScore()
        {
            try
            {
                string dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.DBFILENAME);
                using (var context = new ApplicationContext(dbPath))
                {
                    _bestScore = (await context.Results.MinAsync(r => r.Steps)).ToString();
                    if (int.Parse(_bestScore) == 0)
                    {
                        _bestScore = "None";
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                return;
            }
        }

        private async void OnStartGameClicked(object obj)
        {
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

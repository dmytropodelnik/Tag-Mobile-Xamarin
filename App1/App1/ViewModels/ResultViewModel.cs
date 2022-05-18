using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class ResultViewModel : BaseViewModel
    {
        private static ResultViewModel _instance = null;

        private string _resultScore = "0";
        public string ResultScore
        {
            get => _resultScore;
            set
            {
                _resultScore = value;
                OnPropertyChanged(nameof(ResultScore));
            }
        }

        public ICommand ContinueCommand { get; }

        public static ResultViewModel Create()
        {
            if (_instance is null)
            {
                _instance = new ResultViewModel();
            }
            return _instance;
        }

        private ResultViewModel()
        {
            ContinueCommand = new Command(OnContinueClicked);
        }

        private async void OnContinueClicked(object obj)
        {
            await Shell.Current.GoToAsync("//FirstGameMenuPage");
        }
    }
}

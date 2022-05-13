using App1.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class FirstGameMenuViewModel : BaseViewModel
    {
        private string _rules = "After the “tags” are shuffled randomly, you can start the game.\n To move the \"stone\" from the \"tags\" to an empty cell, you need to \"click\" on it with the mouse. You can move in this way only the “stone”, which is located to the left / right or top / bottom of an empty cell. The goal of the game of fifteen is to collect all the numbers in order from 1 to 15 in this way, as quickly as possible and in the least number of moves.;";

        private string description = "test";
        public string Id { get; set; }

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

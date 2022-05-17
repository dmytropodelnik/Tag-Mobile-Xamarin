using App1.Models;
using App1.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class GameViewModel : BaseViewModel
    {
        private static GameViewModel _instance = null;

        private static CubeInfo[,] _cubesInfo = new CubeInfo[4, 4];

        private static string _currentScore = "0";

        private const int _ROWS_AMOUNT = 4;
        private const int _COLUMNS_AMOUNT = 4;

        private static Random _random = new Random();
        private static Grid _playField;

        #region INotifyPropertyChanged
        public new static event PropertyChangedEventHandler PropertyChanged;
        protected new static void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(Create(_playField), new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private static ObservableCollection<Button> _playGrid { get; set; } = new ObservableCollection<Button>();
        public static ObservableCollection<Button> PlayGrid
        {
            get => _playGrid;
            set => _playGrid = value;
        }

        public static string CurrentScore
        {
            get => _currentScore;
            set
            {
                _currentScore = value;
                OnPropertyChanged(nameof(CurrentScore));
            }
        }

        public ICommand GameOverCommand { get; }
        public ICommand PressButtonCommand { get; }

        public static GameViewModel Create(Grid playField)
        {
            if (_instance is null)
            {
                _instance = new GameViewModel(playField);
            }
            else
            {
                FillGrid();
            }
            return _instance;
        }

        private GameViewModel(Grid playField)
        {
            _playField = playField;
            FillGrid();
            GameOverCommand = new Command(OnGameOverClicked);
        }

        private static void FillGrid()
        {
            try
            {
                bool check = false;
                int randomValue = _random.Next(0, 15);
                int counter = 0;
                for (int i = 0; i < _ROWS_AMOUNT; i++)
                {
                    for (int j = 0; j < _COLUMNS_AMOUNT; j++)
                    {
                        _cubesInfo[i, j] = new CubeInfo();

                        Button newCube = new Button();

                        if (counter == randomValue && !check)
                        {
                            check = true;
                            newCube.BackgroundColor = Color.Transparent;
                        }
                        else
                        {
                            newCube.Text = (++counter).ToString();
                            newCube.FontSize = 35;
                            newCube.TextColor = Color.Black;
                            newCube.Clicked += ShiftCube;

                            _cubesInfo[i, j].Text = newCube.Text;
                        }
                        _playGrid.Add(newCube);
                    }
                }

                MixCollection(_playGrid);
                AddCubesToGrid();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private static void AddCubesToGrid()
        {
            try
            {
                int counter = 0;
                for (int i = 0; i < _ROWS_AMOUNT; i++)
                {
                    for (int j = 0; j < _COLUMNS_AMOUNT; j++)
                    {
                        _cubesInfo[i, j].Y = i;
                        _cubesInfo[i, j].X = j;

                        if (!string.IsNullOrWhiteSpace(_cubesInfo[i, j].Text))
                        {
                            _cubesInfo[i, j].IsFree = false;
                        }
                        _playField.Children.Add(_playGrid[counter++], j, i);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private static void RefreshCubes()
        {
            try
            {
                _playField.Children.Clear();

                int counter = 0;
                for (int i = 0; i < _ROWS_AMOUNT; i++)
                {
                    for (int j = 0; j < _COLUMNS_AMOUNT; j++)
                    {
                        _playField.Children.Add(_playGrid[counter++], j, i);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private static void MixCollection(ObservableCollection<Button> collection)
        {
            try
            {
                for (int i = collection.Count - 1; i >= 1; i--)
                {
                    int j = _random.Next(i + 1);
                    // wrap values data[j] and data[i]
                    var temp = collection[j];
                    collection[j] = collection[i];
                    collection[i] = temp;

                    var tempInfo = _cubesInfo[j / _COLUMNS_AMOUNT, j % _COLUMNS_AMOUNT];
                    _cubesInfo[j / _COLUMNS_AMOUNT, j % _COLUMNS_AMOUNT] = _cubesInfo[i / _COLUMNS_AMOUNT, i % _COLUMNS_AMOUNT];
                    _cubesInfo[i / _COLUMNS_AMOUNT, i % _COLUMNS_AMOUNT] = tempInfo;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private static void ShiftCube(object sender, EventArgs e)
        {
            try
            {
                // explicit cast from RoutedEventArgs to Button
                Button button = (Button)sender;

                for (int i = 0; i < _ROWS_AMOUNT; i++)
                {
                    for (int j = 0; j < _COLUMNS_AMOUNT; j++)
                    {
                        if (_cubesInfo[i, j].Text == button.Text)
                        {
                            if (i == 0)
                            {
                                if (j == 0)
                                {
                                    if (_cubesInfo[i, j + 1].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i, j + 1], (i, j), (i, j + 1));
                                    }
                                    else if (_cubesInfo[i + 1, j].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i + 1, j], (i, j), (i + 1, j));
                                    }
                                }
                                else if (j == 3)
                                {
                                    if (_cubesInfo[i, j - 1].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i, j - 1], (i, j), (i, j - 1));
                                    }
                                    else if (_cubesInfo[i + 1, j].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i + 1, j], (i, j), (i + 1, j));
                                    }
                                }
                                else
                                {
                                    if (_cubesInfo[i + 1, j].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i + 1, j], (i, j), (i + 1, j));
                                    }
                                    else if (_cubesInfo[i, j + 1].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i, j + 1], (i, j), (i, j + 1));
                                    }
                                    else if (_cubesInfo[i, j - 1].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i, j - 1], (i, j), (i, j - 1));
                                    }
                                }
                            }
                            else if (i == 3)
                            {
                                if (j == 0)
                                {
                                    if (_cubesInfo[i, j + 1].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i, j + 1], (i, j), (i, j + 1));
                                    }
                                    else if (_cubesInfo[i - 1, j].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i - 1, j], (i, j), (i - 1, j));
                                    }
                                }
                                else if (j == 3)
                                {
                                    if (_cubesInfo[i, j - 1].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i, j - 1], (i, j), (i, j - 1));
                                    }
                                    else if (_cubesInfo[i - 1, j].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i - 1, j], (i, j), (i - 1, j));
                                    }
                                }
                                else
                                {
                                    if (_cubesInfo[i - 1, j].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i - 1, j], (i, j), (i - 1, j));
                                    }
                                    else if (_cubesInfo[i, j + 1].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i, j + 1], (i, j), (i, j + 1));
                                    }
                                    else if (_cubesInfo[i, j - 1].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i, j - 1], (i, j), (i, j - 1));
                                    }
                                }
                            }
                            else if (j == 0)
                            {
                                if (i == 0)
                                {
                                    if (_cubesInfo[i, j + 1].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i, j + 1], (i, j), (i, j + 1));
                                    }
                                    else if (_cubesInfo[i + 1, j].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i + 1, j], (i, j), (i + 1, j));
                                    }
                                }
                                else if (i == 3)
                                {
                                    if (_cubesInfo[i, j + 1].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i, j + 1], (i, j), (i, j + 1));
                                    }
                                    else if (_cubesInfo[i - 1, j].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i - 1, j], (i, j), (i - 1, j));
                                    }
                                }
                                else
                                {
                                    if (_cubesInfo[i, j + 1].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i, j + 1], (i, j), (i, j + 1));
                                    }
                                    else if (_cubesInfo[i + 1, j].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i + 1, j], (i, j), (i + 1, j));
                                    }
                                    else if (_cubesInfo[i - 1, j].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i - 1, j], (i, j), (i - 1, j));
                                    }
                                }
                            }
                            else if (j == 3)
                            {
                                if (i == 0)
                                {
                                    if (_cubesInfo[i, j - 1].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i, j - 1], (i, j), (i, j - 1));
                                    }
                                    else if (_cubesInfo[i + 1, j].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i + 1, j], (i, j), (i + 1, j));
                                    }
                                }
                                else if (i == 3)
                                {
                                    if (_cubesInfo[i, j - 1].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i, j - 1], (i, j), (i, j - 1));
                                    }
                                    else if (_cubesInfo[i - 1, j].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i - 1, j], (i, j), (i - 1, j));
                                    }
                                }
                                else
                                {
                                    if (_cubesInfo[i - 1, j].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i - 1, j], (i, j), (i - 1, j));
                                    }
                                    else if (_cubesInfo[i + 1, j].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i + 1, j], (i, j), (i + 1, j));
                                    }
                                    else if (_cubesInfo[i, j - 1].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i, j - 1], (i, j), (i, j - 1));
                                    }
                                }
                            }
                            else
                            {
                                if (_cubesInfo[i - 1, j].IsFree)
                                {
                                    WrapCubes(_cubesInfo[i - 1, j], (i, j), (i - 1, j));
                                }
                                else if (_cubesInfo[i + 1, j].IsFree)
                                {
                                    WrapCubes(_cubesInfo[i + 1, j], (i, j), (i + 1, j));
                                }
                                else if (_cubesInfo[i, j - 1].IsFree)
                                {
                                    WrapCubes(_cubesInfo[i, j - 1], (i, j), (i, j - 1));
                                }
                                else if (_cubesInfo[i, j + 1].IsFree)
                                {
                                    WrapCubes(_cubesInfo[i, j + 1], (i, j), (i, j + 1));
                                }
                            }
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private static void WrapCubes(CubeInfo to, (int, int) from, (int, int) toCoords)
        {
            _cubesInfo[toCoords.Item1, toCoords.Item2] = _cubesInfo[from.Item1, from.Item2];
            _cubesInfo[from.Item1, from.Item2] = new CubeInfo();

            Button tempCube = _playGrid[_COLUMNS_AMOUNT * from.Item1 + from.Item2];
            _playGrid.RemoveAt(_COLUMNS_AMOUNT * from.Item1 + from.Item2);
            _playGrid.Insert(_COLUMNS_AMOUNT * toCoords.Item1 + toCoords.Item2, tempCube);

            //_playField.Children.RemoveAt(_COLUMNS_AMOUNT * from.Item1 + from.Item2);
            //_playField.Children.Insert(_COLUMNS_AMOUNT * toCoords.Item1 + toCoords.Item2, tempCube);

            RefreshCubes();

            int previousScore = int.Parse(_currentScore);
            CurrentScore = (++previousScore).ToString();
        }

        private async void OnGameOverClicked(object obj)
        {
            ResetData();
            await Shell.Current.GoToAsync("//FirstGameMenuPage");
        }

        private void ResetData()
        {
            _cubesInfo = new CubeInfo[_ROWS_AMOUNT, _COLUMNS_AMOUNT];
            _playField.Children.Clear();
            _playGrid.Clear();
            CurrentScore = "0";
        }

        private void SaveResult()
        {

        }
    }
}

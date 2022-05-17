using App1.Models;
using App1.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class GameViewModel : BaseViewModel
    {
        private CubeInfo[,] _cubesInfo = new CubeInfo[4, 4];

        private string _currentScore = "0";

        private Random _random = new Random();
        private Grid _playField;

        private ObservableCollection<Button> _playGrid { get; set; } = new ObservableCollection<Button>();
        public ObservableCollection<Button> PlayGrid
        {
            get => _playGrid;
            set => _playGrid = value;
        }

        public string CurrentScore
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

        public GameViewModel(Grid playField)
        {
            _playField = playField;
            FillGrid();
            GameOverCommand = new Command(OnGameOverClicked);
        }

        private void FillGrid()
        {
            try
            {
                int randomValue = _random.Next(0, 15);
                int counter = 0;
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        _cubesInfo[i, j] = new CubeInfo();

                        if (counter == randomValue)
                        {
                            counter++;
                            continue;
                        }
                        Button newCube = new Button();
                        newCube.Text = (++counter).ToString();
                        newCube.FontSize = 35;
                        newCube.TextColor = Color.Black;
                        newCube.Clicked += ShiftCube;

                        _cubesInfo[i, j].Text = newCube.Text;

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

        private void AddCubesToGrid()
        {
            try
            {
                int counter = 0;
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (string.IsNullOrWhiteSpace(_cubesInfo[i, j].Text))
                        {
                            continue;
                        }
                        _cubesInfo[i, j].IsFree = false;
                        _cubesInfo[i, j].Y = i;
                        _cubesInfo[i, j].X = j;
                        _playField.Children.Add(_playGrid[counter++], i, j);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void RefreshCubes()
        {
            try
            {
                _playField.Children.Clear();

                int counter = 0;
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (string.IsNullOrWhiteSpace(_cubesInfo[i, j].Text))
                        {
                            continue;
                        }
                        _playField.Children.Add(_playGrid[counter++], i, j);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void MixCollection(ObservableCollection<Button> collection)
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
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void ShiftCube(object sender, EventArgs e)
        {
            try
            {
                // explicit cast from RoutedEventArgs to Button
                Button button = (Button)sender;

                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (_cubesInfo[i, j].Text == button.Text)
                        {
                            if (i == 0)
                            {
                                if (j == 0)
                                {
                                    if (_cubesInfo[i, j + 1].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i, j + 1], (i, j));
                                    }
                                    else if (_cubesInfo[i + 1, j].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i + 1, j], (i, j));
                                    }
                                }
                                else if (j == 3)
                                {
                                    if (_cubesInfo[i, j - 1].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i, j - 1], (i, j));
                                    }
                                    else if (_cubesInfo[i + 1, j].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i + 1, j], (i, j));
                                    }
                                }
                                else
                                {
                                    if (_cubesInfo[i + 1, j].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i + 1, j], (i, j));
                                    }
                                    else if (_cubesInfo[i, j + 1].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i, j + 1], (i, j));
                                    }
                                    else if (_cubesInfo[i, j - 1].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i, j - 1], (i, j));
                                    }
                                }
                            }
                            else if (i == 3)
                            {
                                if (j == 0)
                                {
                                    if (_cubesInfo[i, j + 1].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i, j + 1], (i, j));
                                    }
                                    else if (_cubesInfo[i - 1, j].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i - 1, j], (i, j));
                                    }
                                }
                                else if (j == 3)
                                {
                                    if (_cubesInfo[i, j - 1].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i, j - 1], (i, j));
                                    }
                                    else if (_cubesInfo[i - 1, j].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i - 1, j], (i, j));
                                    }
                                }
                                else
                                {
                                    if (_cubesInfo[i - 1, j].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i - 1, j], (i, j));
                                    }
                                    else if (_cubesInfo[i, j + 1].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i, j + 1], (i, j));
                                    }
                                    else if (_cubesInfo[i, j - 1].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i, j - 1], (i, j));
                                    }
                                }
                            }
                            else if (j == 0)
                            {
                                if (i == 0)
                                {
                                    if (_cubesInfo[i, j + 1].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i, j + 1], (i, j));
                                    }
                                    else if (_cubesInfo[i + 1, j].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i + 1, j], (i, j));
                                    }
                                }
                                else if (i == 3)
                                {
                                    if (_cubesInfo[i, j + 1].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i, j + 1], (i, j));
                                    }
                                    else if (_cubesInfo[i - 1, j].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i - 1, j], (i, j));
                                    }
                                }
                                else
                                {
                                    if (_cubesInfo[i, j - 1].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i, j - 1], (i, j));
                                    }
                                    else if (_cubesInfo[i, j - 1].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i, j - 1], (i, j));
                                    }
                                    else if (_cubesInfo[i, j - 1].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i, j - 1], (i, j));
                                    }
                                }
                            }
                            else if (j == 3)
                            {
                                if (i == 0)
                                {
                                    if (_cubesInfo[i, j - 1].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i, j - 1], (i, j));
                                    }
                                    else if (_cubesInfo[i + 1, j].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i + 1, j], (i, j));
                                    }
                                }
                                else if (i == 3)
                                {
                                    if (_cubesInfo[i, j - 1].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i, j - 1], (i, j));
                                    }
                                    else if (_cubesInfo[i - 1, j].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i - 1, j], (i, j));
                                    }
                                }
                                else
                                {
                                    if (_cubesInfo[i - 1, j].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i - 1, j], (i, j));
                                    }
                                    else if (_cubesInfo[i + 1, j].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i + 1, j], (i, j));
                                    }
                                    else if (_cubesInfo[i, j - 1].IsFree)
                                    {
                                        WrapCubes(_cubesInfo[i, j - 1], (i, j));
                                    }
                                }
                            }
                            else
                            {
                                if (_cubesInfo[i - 1, j].IsFree)
                                {
                                    WrapCubes(_cubesInfo[i - 1, j], (i, j));
                                }
                                else if (_cubesInfo[i + 1, j].IsFree)
                                {
                                    WrapCubes(_cubesInfo[i + 1, j], (i, j));
                                }
                                else if (_cubesInfo[i, j - 1].IsFree)
                                {
                                    WrapCubes(_cubesInfo[i, j - 1], (i, j));
                                }
                                else if (_cubesInfo[i, j + 1].IsFree)
                                {
                                    WrapCubes(_cubesInfo[i, j + 1], (i, j));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void WrapCubes(CubeInfo to, (int, int) from)
        {
            _cubesInfo[to.X, to.Y].IsFree = false;
            _cubesInfo[from.Item1, from.Item2].IsFree = true;

            // CubeInfo tempCube = _cubesInfo[from.Item1, from.Item2];
            _cubesInfo[to.X, to.Y] = _cubesInfo[from.Item1, from.Item2];
            _cubesInfo[from.Item1, from.Item2] = new CubeInfo();

            RefreshCubes();
        }

        private async void OnGameOverClicked(object obj)
        {
            ResetData();
        }

        private void ResetData()
        {
            _cubesInfo = new CubeInfo[4, 4];
            _playField.Children.Clear();
            _playGrid.Clear();
        }

        private void SaveResult()
        {

        }
    }
}

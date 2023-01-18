using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Channels;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using kursa4.Properties;
using Label = System.Windows.Controls.Label;

namespace kursa4
{
    public partial class MainWindow
    {
        private Border[,] cells;
        private Border[,] killArr;
        private Border[,] killArr2;
        private int[,] compships = new int[10,10];
        private string nameOfPlayer;
        private Window InputName;
        private TextBox Box;

        public MainWindow()
        {
            cells = new Border[10, 10];
            game_cells = new Button[10, 10];
            killArr = new Border[10, 10];
            killArr2 = new Border[10, 10];
            ListRecords = new List<RecordsData>();
            InitializeComponent();
        }

        private void FillImageSource()
        {
            ImageBrush myBrush = new ImageBrush();
            myBrush.ImageSource = (new BitmapImage(new Uri(@"C:\\Users\\halop\\RiderProjects\\kursa4\\kursa4\\Kill.png")));
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    killArr[i, j] = new Border();
                    killArr[i, j].Background = myBrush;
                    killArr[i, j].Visibility = Visibility.Hidden;
                    killArr[i, j].Height = 60;
                    killArr[i, j].Width = 60;
                    Canvas.SetLeft(killArr[i, j], 0 + j * 60);
                    Canvas.SetTop(killArr[i, j], 0 + i * 60);
                    KillField.Children.Add(killArr[i, j]);
                    cells[i, j].Name = "KillArr_" + i.ToString() + j.ToString();
                    
                    
                    killArr2[i, j] = new Border();
                    killArr2[i, j].Background = myBrush;
                    killArr2[i, j].Visibility = Visibility.Hidden;
                    killArr2[i, j].Height = 60;
                    killArr2[i, j].Width = 60;
                    Canvas.SetLeft(killArr2[i, j], 0 + j * 60);
                    Canvas.SetTop(killArr2[i, j], 0 + i * 60);
                    KillField2.Children.Add(killArr2[i, j]);
                    cells[i, j].Name = "KillArr_" + i.ToString() + j.ToString();
                }
            }
        }
        
        private bool startGame = false;
        
        private void ButtonPlay_OnClick(object sender, RoutedEventArgs e)
        {
            InputName = new Window();
            InputName.Owner = window;
            InputName.Height = 150;
            InputName.Width = 300;

            InputName.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            InputName.ResizeMode = ResizeMode.NoResize;
            InputName.WindowStyle = WindowStyle.ToolWindow;

            StackPanel NewPanel = new StackPanel();

            Label InputYourName = new Label();
            InputYourName.HorizontalContentAlignment = HorizontalAlignment.Center;
            Box = new TextBox();
            Box.Width = 200;
            Box.Margin = new Thickness(10);
            Button Ok = new Button();
            Ok.Content = "Ок";
            Ok.Height = 20;
            Ok.Width = 40;
            Ok.Click += Play;
            Ok.Margin = new Thickness(20);
            InputYourName.Content = "Введите свой никнейм";

            NewPanel.Children.Add(InputYourName);
            NewPanel.Children.Add(Box);
            NewPanel.Children.Add(Ok);

            InputName.Content = NewPanel;
            InputName.ShowDialog();
        }

        private void Play(object sender, RoutedEventArgs e)
        {
            bool ifEmpty = false;
            for (int i = 0; i < Box.Text.Length; i++)
            {
                if (Box.Text[i] == ' ')
                {
                    ifEmpty = true;
                }
                else
                {
                    ifEmpty = false;
                    i = Box.Text.Length;
                }
            }

            if (Box.Text != "" && !ifEmpty)
            {
                nameOfPlayer = Box.Text;
                InputName.Close();
                DrawField();
                Application.Current.MainWindow.Height = 800;
                Application.Current.MainWindow.Width = 1900;
                Application.Current.MainWindow.MinWidth = 1900;
                ImageBrush myBrush = new ImageBrush();
                myBrush.ImageSource =
                    (new BitmapImage(new Uri(@"C:\\Users\\halop\\RiderProjects\\kursa4\\kursa4\\GameField.jpg")));
                canvas.Background = myBrush;
                Ship4.Visibility = Visibility.Visible;
                Ship4.LayoutTransform = new RotateTransform(0);
                Ship3_1.Visibility = Visibility.Visible;
                Ship3_1.LayoutTransform = new RotateTransform(0);
                Ship3_2.Visibility = Visibility.Visible;
                Ship3_2.LayoutTransform = new RotateTransform(0);
                Ship2_1.Visibility = Visibility.Visible;
                Ship2_1.LayoutTransform = new RotateTransform(0);
                Ship2_2.Visibility = Visibility.Visible;
                Ship2_2.LayoutTransform = new RotateTransform(0);
                Ship2_3.Visibility = Visibility.Visible;
                Ship2_3.LayoutTransform = new RotateTransform(0);
                Ship1_1.Visibility = Visibility.Visible;
                Ship1_1.LayoutTransform = new RotateTransform(0);
                Ship1_2.Visibility = Visibility.Visible;
                Ship1_2.LayoutTransform = new RotateTransform(0);
                Ship1_3.Visibility = Visibility.Visible;
                Ship1_3.LayoutTransform = new RotateTransform(0);
                Ship1_4.Visibility = Visibility.Visible;
                Ship1_4.LayoutTransform = new RotateTransform(0);

                ButtonField();
                FillImageSource();
                Border1.Visibility = Visibility.Visible;
                Border2.Visibility = Visibility.Visible;
                ButtonRecords.Visibility = Visibility.Hidden;
                ButtonCloseApp.Visibility = Visibility.Hidden;
                ButtonInstruction2.Visibility = Visibility.Visible;
                ButtonCloseInstructions.Visibility = Visibility.Hidden;
                Instructions.Visibility = Visibility.Hidden;
                ButtonInstruction.Visibility = Visibility.Hidden;
                ButtonPlay.Visibility = Visibility.Hidden;
                field.Visibility = Visibility.Visible;
                ButtonStartGame.Visibility = Visibility.Visible;
                ButtonResetShips.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Поле не может быть пустым!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning
                    , MessageBoxResult.OK);
            }
        }
        
        private void DrawField()
        {
            for (int i = 0; i < 10; ++i)
            {
                for (int j = 0; j < 10; ++j)
                {
                    cells[i, j] = new Border()
                    {
                        Width = 60, Height = 60,
                        BorderThickness = new Thickness(1),
                        BorderBrush = Brushes.Black, Background = Brushes.Cornsilk,
                    };
                    cells[i, j].AllowDrop = true;
                    cells[i, j].Name = "Border_" + i.ToString() + j.ToString();
                    if (j == 9)
                    {
                        cells[i, j].Tag = "critical_..9";
                    }
                    else if (j == 8)
                    {
                        cells[i, j].Tag = "critical_..8";
                    }
                    else if (j == 7)
                    {
                        cells[i, j].Tag = "critical_..7";
                    }

                    if (i == 9 && j != 9 && j != 8 && j != 7)
                    {
                        cells[i, j].Tag = "critical_9..";
                    }
                    else if (i == 8 && j != 9 && j != 8 && j != 7)
                    {
                        cells[i, j].Tag = "critical_8..";
                    }
                    else if (i == 7 && j != 9 && j != 8 && j != 7)
                    {
                        cells[i, j].Tag = "critical_7..";
                    }

                    Canvas.SetLeft(cells[i, j], 0 + j * 60);
                    Canvas.SetTop(cells[i, j], 0 + i * 60);
                    field.Children.Add(cells[i, j]);
                    cells[i, j].DragLeave += OnDragLeave;
                    cells[i, j].DragEnter += OnDragEnter;
                    cells[i, j].PreviewDrop += OnPreviewDrop;
                }
            }

        }

        private void OnDragLeave(object sender, DragEventArgs e)
        {
            if (point_true)
            {
                _poinOfDrag = sender as Border;
                point_true = false;
            }
        }

        private void SetPointerInTemp()
        {
            result = false;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    TempPlace(cells[i, j], this.tship);
                    if (result)
                    {
                        temp = cells[i, j];
                        this.enter_in_bord = true;
                        
                        i = 10;
                        j = 10;
                    }
                }
            }

            if (!result)
            {
                temp = null;
                this.enter_in_bord = false;
            }
        }

        private int[] TryPointOfDrag()
        {
            int[] tI = new int[2];
            
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    TempPlace(cells[i, j], this.tship);
                    if (result)
                    {
                        tI[0] = i;
                        tI[1] = j;
                        
                        i = 10;
                        j = 10;
                    }
                }
            }

            return tI;
        }
        
        private void OnPreviewDrop(object sender, DragEventArgs e)
        {
            SetPointerInTemp();
            tetetemop();
            dropped_function();
        }

        private void tetetemop()
        {
            if (this.dragobject != null && temp != null && this.enter_in_bord)
            {
                place_ship(temp);
            }
        }

        private void dropped_function()
        {
            unDrag = false;
            ship_collision();

            if (this.dragobject != null && result)
            {
                if (this._poinOfDrag != null)
                {
                    place_ship(this._poinOfDrag);
                }
                else
                {
                    replace_ship();
                }

            }

            result = false;

            ship_collision();
            
            if (this._poinOfDrag != null && this.dragobject != null && result)
            {
                place_ship(this._poinOfDrag);
            }
            else if (result && this.dragobject != null)
            {
                if (_poinOfDrag != null && (Canvas.GetLeft(this.dragobject) > (window.ActualWidth - 1220) / 2 + 600  || 
                     Canvas.GetLeft(this.dragobject) < (window.ActualWidth - 1220) / 2 
                     || Canvas.GetTop(this.dragobject) < (window.ActualHeight - field.ActualHeight) / 2 || 
                     Canvas.GetTop(this.dragobject) > window.ActualHeight - (window.ActualHeight - field.ActualHeight) / 2))
                {
                    place_ship(_poinOfDrag);
                }
                else
                {

                    replace_ship();
                }
            }
            else if (_poinOfDrag == null && this.dragobject != null && !this.enter_in_bord)
            {
                replace_ship();
            }

            if (this.dragobject != null && _poinOfDrag != null && !this.enter_in_bord) 
            {
                place_ship(_poinOfDrag);
            }

            _poinOfDrag = null;
            this.dragobject = null;
            this.temp = null;
        }

        private void replace_ship()
        {
            if (this.tship.Name == "Ship4")
            {
                Canvas.SetTop(this.dragobject, 0);
                Canvas.SetLeft(this.dragobject, 0);
                tship.Tag = "untouched";
                Ship4.LayoutTransform = new RotateTransform(0);
            }
            else if (this.tship.Name == "Ship3_1")
            {
                Canvas.SetTop(this.dragobject, 80);
                Canvas.SetLeft(this.dragobject, 0);
                tship.Tag = "untouched";
                Ship3_1.LayoutTransform = new RotateTransform(0);
            }
            else if (tship.Name == "Ship3_2")
            {
                Canvas.SetTop(this.dragobject, 160);
                Canvas.SetLeft(this.dragobject, 0);
                tship.Tag = "untouched";
                Ship3_2.LayoutTransform = new RotateTransform(0);
            }
            else if (tship.Name == "Ship2_1")
            {
                Canvas.SetTop(this.dragobject, 240);
                Canvas.SetLeft(this.dragobject, 0);
                tship.Tag = "untouched";
                Ship2_1.LayoutTransform = new RotateTransform(0);
            }
            else if (tship.Name == "Ship2_2")
            {
                Canvas.SetTop(this.dragobject, 320);
                Canvas.SetLeft(this.dragobject, 0);
                tship.Tag = "untouched";
                Ship2_2.LayoutTransform = new RotateTransform(0);
            }
            else if (tship.Name == "Ship2_3")
            {
                Canvas.SetTop(this.dragobject, 400);
                Canvas.SetLeft(this.dragobject, 0);
                tship.Tag = "untouched";
                Ship2_3.LayoutTransform = new RotateTransform(0);
            }
            else if (tship.Name == "Ship1_1")
            {
                Canvas.SetTop(this.dragobject, 480);
                Canvas.SetLeft(this.dragobject, 0);
                tship.Tag = "untouched";
                Ship1_1.LayoutTransform = new RotateTransform(0);
            }
            else if (tship.Name == "Ship1_2")
            {
                Canvas.SetTop(this.dragobject, 480);
                Canvas.SetLeft(this.dragobject, 75);
                tship.Tag = "untouched";
                Ship1_2.LayoutTransform = new RotateTransform(0);
            }
            else if (tship.Name == "Ship1_3")
            {
                Canvas.SetTop(this.dragobject, 480);
                Canvas.SetLeft(this.dragobject, 150);
                tship.Tag = "untouched";
                Ship1_3.LayoutTransform = new RotateTransform(0);
            }
            else if (tship.Name == "Ship1_4")
            {
                Canvas.SetTop(this.dragobject, 480);
                Canvas.SetLeft(this.dragobject, 225);
                tship.Tag = "untouched";
                Ship1_4.LayoutTransform = new RotateTransform(0);
            }
        }

        private int[] arrShipPosition = new int[20];
        
        private void place_ship(Border ttemp)
        {
            int[] tarr = find_temp_name(ttemp);
            int i = tarr[0];
            int j = tarr[1];

            if (!situtation)
            {
                if ((string)ttemp.Tag == "critical_..9")
                {
                    if (tship.Width == 240)
                    {
                        j -= 3;
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (window.ActualWidth - 1220) / 2);
                        tship.Tag = "touched";
                    }
                    else if (tship.Width == 180)
                    {
                        j -= 2;
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (window.ActualWidth - 1220) / 2);
                        tship.Tag = "touched";
                    }
                    else if (tship.Width == 120)
                    {
                        j -= 1;
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (window.ActualWidth - 1220) / 2);
                        tship.Tag = "touched";
                    }
                    else if (tship.Width == 60)
                    {
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (window.ActualWidth - 1220) / 2);
                        tship.Tag = "touched";
                    }
                }
                else if ((string)ttemp.Tag == "critical_..8")
                {
                    if (tship.Width == 240)
                    {
                        j -= 2;
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (window.ActualWidth - 1220) / 2);
                        tship.Tag = "touched";
                    }
                    else if (tship.Width == 180)
                    {
                        j -= 1;
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (window.ActualWidth - 1220) / 2);
                        tship.Tag = "touched";
                    }
                    else if (tship.Width == 120)
                    {
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (window.ActualWidth - 1220) / 2);
                        tship.Tag = "touched";
                    }
                    else if (tship.Width == 60)
                    {
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (window.ActualWidth - 1220) / 2);
                        tship.Tag = "touched";
                    }
                }
                else if ((string)ttemp.Tag == "critical_..7")
                {
                    if (tship.Width == 240)
                    {
                        j -= 1;
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (window.ActualWidth - 1220) / 2);
                        tship.Tag = "touched";
                    }
                    else if (tship.Width == 180)
                    {
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (window.ActualWidth - 1220) / 2);
                        tship.Tag = "touched";
                    }
                    else if (tship.Width == 120)
                    {
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (window.ActualWidth - 1220) / 2);
                        tship.Tag = "touched";
                    }
                    else if (tship.Width == 60)
                    {
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (window.ActualWidth - 1220) / 2);
                        tship.Tag = "touched";
                    }
                }
                else
                {
                    temp = cells[i, j];
                    Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                    Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (window.ActualWidth - 1220) / 2);
                    tship.Tag = "touched";
                }
            }
            else
            {
                if ((string)ttemp.Tag == "critical_9.." || (string)ttemp.Name == "Border_99" ||
                    (string)ttemp.Name == "Border_98" || (string)ttemp.Name == "Border_97")
                {
                    if (tship.Width == 240)
                    {
                        i -= 3;
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (window.ActualWidth - 1220) / 2);
                        tship.Tag = "touched";
                        tship.LayoutTransform = new RotateTransform(90);
                    }
                    else if (tship.Width == 180)
                    {
                        i -= 2;
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (window.ActualWidth - 1220) / 2);
                        tship.Tag = "touched";
                        tship.LayoutTransform = new RotateTransform(90);
                    }
                    else if (tship.Width == 120)
                    {
                        i -= 1;
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (window.ActualWidth - 1220) / 2);
                        tship.Tag = "touched";
                        tship.LayoutTransform = new RotateTransform(90);
                    }
                    else if (tship.Width == 60)
                    {
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (window.ActualWidth - 1220) / 2);
                        tship.Tag = "touched";
                        tship.LayoutTransform = new RotateTransform(90);
                    }
                }
                else if ((string)ttemp.Tag == "critical_8.." || (string)ttemp.Name == "Border_89" ||
                         (string)ttemp.Name == "Border_88" || (string)ttemp.Name == "Border_87")
                {
                    if (tship.Width == 240)
                    {
                        i -= 2;
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (window.ActualWidth - 1220) / 2);
                        tship.Tag = "touched";
                        tship.LayoutTransform = new RotateTransform(90);
                    }
                    else if (tship.Width == 180)
                    {
                        i -= 1;
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (window.ActualWidth - 1220) / 2);
                        tship.Tag = "touched";
                        tship.LayoutTransform = new RotateTransform(90);
                    }
                    else if (tship.Width == 120)
                    {
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (window.ActualWidth - 1220) / 2);
                        tship.Tag = "touched";
                        tship.LayoutTransform = new RotateTransform(90);
                    }
                    else if (tship.Width == 60)
                    {
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (window.ActualWidth - 1220) / 2);
                        tship.Tag = "touched";
                        tship.LayoutTransform = new RotateTransform(90);
                    }
                }
                else if ((string)ttemp.Tag == "critical_7.." || (string)ttemp.Name == "Border_79" ||
                         (string)ttemp.Name == "Border_78" || (string)ttemp.Name == "Border_77")
                {
                    if (tship.Width == 240)
                    {
                        i -= 1;
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (window.ActualWidth - 1220) / 2);
                        tship.Tag = "touched";
                        tship.LayoutTransform = new RotateTransform(90);
                    }
                    else if (tship.Width == 180)
                    {
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (window.ActualWidth - 1220) / 2);
                        tship.Tag = "touched";
                        tship.LayoutTransform = new RotateTransform(90);
                    }
                    else if (tship.Width == 120)
                    {
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (window.ActualWidth - 1220) / 2);
                        tship.Tag = "touched";
                        tship.LayoutTransform = new RotateTransform(90);
                    }
                    else if (tship.Width == 60)
                    {
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (window.ActualWidth - 1220) / 2);
                        tship.Tag = "touched";
                        tship.LayoutTransform = new RotateTransform(90);
                    }
                }
                else
                {
                    temp = cells[i, j];
                    Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                    Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (window.ActualWidth - 1220) / 2);
                    tship.Tag = "touched";
                    tship.LayoutTransform = new RotateTransform(90);
                }
            }

            result = false;
            ship_collision();
            
            if (result)
            {
                replace_ship();
            }
            result = false;

            if (tship == Ship1_1)
            {
                arrShipPosition[0] = i;
                arrShipPosition[1] = j;
            }
            else if (tship == Ship1_2)
            {
                arrShipPosition[2] = i;
                arrShipPosition[3] = j;
            }
            else if (tship == Ship1_3)
            {
                arrShipPosition[4] = i;
                arrShipPosition[5] = j;
            }
            else if (tship == Ship1_4)
            {
                arrShipPosition[6] = i;
                arrShipPosition[7] = j;
            }
            else if (tship == Ship2_1)
            {
                arrShipPosition[8] = i;
                arrShipPosition[9] = j;
            }
            else if (tship == Ship2_2)
            {
                arrShipPosition[10] = i;
                arrShipPosition[11] = j;
            }
            else if (tship == Ship2_3)
            {
                arrShipPosition[12] = i;
                arrShipPosition[13] = j;
            }
            else if (tship == Ship3_1)
            {
                arrShipPosition[14] = i;
                arrShipPosition[15] = j;
            }
            else if (tship == Ship3_2)
            {
                arrShipPosition[16] = i;
                arrShipPosition[17] = j;
            }
            else if (tship == Ship4)
            {
                arrShipPosition[18] = i;
                arrShipPosition[19] = j;
            }

        }

        private void ship_collision()
        {
            if (temp != null)
            {
                int[] tarr = find_temp_name(temp);
                int i = tarr[0];
                int j = tarr[1];

                if (tship.Width == 240)
                {
                    if (!situtation)
                    {
                        check_deadline(cells[i, j]);
                        check_deadline(cells[i, j + 1]);
                        check_deadline(cells[i, j + 2]);
                        check_deadline(cells[i, j + 3]);
                    }
                    else if (situtation)
                    {
                        check_deadline(cells[i, j]);
                        check_deadline(cells[i + 1, j]);
                        check_deadline(cells[i + 2, j]);
                        check_deadline(cells[i + 3, j]);
                    }
                }
                else if (tship.Width == 180)
                {
                    if (!situtation)
                    {
                        check_deadline(cells[i, j]);
                        check_deadline(cells[i, j + 1]);
                        check_deadline(cells[i, j + 2]);
                    }
                    else if (situtation)
                    {
                        check_deadline(cells[i, j]);
                        check_deadline(cells[i + 1, j]);
                        check_deadline(cells[i + 2, j]);
                    }
                }
                else if (tship.Width == 120)
                {
                    if (!situtation)
                    {
                        check_deadline(cells[i, j]);
                        check_deadline(cells[i, j + 1]);
                    }
                    else if (situtation)
                    {
                        check_deadline(cells[i, j]);
                        check_deadline(cells[i + 1, j]);
                    }
                }
                else if (tship.Width == 60)
                {
                    check_deadline(cells[i, j]);
                }
            }
        }

        private UIElement dragobject = null;
        private Border temp = null;
        private Label tship = null;
        private bool enter_in_bord = false;
        private Border _poinOfDrag = null;
        private bool point_true = false;
        private bool result;
        private double _positionMouseX;
        private double _positionMouseY; 

        private void Ship_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.dragobject = sender as UIElement;
            this.tship = sender as Label;
            int[] tempIndex = new int[2];
            if (this.dragobject != null)
            {
                _positionMouseX = Mouse.GetPosition(sender as IInputElement).X;
                _positionMouseY = Mouse.GetPosition(sender as IInputElement).Y;

                int[] tI = new int[2];
                result = false;
                tI = TryPointOfDrag();
                
                if (result)
                {
                    _poinOfDrag = cells[tI[0], tI[1]];
                }

                if (tship != null)
                {
                    this.tship.Tag = "untouched";
                }

                checkRotateShip();

                DragDrop.DoDragDrop(this.dragobject, this.dragobject, DragDropEffects.All);
            }
        }


        private void checkRotateShip()
        {
            RotateTransform trnsf = tship.LayoutTransform as RotateTransform;

            if (tship != null && trnsf != null && trnsf.Angle == 90)
            {
                situtation = true;
            }
            else if (tship != null && trnsf != null && trnsf.Angle == 0)
            {
                situtation = false;
            }
        }


        private void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            grid.Width = window.ActualWidth;
            grid.Height = window.ActualHeight;
            
            grid2.Width = window.ActualWidth;
            grid2.Height = window.ActualHeight;
            
            ReplaceShipsWhenSizeChanged();
        }

        private void ReplaceShipsWhenSizeChanged()
        {
            if ((string)Ship1_1.Tag == "touched" || (string)Ship1_1.Tag == "destroyed")
            {
                ReplaceShip(Ship1_1, cells[arrShipPosition[0], arrShipPosition[1]]);
            }

            if ((string)Ship1_2.Tag == "touched" || (string)Ship1_2.Tag == "destroyed")
            {
                ReplaceShip(Ship1_2, cells[arrShipPosition[2], arrShipPosition[3]]);
            }

            if ((string)Ship1_3.Tag == "touched" || (string)Ship1_3.Tag == "destroyed")
            {
                ReplaceShip(Ship1_3,cells[arrShipPosition[4],arrShipPosition[5]]);
            }

            if ((string)Ship1_4.Tag == "touched" || (string)Ship1_4.Tag == "destroyed")
            {
                ReplaceShip(Ship1_4,cells[arrShipPosition[6],arrShipPosition[7]]);
            }

            if ((string)Ship2_1.Tag == "touched" || (string)Ship2_1.Tag == "destroyed")
            {
                ReplaceShip(Ship2_1,cells[arrShipPosition[8],arrShipPosition[9]]);
            }

            if ((string)Ship2_2.Tag == "touched" || (string)Ship2_2.Tag == "destroyed")
            {
                ReplaceShip(Ship2_2,cells[arrShipPosition[10],arrShipPosition[11]]);
            }

            if ((string)Ship2_3.Tag == "touched" || (string)Ship2_3.Tag == "destroyed")
            {
                ReplaceShip(Ship2_3,cells[arrShipPosition[12],arrShipPosition[13]]);
            }

            if ((string)Ship3_1.Tag == "touched" || (string)Ship3_1.Tag == "destroyed")
            {
                ReplaceShip(Ship3_1,cells[arrShipPosition[14],arrShipPosition[15]]);
            }

            if ((string)Ship3_2.Tag == "touched" || (string)Ship3_2.Tag == "destroyed")
            {
                ReplaceShip(Ship3_2,cells[arrShipPosition[16],arrShipPosition[17]]);
            }

            if ((string)Ship4.Tag == "touched" || (string)Ship4.Tag == "destroyed")
            {
                ReplaceShip(Ship4,cells[arrShipPosition[18],arrShipPosition[19]]);
            }
        }
        
        private void ReplaceShip(Label shipT, Border bord)
        {
            double bordX = Canvas.GetLeft(bord);
            double bordY = Canvas.GetTop(bord);
            
            Canvas.SetLeft(shipT,bordX + (grid.Width - 1220) / 2);
            Canvas.SetTop(shipT,bordY + (grid.Height - field.Height) / 2);
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {

            this.temp = sender as Border;
            enter_in_bord = true;
        }

        bool situtation = false;
        bool mayroll = true;
        private bool unDrag = false;

        private void Ship_MouseMove(object sender, DragEventArgs e)
        {
            if (this.dragobject == null)
            {
                return;
            }

            bool isKeyPressed = Keyboard.IsKeyDown(Key.R);
            bool isKeyRelease = Keyboard.IsKeyUp(Key.R);
            if (this.dragobject != null && isKeyPressed && mayroll)
            {
                if (!situtation)
                {
                    tship.LayoutTransform = new RotateTransform(90);
                    situtation = true;
                }
                else
                {
                    tship.LayoutTransform = new RotateTransform(0);
                    situtation = false;
                }

                mayroll = false;
            }

            if (isKeyRelease)
            {
                mayroll = true;
            }

            var position = e.GetPosition(canvas);

            checkRotateShip();

            if (situtation && !unDrag)
            {
                _positionMouseY = 60 - _positionMouseY;
                unDrag = true;
            }

            if (!situtation)
            {
                Canvas.SetTop(this.dragobject, position.Y - _positionMouseY);
                Canvas.SetLeft(this.dragobject, position.X - _positionMouseX);
            }
            else
            {
                Canvas.SetTop(this.dragobject, position.Y - _positionMouseX);
                Canvas.SetLeft(this.dragobject, position.X - _positionMouseY);
            }
        }

        private void check_ship_on_border(Border bord, Label ttship)
        {
            GeneralTransform t1 = bord.TransformToVisual(this);
            GeneralTransform t2 = ttship.TransformToVisual(this);
            Rect r1 = t1.TransformBounds(new Rect()
                { X = 0, Y = 0, Width = bord.ActualWidth + 5, Height = bord.ActualHeight + 5});
            Rect r2 = t2.TransformBounds(new Rect()
                { X = 0, Y = 0, Width = ttship.ActualWidth, Height = ttship.ActualHeight });
            result = r1.IntersectsWith(r2);
        }
        
        private void TempPlace(Border bord, Label ttship)
        {
            GeneralTransform t1 = bord.TransformToVisual(this);
            GeneralTransform t2 = ttship.TransformToVisual(this);
            Rect r1 = t1.TransformBounds(new Rect()
                { X = 0, Y = 0, Width = bord.ActualWidth - 25, Height = bord.ActualHeight - 25 });
            Rect r2;
            if (!situtation)
            {
                r2 = t2.TransformBounds(new Rect()
                    { X = 5, Y = 5, Width = ttship.ActualWidth, Height = ttship.ActualHeight });
            }
            else
            {
                r2 = t2.TransformBounds(new Rect()
                    { X = 5, Y = -5, Width = ttship.ActualWidth, Height = ttship.ActualHeight });
            }

            result = r1.IntersectsWith(r2);
        }

        private void check_deadline(Border bord)
        {
            if (tship.Name != "Ship4" && !result)
            {
                check_ship_on_border(bord, Ship4);
            }

            if (tship.Name != "Ship3_1" && !result)
            {
                check_ship_on_border(bord, Ship3_1);
            }

            if (tship.Name != "Ship3_2" && !result)
            {
                check_ship_on_border(bord, Ship3_2);
            }

            if (tship.Name != "Ship2_1" && !result)
            {
                check_ship_on_border(bord, Ship2_1);
            }

            if (tship.Name != "Ship2_2" && !result)
            {
                check_ship_on_border(bord, Ship2_2);
            }

            if (tship.Name != "Ship2_3" && !result)
            {
                check_ship_on_border(bord, Ship2_3);
            }

            if (tship.Name != "Ship1_1" && !result)
            {
                check_ship_on_border(bord, Ship1_1);
            }

            if (tship.Name != "Ship1_2" && !result)
            {
                check_ship_on_border(bord, Ship1_2);
            }

            if (tship.Name != "Ship1_3" && !result)
            {
                check_ship_on_border(bord, Ship1_3);
            }

            if (tship.Name != "Ship1_4" && !result)
            {
                check_ship_on_border(bord, Ship1_4);
            }

        }

        private int[] find_temp_name(Border ttemp)
        {
            int[] arrtemp = new int[2];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (ttemp != null && ttemp.Name == cells[i, j].Name)
                    {
                        arrtemp[0] = i;
                        arrtemp[1] = j;
                        return arrtemp;
                    }
                }
            }

            return arrtemp;
        }

        private void ButtonStartGame_OnClick(object sender, RoutedEventArgs e)
        {
            result = false;

            if (check_all_ships())
            {
                startGame = true;
                ButtonField();
                gameField();
            }
            else
            {
                MessageBox.Show("Пожалуйста расставьте все корабли!", "GameChecker", MessageBoxButton.OK,
                    MessageBoxImage.Warning, MessageBoxResult.OK);
            }
        }

        private bool check_all_ships()
        {
            if ((string)Ship4.Tag == "touched" && (string)Ship3_1.Tag == "touched" && (string)Ship3_2.Tag == "touched"
                && (string)Ship2_1.Tag == "touched" && (string)Ship2_2.Tag == "touched" &&
                (string)Ship2_3.Tag == "touched"
                && (string)Ship1_1.Tag == "touched" && (string)Ship1_2.Tag == "touched" &&
                (string)Ship1_3.Tag == "touched"
                && (string)Ship1_4.Tag == "touched")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ReplaceShip()
        {
            Canvas.SetTop(Ship4, 0);
            Canvas.SetLeft(Ship4, 0);
            Ship4.Tag = "untouched";

            Canvas.SetTop(Ship3_1, 80);
            Canvas.SetLeft(Ship3_1, 0);
            Ship3_1.Tag = "untouched";

            Canvas.SetTop(Ship3_2, 160);
            Canvas.SetLeft(Ship3_2, 0);
            Ship3_2.Tag = "untouched";

            Canvas.SetTop(Ship2_1, 240);
            Canvas.SetLeft(Ship2_1, 0);
            Ship2_1.Tag = "untouched";

            Canvas.SetTop(Ship2_2, 320);
            Canvas.SetLeft(Ship2_2, 0);
            Ship2_2.Tag = "untouched";

            Canvas.SetTop(Ship2_3, 400);
            Canvas.SetLeft(Ship2_3, 0);
            Ship2_3.Tag = "untouched";

            Canvas.SetTop(Ship1_1, 480);
            Canvas.SetLeft(Ship1_1, 0);
            Ship1_1.Tag = "untouched";

            Canvas.SetTop(Ship1_2, 480);
            Canvas.SetLeft(Ship1_2, 75);
            Ship1_2.Tag = "untouched";

            Canvas.SetTop(Ship1_3, 480);
            Canvas.SetLeft(Ship1_3, 150);
            Ship1_3.Tag = "untouched";

            Canvas.SetTop(Ship1_4, 480);
            Canvas.SetLeft(Ship1_4, 225);
            Ship1_4.Tag = "untouched";
            
            Ship4.LayoutTransform = new RotateTransform(0);
            Ship3_1.LayoutTransform = new RotateTransform(0);
            Ship3_2.LayoutTransform = new RotateTransform(0);
            Ship2_1.LayoutTransform = new RotateTransform(0);
            Ship2_2.LayoutTransform = new RotateTransform(0);
            Ship2_3.LayoutTransform = new RotateTransform(0);
            Ship1_1.LayoutTransform = new RotateTransform(0);
            Ship1_2.LayoutTransform = new RotateTransform(0);
            Ship1_3.LayoutTransform = new RotateTransform(0);
            Ship1_4.LayoutTransform = new RotateTransform(0);
        }
        

        private void ButtonResetShips_OnClick(object sender, RoutedEventArgs e)
        { 
            ReplaceShip();
        }

        private void gameField()
        {
            ImageBrush myBrush = new ImageBrush();
            myBrush.ImageSource = (new BitmapImage(new Uri(@"C:\\Users\\halop\\RiderProjects\\kursa4\\kursa4\\GameFieldStart.jpg")));
            canvas.Background = myBrush;
            
            ButtonStartGame.Visibility = Visibility.Hidden;
            ButtonResetShips.Visibility = Visibility.Hidden;
            ButtonInstruction2.Visibility = Visibility.Hidden;
            KillField.Visibility = Visibility.Visible;
            KillField2.Visibility = Visibility.Visible;

            Ship4.IsHitTestVisible = false;
            Ship3_1.IsHitTestVisible = false;
            Ship3_2.IsHitTestVisible = false;
            Ship2_1.IsHitTestVisible = false;
            Ship2_2.IsHitTestVisible = false;
            Ship2_3.IsHitTestVisible = false;
            Ship1_1.IsHitTestVisible = false;
            Ship1_2.IsHitTestVisible = false;
            Ship1_3.IsHitTestVisible = false;
            Ship1_4.IsHitTestVisible = false;

        }

        private Button[,] game_cells;

        private void ButtonField()
        {
            show_unvisible();
            comp_field();

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    game_cells[i, j] = new Button()
                    {
                        Height = 60, Width = 60
                    };

                    game_cells[i, j].Name = "Gbutton_" + i.ToString() + j.ToString();
                    Canvas.SetLeft(game_cells[i, j], 0 + j * 60);
                    Canvas.SetTop(game_cells[i, j], 0 + i * 60);
                    GameField.Children.Add(game_cells[i, j]);
                    game_cells[i, j].Click += GameCellClick;
                }
            }
        }

        private int count = 0;
        Random ran = new Random();
        private int position;
        
        private void comp_field()
        {
            int i = 0, j = 0;

            while (count < 10)
            {
                i = ran.Next(0, 10);
                j = ran.Next(0, 10);

                if (compships[i, j] == 0)
                {
                    int tpos = ran.Next(0, 100);

                    if (tpos > 50)
                    {
                        position = 1;
                    }
                    else
                    {
                        position = 0;
                    }
                    compship_generator(i, j);
                    count++;
                }
            }
        }

        private int[] compShip4 = new int[8];
        private int[] compShip3_1 = new int[6];
        private int[] compShip3_2 = new int[6];
        private int[] compShip2_1 = new int[4];
        private int[] compShip2_2 = new int[4];
        private int[] compShip2_3 = new int[4];

        private void compship_generator(int i, int j)
        {
            bool j_edge_l = false, i_edge_u = false, i_edge_d = false, j_edge_r = false, leftBuild = false, downBuild = false, rightBuild = false, upBuild = false;
            bool leftDeadZone = false, rightDeadZone = false, upDeadZone = false, downDeadZone = false, diagLeftUp = false,
                diagLeftDown = false, diagRightUp = false, diagRightDown = false;
            bool[] tempbool = new bool[4];

            if (position == 0)
            {
                if (count == 0)
                {
                    tempbool = checked_edges(i, j, 6, 3);
                    j_edge_r = tempbool[0];

                    compships[i, j] = 1;
                    compShip4[0] = i;
                    compShip4[1] = j;

                    if (!j_edge_r)
                    {
                        leftBuild = true;
                    }

                    if (leftBuild)
                    {
                        if (j != 9)
                        {
                            compships[i, j + 1] = 2;
                        }

                        compships[i, j - 1] = 1;
                        compships[i, j - 2] = 1;
                        compships[i, j - 3] = 1;
                        compships[i, j - 4] = 2;

                        compShip4[2] = i;
                        compShip4[3] = j - 1;
                        compShip4[4] = i;
                        compShip4[5] = j - 2;
                        compShip4[6] = i;
                        compShip4[7] = j - 3;
                        
                        if (i != 9)
                        {
                            if (j != 9)
                            {
                                compships[i + 1, j + 1] = 2;
                            }

                            compships[i + 1, j] = 2;
                            compships[i + 1, j - 1] = 2;
                            compships[i + 1, j - 2] = 2;
                            compships[i + 1, j - 3] = 2;
                            compships[i + 1, j - 4] = 2;
                        }

                        if (i != 0)
                        {
                            if (j != 9)
                            {
                                compships[i - 1, j + 1] = 2;
                            }

                            compships[i - 1, j] = 2;
                            compships[i - 1, j - 1] = 2;
                            compships[i - 1, j - 2] = 2;
                            compships[i - 1, j - 3] = 2;
                            compships[i - 1, j - 4] = 2;
                        }
                    }
                    else
                    {
                        if (j != 0)
                        {
                            compships[i, j - 1] = 2;
                        }

                        compships[i, j + 1] = 1;
                        compships[i, j + 2] = 1;
                        compships[i, j + 3] = 1;
                        compships[i, j + 4] = 2;
                        
                        compShip4[2] = i;
                        compShip4[3] = j + 1;
                        compShip4[4] = i;
                        compShip4[5] = j + 2;
                        compShip4[6] = i;
                        compShip4[7] = j + 3;

                        if (i != 9)
                        {
                            if (j != 0)
                            {
                                compships[i + 1, j - 1] = 2;
                            }

                            compships[i + 1, j] = 2;
                            compships[i + 1, j + 1] = 2;
                            compships[i + 1, j + 2] = 2;
                            compships[i + 1, j + 3] = 2;
                            compships[i + 1, j + 4] = 2;
                        }

                        if (i != 0)
                        {
                            if (j != 0)
                            {
                                compships[i - 1, j - 1] = 2;
                            }

                            compships[i - 1, j] = 2;
                            compships[i - 1, j + 1] = 2;
                            compships[i - 1, j + 2] = 2;
                            compships[i - 1, j + 3] = 2;
                            compships[i - 1, j + 4] = 2;
                        }
                    }
                }

                if (count == 1 || count == 2)
                {
                    tempbool = checked_edges(i, j, 7, 2);
                    j_edge_r = tempbool[0];
                    j_edge_l = tempbool[1];
                    rightDeadZone = tempbool[4];
                    leftDeadZone = tempbool[5];
                    diagLeftDown = tempbool[11];
                    diagLeftUp = tempbool[9];
                    diagRightDown = tempbool[10];
                    diagRightUp = tempbool[8];

                    if (j_edge_r && rightDeadZone)
                    {
                        if (j != 7)
                        {
                            if (compships[i, j + 3] != 1 && (diagRightDown || diagRightUp))
                            {
                                if (diagRightUp && diagRightDown)
                                {
                                    string right = "right";
                                    
                                    if (CheckedAroundShip(i,j,3,right))
                                    {
                                        rightBuild = true;
                                    }
                                }
                                else
                                {
                                    bool fCheck = false, secCheck = false;
                                    if (i == 0 && !diagRightUp)
                                    {
                                        fCheck = true;
                                    }

                                    if (i == 9 && !diagRightDown)
                                    {
                                        secCheck = true;
                                    }

                                    if (fCheck || secCheck)
                                    {
                                        string right = "right";
                                    
                                        if (CheckedAroundShip(i,j,3,right))
                                        {
                                            rightBuild = true;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            string right = "right";
                                    
                            if (CheckedAroundShip(i,j,3,right))
                            {
                                rightBuild = true;
                            }
                        }
                    }
                    else if (j_edge_l && leftDeadZone)
                    {
                        if (j != 2)
                        {
                            if (compships[i, j - 3] != 1 && (diagLeftUp || diagLeftDown))
                            {
                                if (diagLeftDown && diagLeftUp)
                                {
                                    string left = "left";
                                    
                                    if (CheckedAroundShip(i,j,3,left))
                                    {
                                        leftBuild = true;
                                    }
                                }
                                else
                                {
                                    bool fCheck = false, secCheck = false;
                                    if (i == 0 && !diagLeftUp)
                                    {
                                        fCheck = true;
                                    }

                                    if (i == 9 && !diagLeftDown)
                                    {
                                        secCheck = true;
                                    }

                                    if (fCheck || secCheck)
                                    {
                                        string left = "left";
                                    
                                        if (CheckedAroundShip(i,j,3,left))
                                        {
                                            leftBuild = true;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            string left = "left";
                                    
                            if (CheckedAroundShip(i,j,3,left))
                            {
                                leftBuild = true;
                            }
                        }
                    }

                    if (leftBuild)
                    {
                        compships[i, j] = 1;

                        if (count == 1)
                        {
                            compShip3_1[0] = i;
                            compShip3_1[1] = j;
                            compShip3_1[2] = i;
                            compShip3_1[3] = j - 1;
                            compShip3_1[4] = i;
                            compShip3_1[5] = j - 2;
                        }
                        else
                        {
                            compShip3_2[0] = i;
                            compShip3_2[1] = j;
                            compShip3_2[2] = i;
                            compShip3_2[3] = j - 1;
                            compShip3_2[4] = i;
                            compShip3_2[5] = j - 2;
                        }
                        
                        if (j != 9 && compships[i, j + 1] == 0)
                        {
                            compships[i, j + 1] = 2;
                        }

                        compships[i, j - 1] = 1;
                        compships[i, j - 2] = 1;

                        if (j != 2 && compships[i, j - 3] == 0)
                        {
                            compships[i, j - 3] = 2;
                        }

                        if (i != 0 && compships[i - 1, j] == 0 && compships[i - 1, j - 1] == 0 &&
                            compships[i - 1, j - 2] == 0)
                        {
                            if (j != 9 && compships[i - 1, j + 1] == 0)
                            {
                                compships[i - 1, j + 1] = 2;
                            }

                            compships[i - 1, j] = 2;
                            compships[i - 1, j - 1] = 2;
                            compships[i - 1, j - 2] = 2;

                            if (j != 2 && compships[i - 1, j - 3] == 0)
                            {
                                compships[i - 1, j - 3] = 2;
                            }
                        }

                        if (i != 9 && compships[i + 1, j] == 0 && compships[i + 1, j - 1] == 0 &&
                            compships[i + 1, j - 2] == 0)
                        {
                            if (j != 9 && compships[i + 1, j + 1] == 0)
                            {
                                compships[i + 1, j + 1] = 2;
                            }

                            compships[i + 1, j] = 2;
                            compships[i + 1, j - 1] = 2;
                            compships[i + 1, j - 2] = 2;

                            if (j != 2 && compships[i + 1, j - 3] == 0)
                            {
                                compships[i + 1, j - 3] = 2;
                            }
                        }
                    }
                    else if (rightBuild)
                    {
                        compships[i, j] = 1;
                        
                        if (count == 1)
                        {
                            compShip3_1[0] = i;
                            compShip3_1[1] = j;
                            compShip3_1[2] = i;
                            compShip3_1[3] = j + 1;
                            compShip3_1[4] = i;
                            compShip3_1[5] = j + 2;
                        }
                        else
                        {
                            compShip3_2[0] = i;
                            compShip3_2[1] = j;
                            compShip3_2[2] = i;
                            compShip3_2[3] = j + 1;
                            compShip3_2[4] = i;
                            compShip3_2[5] = j + 2;
                        }
                        
                        if (j != 0 && compships[i, j - 1] == 0)
                        {
                            compships[i, j - 1] = 2;
                        }

                        compships[i, j + 1] = 1;
                        compships[i, j + 2] = 1;

                        if (j != 7 && compships[i, j + 3] == 0)
                        {
                            compships[i, j + 3] = 2;
                        }

                        if (i != 0 && compships[i - 1, j] == 0 && compships[i - 1, j + 1] == 0 &&
                            compships[i - 1, j + 2] == 0)
                        {
                            if (j != 0 && compships[i - 1, j - 1] == 0)
                            {
                                compships[i - 1, j - 1] = 2;
                            }

                            compships[i - 1, j] = 2;
                            compships[i - 1, j + 1] = 2;
                            compships[i - 1, j + 2] = 2;

                            if (j != 7 && compships[i - 1, j + 3] == 0)
                            {
                                compships[i - 1, j + 3] = 2;
                            }
                        }

                        if (i != 9 && compships[i + 1, j] == 0 && compships[i + 1, j + 1] == 0 &&
                            compships[i + 1, j + 2] == 0)
                        {
                            if (j != 0 && compships[i + 1, j - 1] == 0)
                            {
                                compships[i + 1, j - 1] = 2;
                            }

                            compships[i + 1, j] = 2;
                            compships[i + 1, j + 1] = 2;
                            compships[i + 1, j + 2] = 2;

                            if (j != 7 && compships[i + 1, j + 3] == 0)
                            {
                                compships[i + 1, j + 3] = 2;
                            }
                        }
                    }
                    else
                    {
                        comp_field();
                        return;
                    }
                }

                if (count > 2 && count < 6)
                {
                    tempbool = checked_edges(i, j, 8, 1);
                    j_edge_r = tempbool[0];
                    j_edge_l = tempbool[1];
                    rightDeadZone = tempbool[4];
                    leftDeadZone = tempbool[5];
                    diagLeftDown = tempbool[11];
                    diagLeftUp = tempbool[9];
                    diagRightDown = tempbool[10];
                    diagRightUp = tempbool[8];

                    if (j_edge_r && rightDeadZone)
                    {
                        if (j != 8)
                        {
                            if (compships[i, j + 2] != 1 && (diagRightDown || diagRightUp))
                            {
                                if (diagRightUp && diagRightDown)
                                {
                                    string right = "right";
                                    
                                    if (CheckedAroundShip(i,j,2,right))
                                    {
                                        rightBuild = true;
                                    }
                                }
                                else
                                {
                                    bool fCheck = false, secCheck = false;
                                    if (i == 0 && !diagRightUp)
                                    {
                                        fCheck = true;
                                    }

                                    if (i == 9 && !diagRightDown)
                                    {
                                        secCheck = true;
                                    }

                                    if (fCheck || secCheck)
                                    {
                                        string right = "right";
                                    
                                        if (CheckedAroundShip(i,j,2,right))
                                        {
                                            rightBuild = true;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            string right = "right";
                                    
                            if (CheckedAroundShip(i,j,2,right))
                            {
                                rightBuild = true;
                            }
                        }
                    }
                    else if (j_edge_l && leftDeadZone)
                    {
                        if (j != 1)
                        {
                            if (compships[i, j - 2] != 1 && (diagLeftUp || diagLeftDown))
                            {
                                if (diagLeftDown && diagLeftUp)
                                {
                                    string left = "left";
                                    
                                    if (CheckedAroundShip(i,j,2,left))
                                    {
                                        leftBuild = true;
                                    }
                                }
                                else
                                {
                                    bool fCheck = false, secCheck = false;
                                    if (i == 0 && !diagLeftUp)
                                    {
                                        fCheck = true;
                                    }

                                    if (i == 9 && !diagLeftDown)
                                    {
                                        secCheck = true;
                                    }

                                    if (fCheck || secCheck)
                                    {
                                        string left = "left";
                                    
                                        if (CheckedAroundShip(i,j,2,left))
                                        {
                                            leftBuild = true;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            string left = "left";
                                    
                            if (CheckedAroundShip(i,j,2,left))
                            {
                                leftBuild = true;
                            }
                        }
                    }

                    if (leftBuild)
                    {
                        compships[i, j] = 1;

                        if (count == 3)
                        {
                            compShip2_1[0] = i;
                            compShip2_1[1] = j;
                            compShip2_1[2] = i;
                            compShip2_1[3] = j - 1;
                        } else if (count == 4)
                        {
                            compShip2_2[0] = i;
                            compShip2_2[1] = j;
                            compShip2_2[2] = i;
                            compShip2_2[3] = j - 1;
                        } else if (count == 5)
                        {
                            compShip2_3[0] = i;
                            compShip2_3[1] = j;
                            compShip2_3[2] = i;
                            compShip2_3[3] = j - 1;
                        }

                        if (j != 9 && compships[i, j + 1] == 0)
                        {
                            compships[i, j + 1] = 2;
                        }

                        compships[i, j - 1] = 1;

                        if (j != 1 && compships[i, j - 2] == 0)
                        {
                            compships[i, j - 2] = 2;
                        }

                        if (i != 0 && compships[i - 1, j] == 0 && compships[i - 1, j - 1] == 0)
                        {
                            if (j != 9 && compships[i - 1, j + 1] == 0)
                            {
                                compships[i - 1, j + 1] = 2;
                            }

                            compships[i - 1, j] = 2;
                            compships[i - 1, j - 1] = 2;

                            if (j != 1 && compships[i - 1, j - 2] == 0)
                            {
                                compships[i - 1, j - 2] = 2;
                            }
                        }

                        if (i != 9 && compships[i + 1, j] == 0 && compships[i + 1, j - 1] == 0)
                        {
                            if (j != 9 && compships[i + 1, j + 1] == 0)
                            {
                                compships[i + 1, j + 1] = 2;
                            }

                            compships[i + 1, j] = 2;
                            compships[i + 1, j - 1] = 2;

                            if (j != 1 && compships[i + 1, j - 2] == 0)
                            {
                                compships[i + 1, j - 2] = 2;
                            }
                        }
                    }
                    else if (rightBuild)
                    {
                        compships[i, j] = 1;
                        
                        if (count == 3)
                        {
                            compShip2_1[0] = i;
                            compShip2_1[1] = j;
                            compShip2_1[2] = i;
                            compShip2_1[3] = j + 1;
                        } else if (count == 4)
                        {
                            compShip2_2[0] = i;
                            compShip2_2[1] = j;
                            compShip2_2[2] = i;
                            compShip2_2[3] = j + 1;
                        } else if (count == 5)
                        {
                            compShip2_3[0] = i;
                            compShip2_3[1] = j;
                            compShip2_3[2] = i;
                            compShip2_3[3] = j + 1;
                        }
                        
                        if (j != 0 && compships[i, j - 1] == 0)
                        {
                            compships[i, j - 1] = 2;
                        }

                        compships[i, j + 1] = 1;

                        if (j != 8 && compships[i, j + 2] == 0)
                        {
                            compships[i, j + 2] = 2;
                        }

                        if (i != 0 && compships[i - 1, j] == 0 && compships[i - 1, j + 1] == 0)
                        {
                            if (j != 0 && compships[i - 1, j - 1] == 0)
                            {
                                compships[i - 1, j - 1] = 2;
                            }

                            compships[i - 1, j] = 2;
                            compships[i - 1, j + 1] = 2;

                            if (j != 8 && compships[i - 1, j + 2] == 0)
                            {
                                compships[i - 1, j + 2] = 2;
                            }
                        }

                        if (i != 9 && compships[i + 1, j] == 0 && compships[i + 1, j + 1] == 0)
                        {
                            if (j != 0 && compships[i + 1, j - 1] == 0)
                            {
                                compships[i + 1, j - 1] = 2;
                            }

                            compships[i + 1, j] = 2;
                            compships[i + 1, j + 1] = 2;

                            if (j != 8 && compships[i + 1, j + 2] == 0)
                            {
                                compships[i + 1, j + 2] = 2;
                            }
                        }
                    }
                    else
                    {
                        comp_field();
                        return;
                    }
                }

                if (count > 5)
                {
                    tempbool = checked_edges(i, j, 9, 1);
                    diagLeftDown = tempbool[11];
                    diagLeftUp = tempbool[9];
                    diagRightDown = tempbool[10];
                    diagRightUp = tempbool[8];

                    if (diagLeftDown && diagLeftUp && diagRightDown && diagRightUp)
                    {
                        compships[i, j] = 1;

                        if (i != 0)
                        {
                            compships[i - 1, j] = 2;

                            if (j != 0)
                            {
                                compships[i - 1, j - 1] = 2;
                            }

                            if (j != 9)
                            {
                                compships[i - 1, j + 1] = 2;
                            }
                        }

                        if (i != 9)
                        {
                            compships[i + 1, j] = 2;

                            if (j != 0)
                            {
                                compships[i + 1, j - 1] = 2;
                            }

                            if (j != 9)
                            {
                                compships[i + 1, j + 1] = 2;
                            }
                        }

                        if (j != 9)
                        {
                            compships[i, j + 1] = 2;
                        }

                        if (j != 0)
                        {
                            compships[i, j - 1] = 2;
                        }
                    }
                    else
                    {
                        comp_field();
                        return;
                    }
                }
            }
            else
            {
                if (count == 0)
                {
                    tempbool = checked_edges(i, j, 6, 3);
                    i_edge_d = tempbool[3];

                    if (i_edge_d)
                    {
                        downBuild = true;
                    }

                    compships[i, j] = 1;
                    compShip4[0] = i;
                    compShip4[1] = j;

                    if (downBuild)
                    {
                        if (i != 0)
                        {
                            compships[i - 1, j] = 2;
                        }

                        compships[i + 1, j] = 1;
                        compships[i + 2, j] = 1;
                        compships[i + 3, j] = 1;
                        compships[i + 4, j] = 2;
                        
                        compShip4[2] = i + 1;
                        compShip4[3] = j;
                        compShip4[4] = i + 2;
                        compShip4[5] = j;
                        compShip4[6] = i + 3;
                        compShip4[7] = j;

                        if (j != 9)
                        {
                            if (i != 0)
                            {
                                compships[i - 1, j + 1] = 2;
                            }

                            compships[i, j + 1] = 2;
                            compships[i + 1, j + 1] = 2;
                            compships[i + 2, j + 1] = 2;
                            compships[i + 3, j + 1] = 2;
                            compships[i + 4, j + 1] = 2;
                        }

                        if (j != 0)
                        {
                            if (i != 0)
                            {
                                compships[i - 1, j - 1] = 2;
                            }

                            compships[i, j - 1] = 2;
                            compships[i + 1, j - 1] = 2;
                            compships[i + 2, j - 1] = 2;
                            compships[i + 3, j - 1] = 2;
                            compships[i + 4, j - 1] = 2;
                        }
                    }
                    else
                    {
                        if (i != 9)
                        {
                            compships[i + 1, j] = 2;
                        }

                        compships[i - 1, j] = 1;
                        compships[i - 2, j] = 1;
                        compships[i - 3, j] = 1;
                        compships[i - 4, j] = 2;

                        compShip4[2] = i - 1;
                        compShip4[3] = j;
                        compShip4[4] = i - 2;
                        compShip4[5] = j;
                        compShip4[6] = i - 3;
                        compShip4[7] = j;
                        
                        if (j != 9)
                        {
                            if (i != 9)
                            {
                                compships[i + 1, j + 1] = 2;
                            }

                            compships[i, j + 1] = 2;
                            compships[i - 1, j + 1] = 2;
                            compships[i - 2, j + 1] = 2;
                            compships[i - 3, j + 1] = 2;
                            compships[i - 4, j + 1] = 2;
                        }

                        if (j != 0)
                        {
                            if (i != 9)
                            {
                                compships[i + 1, j - 1] = 2;
                            }

                            compships[i, j - 1] = 2;
                            compships[i - 1, j - 1] = 2;
                            compships[i - 2, j - 1] = 2;
                            compships[i - 3, j - 1] = 2;
                            compships[i - 4, j - 1] = 2;
                        }
                    }

                }

                if (count == 1 || count == 2)
                {
                    tempbool = checked_edges(i, j, 7, 2);
                    i_edge_u = tempbool[2];
                    i_edge_d = tempbool[3];
                    upDeadZone = tempbool[6];
                    downDeadZone = tempbool[7];
                    diagLeftDown = tempbool[11];
                    diagLeftUp = tempbool[9];
                    diagRightDown = tempbool[10];
                    diagRightUp = tempbool[8];

                    if (i_edge_u && downDeadZone)
                    {
                        if (i != 2)
                        {
                            if (compships[i - 3, j] != 1 && (diagLeftUp || diagRightUp))
                            {
                                if (diagLeftUp && diagRightUp)
                                {
                                    string up = "up";
                                    
                                    if (CheckedAroundShip(i,j,3,up))
                                    {
                                        upBuild = true;
                                    }
                                }
                                else
                                {
                                    bool fCheck = false, secCheck = false;
                                    if (j == 0 && !diagLeftUp)
                                    {
                                        fCheck = true;
                                    }

                                    if (j == 9 && !diagRightUp)
                                    {
                                        secCheck = true;
                                    }

                                    if (fCheck || secCheck)
                                    {
                                        string up = "up";
                                    
                                        if (CheckedAroundShip(i,j,3,up))
                                        {
                                            upBuild = true;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            string up = "up";
                                    
                            if (CheckedAroundShip(i,j,3,up))
                            {
                                upBuild = true;
                            }
                        }
                    }
                    else if (i_edge_d && upDeadZone)
                    {
                         if (i != 7)
                        {
                            if (compships[i + 3, j] != 1 && (diagLeftDown || diagRightDown))
                            {
                                if (diagLeftDown && diagRightDown)
                                {
                                    string down = "down";
                                    
                                    if (CheckedAroundShip(i,j,3,down))
                                    {
                                        downBuild = true;
                                    }
                                }
                                else
                                {
                                    bool fCheck = false, secCheck = false;
                                    if (j == 0 && !diagLeftDown)
                                    {
                                        fCheck = true;
                                    }

                                    if (j == 9 && !diagRightDown)
                                    {
                                        secCheck = true;
                                    }

                                    if (fCheck || secCheck)
                                    {
                                        string down = "down";
                                    
                                        if (CheckedAroundShip(i,j,3,down))
                                        {
                                            downBuild = true;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            string down = "down";
                                    
                            if (CheckedAroundShip(i,j,3,down))
                            {
                                downBuild = true;
                            }
                        }
                    }

                    if (upBuild)
                    {
                        compships[i, j] = 1;
                        
                        if (count == 1)
                        {
                            compShip3_1[0] = i;
                            compShip3_1[1] = j;
                            compShip3_1[2] = i - 1;
                            compShip3_1[3] = j;
                            compShip3_1[4] = i - 2;
                            compShip3_1[5] = j;
                        }
                        else
                        {
                            compShip3_2[0] = i;
                            compShip3_2[1] = j;
                            compShip3_2[2] = i - 1;
                            compShip3_2[3] = j;
                            compShip3_2[4] = i - 2;
                            compShip3_2[5] = j;
                        }
                        
                        if (i != 9 && compships[i + 1, j] == 0)
                        {
                            compships[i + 1, j] = 2;
                        }

                        compships[i - 1, j] = 1;
                        compships[i - 2, j] = 1;

                        if (i != 2 && compships[i - 3, j] == 0)
                        {
                            compships[i - 3, j] = 2;
                        }

                        if (j != 0 && compships[i, j - 1] == 0 && compships[i - 1, j - 1] == 0 &&
                            compships[i - 2, j - 1] == 0)
                        {
                            if (i != 9 && compships[i + 1, j - 1] == 0)
                            {
                                compships[i + 1, j - 1] = 2;
                            }

                            compships[i, j - 1] = 2;
                            compships[i - 1, j - 1] = 2;
                            compships[i - 2, j - 1] = 2;

                            if (i != 2 && compships[i - 3, j - 1] == 0)
                            {
                                compships[i - 3, j - 1] = 2;
                            }
                        }

                        if (j != 9 && compships[i, j + 1] == 0 && compships[i - 1, j + 1] == 0 &&
                            compships[i - 2, j + 1] == 0)
                        {
                            if (i != 9 && compships[i + 1, j + 1] == 0)
                            {
                                compships[i + 1, j + 1] = 2;
                            }

                            compships[i, j + 1] = 2;
                            compships[i - 1, j + 1] = 2;
                            compships[i - 2, j + 1] = 2;

                            if (i != 2 && compships[i - 3, j + 1] == 0)
                            {
                                compships[i - 3, j + 1] = 2;
                            }
                        }
                    }
                    else if (downBuild)
                    {
                        compships[i, j] = 1;
                        
                        if (count == 1)
                        {
                            compShip3_1[0] = i;
                            compShip3_1[1] = j;
                            compShip3_1[2] = i + 1;
                            compShip3_1[3] = j;
                            compShip3_1[4] = i + 2;
                            compShip3_1[5] = j;
                        }
                        else
                        {
                            compShip3_2[0] = i;
                            compShip3_2[1] = j;
                            compShip3_2[2] = i + 1;
                            compShip3_2[3] = j;
                            compShip3_2[4] = i + 2;
                            compShip3_2[5] = j;
                        }
                        
                        if (i != 0 && compships[i - 1, j] == 0)
                        {
                            compships[i - 1, j] = 2;
                        }

                        compships[i + 1, j] = 1;
                        compships[i + 2, j] = 1;

                        if (i != 7 && compships[i + 3, j] == 0)
                        {
                            compships[i + 3, j] = 2;
                        }

                        if (j != 0 && compships[i, j - 1] == 0 && compships[i + 1, j - 1] == 0 &&
                            compships[i + 2, j - 1] == 0)
                        {
                            if (i != 0 && compships[i - 1, j - 1] == 0)
                            {
                                compships[i - 1, j - 1] = 2;
                            }

                            compships[i, j - 1] = 2;
                            compships[i + 1, j - 1] = 2;
                            compships[i + 2, j - 1] = 2;

                            if (i != 7 && compships[i + 3, j - 1] == 0)
                            {
                                compships[i + 3, j - 1] = 2;
                            }
                        }

                        if (j != 9 && compships[i, j + 1] == 0 && compships[i + 1, j + 1] == 0 &&
                            compships[i + 2, j + 1] == 0)
                        {
                            if (i != 0 && compships[i - 1, j + 1] == 0)
                            {
                                compships[i - 1, j + 1] = 2;
                            }

                            compships[i, j + 1] = 2;
                            compships[i + 1, j + 1] = 2;
                            compships[i + 2, j + 1] = 2;

                            if (i != 7 && compships[i + 3, j + 1] == 0)
                            {
                                compships[i + 3, j + 1] = 2;
                            }
                        }
                    }
                    else
                    {
                        comp_field();
                        return;
                    }

                }

                if (count > 2 && count < 6)
                {
                    tempbool = checked_edges(i, j, 8, 1);
                    i_edge_u = tempbool[2];
                    i_edge_d = tempbool[3];
                    upDeadZone = tempbool[6];
                    downDeadZone = tempbool[7];
                    diagLeftDown = tempbool[11];
                    diagLeftUp = tempbool[9];
                    diagRightDown = tempbool[10];
                    diagRightUp = tempbool[8];

                    if (i_edge_u && downDeadZone)
                    {
                        if (i != 1)
                        {
                            if (compships[i - 2, j] != 1 && (diagLeftUp || diagRightUp))
                            {
                                if (diagLeftUp && diagRightUp)
                                {
                                    string up = "up";
                                    
                                    if (CheckedAroundShip(i,j,2,up))
                                    {
                                        upBuild = true;
                                    }
                                }
                                else
                                {
                                    bool fCheck = false, secCheck = false;
                                    if (j == 0 && !diagLeftUp)
                                    {
                                        fCheck = true;
                                    }

                                    if (j == 9 && !diagRightUp)
                                    {
                                        secCheck = true;
                                    }

                                    if (fCheck || secCheck)
                                    {
                                        string up = "up";
                                    
                                        if (CheckedAroundShip(i,j,2,up))
                                        {
                                            upBuild = true;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            string up = "up";
                                    
                            if (CheckedAroundShip(i,j,2,up))
                            {
                                upBuild = true;
                            }
                        }
                        
                    }
                    else if (i_edge_d && upDeadZone)
                    {
                        if (i != 8)
                        {
                            if (compships[i + 2, j] != 1 && (diagLeftDown || diagRightDown))
                            {
                                if (diagLeftDown && diagRightDown)
                                {
                                    string down = "down";
                                    
                                    if (CheckedAroundShip(i,j,2,down))
                                    {
                                        downBuild = true;
                                    }
                                }
                                else
                                {
                                    bool fCheck = false, secCheck = false;
                                    if (j == 0 && !diagLeftDown)
                                    {
                                        fCheck = true;
                                    }

                                    if (j == 9 && !diagRightDown)
                                    {
                                        secCheck = true;
                                    }

                                    if (fCheck || secCheck)
                                    {
                                        string down = "down";
                                    
                                        if (CheckedAroundShip(i,j,2,down))
                                        {
                                            downBuild = true;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            string down = "down";
                                    
                            if (CheckedAroundShip(i,j,2,down))
                            {
                                downBuild = true;
                            }
                        }
                    }

                    if (upBuild)
                    {
                        compships[i, j] = 1;
                        
                        if (count == 3)
                        {
                            compShip2_1[0] = i;
                            compShip2_1[1] = j;
                            compShip2_1[2] = i - 1;
                            compShip2_1[3] = j;
                        } else if (count == 4)
                        {
                            compShip2_2[0] = i;
                            compShip2_2[1] = j;
                            compShip2_2[2] = i - 1;
                            compShip2_2[3] = j;
                        } else if (count == 5)
                        {
                            compShip2_3[0] = i;
                            compShip2_3[1] = j;
                            compShip2_3[2] = i - 1;
                            compShip2_3[3] = j;
                        }
                        
                        if (i != 9 && compships[i + 1, j] == 0)
                        {
                            compships[i + 1, j] = 2;
                        }

                        compships[i - 1, j] = 1;

                        if (i != 1 && compships[i - 2, j] == 0)
                        {
                            compships[i - 2, j] = 2;
                        }

                        if (j != 0 && compships[i, j - 1] == 0 && compships[i - 1, j - 1] == 0)
                        {
                            if (i != 9 && compships[i + 1, j - 1] == 0)
                            {
                                compships[i + 1, j - 1] = 2;
                            }

                            compships[i, j - 1] = 2;
                            compships[i - 1, j - 1] = 2;

                            if (i != 1 && compships[i - 2, j - 1] == 0)
                            {
                                compships[i - 2, j - 1] = 2;
                            }
                        }

                        if (j != 9 && compships[i, j + 1] == 0 && compships[i - 1, j + 1] == 0)
                        {
                            if (i != 9 && compships[i + 1, j + 1] == 0)
                            {
                                compships[i + 1, j + 1] = 2;
                            }

                            compships[i, j + 1] = 2;
                            compships[i - 1, j + 1] = 2;

                            if (i != 1 && compships[i - 2, j + 1] == 0)
                            {
                                compships[i - 2, j + 1] = 2;
                            }
                        }
                    }
                    else if (downBuild)
                    {
                        compships[i, j] = 1;
                        
                        if (count == 3)
                        {
                            compShip2_1[0] = i;
                            compShip2_1[1] = j;
                            compShip2_1[2] = i + 1;
                            compShip2_1[3] = j;
                        } else if (count == 4)
                        {
                            compShip2_2[0] = i;
                            compShip2_2[1] = j;
                            compShip2_2[2] = i + 1;
                            compShip2_2[3] = j;
                        } else if (count == 5)
                        {
                            compShip2_3[0] = i;
                            compShip2_3[1] = j;
                            compShip2_3[2] = i + 1;
                            compShip2_3[3] = j;
                        }
                        
                        if (i != 0 && compships[i - 1, j] == 0)
                        {
                            compships[i - 1, j] = 2;
                        }

                        compships[i + 1, j] = 1;

                        if (i != 8 && compships[i + 2, j] == 0)
                        {
                            compships[i + 2, j] = 2;
                        }

                        if (j != 0 && compships[i, j - 1] == 0 && compships[i + 1, j - 1] == 0)
                        {
                            if (i != 0 && compships[i - 1, j - 1] == 0)
                            {
                                compships[i - 1, j - 1] = 2;
                            }

                            compships[i, j - 1] = 2;
                            compships[i + 1, j - 1] = 2;

                            if (i != 8 && compships[i + 2, j - 1] == 0)
                            {
                                compships[i + 2, j - 1] = 2;
                            }
                        }

                        if (j != 9 && compships[i, j + 1] == 0 && compships[i + 1, j + 1] == 0)
                        {
                            if (i != 0 && compships[i - 1, j + 1] == 0)
                            {
                                compships[i - 1, j + 1] = 2;
                            }

                            compships[i, j + 1] = 2;
                            compships[i + 1, j + 1] = 2;

                            if (i != 8 && compships[i + 2, j + 1] == 0)
                            {
                                compships[i + 2, j + 1] = 2;
                            }
                        }
                    }
                    else
                    {
                        comp_field();
                        return;
                    }
                }

                if (count > 5)
                {
                    if (diagLeftDown && diagLeftUp && diagRightDown && diagRightUp)
                    {
                        compships[i, j] = 1;

                        if (i != 0)
                        {
                            compships[i - 1, j] = 2;

                            if (j != 0)
                            {
                                compships[i - 1, j - 1] = 2;
                            }

                            if (j != 9)
                            {
                                compships[i - 1, j + 1] = 2;
                            }
                        }

                        if (i != 9)
                        {
                            compships[i + 1, j] = 2;

                            if (j != 0)
                            {
                                compships[i + 1, j - 1] = 2;
                            }

                            if (j != 9)
                            {
                                compships[i + 1, j + 1] = 2;
                            }
                        }

                        if (j != 9)
                        {
                            compships[i, j + 1] = 2;
                        }

                        if (j != 0)
                        {
                            compships[i, j - 1] = 2;
                        }
                    }
                    else
                    {
                        comp_field();
                        return;
                    }
                }
            }
        }

        private bool[] checked_edges(int i, int j, int cc, int cc2)
        {
            bool[] tempbool = new bool[12];

            if (cc == 6)
            {
                if (j < cc)
                {
                    tempbool[0] = true;
                }
                

                if (j > cc2)
                {
                    tempbool[1] = true;
                }
                
                
                if (i > cc2)
                {
                    tempbool[2] = true;
                }
                

                if (i < cc)
                {
                    tempbool[3] = true;
                }
            }
            else if (cc == 7)
            {
                if (j > 2 && i > 0 && compships[i - 1, j - 3] == 0)
                {
                    tempbool[9] = true; //левый верхний угол 
                }

                if (j > 2 && i < 9 && compships[i + 1, j - 3] == 0)
                {
                    tempbool[11] = true; //левый нижний угол
                }
                
                if (j < 7 && i > 0 && compships[i - 1, j + 3] == 0)
                {
                    tempbool[8] = true; //правый верхний угол
                }

                if (j < 7 && i < 9 && compships[i + 1, j + 3] == 0)
                {
                    tempbool[10] = true; //правый нижний угол
                }
                
                if (j <= cc)
                {
                    tempbool[0] = true;
                }
                
                if (j < 8 && compships[i, j + 1] == 0 && compships[i, j + 2] == 0)
                {
                    tempbool[4] = true;
                }

                if (j >= cc2)
                {
                    tempbool[1] = true;
                }

                if (j > 1 && compships[i, j - 1] == 0 && compships[i, j - 2] == 0)
                {
                    tempbool[5] = true;
                }

                if (i >= cc2)
                {
                    tempbool[2] = true;
                }
                
                if (i < 8 && compships[i + 1, j] == 0 && compships[i + 2, j] == 0)
                {
                    tempbool[6] = true;
                }

                if (i <= cc)
                {
                    tempbool[3] = true;
                }

                if (i > 1 && compships[i - 1, j] == 0 && compships[i - 2, j] == 0)
                {
                    tempbool[7] = true;
                }

            }
            else if (cc == 8)
            {
                if (j <= cc)
                {
                    tempbool[0] = true;
                }
                
                if (j < 9 && compships[i, j + 1] == 0)
                {
                    tempbool[4] = true;
                }

                if (j < 8 && i > 0 && compships[i - 1, j + 2] == 0)
                {
                    tempbool[8] = true; //правый верхний угол
                }

                if (j < 8 && i < 9 && compships[i + 1, j + 2] == 0)
                {
                    tempbool[10] = true; //правый нижний угол
                }

                if (j >= cc2)
                {
                    tempbool[1] = true;
                }

                if (j > 0 && compships[i, j - 1] == 0)
                {
                    tempbool[5] = true;
                }

                if (j > 1 && i > 0 && compships[i - 1, j - 2] == 0)
                {
                    tempbool[9] = true; //левый верхний угол 
                }

                if (j > 1 && i < 9 && compships[i + 1, j - 2] == 0)
                {
                    tempbool[11] = true; //левый нижний угол
                }

                if (i >= cc2)
                {
                    tempbool[2] = true;
                }
                
                if (i < 9 && compships[i + 1, j] == 0)
                {
                    tempbool[6] = true;
                }

                if (i <= cc)
                {
                    tempbool[3] = true;
                }

                if (i > 0 && compships[i - 1, j] == 0)
                {
                    tempbool[7] = true;
                }

            }
            else if (cc == 9)
            {
                if (i != 9 && j != 0 && compships[i + 1, j - 1] != 1)
                {
                    tempbool[9] = true; //лево-вверх
                }
                else if (i == 9 || j == 0)
                {
                    tempbool[9] = true;
                }

                if (i != 9 && j != 9 && compships[i + 1, j + 1] != 1)
                {
                    tempbool[8] = true; //право-вверх
                }
                else if (i == 9 || j == 9)
                {
                    tempbool[8] = true;
                }

                if (i != 0 && j != 0 && compships[i - 1, j - 1] != 1)
                {
                    tempbool[11] = true; //лево-низ
                }
                else if (i == 0 || j == 0)
                {
                    tempbool[11] = true;
                }

                if (i != 0 && j != 9 && compships[i - 1, j + 1] != 1)
                {
                    tempbool[10] = true; //право-низ
                }
                else if (i == 0 || j == 9)
                {
                    tempbool[10] = true;
                }
            }
            return tempbool;
        }

        private bool CheckedAroundShip(int i, int j,int flag,string navigate)
        {
            bool[] truesarr = new bool[7];
            
            if (flag == 2 && navigate == "left")
            {
                if (i != 0 && compships[i - 1,j] != 1) //вверх
                {
                    truesarr[0] = true;
                }
                else if (i == 0)
                {
                    truesarr[0] = true;
                    truesarr[3] = true;
                    truesarr[5] = true;
                }

                if (i != 9 && compships[i + 1, j] != 1) //вниз
                {
                    truesarr[1] = true;
                }
                else if (i == 9)
                {
                    truesarr[1] = true;
                    truesarr[4] = true;
                    truesarr[6] = true;
                }

                if (j != 9 && compships[i,j + 1] != 1) //вправо
                {
                    truesarr[2] = true;
                }
                else if (j == 9)
                {
                    truesarr[2] = true;
                    truesarr[5] = true;
                    truesarr[6] = true;
                }

                if (i != 0 && compships[i - 1, j - 1] != 1) //влево-вверх
                {
                    truesarr[3] = true;
                }

                if (i != 9 && compships[i + 1, j - 1] != 1) //влево-вниз
                {
                    truesarr[4] = true;
                }

                if (j != 9 && i != 0 && compships[i - 1, j + 1] != 1) //вправо вверх
                {
                    truesarr[5] = true;
                }

                if (j != 9 && i != 9 && compships[i + 1, j + 1] != 1) //вправо вниз
                {
                    truesarr[6] = true;
                }
            }
            else if (flag == 2 && navigate == "right")
            {
                if (i != 0 && compships[i - 1,j] != 1) //вверх
                {
                    truesarr[0] = true;
                }
                else if (i == 0)
                {
                    truesarr[0] = true;
                    truesarr[3] = true;
                    truesarr[5] = true;
                }
                
                if (i != 9 && compships[i + 1, j] != 1) //вниз
                {
                    truesarr[1] = true;
                }
                else if (i == 9)
                {
                    truesarr[1] = true;
                    truesarr[4] = true;
                    truesarr[6] = true;
                }

                if (j != 0 && compships[i, j - 1] != 1) //влево
                {
                    truesarr[2] = true;
                }
                else if (j == 0)
                {
                    truesarr[2] = true;
                    truesarr[5] = true;
                    truesarr[6] = true;
                }

                if (i != 0 && compships[i - 1, j + 1] != 1) //вправо-вверх
                {
                    truesarr[3] = true;
                }

                if (i != 9 && compships[i + 1, j + 1] != 1) //вправо-вниз
                {
                    truesarr[4] = true;
                }

                if (j != 0 && i != 0 && compships[i - 1, j - 1] != 1) //влево-вверх
                {
                    truesarr[5] = true;
                }

                if (j != 0 && i != 9 && compships[i + 1, j - 1] != 1) //влево-вниз
                {
                    truesarr[6] = true;
                }
            }
            else if (flag == 2 && navigate == "up")
            {
                if (j != 0 && compships[i, j - 1] != 1) //влево
                {
                    truesarr[0] = true;
                }
                else if (j == 0)
                {
                    truesarr[0] = true;
                    truesarr[3] = true;
                    truesarr[5] = true;
                }

                if (j != 9 && compships[i, j + 1] != 1) //вправо
                {
                    truesarr[1] = true;
                }
                else if (j == 9)
                {
                    truesarr[1] = true;
                    truesarr[4] = true;
                    truesarr[6] = true;
                }

                if (i != 9 && compships[i + 1, j] != 1) //вниз
                {
                    truesarr[2] = true;
                }
                else if (i == 9)
                {
                    truesarr[2] = true;
                    truesarr[5] = true;
                    truesarr[6] = true;
                }

                if (j != 0 && compships[i - 1, j - 1] != 1) //влево-вверх
                {
                    truesarr[3] = true;
                }

                if (j != 9 && compships[i - 1, j + 1] != 1) //вправо-вверх
                {
                    truesarr[4] = true;
                }

                if (i != 9 && j != 0 && compships[i + 1, j - 1] != 1) //влево-вниз
                {
                    truesarr[5] = true;
                }

                if (i != 9 && j != 9 && compships[i + 1, j + 1] != 1) //вправо-вниз
                {
                    truesarr[6] = true;
                }
            }
            else if (flag == 2 && navigate == "down")
            {
                if (j != 0 && compships[i, j - 1] != 1) //влево
                {
                    truesarr[0] = true;
                }
                else if (j == 0)
                {
                    truesarr[0] = true;
                    truesarr[3] = true;
                    truesarr[5] = true;
                }

                if (j != 9 && compships[i, j + 1] != 1) //вправо
                {
                    truesarr[1] = true;
                }
                else if (j == 9)
                {
                    truesarr[1] = true;
                    truesarr[4] = true;
                    truesarr[6] = true;
                }

                if (i != 0 && compships[i - 1, j] != 1) //вверх
                {
                    truesarr[2] = true;
                }
                else if (i == 0)
                {
                    truesarr[2] = true;
                    truesarr[5] = true;
                    truesarr[6] = true;
                }

                if (j != 0 && compships[i + 1, j - 1] != 1) //влево-вниз
                {
                    truesarr[3] = true;
                }

                if (j != 9 && compships[i + 1, j + 1] != 1) //вправо-вниз
                {
                    truesarr[4] = true;
                }

                if (i != 0 && j != 0 && compships[i - 1, j - 1] != 1) //вверх-влево
                {
                    truesarr[5] = true;
                }

                if (i != 0 && j != 9 && compships[i - 1, j + 1] != 1) //вверх-вправо
                {
                    truesarr[6] = true;
                }
            }
            else if (flag == 3 && navigate == "left")
            {
                if (i != 0 && compships[i - 1,j] != 1) //вверх
                {
                    truesarr[0] = true;
                }
                else if (i == 0)
                {
                    truesarr[0] = true;
                    truesarr[3] = true;
                    truesarr[5] = true;
                }

                if (i != 9 && compships[i + 1, j] != 1) //вниз
                {
                    truesarr[1] = true;
                }
                else if (i == 9)
                {
                    truesarr[1] = true;
                    truesarr[4] = true;
                    truesarr[6] = true;
                }

                if (j != 9 && compships[i,j + 1] != 1) //вправо
                {
                    truesarr[2] = true;
                }
                else if (j == 9)
                {
                    truesarr[2] = true;
                    truesarr[5] = true;
                    truesarr[6] = true;
                }

                if (i != 0 && compships[i - 1, j - 1] != 1 && compships[i - 1, j - 2] != 1) //влево-вверх
                {
                    truesarr[3] = true;
                }

                if (i != 9 && compships[i + 1, j - 1] != 1 && compships[i + 1, j - 2] != 1) //влево-вниз
                {
                    truesarr[4] = true;
                }

                if (j != 9 && i != 0 && compships[i - 1, j + 1] != 1) //вправо вверх
                {
                    truesarr[5] = true;
                }

                if (j != 9 && i != 9 && compships[i + 1, j + 1] != 1) //вправо вниз
                {
                    truesarr[6] = true;
                }
            }
            else if (flag == 3 && navigate == "right")
            {
                if (i != 0 && compships[i - 1,j] != 1) //вверх
                {
                    truesarr[0] = true;
                }
                else if (i == 0)
                {
                    truesarr[0] = true;
                    truesarr[3] = true;
                    truesarr[5] = true;
                }
                
                if (i != 9 && compships[i + 1, j] != 1) //вниз
                {
                    truesarr[1] = true;
                }
                else if (i == 9)
                {
                    truesarr[1] = true;
                    truesarr[4] = true;
                    truesarr[6] = true;
                }

                if (j != 0 && compships[i, j - 1] != 1) //влево
                {
                    truesarr[2] = true;
                }
                else if (j == 0)
                {
                    truesarr[2] = true;
                    truesarr[5] = true;
                    truesarr[6] = true;
                }

                if (i != 0 && compships[i - 1, j + 1] != 1 && compships[i - 1, j + 2] != 1) //вправо-вверх
                {
                    truesarr[3] = true;
                }

                if (i != 9 && compships[i + 1, j + 1] != 1 && compships[i + 1, j + 2] != 1) //вправо-вниз
                {
                    truesarr[4] = true;
                }

                if (j != 0 && i != 0 && compships[i - 1, j - 1] != 1) //влево-вверх
                {
                    truesarr[5] = true;
                }

                if (j != 0 && i != 9 && compships[i + 1, j - 1] != 1) //влево-вниз
                {
                    truesarr[6] = true;
                }
            }
            else if (flag == 3 && navigate == "up")
            {
                if (j != 0 && compships[i, j - 1] != 1) //влево
                {
                    truesarr[0] = true;
                }
                else if (j == 0)
                {
                    truesarr[0] = true;
                    truesarr[3] = true;
                    truesarr[5] = true;
                }

                if (j != 9 && compships[i, j + 1] != 1) //вправо
                {
                    truesarr[1] = true;
                }
                else if (j == 9)
                {
                    truesarr[1] = true;
                    truesarr[4] = true;
                    truesarr[6] = true;
                }

                if (i != 9 && compships[i + 1, j] != 1) //вниз
                {
                    truesarr[2] = true;
                }
                else if (i == 9)
                {
                    truesarr[2] = true;
                    truesarr[5] = true;
                    truesarr[6] = true;
                }

                if (j != 0 && compships[i - 1, j - 1] != 1 && compships[i - 2, j - 1] != 1) //влево-вверх
                {
                    truesarr[3] = true;
                }

                if (j != 9 && compships[i - 1, j + 1] != 1 && compships[i - 2, j + 1] != 1) //вправо-вверх
                {
                    truesarr[4] = true;
                }

                if (i != 9 && j != 0 && compships[i + 1, j - 1] != 1) //влево-вниз
                {
                    truesarr[5] = true;
                }

                if (i != 9 && j != 9 && compships[i + 1, j + 1] != 1) //вправо-вниз
                {
                    truesarr[6] = true;
                }
            }
            else if (flag == 3 && navigate == "down")
            {
                if (j != 0 && compships[i, j - 1] != 1) //влево
                {
                    truesarr[0] = true;
                }
                else if (j == 0)
                {
                    truesarr[0] = true;
                    truesarr[3] = true;
                    truesarr[5] = true;
                }

                if (j != 9 && compships[i, j + 1] != 1) //вправо
                {
                    truesarr[1] = true;
                }
                else if (j == 9)
                {
                    truesarr[1] = true;
                    truesarr[4] = true;
                    truesarr[6] = true;
                }

                if (i != 0 && compships[i - 1, j] != 1) //вверх
                {
                    truesarr[2] = true;
                }
                else if (i == 0)
                {
                    truesarr[2] = true;
                    truesarr[5] = true;
                    truesarr[6] = true;
                }

                if (j != 0 && compships[i + 1, j - 1] != 1 && compships[i + 2, j - 1] != 1) //влево-вниз
                {
                    truesarr[3] = true;
                }

                if (j != 9 && compships[i + 1, j + 1] != 1 && compships[i + 2, j + 1] != 1) //вправо-вниз
                {
                    truesarr[4] = true;
                }

                if (i != 0 && j != 0 && compships[i - 1, j - 1] != 1) //вверх-влево
                {
                    truesarr[5] = true;
                }

                if (i != 0 && j != 9 && compships[i - 1, j + 1] != 1) //вверх-вправо
                {
                    truesarr[6] = true;
                }
            }
            
            if (truesarr[0] && truesarr[1] && truesarr[2] && truesarr[3]
                && truesarr[4] && truesarr[5] && truesarr[6])
            {
                return true;
            }
            
            return false;
        }
        
        private void show_unvisible()
        {
            GameField.Visibility = Visibility.Visible;
        }

        private void check_hit_of_ship(Border ttemp, Label ttship)
        {
            GeneralTransform t1 = ttemp.TransformToVisual(this);
            GeneralTransform t2 = ttship.TransformToVisual(this);
            Rect r1 = t1.TransformBounds(new Rect()
                { X = 5, Y = 5, Width = ttemp.ActualWidth - 20, Height = ttemp.ActualHeight - 20 });
            Rect r2 = t2.TransformBounds(new Rect()
                { X = 0, Y = 0, Width = ttship.ActualWidth, Height = ttship.ActualHeight });
            hitShip = r1.IntersectsWith(r2);
            if (hitShip)
            {
                if ((string)ttship.Tag == "destroyed")
                {
                    originalHit[0] = -1;
                    loopComphit = true;
                }
                else
                {
                    catch_ship = ttship;
                }
            }
        }

        private bool hitShip = false;
        private int[] hitCapacity = new int[6];
        private Label catch_ship = null;

        private void check_hit(Border bord)
        {
            if (!hitShip)
            {
                check_hit_of_ship(bord, Ship4);
            }

            if (!hitShip)
            {
                check_hit_of_ship(bord, Ship3_1);
            }

            if (!hitShip)
            {
                check_hit_of_ship(bord, Ship3_2);
            }

            if (!hitShip)
            {
                check_hit_of_ship(bord, Ship2_1);
            }

            if (!hitShip)
            {
                check_hit_of_ship(bord, Ship2_2);
            }

            if (!hitShip)
            {
                check_hit_of_ship(bord, Ship2_3);
            }

            if (!hitShip)
            {
                check_hit_of_ship(bord, Ship1_1);
            }

            if (!hitShip)
            {
                check_hit_of_ship(bord, Ship1_2);
            }

            if (!hitShip)
            {
                check_hit_of_ship(bord, Ship1_3);
            }

            if (!hitShip)
            {
                check_hit_of_ship(bord, Ship1_4);
            }
        }

        private int playerHits = 0;
        private bool hit = false;

        private void GameCellClick(object sender, RoutedEventArgs e)
        {
            Button ttemp = sender as Button;
            check_hitplayer(ttemp);

            if ((string)ttemp.Tag != "false" && (string)ttemp.Tag != "true" && !endGame && startGame)
            {
                if (!hit)
                {
                    ttemp.Background = new SolidColorBrush(Colors.Firebrick);
                    ttemp.Tag = "false";
                    field_with_comphits();
                }
                else if (hit)
                {
                    ttemp.Tag = "true";
                    hit = false;
                    playerHits++;
                    HitOnCompShip(ttemp);
                    if (playerHits == 20)
                    {
                        Win();
                    }
                }
            }
        }

        private int[] hitOnCompCapacity = new int[6];

        private void HitOnCompShip(Button ttemp)
        {
            int[] tempInd = new int[2];
            tempInd = find_name_gbutton(ttemp);
            bool hitOnComp = false;

            killArr2[tempInd[0], tempInd[1]].Visibility = Visibility.Visible;

            int tCount = 1;
            for (int i = 0; i < 8; i+=2)
            {
                if (tempInd[1] == compShip4[tCount] && tempInd[0] == compShip4[i])
                {
                    if (hitOnCompCapacity[0] == 3)
                    {
                        DeleteCompShip(compShip4, 8,ttemp);
                        hitOnComp = true;
                    }
                    else
                    {
                        hitOnCompCapacity[0]++;
                        hitOnComp = true;
                    }
                }
                tCount += 2;
            }

            tCount = 1;
            if (!hitOnComp)
            {
                for (int i = 0; i < 6; i+=2)
                {

                    if (tempInd[1] == compShip3_1[tCount] && tempInd[0] == compShip3_1[i])
                    {
                        if (hitOnCompCapacity[1] == 2)
                        {
                            DeleteCompShip(compShip3_1, 6,ttemp);
                            hitOnComp = true;
                        }
                        else
                        {
                            hitOnCompCapacity[1]++;
                            hitOnComp = true;
                        }
                    }
                    
                    tCount += 2;
                }
            }

            tCount = 1;
            if (!hitOnComp)
            {
                for (int i = 0; i < 6; i+=2)
                {

                    if (tempInd[1] == compShip3_2[tCount] && tempInd[0] == compShip3_2[i])
                    {
                        if (hitOnCompCapacity[2] == 2)
                        {
                            DeleteCompShip(compShip3_2, 6,ttemp);
                            hitOnComp = true;
                        }
                        else
                        {
                            hitOnCompCapacity[2]++;
                            hitOnComp = true;
                        }
                    }
                    
                    tCount += 2;
                }
            }

            tCount = 1;
            if (!hitOnComp)
            {
                for (int i = 0; i < 4; i+=2)
                {

                    if (tempInd[1] == compShip2_1[tCount] && tempInd[0] == compShip2_1[i])
                    {
                        if (hitOnCompCapacity[3] == 1)
                        {
                            DeleteCompShip(compShip2_1, 4,ttemp);
                            hitOnComp = true;
                        }
                        else
                        {
                            hitOnCompCapacity[3]++;
                            hitOnComp = true;
                        }
                    }
                    
                    tCount += 2;
                }
            }

            tCount = 1;
            if (!hitOnComp)
            {
                for (int i = 0; i < 4; i+=2)
                {
                    if (tempInd[1] == compShip2_2[tCount] && tempInd[0] == compShip2_2[i])
                    {
                        if (hitOnCompCapacity[4] == 1)
                        {
                            DeleteCompShip(compShip2_2, 4,ttemp);
                            hitOnComp = true;
                        }
                        else
                        {
                            hitOnCompCapacity[4]++;
                            hitOnComp = true;
                        }
                    }
                    
                    tCount += 2;
                }
            }

            tCount = 1;
            if (!hitOnComp)
            {
                for (int i = 0; i < 4; i+=2)
                {
                    if (tempInd[1] == compShip2_3[tCount] && tempInd[0] == compShip2_3[i])
                    {
                        if (hitOnCompCapacity[5] == 1)
                        {
                            DeleteCompShip(compShip2_3, 4,ttemp);
                            hitOnComp = true;
                        }
                        else
                        {
                            hitOnCompCapacity[5]++;
                            hitOnComp = true;
                        }
                    }
                    
                    tCount += 2;
                }
            }
            
            if (!hitOnComp)
            {
                DeleteCompShip(tempInd, 2,ttemp);
            }
        }

        
        private int count1Ship = 0;

        private void DeleteCompShip(int[] tempCS, int tCount, Button ttemp)
        {
            if (tCount == 2)
            {
                if (count1Ship == 0)
                {
                    Canvas.SetLeft(compRShip1_1, Canvas.GetLeft(ttemp) + (window.ActualWidth - 1220) / 2 + 620);
                    Canvas.SetTop(compRShip1_1, Canvas.GetTop(ttemp) + (window.ActualHeight - field.ActualHeight) / 2);
                    compRShip1_1.Visibility = Visibility.Visible;
                    count1Ship++;
                }
                else if (count1Ship == 1)
                {
                    Canvas.SetLeft(compRShip1_2, Canvas.GetLeft(ttemp) + (window.ActualWidth - 1220) / 2 + 620);
                    Canvas.SetTop(compRShip1_2, Canvas.GetTop(ttemp) + (window.ActualHeight - field.ActualHeight) / 2);
                    compRShip1_2.Visibility = Visibility.Visible;
                    count1Ship++;
                }
                else if (count1Ship == 2)
                {
                    Canvas.SetLeft(compRShip1_3, Canvas.GetLeft(ttemp) + (window.ActualWidth - 1220) / 2 + 620);
                    Canvas.SetTop(compRShip1_3, Canvas.GetTop(ttemp) + (window.ActualHeight - field.ActualHeight) / 2);
                    compRShip1_3.Visibility = Visibility.Visible;
                    count1Ship++;
                }
                else if (count1Ship == 3)
                {
                    Canvas.SetLeft(compRShip1_4, Canvas.GetLeft(ttemp) + (window.ActualWidth - 1220) / 2 + 620);
                    Canvas.SetTop(compRShip1_4, Canvas.GetTop(ttemp) + (window.ActualHeight - field.ActualHeight) / 2);
                    compRShip1_4.Visibility = Visibility.Visible;
                    count1Ship++;
                }
            }

            if (tempCS == compShip2_1)
            {
                if (tempCS[0] == tempCS[2] + 1)
                {
                    Canvas.SetLeft(compRShip2_1, Canvas.GetLeft(game_cells[tempCS[2], tempCS[3]]) + (window.ActualWidth - 1220) / 2 + 620);
                    Canvas.SetTop(compRShip2_1, Canvas.GetTop(game_cells[tempCS[2], tempCS[3]]) + (window.ActualHeight - field.ActualHeight) / 2);
                    compRShip2_1.LayoutTransform = new RotateTransform(90);
                    compRShip2_1.Visibility = Visibility.Visible;
                }
                else if (tempCS[0] == tempCS[2] - 1)
                {
                    Canvas.SetLeft(compRShip2_1, Canvas.GetLeft(game_cells[tempCS[0], tempCS[1]]) + (window.ActualWidth - 1220) / 2 + 620);
                    Canvas.SetTop(compRShip2_1, Canvas.GetTop(game_cells[tempCS[0], tempCS[1]]) + (window.ActualHeight - field.ActualHeight) / 2);
                    compRShip2_1.LayoutTransform = new RotateTransform(90);
                    compRShip2_1.Visibility = Visibility.Visible;
                }
                else if (tempCS[1] == tempCS[3] + 1)
                {
                    Canvas.SetLeft(compRShip2_1, Canvas.GetLeft(game_cells[tempCS[2], tempCS[3]]) + (window.ActualWidth - 1220) / 2 + 620);
                    Canvas.SetTop(compRShip2_1, Canvas.GetTop(game_cells[tempCS[2], tempCS[3]]) + (window.ActualHeight - field.ActualHeight) / 2);
                    compRShip2_1.LayoutTransform = new RotateTransform(0);
                    compRShip2_1.Visibility = Visibility.Visible;
                }
                else if (tempCS[1] == tempCS[3] - 1)
                {
                    Canvas.SetLeft(compRShip2_1, Canvas.GetLeft(game_cells[tempCS[0], tempCS[1]]) + (window.ActualWidth - 1220) / 2 + 620);
                    Canvas.SetTop(compRShip2_1, Canvas.GetTop(game_cells[tempCS[0], tempCS[1]]) + (window.ActualHeight - field.ActualHeight) / 2);
                    compRShip2_1.LayoutTransform = new RotateTransform(0);
                    compRShip2_1.Visibility = Visibility.Visible;
                }
            }

            if (tempCS == compShip2_2)
            {
                if (tempCS[0] == tempCS[2] + 1)
                {
                    Canvas.SetLeft(compRShip2_2, Canvas.GetLeft(game_cells[tempCS[2], tempCS[3]]) + (window.ActualWidth - 1220) / 2 + 620);
                    Canvas.SetTop(compRShip2_2, Canvas.GetTop(game_cells[tempCS[2], tempCS[3]]) + (window.ActualHeight - field.ActualHeight) / 2);
                    compRShip2_2.LayoutTransform = new RotateTransform(90);
                    compRShip2_2.Visibility = Visibility.Visible;
                }
                else if (tempCS[0] == tempCS[2] - 1)
                {
                    Canvas.SetLeft(compRShip2_2, Canvas.GetLeft(game_cells[tempCS[0], tempCS[1]]) + (window.ActualWidth - 1220) / 2 + 620);
                    Canvas.SetTop(compRShip2_2, Canvas.GetTop(game_cells[tempCS[0], tempCS[1]]) + (window.ActualHeight - field.ActualHeight) / 2);
                    compRShip2_2.LayoutTransform = new RotateTransform(90);
                    compRShip2_2.Visibility = Visibility.Visible;
                }
                else if (tempCS[1] == tempCS[3] + 1)
                {
                    Canvas.SetLeft(compRShip2_2, Canvas.GetLeft(game_cells[tempCS[2], tempCS[3]]) + (window.ActualWidth - 1220) / 2 + 620);
                    Canvas.SetTop(compRShip2_2, Canvas.GetTop(game_cells[tempCS[2], tempCS[3]]) + (window.ActualHeight - field.ActualHeight) / 2);
                    compRShip2_2.LayoutTransform = new RotateTransform(0);
                    compRShip2_2.Visibility = Visibility.Visible;
                }
                else if (tempCS[1] == tempCS[3] - 1)
                {
                    Canvas.SetLeft(compRShip2_2, Canvas.GetLeft(game_cells[tempCS[0], tempCS[1]]) + (window.ActualWidth - 1220) / 2 + 620);
                    Canvas.SetTop(compRShip2_2, Canvas.GetTop(game_cells[tempCS[0], tempCS[1]]) + (window.ActualHeight - field.ActualHeight) / 2);
                    compRShip2_2.LayoutTransform = new RotateTransform(0);
                    compRShip2_2.Visibility = Visibility.Visible;
                }
            }

            if (tempCS == compShip2_3)
            {
                if (tempCS[0] == tempCS[2] + 1)
                {
                    Canvas.SetLeft(compRShip2_3, Canvas.GetLeft(game_cells[tempCS[2], tempCS[3]]) + (window.ActualWidth - 1220) / 2 + 620);
                    Canvas.SetTop(compRShip2_3, Canvas.GetTop(game_cells[tempCS[2], tempCS[3]]) + (window.ActualHeight - field.ActualHeight) / 2);
                    compRShip2_3.LayoutTransform = new RotateTransform(90);
                    compRShip2_3.Visibility = Visibility.Visible;
                }
                else if (tempCS[0] == tempCS[2] - 1)
                {
                    Canvas.SetLeft(compRShip2_3, Canvas.GetLeft(game_cells[tempCS[0], tempCS[1]]) + (window.ActualWidth - 1220) / 2 + 620);
                    Canvas.SetTop(compRShip2_3, Canvas.GetTop(game_cells[tempCS[0], tempCS[1]]) + (window.ActualHeight - field.ActualHeight) / 2);
                    compRShip2_3.LayoutTransform = new RotateTransform(90);
                    compRShip2_3.Visibility = Visibility.Visible;
                }
                else if (tempCS[1] == tempCS[3] + 1)
                {
                    Canvas.SetLeft(compRShip2_3, Canvas.GetLeft(game_cells[tempCS[2], tempCS[3]]) + (window.ActualWidth - 1220) / 2 + 620);
                    Canvas.SetTop(compRShip2_3, Canvas.GetTop(game_cells[tempCS[2], tempCS[3]]) + (window.ActualHeight - field.ActualHeight) / 2);
                    compRShip2_3.LayoutTransform = new RotateTransform(0);
                    compRShip2_3.Visibility = Visibility.Visible;
                }
                else if (tempCS[1] == tempCS[3] - 1)
                {
                    Canvas.SetLeft(compRShip2_3, Canvas.GetLeft(game_cells[tempCS[0], tempCS[1]]) + (window.ActualWidth - 1220) / 2 + 620);
                    Canvas.SetTop(compRShip2_3, Canvas.GetTop(game_cells[tempCS[0], tempCS[1]]) + (window.ActualHeight - field.ActualHeight) / 2);
                    compRShip2_3.LayoutTransform = new RotateTransform(0);
                    compRShip2_3.Visibility = Visibility.Visible;
                }
            }

            if (tempCS == compShip3_1)
            {
                if (tempCS[0] == tempCS[2] + 1)
                {
                    Canvas.SetLeft(compRShip3_1, Canvas.GetLeft(game_cells[tempCS[4], tempCS[5]]) + (window.ActualWidth - 1220) / 2 + 620);
                    Canvas.SetTop(compRShip3_1, Canvas.GetTop(game_cells[tempCS[4], tempCS[5]]) + (window.ActualHeight - field.ActualHeight) / 2);
                    compRShip3_1.LayoutTransform = new RotateTransform(90);
                    compRShip3_1.Visibility = Visibility.Visible;
                }
                else if (tempCS[0] == tempCS[2] - 1)
                {
                    Canvas.SetLeft(compRShip3_1, Canvas.GetLeft(game_cells[tempCS[0], tempCS[1]]) + (window.ActualWidth - 1220) / 2 + 620);
                    Canvas.SetTop(compRShip3_1, Canvas.GetTop(game_cells[tempCS[0], tempCS[1]]) + (window.ActualHeight - field.ActualHeight) / 2);
                    compRShip3_1.LayoutTransform = new RotateTransform(90);
                    compRShip3_1.Visibility = Visibility.Visible;
                }
                else if (tempCS[1] == tempCS[3] + 1)
                {
                    Canvas.SetLeft(compRShip3_1, Canvas.GetLeft(game_cells[tempCS[4], tempCS[5]]) + (window.ActualWidth - 1220) / 2 + 620);
                    Canvas.SetTop(compRShip3_1, Canvas.GetTop(game_cells[tempCS[4], tempCS[5]]) + (window.ActualHeight - field.ActualHeight) / 2);
                    compRShip3_1.LayoutTransform = new RotateTransform(0);
                    compRShip3_1.Visibility = Visibility.Visible;
                }
                else if (tempCS[1] == tempCS[3] - 1)
                {
                    Canvas.SetLeft(compRShip3_1, Canvas.GetLeft(game_cells[tempCS[0], tempCS[1]]) + (window.ActualWidth - 1220) / 2 + 620);
                    Canvas.SetTop(compRShip3_1, Canvas.GetTop(game_cells[tempCS[0], tempCS[1]]) + (window.ActualHeight - field.ActualHeight) / 2);
                    compRShip3_1.LayoutTransform = new RotateTransform(0);
                    compRShip3_1.Visibility = Visibility.Visible;
                }
            }
        


            if (tempCS == compShip3_2)
            {
                if (tempCS[0] == tempCS[2] + 1)
                {
                    Canvas.SetLeft(compRShip3_2,Canvas.GetLeft(game_cells[tempCS[4],tempCS[5]]) + (window.ActualWidth - 1220) / 2 + 620);
                    Canvas.SetTop(compRShip3_2, Canvas.GetTop(game_cells[tempCS[4],tempCS[5]]) + (window.ActualHeight - field.ActualHeight) / 2);
                    compRShip3_2.LayoutTransform = new RotateTransform(90);
                    compRShip3_2.Visibility = Visibility.Visible;
                } 
                else if (tempCS[0] == tempCS[2] - 1)
                {
                    Canvas.SetLeft(compRShip3_2,Canvas.GetLeft(game_cells[tempCS[0],tempCS[1]]) + (window.ActualWidth - 1220) / 2 + 620);
                    Canvas.SetTop(compRShip3_2, Canvas.GetTop(game_cells[tempCS[0],tempCS[1]]) + (window.ActualHeight - field.ActualHeight) / 2);
                    compRShip3_2.LayoutTransform = new RotateTransform(90);
                    compRShip3_2.Visibility = Visibility.Visible;
                }
                else if (tempCS[1] == tempCS[3] + 1)
                {
                    Canvas.SetLeft(compRShip3_2,Canvas.GetLeft(game_cells[tempCS[4],tempCS[5]]) + (window.ActualWidth - 1220) / 2 + 620);
                    Canvas.SetTop(compRShip3_2, Canvas.GetTop(game_cells[tempCS[4],tempCS[5]]) + (window.ActualHeight - field.ActualHeight) / 2);
                    compRShip3_2.LayoutTransform = new RotateTransform(0);
                    compRShip3_2.Visibility = Visibility.Visible;
                }
                else if (tempCS[1] == tempCS[3] - 1)
                {
                    Canvas.SetLeft(compRShip3_2,Canvas.GetLeft(game_cells[tempCS[0],tempCS[1]]) + (window.ActualWidth - 1220) / 2 + 620);
                    Canvas.SetTop(compRShip3_2, Canvas.GetTop(game_cells[tempCS[0],tempCS[1]]) + (window.ActualHeight - field.ActualHeight) / 2);
                    compRShip3_2.LayoutTransform = new RotateTransform(0);
                    compRShip3_2.Visibility = Visibility.Visible;
                }
            }
            
            if (tempCS == compShip4)
            {
                if (tempCS[0] == tempCS[2] + 1)
                {
                    Canvas.SetLeft(compRShip4,Canvas.GetLeft(game_cells[tempCS[6],tempCS[7]]) + (window.ActualWidth - 1220) / 2 + 620);
                    Canvas.SetTop(compRShip4, Canvas.GetTop(game_cells[tempCS[6],tempCS[7]]) + (window.ActualHeight - field.ActualHeight) / 2);
                    compRShip4.LayoutTransform = new RotateTransform(90);
                    compRShip4.Visibility = Visibility.Visible;
                } 
                else if (tempCS[0] == tempCS[2] - 1)
                {
                    Canvas.SetLeft(compRShip4,Canvas.GetLeft(game_cells[tempCS[0],tempCS[1]]) + (window.ActualWidth - 1220) / 2 + 620);
                    Canvas.SetTop(compRShip4, Canvas.GetTop(game_cells[tempCS[0],tempCS[1]]) + (window.ActualHeight - field.ActualHeight) / 2);
                    compRShip4.LayoutTransform = new RotateTransform(90);
                    compRShip4.Visibility = Visibility.Visible;
                }
                else if (tempCS[1] == tempCS[3] + 1)
                {
                    Canvas.SetLeft(compRShip4,Canvas.GetLeft(game_cells[tempCS[6],tempCS[7]]) + (window.ActualWidth - 1220) / 2 + 620);
                    Canvas.SetTop(compRShip4, Canvas.GetTop(game_cells[tempCS[6],tempCS[7]]) + (window.ActualHeight - field.ActualHeight) / 2);
                    compRShip4.LayoutTransform = new RotateTransform(0);
                    compRShip4.Visibility = Visibility.Visible;
                }
                else if (tempCS[1] == tempCS[3] - 1)
                {
                    Canvas.SetLeft(compRShip4,Canvas.GetLeft(game_cells[tempCS[0],tempCS[1]]) + (window.ActualWidth - 1220) / 2 + 620);
                    Canvas.SetTop(compRShip4, Canvas.GetTop(game_cells[tempCS[0],tempCS[1]]) + (window.ActualHeight - field.ActualHeight) / 2);
                    compRShip4.LayoutTransform = new RotateTransform(0);
                    compRShip4.Visibility = Visibility.Visible;
                }
            }
        }
        
        private int[] originalHit = new int[2];
        private bool hitTrue = false;
        private int csds = 0;
        
        private void comphit()
        {
            if (originalHit[0] == -1)
            {
                originalHit[0] = ran.Next(10);
                originalHit[1] = ran.Next(10);
                csds++;
            }

            if (csds == 1)
            {
                originalHit[0] = 2;
                originalHit[1] = 2;
            } else if (csds == 2)
            {
                originalHit[0] = 6;
                originalHit[1] = 2;
            } else if (csds == 3)
            {
                originalHit[0] = 4;
                originalHit[1] = 2;
            }

            int i = originalHit[0];
            int j = originalHit[1];

            if (tryHitShip)
            {
                if_no_hit(i,j);
            }
            else if (reverse)
            {
                if_hit(i,j);
            }
            else
            {
                check_hit(cells[i, j]);
            }

            if ((string)cells[i, j].Tag != "miss" && (string)cells[i,j].Tag != "hit" && (string)cells[i,j].Tag != "deadzone")
            {
                if (hitShip)
                {
                    ifCompHitOnPlayerShip(i,j);
                    cells[i, j].Tag = "hit";
                    hit_on_ship();
                    if (!kill1ship)
                    {
                        if_hit(i, j);
                    }
                }
                else
                {
                    cells[i, j].Background = new SolidColorBrush(Colors.Firebrick);
                    cells[i, j].Tag = "miss";
                }
            }
            else if (!tryHitShip)
            {
                originalHit[0] = -1;
                hitShip = false;
                loopComphit = true;
            }

            if (hitTrue)
            {
                tryHitShip = false;
                hitTrue = false;
            }
            
            killShip = false;
            kill1ship = false;
        }

        private int[] positionsOfHit = new int[4];
        private bool firstShot = true;
        private bool tryHitShip = false;
        private bool jL = true, jR = true, iU = true, iD = true;
        private int reverseJ = 0, reverseI = 0;
        private bool reverse = false, loopIf_hit = true;
        private bool reverseFromCheckLayWays = false;

        private void CheckLayWays(int i, int j)
        {
            if (iU && ((string)cells[i - 1, j].Tag == "miss" || (string)cells[i - 1,j].Tag == "deadzone"))
            {
                if (positionsOfHit[0] == 1)
                {
                    reverseI = 1;
                    reverse = true;
                    reverseFromCheckLayWays = true;
                }
                positionsOfHit[0] = 2;
            }
            else if (positionsOfHit[0] != 1)
            {
                positionsOfHit[0] = 0;
            }
            
            if (jR && ((string)cells[i, j + 1].Tag == "miss" || (string)cells[i,j + 1].Tag == "deadzone"))
            {
                if (positionsOfHit[1] == 1)
                {
                    reverseJ = 1;
                    reverse = true;
                    reverseFromCheckLayWays = true;
                }
                positionsOfHit[1] = 2;
            }
            else if (positionsOfHit[1] != 1)
            {
                positionsOfHit[1] = 0;
            }
            
            if (iD && ((string)cells[i + 1, j].Tag == "miss" || (string)cells[i + 1,j].Tag == "deadzone"))
            {
                if (positionsOfHit[2] == 1)
                {
                    reverseI = 2;
                    reverse = true;
                    reverseFromCheckLayWays = true;
                }
                positionsOfHit[2] = 2;
            }
            else if (positionsOfHit[2] != 1)
            {
                positionsOfHit[2] = 0;
            }
            
            if (jL && ((string)cells[i, j - 1].Tag == "miss" || (string)cells[i,j - 1].Tag == "deadzone"))
            {
                if (positionsOfHit[3] == 1)
                {
                    reverseJ = 2;
                    reverse = true;
                    reverseFromCheckLayWays = true;
                }
                positionsOfHit[3] = 2;
            }
            else if (positionsOfHit[3] != 1)
            {
                positionsOfHit[3] = 0;
            }
        }
        
        private void CheckHitEdges(int I, int J)
        {
            if (J == 0)
            {
                jL = false;
            } else
            {
                jL = true;
            }

            if (J == 9)
            {
                jR = false;
            }
            else
            {
                jR = true;
            }

            if (I == 0)
            {
                iU = false;
            }
            else
            {
                iU = true;
            }

            if (I == 9)
            {
                iD = false;
            }
            else
            {
                iD = true;
            }

        }
        
        private void if_hit(int i, int j)
        {
            hitShip = false;
            bool thisRun = false;
            int I = 0, J = 0;

            I = i;
            J = j;

            while (loopIf_hit)
            {
                CheckHitEdges(I,J);
                CheckLayWays(I,J);

                if (reverseFromCheckLayWays)
                {
                    reverseFromCheckLayWays = false;
                    I = originalHit[0];
                    J = originalHit[1];
                }
                
                loopIf_hit = false;

                if (firstShot)
                {
                    firstShot = false;


                    int luck = 0;

                    while (luck != 5)
                    {
                        luck = ran.Next(4);

                        if (luck == 0 && iU && positionsOfHit[0] != 2)
                        {
                            luck = 5;
                            check_hit(cells[I - 1, J]);
                            if (hitShip)
                            {
                                hitShip = false;
                                positionsOfHit[0] = 1;
                                ifCompHitOnPlayerShip(I - 1,J);
                                cells[I - 1, J].Tag = "hit";
                                hit_on_ship();
                                if (!killShip)
                                {
                                    loopIf_hit = true;
                                    I--;
                                }
                            }
                            else
                            {
                                cells[I - 1, J].Background = new SolidColorBrush(Colors.Firebrick);
                                cells[I - 1, J].Tag = "miss";
                                positionsOfHit[0] = 2;
                                tryHitShip = true;
                            }
                        }

                        if (luck == 1 && jR && positionsOfHit[1] != 2)
                        {
                            luck = 5;
                            check_hit(cells[I, J + 1]);
                            if (hitShip)
                            {
                                hitShip = false;
                                positionsOfHit[1] = 1;
                                ifCompHitOnPlayerShip(I,J + 1);
                                cells[I, J + 1].Tag = "hit";
                                hit_on_ship();
                                if (!killShip)
                                {
                                    J++;
                                    loopIf_hit = true;
                                }
                            }
                            else
                            {
                                cells[I, J + 1].Background = new SolidColorBrush(Colors.Firebrick);
                                cells[I, J + 1].Tag = "miss";
                                positionsOfHit[1] = 2;
                                tryHitShip = true;
                            }
                        }

                        if (luck == 2 && iD && positionsOfHit[2] != 2)
                        {
                            luck = 5;
                            check_hit(cells[I + 1, J]);
                            if (hitShip)
                            {
                                hitShip = false;
                                positionsOfHit[2] = 1;
                                ifCompHitOnPlayerShip(I + 1,J);
                                cells[I + 1, J].Tag = "hit";
                                hit_on_ship();
                                if (!killShip)
                                {
                                    I++;
                                    loopIf_hit = true;
                                }
                            }
                            else
                            {
                                cells[I + 1, J].Background = new SolidColorBrush(Colors.Firebrick);
                                cells[I + 1, J].Tag = "miss";
                                positionsOfHit[2] = 2;
                                tryHitShip = true;
                            }
                        }

                        if (luck == 3 && jL && positionsOfHit[3] != 2)
                        {
                            luck = 5;
                            check_hit(cells[I, J - 1]);
                            if (hitShip)
                            {
                                hitShip = false;
                                positionsOfHit[3] = 1;
                                ifCompHitOnPlayerShip(I,J - 1);
                                cells[I, J - 1].Tag = "hit";
                                hit_on_ship();
                                if (!killShip)
                                {
                                    J--;
                                    loopIf_hit = true;
                                }
                            }
                            else
                            {
                                cells[I, J - 1].Background = new SolidColorBrush(Colors.Firebrick);
                                cells[I, J - 1].Tag = "miss";
                                positionsOfHit[3] = 2;
                                tryHitShip = true;
                            }
                        }
                    }
                }
                else
                {
                    if (iU)
                    {
                        if (positionsOfHit[0] == 1 || (reverseI == 2 && !thisRun))
                        {
                            check_hit(cells[I - 1, J]);
                            if (hitShip)
                            {
                                hitShip = false;
                                ifCompHitOnPlayerShip(I - 1,J);
                                cells[I - 1, J].Tag = "hit";
                                hit_on_ship();
                                if (!killShip)
                                {
                                    I--;
                                    loopIf_hit = true;
                                }
                            }
                            else
                            {
                                cells[I - 1, J].Background = new SolidColorBrush(Colors.Firebrick);
                                cells[I - 1, J].Tag = "miss";
                                positionsOfHit[0] = 2;
                                reverseI = 1;
                                reverse = true;
                                loopIf_hit = false;
                                thisRun = true;
                            }
                        }
                    }
                    else if (!iU && positionsOfHit[0] == 1)
                    {
                        positionsOfHit[0] = 2;
                        reverseI = 1;
                        reverse = true;
                        I = originalHit[0];
                        J = originalHit[1];
                        loopIf_hit = true;
                    }
                    else
                    {
                        positionsOfHit[0] = 2;
                    }

                    if (jR)
                    {
                        if (positionsOfHit[1] == 1 || (reverseJ == 2 && !thisRun))
                        {
                            check_hit(cells[I, J + 1]);
                            if (hitShip)
                            {
                                hitShip = false;
                                ifCompHitOnPlayerShip(I,J + 1);
                                cells[I, J + 1].Tag = "hit";
                                hit_on_ship();
                                if (!killShip)
                                {
                                    J++;
                                    loopIf_hit = true;
                                }
                            }
                            else
                            {
                                cells[I, J + 1].Background = new SolidColorBrush(Colors.Firebrick);
                                cells[I, J + 1].Tag = "miss";
                                positionsOfHit[1] = 2;
                                reverseJ = 1;
                                reverse = true;
                                loopIf_hit = false;
                                thisRun = true;
                            }
                        }
                    }
                    else if (!jR && positionsOfHit[1] == 1)
                    {
                        positionsOfHit[1] = 2;
                        reverseJ = 1;
                        reverse = true;
                        I = originalHit[0];
                        J = originalHit[1];
                        loopIf_hit = true;
                    }
                    else
                    {
                        positionsOfHit[1] = 2;
                    }

                    if (iD)
                    {
                        if (positionsOfHit[2] == 1 || (reverseI == 1 && !thisRun))
                        {
                            check_hit(cells[I + 1, J]);
                            if (hitShip)
                            {
                                hitShip = false;
                                ifCompHitOnPlayerShip(I + 1,J);
                                cells[I + 1, J].Tag = "hit";
                                hit_on_ship();
                                if (!killShip)
                                {
                                    I++;
                                    loopIf_hit = true;
                                }
                            }
                            else
                            {
                                cells[I + 1, J].Background = new SolidColorBrush(Colors.Firebrick);
                                cells[I + 1, J].Tag = "miss";
                                positionsOfHit[2] = 2;
                                reverseI = 2;
                                reverse = true;
                                loopIf_hit = false;
                                thisRun = true;
                            }
                        }
                    }
                    else if (!iD && positionsOfHit[2] == 1)
                    {
                        positionsOfHit[2] = 2;
                        reverseI = 2;
                        reverse = true;
                        I = originalHit[0];
                        J = originalHit[1];
                        loopIf_hit = true;
                    }
                    else
                    {
                        positionsOfHit[2] = 2;
                    }

                    if (jL)
                    {
                        if (positionsOfHit[3] == 1 || (reverseJ == 1 && !thisRun))
                        {
                            check_hit(cells[I, J - 1]);
                            if (hitShip)
                            {
                                hitShip = false;
                                ifCompHitOnPlayerShip(I,J - 1);
                                cells[I, J - 1].Tag = "hit";
                                hit_on_ship();
                                if (!killShip)
                                {
                                    J--;
                                    loopIf_hit = true;
                                }
                            }
                            else
                            {
                                cells[I, J - 1].Background = new SolidColorBrush(Colors.Firebrick);
                                cells[I, J - 1].Tag = "miss";
                                positionsOfHit[3] = 2;
                                reverseJ = 2;
                                reverse = true;
                                loopIf_hit = false;
                            }
                        }
                    }
                    else if (!jL && positionsOfHit[3] == 1)
                    {
                        positionsOfHit[3] = 2;
                        reverseJ = 2;
                        reverse = true;
                        I = originalHit[0];
                        J = originalHit[1];
                        loopIf_hit = true;
                    }
                    else
                    {
                        positionsOfHit[3] = 2;
                    }
                }
            }

            killShip = false;
        }

        private void if_no_hit(int i, int j)
        {
            int luck = 0;
            luck = ran.Next(4);

            while (luck != 5)
            {
                CheckHitEdges(i,j);
                CheckLayWays(i,j);
                
                if (iU && luck == 0 && positionsOfHit[0] != 2)
                {
                    check_hit(cells[i - 1,j]);
                    luck = 5;
                    if (hitShip)
                    {
                        hitShip = false;
                        ifCompHitOnPlayerShip(i - 1,j);
                        cells[i - 1, j].Tag = "hit";
                        positionsOfHit[0] = 1;
                        hitTrue = true;
                        hit_on_ship();
                        if (!killShip)
                        {
                            if_hit(i - 1, j);
                        }
                    }
                    else
                    {
                        cells[i - 1, j].Background = new SolidColorBrush(Colors.Firebrick);
                        cells[i - 1, j].Tag = "miss";
                        positionsOfHit[0] = 2;
                    }
                }
                else if (jR && luck == 1 && positionsOfHit[1] != 2)
                {
                    check_hit(cells[i,j + 1]);
                    luck = 5;
                    if (hitShip)
                    {
                        hitShip = false;
                        positionsOfHit[1] = 1;
                        ifCompHitOnPlayerShip(i,j + 1);
                        cells[i, j + 1].Tag = "hit";
                        hitTrue = true;
                        hit_on_ship();
                        if (!killShip)
                        {
                            if_hit(i, j + 1);
                        }
                    }
                    else
                    {
                        cells[i, j + 1].Background = new SolidColorBrush(Colors.Firebrick);
                        cells[i, j + 1].Tag = "miss";
                        positionsOfHit[1] = 2;
                    }
                }
                else if (iD && luck == 2 && positionsOfHit[2] != 2)
                {
                    check_hit(cells[i + 1,j]);
                    luck = 5;
                    if (hitShip)
                    {
                        hitShip = false;
                        positionsOfHit[2] = 1;
                        ifCompHitOnPlayerShip(i + 1,j);
                        cells[i + 1, j].Tag = "hit";
                        hitTrue = true;
                        hit_on_ship();
                        if (!killShip)
                        {
                            if_hit(i + 1, j);
                        }
                    }
                    else
                    {
                        cells[i + 1, j].Background = new SolidColorBrush(Colors.Firebrick);
                        cells[i + 1, j].Tag = "miss";
                        positionsOfHit[2] = 2;
                    }
                }
                else if (jL && luck == 3 && positionsOfHit[3] != 2)
                {
                    check_hit(cells[i,j - 1]);
                    luck = 5;
                    if (hitShip)
                    {
                        hitShip = false;
                        positionsOfHit[3] = 1;
                        ifCompHitOnPlayerShip(i,j - 1);
                        cells[i, j - 1].Tag = "hit";
                        hitTrue = true;
                        hit_on_ship();
                        if (!killShip)
                        {
                            if_hit(i,j - 1);
                        }
                    }
                    else
                    {
                        cells[i, j - 1].Background = new SolidColorBrush(Colors.Firebrick);
                        cells[i, j - 1].Tag = "miss";
                        positionsOfHit[3] = 2;
                    }
                }
                else
                {                        
                    luck = ran.Next(4);
                }
            }

            killShip = false;
        }

        private bool loopComphit = true;

        private void ifCompHitOnPlayerShip(int i, int j)
        {
            killArr[i, j].Visibility = Visibility.Visible;
        }
        
        
        
        private void field_with_comphits()
        {
            if (!tryHitShip && !reverse)
            {
                originalHit[0] = -1;
            }

            loopComphit = true;
            while (loopComphit)
            {
                loopComphit = false;
                loopIf_hit = true;
                comphit();
            }
            
        }

        private void hit_on_ship() 
        {
            if (catch_ship.Name == "Ship4" && (string)catch_ship.Tag != "destroyed")
            {
                if (hitCapacity[0] == 3)
                {
                    delete_ship();
                }
                else
                {
                    hitCapacity[0]++;
                }
            }
            else if (catch_ship.Name == "Ship3_1" && (string)catch_ship.Tag != "destroyed")
            {
                if (hitCapacity[1] == 2)
                {
                    delete_ship();
                }
                else
                {
                    hitCapacity[1]++;
                }
            }
            else if (catch_ship.Name == "Ship3_2" && (string)catch_ship.Tag != "destroyed")
            {
                if (hitCapacity[2] == 2)
                {
                    delete_ship();
                }
                else
                {
                    hitCapacity[2]++;
                }
            }
            else if (catch_ship.Name == "Ship2_1" && (string)catch_ship.Tag != "destroyed")
            {
                if (hitCapacity[3] == 1)
                {
                    delete_ship();
                }
                else
                {
                    hitCapacity[3]++;
                }
            }
            else if (catch_ship.Name == "Ship2_2" && (string)catch_ship.Tag != "destroyed")
            {
                if (hitCapacity[4] == 1)
                {
                    delete_ship();
                }
                else
                {
                    hitCapacity[4]++;
                }
            }
            else if (catch_ship.Name == "Ship2_3" && (string)catch_ship.Tag != "destroyed")
            {
                if (hitCapacity[5] == 1)
                {
                    delete_ship();
                }
                else
                {
                    hitCapacity[5]++;
                }
            }
            else if ((string)catch_ship.Tag != "destroyed")
            {
                kill1ship = true;
                delete_ship();
            }
        }

        private bool kill1ship = false;
        private bool killShip = false;
        
        private void delete_ship()
        {
            catch_ship.Tag = "destroyed";
            
            for (int i = 0; i < 4; i++)
            {
                positionsOfHit[i] = 0;
            }

            AddDeadZoneForHit();
            
            tryHitShip = false;
            reverse = false;
            reverseI = 0;
            reverseJ = 0;
            originalHit[0] = -1;
            hitShip = false;
            loopComphit = true;
            hitTrue = false;
            firstShot = true;
            killShip = true;
            LoseEvent();
        } 

        private void check_hitplayer(Button ttemp)
        {
            int[] klp = find_name_gbutton(ttemp);
            if (compships[klp[0], klp[1]] == 1)
            {
                hit = true;
                compships[klp[0], klp[1]] = 0;
            }
        }
        
        private int[] find_name_gbutton(Button ttemp)
        {
            int[] arrtemp = new int[2];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (ttemp != null && ttemp.Name == game_cells[i, j].Name)
                    {
                        arrtemp[0] = i;
                        arrtemp[1] = j;
                        return arrtemp;
                    }
                }
            }

            return arrtemp;
        }

        private void ButtonContinue_OnClick(object sender, RoutedEventArgs e)
        {
            show_unvisible();
        }

        /*
        private int[,] testField = new int[10, 10];
        
        private bool CheckAroundPlayerShip(int i, int j)
        {
            // if (cells[])
            return false;
        }

        private void AddDeadZoneAroundPlayerShip(int i, int j, int uD, int ship)
        {
            if (uD == 0)
            {
                if (ship == 4)
                {
                    if (j != 9)
                    {
                        testField[i, j - 1] = 2;
                    }
                    
                    testField[i, j] = 1;
                    testField[i, j + 1] = 1;
                    testField[i, j + 2] = 1;
                    testField[i, j + 3] = 1;
                    
                    if (j != 6)
                    {
                        testField[i, j + 4] = 2;
                    }

                    if (i != 9)
                    {
                        if (j != 9)
                        {
                            testField[i - 1, j - 1] = 2;
                        }
                        
                        testField[i + 1, j] = 2;
                        testField[i + 1, j + 1] = 2;
                        testField[i + 1, j + 2] = 2;
                        testField[i + 1, j + 3] = 2;
                        
                        if (j != 6)
                        {
                            testField[i + 1, j + 4] = 2;
                        }
                    }
                    
                    if (i != 0)
                    {
                        if (j != 9)
                        {
                            testField[i - 1, j - 1] = 2;
                        }
                        
                        testField[i - 1, j] = 2;
                        testField[i - 1, j + 1] = 2;
                        testField[i - 1, j + 2] = 2;
                        testField[i - 1, j + 3] = 2;
                        
                        if (j != 6)
                        {
                            testField[i - 1, j + 4] = 2;
                        }
                    }
                }
            }
            else
            {
                if (i != 9)
                {
                    testField[i + 1, j] = 2;
                }
                    
                testField[i, j] = 1;
                testField[i, j + 1] = 1;
                testField[i, j + 2] = 1;
                testField[i, j + 3] = 1;
                    
                if (j != 6)
                {
                    testField[i, j + 4] = 2;
                }

                if (i != 9)
                {
                    if (j != 9)
                    {
                        testField[i - 1, j - 1] = 2;
                    }
                        
                    testField[i + 1, j] = 2;
                    testField[i + 1, j + 1] = 2;
                    testField[i + 1, j + 2] = 2;
                    testField[i + 1, j + 3] = 2;
                        
                    if (j != 6)
                    {
                        testField[i + 1, j + 4] = 2;
                    }
                }
                    
                if (i != 0)
                {
                    if (j != 9)
                    {
                        testField[i - 1, j - 1] = 2;
                    }
                        
                    testField[i - 1, j] = 2;
                    testField[i - 1, j + 1] = 2;
                    testField[i - 1, j + 2] = 2;
                    testField[i - 1, j + 3] = 2;
                        
                    if (j != 6)
                    {
                        testField[i - 1, j + 4] = 2;
                    }
                }
            }
        }*/

        /*private void ButtonPlaceShips_OnClick(object sender, RoutedEventArgs e)
        {
            int i;
            int j;
            int count = 0;

            while (count != 10)
            {
                i = ran.Next(10);
                j = ran.Next(10);
                
                if (count == 0)
                {
                    this.dragobject = Ship4;
                    tship = Ship4;
                    int positionNavigate = ran.Next(100);

                    if (positionNavigate < 50)
                    {
                        if (j > 6)
                        {
                            place_ship(cells[i,6]);
                            count++;
                        }
                        else
                        {
                            place_ship(cells[i,j]);
                            count++;
                        }
                    }

                    if (positionNavigate > 50)
                    {
                        if (i > 6)
                        {
                            place_ship(cells[6,j]);
                            count++;
                        }
                        else
                        {
                            place_ship(cells[i,j]);
                            count++;
                        }
                    }
                }
                else if (count == 1)
                {
                    this.dragobject = Ship3_1;
                    tship = Ship3_1;
                    int positionNavigate = ran.Next(100); 
                    
                    if (positionNavigate < 50)
                    {
                        if (j > 7)
                        {
                            place_ship(cells[i,6]);
                            count++;
                        }
                        else
                        {
                            place_ship(cells[i,j]);
                            count++;
                        }
                    }

                    if (positionNavigate > 50)
                    {
                        if (i > 7)
                        {
                            place_ship(cells[6,j]);
                            count++;
                        }
                        else
                        {
                            place_ship(cells[i,j]);
                            count++;
                        }
                    }
                }
            }
        }*/

        
        
        private void LoseEvent()
        {
            if ((string)Ship1_1.Tag == "destroyed" && (string)Ship1_2.Tag == "destroyed" &&
                (string)Ship1_3.Tag == "destroyed" && (string)Ship1_4.Tag == "destroyed" &&
                (string)Ship2_1.Tag == "destroyed" && (string)Ship2_2.Tag == "destroyed" &&
                (string)Ship2_3.Tag == "destroyed" && (string)Ship3_1.Tag == "destroyed" &&
                (string)Ship3_2.Tag == "destroyed" && (string)Ship4.Tag == "destroyed")
            {
                Lose();
            }
        }

        private void Win()
        {
            Winner.Visibility = Visibility.Visible;
            ButtonMainMenu.Visibility = Visibility.Visible;
            endGame = true;

            MyRecords record = new MyRecords();
            record.Deserialize();
            record.AddWin(nameOfPlayer);
        }

        private bool endGame = false;

        private void Lose()
        {
            Loser.Visibility = Visibility.Visible;
            ButtonMainMenu.Visibility = Visibility.Visible;
            loopComphit = false;
            endGame = true;
        }

        private bool flag = false;

        private void ButtonMainMenu_OnClick(object sender, RoutedEventArgs e)
        {
            ImageBrush myBrush = new ImageBrush();
            myBrush.ImageSource = new BitmapImage(new Uri(@"C:\\Users\\halop\\RiderProjects\\kursa4\\kursa4\\temp_background.png"));
            canvas.Background = myBrush;

            ButtonInstruction.Visibility = Visibility.Visible;
            Winner.Visibility = Visibility.Hidden;
            Loser.Visibility = Visibility.Hidden;
            Ship4.Visibility = Visibility.Hidden;
            Ship3_1.Visibility = Visibility.Hidden;
            Ship3_2.Visibility = Visibility.Hidden;
            Ship2_1.Visibility = Visibility.Hidden;
            Ship2_2.Visibility = Visibility.Hidden;
            Ship2_3.Visibility = Visibility.Hidden;
            Ship1_1.Visibility = Visibility.Hidden;
            Ship1_2.Visibility = Visibility.Hidden;
            Ship1_3.Visibility = Visibility.Hidden;
            Ship1_4.Visibility = Visibility.Hidden;
            compRShip4.Visibility = Visibility.Hidden;
            compRShip3_1.Visibility = Visibility.Hidden;
            compRShip3_2.Visibility = Visibility.Hidden;
            compRShip2_1.Visibility = Visibility.Hidden;
            compRShip2_2.Visibility = Visibility.Hidden;
            compRShip2_3.Visibility = Visibility.Hidden;
            compRShip1_1.Visibility = Visibility.Hidden;
            compRShip1_2.Visibility = Visibility.Hidden;
            compRShip1_3.Visibility = Visibility.Hidden;
            compRShip1_4.Visibility = Visibility.Hidden;
            Application.Current.MainWindow.MinWidth = 1200;
            Application.Current.MainWindow.Width = 1200;

            Border1.Visibility = Visibility.Hidden;
            Border2.Visibility = Visibility.Hidden;
            ButtonRecords.Visibility = Visibility.Visible;
            startGame = false;
            ButtonCloseApp.Visibility = Visibility.Visible;
            flag = false;
            field.Visibility = Visibility.Hidden;
            GameField.Visibility = Visibility.Hidden;
            ButtonPlay.Visibility = Visibility.Visible;
            ButtonMainMenu.Visibility = Visibility.Hidden;
            KillField.Visibility = Visibility.Hidden;
            KillField2.Visibility = Visibility.Hidden;

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    cells[i, j] = null;
                    game_cells[i,j] = null;
                    compships[i, j] = 0;
                    killArr[i, j].Visibility = Visibility.Hidden;
                    killArr2[i, j].Visibility = Visibility.Hidden;
                }
            }

            for (int i = 0; i < 6; i++)
            {
                hitCapacity[i] = 0;
            }
            
            for (int i = 0; i < 6; i++)
            {
                hitOnCompCapacity[i] = 0;
            }
            
            ReplaceShip();
            DestroyShips();
        }

        private void DestroyShips()
        {
            Ship4.IsHitTestVisible = true;
            Ship3_1.IsHitTestVisible = true;
            Ship3_2.IsHitTestVisible = true;
            Ship2_1.IsHitTestVisible = true;
            Ship2_2.IsHitTestVisible = true;
            Ship2_3.IsHitTestVisible = true;
            Ship1_1.IsHitTestVisible = true;
            Ship1_2.IsHitTestVisible = true;
            Ship1_3.IsHitTestVisible = true;
            Ship1_4.IsHitTestVisible = true;

            count1Ship = 0;
            playerHits = 0;
            count = 0;
            endGame = false;
        }

        private void AddDeadZoneForHit()
        {
            int[] tempIndex = new int[2];
            
            tempIndex = FindCellOnPosition(catch_ship);

            int i = tempIndex[0];
            int j = tempIndex[1];
            
            bool L = false;
            bool U = false;
            bool R = false;
            bool D = false;

            if (catch_ship == Ship1_1 || catch_ship == Ship1_2 || catch_ship == Ship1_3 || catch_ship == Ship1_4)
            {
                if (j != 0)
                {
                    cells[i, j - 1].Tag = "deadzone";
                    L = true;
                }

                if (j != 9)
                {
                    cells[i, j + 1].Tag = "deadzone";
                    R = true;
                }

                if (i != 0)
                {
                    cells[i - 1, j].Tag = "deadzone";
                    U = true;
                }

                if (i != 9)
                {
                    cells[i + 1, j].Tag = "deadzone";
                    D = true;
                }
                    
                if (L && U)
                {
                    cells[i - 1, j - 1].Tag = "deadzone";
                }

                if (U && R)
                {
                    cells[i - 1, j + 1].Tag = "deadzone";
                }

                if (R && D)
                {
                    cells[i + 1, j + 1].Tag = "deadzone";
                }
                    
                if (L && D)
                {
                    cells[i + 1, j - 1].Tag = "deadzone";
                }
            }

            if (j != 9 && (string)cells[i, j + 1].Tag == "hit")
            {
                if (catch_ship == Ship4)
                {
                    if (j != 0)
                    {
                        cells[i, j - 1].Tag = "deadzone";
                        L = true;
                    }

                    if (j + 3 != 9)
                    {
                        cells[i, j + 4].Tag = "deadzone";
                        R = true;
                    }

                    if (i != 0)
                    {
                        cells[i - 1, j].Tag = "deadzone";
                        cells[i - 1, j + 1].Tag = "deadzone";
                        cells[i - 1, j + 2].Tag = "deadzone";
                        cells[i - 1, j + 3].Tag = "deadzone";
                        U = true;
                    }

                    if (i != 9)
                    {
                        cells[i + 1, j].Tag = "deadzone";
                        cells[i + 1, j + 1].Tag = "deadzone";
                        cells[i + 1, j + 2].Tag = "deadzone";
                        cells[i + 1, j + 3].Tag = "deadzone";
                        D = true;
                    }

                    if (L && U)
                    {
                        cells[i - 1, j - 1].Tag = "deadzone";
                    }

                    if (U && R)
                    {
                        cells[i - 1, j + 4].Tag = "deadzone";
                    }

                    if (R && D)
                    {
                        cells[i + 1, j + 4].Tag = "deadzone";
                    }
                    
                    if (L && D)
                    {
                        cells[i + 1, j - 1].Tag = "deadzone";
                    }
                    
                } 
                else if (catch_ship == Ship3_1 || catch_ship == Ship3_2)
                {
                    if (j != 0)
                    {
                        cells[i, j - 1].Tag = "deadzone";
                        L = true;
                    }

                    if (j + 2 != 9)
                    {
                        cells[i, j + 3].Tag = "deadzone";
                        R = true;
                    }

                    if (i != 0)
                    {
                        cells[i - 1, j].Tag = "deadzone";
                        cells[i - 1, j + 1].Tag = "deadzone";
                        cells[i - 1, j + 2].Tag = "deadzone";
                        U = true;
                    }

                    if (i != 9)
                    {
                        cells[i + 1, j].Tag = "deadzone";
                        cells[i + 1, j + 1].Tag = "deadzone";
                        cells[i + 1, j + 2].Tag = "deadzone";
                        D = true;
                    }
                    
                    if (L && U)
                    {
                        cells[i - 1, j - 1].Tag = "deadzone";
                    }

                    if (U && R)
                    {
                        cells[i - 1, j + 3].Tag = "deadzone";
                    }

                    if (R && D)
                    {
                        cells[i + 1, j + 3].Tag = "deadzone";
                    }
                    
                    if (L && D)
                    {
                        cells[i + 1, j - 1].Tag = "deadzone";
                    }
                }
                else if (catch_ship == Ship2_1 || catch_ship == Ship2_2 || catch_ship == Ship2_3)
                {
                    if (j != 0)
                    {
                        cells[i, j - 1].Tag = "deadzone";
                        L = true;
                    }

                    if (j + 1 != 9)
                    {
                        cells[i, j + 2].Tag = "deadzone";
                        R = true;
                    }

                    if (i != 0)
                    {
                        cells[i - 1, j].Tag = "deadzone";
                        cells[i - 1, j + 1].Tag = "deadzone";
                        U = true;
                    }

                    if (i != 9)
                    {
                        cells[i + 1, j].Tag = "deadzone";
                        cells[i + 1, j + 1].Tag = "deadzone";
                        D = true;
                    }
                    
                    if (L && U)
                    {
                        cells[i - 1, j - 1].Tag = "deadzone";
                    }

                    if (U && R)
                    {
                        cells[i - 1, j + 2].Tag = "deadzone";
                    }

                    if (R && D)
                    {
                        cells[i + 1, j + 2].Tag = "deadzone";
                    }
                    
                    if (L && D)
                    {
                        cells[i + 1, j - 1].Tag = "deadzone";
                    }
                }

            } 
            else if (i != 9 && (string)cells[i + 1, j].Tag == "hit")
            {
                if (catch_ship == Ship4)
                {
                    if (j != 0)
                    {
                        cells[i, j - 1].Tag = "deadzone";
                        cells[i + 1, j - 1].Tag = "deadzone";
                        cells[i + 2, j - 1].Tag = "deadzone";
                        cells[i + 3, j - 1].Tag = "deadzone";
                        L = true;
                    }

                    if (j != 9)
                    {
                        cells[i, j + 1].Tag = "deadzone";
                        cells[i + 1, j + 1].Tag = "deadzone";
                        cells[i + 2, j + 1].Tag = "deadzone";
                        cells[i + 3, j + 1].Tag = "deadzone";
                        R = true;
                    }

                    if (i != 0)
                    {
                        cells[i - 1, j].Tag = "deadzone";
                        U = true;
                    }

                    if (i + 3 != 9)
                    {
                        cells[i + 4, j].Tag = "deadzone";
                        D = true;
                    }

                    if (L && U)
                    {
                        cells[i - 1, j - 1].Tag = "deadzone";
                    }

                    if (U && R)
                    {
                        cells[i - 1, j + 1].Tag = "deadzone";
                    }

                    if (R && D)
                    {
                        cells[i + 4, j + 1].Tag = "deadzone";
                    }
                    
                    if (L && D)
                    {
                        cells[i + 4, j - 1].Tag = "deadzone";
                    }
                    
                } 
                else if (catch_ship == Ship3_1 || catch_ship == Ship3_2)
                {
                    if (j != 0)
                    {
                        cells[i, j - 1].Tag = "deadzone";
                        cells[i + 1, j - 1].Tag = "deadzone";
                        cells[i + 2, j - 1].Tag = "deadzone";
                        L = true;
                    }

                    if (j != 9)
                    {
                        cells[i, j + 1].Tag = "deadzone";
                        cells[i + 1, j + 1].Tag = "deadzone";
                        cells[i + 2, j + 1].Tag = "deadzone";
                        R = true;
                    }

                    if (i != 0)
                    {
                        cells[i - 1, j].Tag = "deadzone";
                        U = true;
                    }

                    if (i + 2 != 9)
                    {
                        cells[i + 3, j].Tag = "deadzone";
                        D = true;
                    }

                    if (L && U)
                    {
                        cells[i - 1, j - 1].Tag = "deadzone";
                    }

                    if (U && R)
                    {
                        cells[i - 1, j + 1].Tag = "deadzone";
                    }

                    if (R && D)
                    {
                        cells[i + 3, j + 1].Tag = "deadzone";
                    }
                    
                    if (L && D)
                    {
                        cells[i + 3, j - 1].Tag = "deadzone";
                    }
                }
                else if (catch_ship == Ship2_1 || catch_ship == Ship2_2 || catch_ship == Ship2_3)
                {
                    if (j != 0)
                    {
                        cells[i, j - 1].Tag = "deadzone";
                        cells[i + 1, j - 1].Tag = "deadzone";
                        L = true;
                    }

                    if (j != 9)
                    {
                        cells[i, j + 1].Tag = "deadzone";
                        cells[i + 1, j + 1].Tag = "deadzone";
                        R = true;
                    }

                    if (i != 0)
                    {
                        cells[i - 1, j].Tag = "deadzone";
                        U = true;
                    }

                    if (i + 1 != 9)
                    {
                        cells[i + 2, j].Tag = "deadzone";
                        D = true;
                    }

                    if (L && U)
                    {
                        cells[i - 1, j - 1].Tag = "deadzone";
                    }

                    if (U && R)
                    {
                        cells[i - 1, j + 1].Tag = "deadzone";
                    }

                    if (R && D)
                    {
                        cells[i + 2, j + 1].Tag = "deadzone";
                    }
                    
                    if (L && D)
                    {
                        cells[i + 2, j - 1].Tag = "deadzone";
                    }
                }
            }
        }
        
        private int[] FindCellOnPosition(Label tttship)
        {
            int[] tempIndex = new int[2];
            double positionShipX = 0, positionShipY = 0;
            var catchShip = tttship;

            positionShipX = Canvas.GetLeft(catchShip);
            positionShipY = Canvas.GetTop(catchShip);

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (positionShipX == Canvas.GetLeft(cells[i, j]) + (window.ActualWidth - 1220) / 2 && 
                        positionShipY == Canvas.GetTop(cells[i, j])  + (window.ActualHeight - field.ActualHeight) / 2)
                    {
                        tempIndex[0] = i;
                        tempIndex[1] = j;
                    }
                }
            }

            return tempIndex;
        }

        private void ButtonInstruction_OnClick(object sender, RoutedEventArgs e)
        {
            Instructions.Visibility = Visibility.Visible;
            ButtonCloseInstructions.Visibility = Visibility.Visible;
        }

        private void ButtonCloseInstructions_OnClick(object sender, RoutedEventArgs e)
        {
            Instructions.Visibility = Visibility.Hidden;
            ButtonCloseInstructions.Visibility = Visibility.Hidden;
        }

        private bool instructionFlag = false;
        
        private void ButtonInstruction2_OnClick(object sender, RoutedEventArgs e)
        {
            if (!instructionFlag)
            {
                Instructions2.Visibility = Visibility.Visible;
                instructionFlag = true;
                HiddenIf();
            }
            else
            {
                Instructions2.Visibility = Visibility.Hidden;
                instructionFlag = false;
                Ship4.Visibility = Visibility.Visible;
                Ship3_1.Visibility = Visibility.Visible;
                Ship3_2.Visibility = Visibility.Visible;
                Ship2_1.Visibility = Visibility.Visible;
                Ship2_2.Visibility = Visibility.Visible;
                Ship2_3.Visibility = Visibility.Visible;
                Ship1_1.Visibility = Visibility.Visible;
                Ship1_2.Visibility = Visibility.Visible;
                Ship1_3.Visibility = Visibility.Visible;
                Ship1_4.Visibility = Visibility.Visible;
                Ship4.IsHitTestVisible = true;
                Ship3_1.IsHitTestVisible = true;
                Ship3_2.IsHitTestVisible = true;
                Ship2_1.IsHitTestVisible = true;
                Ship2_2.IsHitTestVisible = true;
                Ship2_3.IsHitTestVisible = true;
                Ship1_1.IsHitTestVisible = true;
                Ship1_2.IsHitTestVisible = true;
                Ship1_3.IsHitTestVisible = true;
                Ship1_4.IsHitTestVisible = true;
            }
        }

        private void HiddenIf()
        {
            if ((string)Ship4.Tag == "touched")
            {
                Ship4.Visibility = Visibility.Hidden;
            }
            else
            {
                Ship4.IsHitTestVisible = false;
            }
            if ((string)Ship3_1.Tag == "touched")
            {
                Ship3_1.Visibility = Visibility.Hidden;
            }
            else
            {
                Ship3_1.IsHitTestVisible = false;
            }
            if ((string)Ship3_2.Tag == "touched")
            {
                Ship3_2.Visibility = Visibility.Hidden;
            }
            else
            {
                Ship3_2.IsHitTestVisible = false;
            }
            if ((string)Ship2_1.Tag == "touched")
            {
                Ship2_1.Visibility = Visibility.Hidden;
            }
            else
            {
                Ship2_1.IsHitTestVisible = false;
            }
            if ((string)Ship2_2.Tag == "touched")
            {
                Ship2_2.Visibility = Visibility.Hidden;
            }
            else
            {
                Ship2_2.IsHitTestVisible = false;
            }
            if ((string)Ship2_3.Tag == "touched")
            {
                Ship2_3.Visibility = Visibility.Hidden;
            }
            else
            {
                Ship2_3.IsHitTestVisible = false;
            }
            if ((string)Ship1_1.Tag == "touched")
            {
                Ship1_1.Visibility = Visibility.Hidden;
            }
            else
            {
                Ship1_1.IsHitTestVisible = false;
            }
            if ((string)Ship1_2.Tag == "touched")
            {
                Ship1_2.Visibility = Visibility.Hidden;
            }
            else
            {
                Ship1_2.IsHitTestVisible = false;
            }
            if ((string)Ship1_3.Tag == "touched")
            {
                Ship1_3.Visibility = Visibility.Hidden;
            }
            else
            {
                Ship1_3.IsHitTestVisible = false;
            }
            if ((string)Ship1_4.Tag == "touched")
            {
                Ship1_4.Visibility = Visibility.Hidden;
            }
            else
            {
                Ship1_4.IsHitTestVisible = false;
            }
        }

        private void ButtonCloseApp_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


        private bool instructionFlag2 = false;

        private void ButtonRecords_OnClick(object sender, RoutedEventArgs e)
        {
            ButtonPlay.IsHitTestVisible = false;
            ButtonInstruction.IsHitTestVisible = false;
            ButtonRecords.IsHitTestVisible = false;
            ButtonCloseApp.IsHitTestVisible = false;

            MyRecords records = new MyRecords();
            if (records.Deserialize())
            {
                Leaderboards.Visibility = Visibility.Visible;
                RecordsData dataT;

                foreach (var i in records.name)
                {
                    dataT = new RecordsData();
                    dataT.PlayerName = i.Key;
                    dataT.Wins = i.Value;

                    ListRecords.Add(dataT);
                }

                test3.ItemsSource = ListRecords;
                test3.Items.Refresh();
                test3.Columns[0].Header = "Никнейм";
                test3.Columns[0].CanUserResize = false;
                test3.Columns[0].CanUserSort = false;
                test3.Columns[0].Width = 200;
                test3.Columns[1].Header = "Победы";
                test3.Columns[1].CanUserResize = false;
                test3.Columns[1].Width = 90;
            }
        }

        public List<RecordsData> ListRecords;

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ButtonPlay.IsHitTestVisible = true;
            ButtonInstruction.IsHitTestVisible = true;
            ButtonRecords.IsHitTestVisible = true;
            ButtonCloseApp.IsHitTestVisible = true;
            Leaderboards.Visibility = Visibility.Hidden;
            ListRecords.Clear();
            test3.Items.Refresh();
        }

        private void ButtonCheat_OnClick(object sender, RoutedEventArgs e)
        {
            Win();
        }
    }


[Serializable]
    public class MyRecords
    {
        public Dictionary<string, int> name = new Dictionary<string, int>();

        public void AddWin(string nameOfUser)
        {
            if (name.ContainsKey(nameOfUser))
            {
                name[nameOfUser]++;
            }
            else
            {
                name.Add(nameOfUser,1);
            }
            Serialize();
        }

        public bool Deserialize()
        {
            FileInfo fI = new FileInfo("Leaderbords");
            if (fI.Exists)
            {
                FileStream koko = new FileStream("Leaderbords",FileMode.Open);
                BinaryFormatter ioi = new BinaryFormatter();

                name = (Dictionary<string, int>)ioi.Deserialize(koko);
                koko.Close();
                return true;
            }
            else
            {
                MessageBox.Show("Fuck you!", "Fuck you!", MessageBoxButton.OK,
                    MessageBoxImage.Warning, MessageBoxResult.OK);
                return false;
            }
        }

        public void Serialize()
        {
            FileStream koko = new FileStream("Leaderbords",FileMode.Create);
            BinaryFormatter ioi = new BinaryFormatter();
            
            ioi.Serialize(koko,name);
            koko.Close();
        }
    }

    public class RecordsData
    {
        public string PlayerName { get; set; }
        public int Wins { get; set; }

    }
    
}
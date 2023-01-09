﻿using System;
using System.IO.Pipes;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Remoting.Channels;
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
        private int[,] compships = new int[10,10];

        public MainWindow()
        {
            InitializeComponent();
            cells = new Border[10, 10];
            game_cells = new Button[10, 10];
            DrawField();
        }

        private void ButtonPlay_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Height = 800;
            Application.Current.MainWindow.Width = 1200;
            ImageBrush myBrush = new ImageBrush();
            //myBrush.ImageSource = (new BitmapImage(new Uri(@"C:\\Users\\halop\\RiderProjects\\kursa4\\kursa4\\GameField.jpg")));
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
            ButtonPlay.Visibility = Visibility.Hidden;
            field.Visibility = Visibility.Visible;
            ButtonStartGame.Visibility = Visibility.Visible;
            ButtonResetShips.Visibility = Visibility.Visible;
            ButtonPlaceShips.Visibility = Visibility.Visible;
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
        } //TODO при быстром перемещении корабля хавается не та клетка, может зайти в deadzone корябля UPD. Всё ещё похуй :D 

        private void OnPreviewDrop(object sender, DragEventArgs e)
        {
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

            if (this._poinOfDrag != null && this.dragobject != null && result)
            {
                place_ship(this._poinOfDrag);
            }
            else if (result && this.dragobject != null)
            {
                replace_ship();
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
            }
            else if (this.tship.Name == "Ship3_1")
            {
                Canvas.SetTop(this.dragobject, 80);
                Canvas.SetLeft(this.dragobject, 0);
                tship.Tag = "untouched";
            }
            else if (tship.Name == "Ship3_2")
            {
                Canvas.SetTop(this.dragobject, 160);
                Canvas.SetLeft(this.dragobject, 0);
                tship.Tag = "untouched";
            }
            else if (tship.Name == "Ship2_1")
            {
                Canvas.SetTop(this.dragobject, 240);
                Canvas.SetLeft(this.dragobject, 0);
                tship.Tag = "untouched";
            }
            else if (tship.Name == "Ship2_2")
            {
                Canvas.SetTop(this.dragobject, 320);
                Canvas.SetLeft(this.dragobject, 0);
                tship.Tag = "untouched";
            }
            else if (tship.Name == "Ship2_2")
            {
                Canvas.SetTop(this.dragobject, 400);
                Canvas.SetLeft(this.dragobject, 0);
                tship.Tag = "untouched";
            }
            else if (tship.Name == "Ship1_1")
            {
                Canvas.SetTop(this.dragobject, 480);
                Canvas.SetLeft(this.dragobject, 0);
                tship.Tag = "untouched";
            }
            else if (tship.Name == "Ship1_2")
            {
                Canvas.SetTop(this.dragobject, 480);
                Canvas.SetLeft(this.dragobject, 75);
                tship.Tag = "untouched";
            }
            else if (tship.Name == "Ship1_3")
            {
                Canvas.SetTop(this.dragobject, 480);
                Canvas.SetLeft(this.dragobject, 150);
                tship.Tag = "untouched";
            }
            else if (tship.Name == "Ship1_4")
            {
                Canvas.SetTop(this.dragobject, 480);
                Canvas.SetLeft(this.dragobject, 225);
                tship.Tag = "untouched";
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
                    if ((string)tship.Content == "ЧЕТЫРЁХПАЛУБНИК")
                    {
                        j -= 3;
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                    else if ((string)tship.Content == "ТРЁХПАЛУБНИК")
                    {
                        j -= 2;
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                    else if ((string)tship.Content == "ДВУХПАЛУБНИК")
                    {
                        j -= 1;
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                    else if ((string)tship.Content == "КАТЕР")
                    {
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                }
                else if ((string)ttemp.Tag == "critical_..8")
                {
                    if ((string)tship.Content == "ЧЕТЫРЁХПАЛУБНИК")
                    {
                        j -= 2;
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                    else if ((string)tship.Content == "ТРЁХПАЛУБНИК")
                    {
                        j -= 1;
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                    else if ((string)tship.Content == "ДВУХПАЛУБНИК")
                    {
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                    else if ((string)tship.Content == "КАТЕР")
                    {
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                }
                else if ((string)ttemp.Tag == "critical_..7")
                {
                    if ((string)tship.Content == "ЧЕТЫРЁХПАЛУБНИК")
                    {
                        j -= 1;
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                    else if ((string)tship.Content == "ТРЁХПАЛУБНИК")
                    {
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                    else if ((string)tship.Content == "ДВУХПАЛУБНИК")
                    {
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                    else if ((string)tship.Content == "КАТЕР")
                    {
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                }
                else
                {
                    temp = cells[i, j];
                    Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                    Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                    tship.Tag = "touched";
                }
            }
            else
            {
                if ((string)ttemp.Tag == "critical_9.." || (string)ttemp.Name == "Border_99" ||
                    (string)ttemp.Name == "Border_98" || (string)ttemp.Name == "Border_97")
                {
                    if ((string)tship.Content == "ЧЕТЫРЁХПАЛУБНИК")
                    {
                        i -= 3;
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                    else if ((string)tship.Content == "ТРЁХПАЛУБНИК")
                    {
                        i -= 2;
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                    else if ((string)tship.Content == "ДВУХПАЛУБНИК")
                    {
                        i -= 1;
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                    else if ((string)tship.Content == "КАТЕР")
                    {
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                }
                else if ((string)ttemp.Tag == "critical_8.." || (string)ttemp.Name == "Border_89" ||
                         (string)ttemp.Name == "Border_88" || (string)ttemp.Name == "Border_87")
                {
                    if ((string)tship.Content == "ЧЕТЫРЁХПАЛУБНИК")
                    {
                        i -= 2;
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                    else if ((string)tship.Content == "ТРЁХПАЛУБНИК")
                    {
                        i -= 1;
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                    else if ((string)tship.Content == "ДВУХПАЛУБНИК")
                    {
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                    else if ((string)tship.Content == "КАТЕР")
                    {
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                }
                else if ((string)ttemp.Tag == "critical_7.." || (string)ttemp.Name == "Border_79" ||
                         (string)ttemp.Name == "Border_78" || (string)ttemp.Name == "Border_77")
                {
                    if ((string)tship.Content == "ЧЕТЫРЁХПАЛУБНИК")
                    {
                        i -= 1;
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                    else if ((string)tship.Content == "ТРЁХПАЛУБНИК")
                    {
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                    else if ((string)tship.Content == "ДВУХПАЛУБНИК")
                    {
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                    else if ((string)tship.Content == "КАТЕР")
                    {
                        temp = cells[i, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                }
                else
                {
                    temp = cells[i, j];
                    Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                    Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                    tship.Tag = "touched";
                }
            }

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
            int[] tarr = find_temp_name(temp);
            int i = tarr[0];
            int j = tarr[1];

            if ((string)tship.Content == "ЧЕТЫРЁХПАЛУБНИК")
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
            else if ((string)tship.Content == "ТРЁХПАЛУБНИК")
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
            else if ((string)tship.Content == "ДВУХПАЛУБНИК")
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
            else if ((string)tship.Content == "КАТЕР")
            {
                check_deadline(cells[i, j]);
            }
        }

        private UIElement dragobject = null;
        private Border temp = null;
        private Label tship = null;
        private bool enter_in_bord = false;
        private Border _poinOfDrag = null;
        private bool point_true = false;
        private bool result;

        private void Ship_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.dragobject = sender as UIElement;
            this.tship = sender as Label;
            if (this.dragobject != null)
            {
                if (e.GetPosition(canvas).X > (grid.ActualWidth - 600) / 2 &&
                    e.GetPosition(canvas).X < grid.ActualWidth - (grid.ActualWidth - 600) / 2 &&
                    e.GetPosition(canvas).Y > (grid.ActualHeight - 600) / 2 &&
                    e.GetPosition(canvas).Y < grid.ActualHeight - (grid.ActualHeight - 600) / 2)
                {
                    point_true = true;
                }

                RotateTransform trnsf = tship.LayoutTransform as RotateTransform;

                if (tship != null && trnsf != null && trnsf.Angle == 90)
                {
                    situtation = true;
                }
                else if (tship != null && trnsf != null && trnsf.Angle == 0)
                {
                    situtation = false;
                }

                DragDrop.DoDragDrop(this.dragobject, this.dragobject, DragDropEffects.All);
            }
        }


        private void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            grid.Width = window.ActualWidth;
            grid.Height = window.ActualHeight;
            
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
            
            Canvas.SetLeft(shipT,bordX + (grid.Width - field.Width) / 2);
            Canvas.SetTop(shipT,bordY + (grid.Height - field.Height) / 2);
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {

            this.temp = sender as Border;
            enter_in_bord = true;
        }

        bool situtation = false;
        bool mayroll = true;

        private void Ship_MouseMove(object sender, DragEventArgs e)
        {
            if (this.dragobject == null)
            {
                return;
            }

            bool isKeyPressed = Keyboard.IsKeyDown(Key.R);
            bool isKeyRelease = Keyboard.IsKeyUp(Key.R);
            if (this.dragobject != null && (string)tship.Content != "КАТЕР" && isKeyPressed && mayroll)
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

            Canvas.SetTop(this.dragobject, position.Y + 10);
            Canvas.SetLeft(this.dragobject, position.X + 10);
        }

        private void check_ship_on_border(Border bord, Label ttship)
        {
            GeneralTransform t1 = bord.TransformToVisual(this);
            GeneralTransform t2 = ttship.TransformToVisual(this);
            Rect r1 = t1.TransformBounds(new Rect()
                { X = 0, Y = 0, Width = bord.ActualWidth, Height = bord.ActualHeight });
            Rect r2 = t2.TransformBounds(new Rect()
                { X = 0, Y = 0, Width = ttship.ActualWidth, Height = ttship.ActualHeight });
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


        private void ButtonResetShips_OnClick(object sender, RoutedEventArgs e)
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

        }

        private void gameField()
        {
            ButtonStartGame.Visibility = Visibility.Hidden;
            ButtonResetShips.Visibility = Visibility.Hidden;

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
        } //TODO доделать однопалубники и прихуевертить горизонтальные проверки в расстановку кораблей

        private bool[] checked_edges(int i, int j, int cc, int cc2) //TODO доделать проверки для 3 и 2х палубников, сделать отдельные горизонтальнеые проверки
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
        } //Сделать для однопалубника проверки по диагонали

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

            ButtonPlaceShips.Visibility = Visibility.Hidden;
            ButtonContinue.Visibility = Visibility.Hidden;
            field.Visibility = Visibility.Hidden;
            GameField.Visibility = Visibility.Visible;
            ButtonShowCompShips.Visibility = Visibility.Visible;
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
                    comphit();
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

        
        private bool hit = false;
        private void GameCellClick(object sender, RoutedEventArgs e)
        {
            Button ttemp = sender as Button;
            check_hitplayer(ttemp);

            if ((string)ttemp.Tag != "false" && (string)ttemp.Tag != "true")
            {
                if (!hit)
                {
                    ttemp.Background = new SolidColorBrush(Colors.Firebrick);
                    ttemp.Tag = "false";
                    field_with_comphits();
                }
                else if (hit)
                {
                    ttemp.Background = new SolidColorBrush(Colors.Blue);
                    ttemp.Tag = "true";
                    hit = false;
                }
            }
        }

        private int[] originalHit = new int[2];
        
        private void comphit() //TODO бывает не считывает попадания
        {
            if (originalHit[0] == -1)
            {
                originalHit[0] = ran.Next(0, 9);
                originalHit[1] = ran.Next(0, 9);
            }

            int i = originalHit[0];
            int j = originalHit[1];

            var blue = new SolidColorBrush(Colors.Blue);
            var red = new SolidColorBrush(Colors.Firebrick);

            check_hit(cells[i,j]);
            if (cells[i, j].Background != blue && cells[i, j].Background != red)
            {
                if (hitShip)
                {
                    cells[i, j].Background = blue;
                    hit_on_ship();
                    if_hit(i, j);
                }
                else
                {
                    cells[i, j].Background = red;
                }
            }
            else
            {
                originalHit[0] = -1;
                comphit();
            }
        }

        private int position_hit = 0;
        private bool rightway = true, leftway = true, upway = true, downway = true;
        
        private void if_hit(int i, int j) //TODO при попадании по краблю бывает ложно ставит непопадание 
        {
            if (hitShip)
            {
                bool jEdgeR = false,jEdgeL = false,iEdgeU = false,iEdgeD = false;
                hitShip = false;
                int[] lie = new int[4];

                if (j == 9)
                {
                    jEdgeR = true;
                }

                if (j == 0)
                {
                    jEdgeL = true;
                }

                if (i == 0)
                {
                    iEdgeU = true;
                }

                if (i == 9)
                {
                    iEdgeD = true;
                }

                int luck = 0;

                if (position_hit == 0)
                {
                    if (jEdgeR || !rightway)
                    {
                        lie[1] = 1;
                    }
                    
                    if (jEdgeL || !leftway)
                    {
                        lie[0] = 1;
                    }
                    
                    if (iEdgeU || !upway)
                    {
                        lie[2] = 1;
                    }

                    if (iEdgeD || !downway)
                    {
                        lie[3] = 1;
                    }
                    
                    
                    luck = ran.Next(0, 3);

                    while (lie[luck] == 1)
                    {
                        luck = ran.Next(0, 3);
                    }
                }
                else if (position_hit == 1)
                {
                    if (jEdgeL)
                    {
                        i = originalHit[0];
                        j = originalHit[1];
                        
                        luck = 1;
                    }
                    else
                    {
                        luck = 0;
                    }
                } 
                else if (position_hit == 2)
                {
                    if (jEdgeR)
                    {
                        i = originalHit[0];
                        j = originalHit[1];
                        
                        luck = 0;
                    }
                    else
                    {
                        luck = 1;
                    }
                }
                else if (position_hit == 3)
                {
                    if (iEdgeU)
                    {
                        i = originalHit[0];
                        j = originalHit[1];
                        
                        luck = 3;
                    }
                    else
                    {
                        luck = 2;
                    }
                }
                else if (position_hit == 4)
                {
                    if (iEdgeD)
                    {
                        i = originalHit[0];
                        j = originalHit[1];
                        
                        luck = 2;
                    }
                    else
                    {
                        luck = 3;
                    }
                }
                
                
                if (luck == 0)
                {
                    check_hit(cells[i, j - 1]);
                    if (hitShip)
                    {
                        cells[i, j - 1].Background = new SolidColorBrush(Colors.Blue);
                        hit_on_ship();
                        position_hit = 1;
                        if_hit(i, j - 1);
                    }
                    else if (position_hit == 1)
                    {
                        position_hit = 2;
                        cells[i,j].Background = new SolidColorBrush(Colors.Firebrick);
                    }
                    else
                    {
                        leftway = false;
                        cells[i,j].Background = new SolidColorBrush(Colors.Firebrick);
                    }
                }
                else if (luck == 1)
                {
                    check_hit(cells[i, j + 1]);
                    if (hitShip)
                    {
                        cells[i, j + 1].Background = new SolidColorBrush(Colors.Blue);
                        hit_on_ship();
                        position_hit = 2;
                        if_hit(i, j + 1);
                    }
                    else if (position_hit == 2)
                    {
                        position_hit = 1;
                        cells[i,j].Background = new SolidColorBrush(Colors.Firebrick);
                    }
                    else
                    {
                        rightway = false;
                        cells[i,j].Background = new SolidColorBrush(Colors.Firebrick);
                    }
                }
                else if (luck == 2)
                {
                    check_hit(cells[i - 1, j]);
                    if (hitShip)
                    {
                        cells[i - 1, j].Background = new SolidColorBrush(Colors.Blue);
                        hit_on_ship();
                        position_hit = 3;
                        if_hit(i - 1, j);
                    }
                    else if (position_hit == 3)
                    {
                        position_hit = 4;
                        cells[i,j].Background = new SolidColorBrush(Colors.Firebrick);
                    }
                    else
                    {
                        upway = false;
                        cells[i,j].Background = new SolidColorBrush(Colors.Firebrick);
                    }
                }
                else if (luck == 3)
                {
                    check_hit(cells[i + 1, j]);
                    if (hitShip)
                    {
                        cells[i + 1, j].Background = new SolidColorBrush(Colors.Blue);
                        hit_on_ship();
                        position_hit = 4;
                        if_hit(i + 1, j);
                    }
                    else if (position_hit == 4)
                    {
                        position_hit = 3;
                        cells[i,j].Background = new SolidColorBrush(Colors.Firebrick);
                    }
                    else
                    {
                        downway = false;
                        cells[i,j].Background = new SolidColorBrush(Colors.Firebrick);
                    }
                }
            }
        }

        private void field_with_comphits()
        {
            field.Visibility = Visibility.Hidden;
            originalHit[0] = -1;
            comphit();
            show_unvisible2();
        }

        private void show_unvisible2()
        {
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

            ButtonContinue.Visibility = Visibility.Visible;
            field.Visibility = Visibility.Visible;
            GameField.Visibility = Visibility.Hidden;
        }
        
        private void hit_on_ship() 
        {
            if (catch_ship.Name == "Ship4" && (string)catch_ship.Content != "Корабль уничтожен")
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
            else if (catch_ship.Name == "Ship3_1" && (string)catch_ship.Content != "Корабль уничтожен")
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
            else if (catch_ship.Name == "Ship3_2" && (string)catch_ship.Content != "Корабль уничтожен")
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
            else if (catch_ship.Name == "Ship2_1" && (string)catch_ship.Content != "Корабль уничтожен")
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
            else if (catch_ship.Name == "Ship2_2" && (string)catch_ship.Content != "Корабль уничтожен")
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
            else if (catch_ship.Name == "Ship2_3" && (string)catch_ship.Content != "Корабль уничтожен")
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
            else if ((string)catch_ship.Content != "Корабль уничтожен")
            {
                delete_ship();
            }
        }

        private void delete_ship()
        {
            catch_ship.Content = "Корабль уничтожен";
            catch_ship.Tag = "destroyed";

            position_hit = 0;
            originalHit[0] = -1;
            hitShip = false;
            comphit();
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

        private void ButtonShowCompShips_OnClick(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (compships[i, j] == 1)
                    {
                        game_cells[i,j].Background = new SolidColorBrush(Colors.Blue);
                    }
                    else
                    {
                        game_cells[i,j].Background = new SolidColorBrush(Colors.Firebrick);
                    }
                }
            }
        }

        private void ButtonPlaceShips_OnClick(object sender, RoutedEventArgs e)
        {
            Canvas.SetLeft(Ship4,Canvas.GetLeft(cells[0, 0]) + (window.ActualWidth - field.ActualWidth) / 2);
            Canvas.SetTop(Ship4,Canvas.GetTop(cells[0, 0]) + (window.ActualHeight - field.ActualHeight) / 2);
            Ship4.Tag = "touched";
            
            Canvas.SetLeft(Ship3_1,Canvas.GetLeft(cells[0, 5]) + (window.ActualWidth - field.ActualWidth) / 2);
            Canvas.SetTop(Ship3_1,Canvas.GetTop(cells[0, 5]) + (window.ActualHeight - field.ActualHeight) / 2);
            Ship3_1.Tag = "touched";
            
            Canvas.SetLeft(Ship3_2,Canvas.GetLeft(cells[2, 0]) + (window.ActualWidth - field.ActualWidth) / 2);
            Canvas.SetTop(Ship3_2,Canvas.GetTop(cells[2, 0]) + (window.ActualHeight - field.ActualHeight) / 2);
            Ship3_2.Tag = "touched";
            
            Canvas.SetLeft(Ship2_1,Canvas.GetLeft(cells[2, 4]) + (window.ActualWidth - field.ActualWidth) / 2);
            Canvas.SetTop(Ship2_1,Canvas.GetTop(cells[2, 4]) + (window.ActualHeight - field.ActualHeight) / 2);
            Ship2_1.Tag = "touched";
            
            Canvas.SetLeft(Ship2_2,Canvas.GetLeft(cells[2, 7]) + (window.ActualWidth - field.ActualWidth) / 2);
            Canvas.SetTop(Ship2_2,Canvas.GetTop(cells[2, 7]) + (window.ActualHeight - field.ActualHeight) / 2);
            Ship2_2.Tag = "touched";
            
            Canvas.SetLeft(Ship2_3,Canvas.GetLeft(cells[4, 0]) + (window.ActualWidth - field.ActualWidth) / 2);
            Canvas.SetTop(Ship2_3,Canvas.GetTop(cells[4, 0]) + (window.ActualHeight - field.ActualHeight) / 2);
            Ship2_3.Tag = "touched";
            
            Canvas.SetLeft(Ship1_1,Canvas.GetLeft(cells[0, 9]) + (window.ActualWidth - field.ActualWidth) / 2);
            Canvas.SetTop(Ship1_1,Canvas.GetTop(cells[0, 9]) + (window.ActualHeight - field.ActualHeight) / 2);
            Ship1_1.Tag = "touched";
            
            Canvas.SetLeft(Ship1_2,Canvas.GetLeft(cells[4, 3]) + (window.ActualWidth - field.ActualWidth) / 2);
            Canvas.SetTop(Ship1_2,Canvas.GetTop(cells[4, 3]) + (window.ActualHeight - field.ActualHeight) / 2);
            Ship1_2.Tag = "touched";
            
            Canvas.SetLeft(Ship1_3,Canvas.GetLeft(cells[4, 5]) + (window.ActualWidth - field.ActualWidth) / 2);
            Canvas.SetTop(Ship1_3,Canvas.GetTop(cells[4, 5]) + (window.ActualHeight - field.ActualHeight) / 2);
            Ship1_3.Tag = "touched";
            
            Canvas.SetLeft(Ship1_4,Canvas.GetLeft(cells[4, 7]) + (window.ActualWidth - field.ActualWidth) / 2);
            Canvas.SetTop(Ship1_4,Canvas.GetTop(cells[4, 7]) + (window.ActualHeight - field.ActualHeight) / 2);
            Ship1_4.Tag = "touched";
            
            
            arrShipPosition[0] = 0;
            arrShipPosition[1] = 9;
            arrShipPosition[2] = 4;
            arrShipPosition[3] = 3;
            arrShipPosition[4] = 4;
            arrShipPosition[5] = 5;
            arrShipPosition[6] = 4;
            arrShipPosition[7] = 7;
            arrShipPosition[8] = 2;
            arrShipPosition[9] = 4;
            arrShipPosition[10] = 2;
            arrShipPosition[11] = 7;
            arrShipPosition[12] = 4;
            arrShipPosition[13] = 0;
            arrShipPosition[14] = 0;
            arrShipPosition[15] = 5;
            arrShipPosition[16] = 2;
            arrShipPosition[17] = 0;
            arrShipPosition[18] = 0;
            arrShipPosition[19] = 0;
        }
    }
}
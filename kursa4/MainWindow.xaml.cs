using System;
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
        private bool[,] compships = new bool[10,10];

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
        } //TODO при быстром перемещении корабля хавается не та клетка, может зайти в deadzone корябля

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
                        temp = cells[i, j - 3];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                    else if ((string)tship.Content == "ТРЁХПАЛУБНИК")
                    {
                        temp = cells[i, j - 2];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                    else if ((string)tship.Content == "ДВУХПАЛУБНИК")
                    {
                        temp = cells[i, j - 1];
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
                        temp = cells[i, j - 2];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                    else if ((string)tship.Content == "ТРЁХПАЛУБНИК")
                    {
                        temp = cells[i, j - 1];
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
                        temp = cells[i, j - 1];
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
                        temp = cells[i - 3, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                    else if ((string)tship.Content == "ТРЁХПАЛУБНИК")
                    {
                        temp = cells[i - 2, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                    else if ((string)tship.Content == "ДВУХПАЛУБНИК")
                    {
                        temp = cells[i - 1, j];
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
                        temp = cells[i - 2, j];
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                    else if ((string)tship.Content == "ТРЁХПАЛУБНИК")
                    {
                        temp = cells[i - 1, j];
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
                        temp = cells[i - 1, j];
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
        
        private void comp_field() //TODO доделать генерацию поля ботяры
        {
            int i = 0, j = 0;

            while (count < 10)
            {
                i++;
                while (compships[i, j] == false)
                {
                    if (ran.Next(0, 1) == 0)
                    {
                        compships[i, j] = false;
                    } else if ()
                    {
                        compships[i, j++] = true;
                        compship_generator(i,j);
                    }
                }
            }
        }

        private void compship_generator(int i,int j)
        {
            if (count == 0)
            {
                count++;

                if (ran.Next(0, 1) == 0)
                {
                    if (i == 0 && j == 0)
                    {
                        
                    }
                }
            }
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

            field.Visibility = Visibility.Visible;
            GameField.Visibility = Visibility.Visible;
        }

        private void check_ship_on_button(Button ttemp, Label ttship)
        {
            GeneralTransform t1 = ttemp.TransformToVisual(this);
            GeneralTransform t2 = ttship.TransformToVisual(this);
            Rect r1 = t1.TransformBounds(new Rect()
                { X = 0, Y = 0, Width = ttemp.ActualWidth, Height = ttemp.ActualHeight });
            Rect r2 = t2.TransformBounds(new Rect()
                { X = 0, Y = 0, Width = ttship.ActualWidth, Height = ttship.ActualHeight });
            result = r1.IntersectsWith(r2);
            catch_ship = ttship;
            shipname = ttship.Name;
        }
        
        private int[] hitCapacity = new int[6];
        private Label catch_ship = null;
        private string shipname;

        private void check_hit(Button bord)
        {
            if (!result)
            {
                check_ship_on_button(bord, Ship4);
            }

            if (!result)
            {
                check_ship_on_button(bord, Ship3_1);
            }

            if (!result)
            {
                check_ship_on_button(bord, Ship3_2);
            }

            if (!result)
            {
                check_ship_on_button(bord, Ship2_1);
            }

            if (!result)
            {
                check_ship_on_button(bord, Ship2_2);
            }

            if (!result)
            {
                check_ship_on_button(bord, Ship2_3);
            }

            if (!result)
            {
                check_ship_on_button(bord, Ship1_1);
            }

            if (!result)
            {
                check_ship_on_button(bord, Ship1_2);
            }

            if (!result)
            {
                check_ship_on_button(bord, Ship1_3);
            }

            if (!result)
            {
                check_ship_on_button(bord, Ship1_4);
            }
        }

        private void GameCellClick(object sender, RoutedEventArgs e)
        {
            result = true;
            Button ttemp = sender as Button;

            if (result)
            {
                ttemp.Background = new SolidColorBrush(Colors.Firebrick);
                result = false;
                
            }

        }

        private void hit_on_ship(Button ttemp) 
        {
            if (catch_ship.Name == "Ship4")
            {
                if (hitCapacity[0] == 4)
                {
                    delete_ship();
                }
                else
                {
                    hitCapacity[0]++;
                }
            }
            else if (catch_ship.Name == "Ship3_1")
            {
                if (hitCapacity[1] == 3)
                {
                    delete_ship();
                }
                else
                {
                    hitCapacity[1]++;
                }
            }
            else if (catch_ship.Name == "Ship3_2")
            {
                if (hitCapacity[2] == 3)
                {
                    delete_ship();
                }
                else
                {
                    hitCapacity[2]++;
                }
            }
            else if (catch_ship.Name == "Ship2_1")
            {
                if (hitCapacity[3] == 2)
                {
                    delete_ship();
                }
                else
                {
                    hitCapacity[3]++;
                }
            }
            else if (catch_ship.Name == "Ship2_2")
            {
                if (hitCapacity[4] == 2)
                {
                    delete_ship();
                }
                else
                {
                    hitCapacity[4]++;
                }
            }
            else if (catch_ship.Name == "Ship2_3")
            {
                if (hitCapacity[5] == 2)
                {
                    delete_ship();
                }
                else
                {
                    hitCapacity[5]++;
                }
            }
            else if (catch_ship.Name == "Ship1_1")
            {
                delete_ship();
            }
            else if (catch_ship.Name == "Ship1_2")
            {
                delete_ship();
            }
            else if (catch_ship.Name == "Ship1_3")
            {
                delete_ship();
            }
            else if (catch_ship.Name == "Ship1_4")
            {
                delete_ship();
            }
        }

        private void delete_ship()
        {
            
        } //TODO придумать удаление корабля
        
    }
}
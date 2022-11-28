using System;
using System.IO.Pipes;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Remoting.Channels;
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
        public MainWindow()
        {
            InitializeComponent();
            cells = new Border[10, 10];
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
            Ship3_1.Visibility = Visibility.Visible;
            Ship3_2.Visibility = Visibility.Visible;
            Ship2_1.Visibility = Visibility.Visible;
            Ship2_2.Visibility = Visibility.Visible;
            Ship2_3.Visibility = Visibility.Visible;
            Ship1_1.Visibility = Visibility.Visible;
            Ship1_2.Visibility = Visibility.Visible;
            Ship1_3.Visibility = Visibility.Visible;
            Ship1_4.Visibility = Visibility.Visible;
            ButtonPlay.Visibility = Visibility.Hidden;
            field.Visibility = Visibility.Visible;
            
        }
        
        private void DrawField()
        {
            for (int i = 0; i < 10; ++i)
            {
                for (int j = 0; j < 10; ++j)
                {
                    cells[i, j] = new Border() { Width = 60, Height = 60, 
                        BorderThickness = new Thickness(1), 
                        BorderBrush = Brushes.Black, Background = Brushes.Cornsilk,
                    };
                    cells[i, j].AllowDrop = true;
                    cells[i, j].Name = "Border_" + i.ToString() + j.ToString();
                    if (j == 9)
                    {
                        cells[i, j].Tag = "critical_..9";
                    } else if (j == 8)
                    {
                        cells[i, j].Tag = "critical_..8";
                    } else if (j == 7)
                    {
                        cells[i, j].Tag = "critical_..7";
                    }

                    if (i == 9 && j != 9 && j != 8 && j != 7)
                    { 
                        cells[i, j].Tag = "critical_9..";
                    } else if (i == 8 && j != 9 && j != 8 && j != 7)
                    { 
                        cells[i, j].Tag = "critical_8..";
                    } else if (i == 7 && j != 9 && j != 8 && j != 7)
                    { 
                        cells[i, j].Tag = "critical_7..";
                    }
                    
                    Canvas.SetLeft(cells[i, j], 0 + j * 60);
                    Canvas.SetTop(cells[i, j], 0 + i * 60);
                    field.Children.Add(cells[i, j]);
                    cells[i, j].DragEnter += OnDragEnter;
                    cells[i,j].PreviewDrop += OnPreviewDrop;
                }
            }
        }

        private void OnPreviewDrop(object sender, DragEventArgs e)
        {
            if (this.dragobject != null && temp != null && this.enter_in_bord)
            {
                place_ship();
            }
            

            if (tship != Ship4  && !result)
            {
                tempotemp(Ship4);
            }
            
            if (tship != Ship3_1 && !result)
            {
                tempotemp(Ship3_1);
            }
            
            if (tship != Ship3_2 && !result)
            {
                tempotemp(Ship3_2);
            }

            if (tship != Ship2_1 && !result)
            {
                tempotemp(Ship2_1);
            }

            if (tship != Ship2_2 && !result)
            {
                tempotemp(Ship2_2);
            }

            if (tship != Ship2_3 && !result)
            {
                tempotemp(Ship2_3);
            }

            if (tship != Ship1_1 && !result)
            {
                tempotemp(Ship1_1);    
            }

            if (tship != Ship1_2 && !result)
            {
                tempotemp(Ship1_2);
            }

            if (tship != Ship1_3 && !result)
            {
                tempotemp(Ship1_3);
            }

            if (tship != Ship1_4 && !result) 
            {
                tempotemp(Ship1_4);
            }
            
            if (this.dragobject != null && result)
            {
                Canvas.SetTop(this.dragobject, _positionOfDrag.Y);     //TODO сделать чтобы корабль сначала ставился а потом исправлялся!!
                Canvas.SetLeft(this.dragobject, _positionOfDrag.X);
                result = false;
            }
            this.dragobject = null;
            this.temp = null;
        }

        private void place_ship()
        {
            if (!situtation)
            {
                if ((string)temp.Tag == "critical_..9")
                {
                    if ((string)tship.Content == "ЧЕТЫРЁХПАЛУБНИК")
                    {
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2 - 180);
                        tship.Tag = "touched";
                    }
                    else if ((string)tship.Content == "ТРЁХПАЛУБНИК")
                    {
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2 - 120);
                        tship.Tag = "touched";
                    }
                    else if ((string)tship.Content == "ДВУХПАЛУБНИК")
                    {
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2 - 60);
                        tship.Tag = "touched";
                    }
                    else if ((string)tship.Content == "КАТЕР")
                    {
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                }
                else if ((string)temp.Tag == "critical_..8")
                {
                    if ((string)tship.Content == "ЧЕТЫРЁХПАЛУБНИК")
                    {
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2 - 120);
                        tship.Tag = "touched";
                    }
                    else if ((string)tship.Content == "ТРЁХПАЛУБНИК")
                    {
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2 - 60);
                        tship.Tag = "touched";
                    }
                    else if ((string)tship.Content == "ДВУХПАЛУБНИК")
                    {
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                    else if ((string)tship.Content == "КАТЕР")
                    {
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                }
                else if ((string)temp.Tag == "critical_..7")
                {
                    if ((string)tship.Content == "ЧЕТЫРЁХПАЛУБНИК")
                    {
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2 - 60);
                        tship.Tag = "touched";
                    }
                    else if ((string)tship.Content == "ТРЁХПАЛУБНИК")
                    {
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                    else if ((string)tship.Content == "ДВУХПАЛУБНИК")
                    {
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                    else if ((string)tship.Content == "КАТЕР")
                    {
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                }
                else
                {
                    Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                    Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                    tship.Tag = "touched";
                }
            }
            else
            {
                if ((string)temp.Tag == "critical_9.." || (string)temp.Name == "Border_99" ||
                    (string)temp.Name == "Border_98" || (string)temp.Name == "Border_97")
                {
                    if ((string)tship.Content == "ЧЕТЫРЁХПАЛУБНИК")
                    {
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2 - 180);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                    else if ((string)tship.Content == "ТРЁХПАЛУБНИК")
                    {
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2 - 120);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                    else if ((string)tship.Content == "ДВУХПАЛУБНИК")
                    {
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2 - 60);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                    else if ((string)tship.Content == "КАТЕР")
                    {
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                }
                else if ((string)temp.Tag == "critical_8.." || (string)temp.Name == "Border_89" ||
                         (string)temp.Name == "Border_88" || (string)temp.Name == "Border_87")
                {
                    if ((string)tship.Content == "ЧЕТЫРЁХПАЛУБНИК")
                    {
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2 - 120);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                    else if ((string)tship.Content == "ТРЁХПАЛУБНИК")
                    {
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2 - 60);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                    else if ((string)tship.Content == "ДВУХПАЛУБНИК")
                    {
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                    else if ((string)tship.Content == "КАТЕР")
                    {
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                }
                else if ((string)temp.Tag == "critical_7.." || (string)temp.Name == "Border_79" ||
                         (string)temp.Name == "Border_78" || (string)temp.Name == "Border_77")
                {
                    if ((string)tship.Content == "ЧЕТЫРЁХПАЛУБНИК")
                    {
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2 - 60);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                    else if ((string)tship.Content == "ТРЁХПАЛУБНИК")
                    {
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                    else if ((string)tship.Content == "ДВУХПАЛУБНИК")
                    {
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                    else if ((string)tship.Content == "КАТЕР")
                    {
                        Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                        Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                        tship.Tag = "touched";
                    }
                }
                else
                {
                    Canvas.SetTop(this.dragobject, Canvas.GetTop(temp) + (grid.Height - field.Height) / 2);
                    Canvas.SetLeft(this.dragobject, Canvas.GetLeft(temp) + (grid.Width - field.Width) / 2);
                    tship.Tag = "touched";
                }
            }
        }

        private UIElement dragobject = null;
        private Border temp = null;
        private Label tship = null;
        private bool enter_in_bord = false;
        private Point _positionOfDrag;
        private bool result;
        private void Ship_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.dragobject = sender as UIElement;
            this.tship = sender as Label;
            _positionOfDrag = e.GetPosition(sender as IInputElement);
            if (this.dragobject != null)
            {
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
        
        private void tempotemp(Label ttship)
        {
            GeneralTransform t1 = this.tship.TransformToVisual(this);
            GeneralTransform t2 = ttship.TransformToVisual(this);
            Rect r1 = t1.TransformBounds(new Rect() {X = 0, Y = 0, Width = this.tship.ActualWidth, Height = this.tship.ActualHeight});
            Rect r2 = t2.TransformBounds(new Rect() {X = 0, Y = 0, Width = ttship.ActualWidth, Height = ttship.ActualHeight});
            result = r1.IntersectsWith(r2);
        }

        private void Ship4_OnDragEnter(object sender, DragEventArgs e)
        {
            /*if (sender as Label != this.tship && sender as Label != null)
            {
                var sd = sender as Label;
                tempotemp(sd);
            }*/
        }

        private void Ship4_OnDragOver(object sender, DragEventArgs e)
        {
            /*if (sender as Label != null)
            {
                result = false;
            }*/
        }
    }
}
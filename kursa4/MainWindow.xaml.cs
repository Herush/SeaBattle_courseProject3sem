using System;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Runtime.Remoting.Channels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
                    Canvas.SetLeft(cells[i, j], 0 + j * 60);
                    Canvas.SetTop(cells[i, j], 0 + i * 60);
                    field.Children.Add(cells[i, j]);
                    cells[i, j].PreviewMouseUp += ShipOver;
                    cells[i, j].MouseEnter += OnDragEnter;
                }
            }
        }


        private UIElement dragobject = null;
        private Border temp = null;
        private Point offset;
        private bool enter_in_bord = false;
        
        private void Ship_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.dragobject = sender as UIElement;
            this.offset = e.GetPosition(this.canvas);
            this.offset.Y -= Canvas.GetTop(this.dragobject);
            this.offset.X -= Canvas.GetLeft(this.dragobject);
            this.canvas.CaptureMouse();
        }
        
        private void ShipOver(object sender, MouseButtonEventArgs e)
        {
            this.canvas.ReleaseMouseCapture();

            if (this.enter_in_bord)
            {
                Canvas.SetTop(this.dragobject,Canvas.GetTop(temp)); //todo улетает корабль в пизду
                Canvas.SetLeft(this.dragobject,Canvas.GetLeft(temp));
            }
            this.dragobject = null;
        }

        private void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            grid.Width = window.ActualWidth;
            grid.Height = window.ActualHeight;
        }

       
        private void OnDragEnter(object sender, MouseEventArgs e)
        {
            temp = sender as Border;
            enter_in_bord = true;
        }

        private void Ship_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.dragobject == null)
            {
                return;
            }

            var position = e.GetPosition(sender as IInputElement);
            Canvas.SetTop(this.dragobject, position.Y - this.offset.Y);
            Canvas.SetLeft(this.dragobject, position.X - this.offset.X);
        }

        private void Canvas_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (this.dragobject != null && (string)temp.Tag != "blocked" && e.Key == Key.R)
            {
                var rotateTransform = this.dragobject.RenderTransform as RotateTransform;
                var transform = new RotateTransform(90 + (rotateTransform?.Angle ?? 0));
                this.dragobject.RenderTransform = transform;
            }
        }
    }
}
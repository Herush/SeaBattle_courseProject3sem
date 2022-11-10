using System;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using kursa4.Properties;
using Label = System.Windows.Controls.Label;

namespace kursa4
{
    public partial class MainWindow
    {
        private Button[,] cells;
        public MainWindow()
        {
            InitializeComponent();
            cells = new Button[10, 10];
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
                    cells[i, j] = new Button() { Width = 60, Height = 60 };
                    cells[i, j].Name = "button_" + i.ToString() + j.ToString();
                    Canvas.SetLeft(cells[i, j], 0 + j * 60);
                    Canvas.SetTop(cells[i, j], 0 + i * 60);
                    field.Children.Add(cells[i, j]);
                }
            }
        }
        
        private void Ship_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Label Ship = (Label)sender;
            DragDrop.DoDragDrop(Ship, Ship.Content, DragDropEffects.Move);
        }
        
        private void ShipOver(object sender, DragEventArgs e)
        {
            Point dropPosition = e.GetPosition(canvas);
            
            Canvas.SetLeft(Ship4,dropPosition.X);
            Canvas.SetTop(Ship4,dropPosition.Y);
        }

        private void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            grid.Width = window.ActualWidth;
            grid.Height = window.ActualHeight;
        }

        private void Field_OnDrop(object sender, DragEventArgs e)
        {
            //cells[0,0]
        }
    }
}
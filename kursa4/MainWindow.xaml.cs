using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using kursa4.Properties;

namespace kursa4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void ButtonPlay_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Height = 600;
            Application.Current.MainWindow.Width = 1000;
            ImageBrush myBrush = new ImageBrush();
            myBrush.ImageSource = (new BitmapImage(new Uri(@"C:\\Users\\halop\\RiderProjects\\kursa4\\kursa4\\GameBackground.jpg")));
            kirillpopusk.Background = myBrush;
            ButtonPlay.Visibility = Visibility.Hidden;
        }
    }

    /*public class GameWindow
    {
        public GameField()
        {
            
        }
    } */
}
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using static System.Diagnostics.Debug;

namespace MapEditor
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Image image = new Image();
        BitmapImage bitmapImage = new BitmapImage();
        private string filename;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenMap_Click(object sender, RoutedEventArgs e)
        {
            OpenImage();
            ReadImage();
        }
        private void ReadImage()
        {
            System.Drawing.Bitmap b = new System.Drawing.Bitmap(filename);
            WriteLine(b.Palette.Entries.Length);
            b.Palette.Entries.ToList().ForEach(x =>  System.Diagnostics.Debug.WriteLine($"R: {x.R}, G: {x.G}, B: {x.B}, A: {x.A}"));
            int imageWidth = bitmapImage.PixelWidth;
            int imageHeight = bitmapImage.PixelHeight;
            
            int z = 0;
        }
        private void OpenImage()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
            if (dialog.ShowDialog() == true)
            {
                filename = dialog.FileName;
                image = new Image();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(dialog.FileName);
                bitmapImage.EndInit();
                image.Source = bitmapImage;
            }
            column1.Children.Add(image);
            OpenMap.Visibility = Visibility.Hidden;
        }
    }
}

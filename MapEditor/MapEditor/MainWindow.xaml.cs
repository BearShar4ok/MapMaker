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
        private List<Color> col = new List<Color>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenMap_Click(object sender, RoutedEventArgs e)
        {
            OpenImage();
            ReadImage();
        }
        private bool IsExist(Color c)
        {
            for (int i = 0; i < col.Count; i++)
            {
                if (c == col[i])
                {
                    return true;
                }
            }
            return false;
        }
        private void BroseClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            Image im = new Image();
            dialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
            if (dialog.ShowDialog() == true)
            {
                BitmapImage bit = new BitmapImage();
                bit.BeginInit();
                bit.UriSource = new Uri(dialog.FileName);
                bit.EndInit();
                im.Source = bit;
            }


        }
        private void ReadImage()
        {
            System.Drawing.Bitmap b = new System.Drawing.Bitmap(filename);
            //WriteLine(b.Palette.Entries.Length);
            //b.Palette.Entries.ToList().ForEach(x =>  System.Diagnostics.Debug.WriteLine($"R: {x.R}, G: {x.G}, B: {x.B}, A: {x.A}"));
            int imageWidth = bitmapImage.PixelWidth;
            int imageHeight = bitmapImage.PixelHeight;
            int schet = 0;
            for (int i = 0; i < imageWidth; i+=4)
            {
                for (int j = 0; j < imageHeight; j+=4)
                {
                    var color = b.GetPixel(i, j);
                    Color c = (new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B))).Color;
                    if (!IsExist(c))
                    {
                        
                        ColorsChoice.RowDefinitions.Add(new RowDefinition());
                        col.Add(c);
                        Rectangle r = new Rectangle();
                        r.Margin = new Thickness(30);
                        r.Fill = new SolidColorBrush(c);
                        ColorsChoice.Children.Add(r);
                        r.ToolTip = c.ToString();
                        Grid.SetRow(r, schet);
                        Grid.SetColumn(r, 0);
                       
                        Button but = new Button();
                        but.Content = "Browse...";
                        but.Margin = new Thickness(30, 10, 30, 10);
                        Grid.SetRow(but, schet);
                        Grid.SetColumn(but, 2);
                        ColorsChoice.Children.Add(but);
                        but.Click += BroseClick;
                        schet++;
                    }
                }
            }
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

    public static class Addition
    {
        public static void Print(this string text) => WriteLine(text);
    }
}

using Microsoft.Win32;
using Newtonsoft.Json;
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
        Image imageFirst;
        Image imageLast;
        private Color[] colo;
        BitmapImage bitmapImage = new BitmapImage();
        private string filename;
        private string filenameBrose;
        private List<Color> col = new List<Color>();
        private List<(int X, int Y)> wasSq = new List<(int X, int Y)>();
        private List<Rectangle> recs = new List<Rectangle>();
        private enum Colisions { wall, glass, door, brokenWall, net, intererProhod, intererNoProhod, intererProhodCir, intererNoProhodCir }
        private Dictionary<Colisions, List<BoundingBox>> rectype = new Dictionary<Colisions, List<BoundingBox>>();
        private List<dynamic> saveFile = new List<dynamic>();
        const int pixelSize = 16;


        public MainWindow()
        {
            InitializeComponent();
            foreach (var item in Enum.GetValues(typeof(Colisions)))
            {
                rectype.Add((Colisions)item, new List<BoundingBox>());
            }
        }

        private void OpenMap_Click(object sender, RoutedEventArgs e)
        {
            OpenImage();
            ReadImage();
            
            imageLast.MouseLeftButtonDown += ImageClick;
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
        private void DrawInterer(int i, System.Drawing.Bitmap b1, System.Drawing.Bitmap b2, BitmapImage bit, int j)
        {
            for (int k = 0; k < bit.PixelWidth; k++)
            {
                for (int l = 0; l < bit.PixelHeight; l++)
                {
                    if (i + k < bitmapImage.PixelWidth && j + l < bitmapImage.PixelHeight)
                    {
                        if (b2.GetPixel(k, l).A > 240)
                        {
                            b1.SetPixel(i + k - bit.PixelWidth / 2, j + l - bit.PixelHeight / 2, b2.GetPixel(k, l));
                        }
                    }
                }
            }
        }
        private void BroseClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            Image im = new Image();
            BitmapImage bit = new BitmapImage();
            dialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
            if (dialog.ShowDialog() == true)
            {
                filenameBrose = dialog.FileName;

                bit.BeginInit();
                bit.UriSource = new Uri(dialog.FileName);
                bit.EndInit();
                im.Source = bit;
            }
            Rectangle r = new Rectangle();
            r.Fill = new ImageBrush(bit);
            ColorsChoice.Children.Add(r);
            int thisRow = (sender as Button).Tag.ToString().ToInt();
            Grid.SetRow(r, thisRow);
            Grid.SetColumn(r, 1);

            var rec = ColorsChoice.Children.Cast<UIElement>().First(el => Grid.GetRow(el) == thisRow && Grid.GetColumn(el) == 0) as Rectangle;
            System.Drawing.Bitmap b1 = (imageLast.Source as BitmapImage).ToBitmap();
            System.Drawing.Bitmap b2 = new System.Drawing.Bitmap(dialog.FileName);
            for (int i = 0; i < bitmapImage.PixelWidth; i += pixelSize)
            {
                for (int j = 0; j < bitmapImage.PixelHeight; j += pixelSize)
                {
                    var color = b1.GetPixel(i, j);
                    if (rec.Tag.ToString() == (new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B))).Color.ToString())
                    {
                        var cb = ColorsChoice.Children.Cast<UIElement>().First(el => Grid.GetRow(el) == thisRow && Grid.GetColumn(el) == 3) as ComboBox;
                        string val = cb.SelectedItem.ToString().ToLower();
                        if (val == Colisions.intererNoProhod.ToString().ToLower())
                        {
                            DrawInterer(i, b1, b2, bit, j);
                            rectype[Colisions.wall].Add(new BoundingBox(i, j, bit.PixelWidth, bit.PixelHeight));
                        }
                        else if (val == Colisions.intererNoProhodCir.ToString().ToLower())
                        {
                            DrawInterer(i, b1, b2, bit, j);
                            rectype[Colisions.wall].Add(new BoundingBox(i, j, bit.PixelWidth, bit.PixelHeight, true));
                        }
                        else if (val == Colisions.intererProhod.ToString().ToLower())
                        {
                            DrawInterer(i, b1, b2, bit, j);
                            rectype[Colisions.net].Add(new BoundingBox(i, j, bit.PixelWidth, bit.PixelHeight));
                        }
                        else if (val == Colisions.intererProhodCir.ToString().ToLower())
                        {
                            DrawInterer(i, b1, b2, bit, j);
                            rectype[Colisions.net].Add(new BoundingBox(i, j, bit.PixelWidth, bit.PixelHeight, true));
                        }
                        else if (val == Colisions.door.ToString().ToLower())
                        {

                        }
                        else
                        {
                            for (int k = 0; k < pixelSize; k++)
                            {
                                for (int l = 0; l < pixelSize; l++)
                                {
                                    if (i + k < bitmapImage.PixelWidth && j + l < bitmapImage.PixelHeight)
                                    {
                                        b1.SetPixel(i + k, j + l, b2.GetPixel(k, l));
                                    }

                                }
                            }
                        }
                    }
                }
            }

            imageLast.Source = b1.ToBitmapImage();
        }
        //private void 
        private bool IsSquarWas(int x, int y)
        {
            for (int i = 0; i < wasSq.Count; i++)
            {
                if (x == wasSq[i].X && y == wasSq[i].Y)
                {
                    return true;
                }
            }
            wasSq.Add((x,y));
            return false;
        }
        private void Prohod()
        {
            System.Drawing.Bitmap b = new System.Drawing.Bitmap(filename);
            for (int y = 0; y < bitmapImage.Height; y+=pixelSize)
            {
                for (int x = 0; x < bitmapImage.Width; x+=pixelSize)
                {
                    if (!IsSquarWas(x, y) && y < bitmapImage.Width - pixelSize && x < bitmapImage.Height - pixelSize)
                    {
                        var color = b.GetPixel(x, y);
                        Color c = (new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B))).Color;
                        BoundingBox rect = new BoundingBox(x, y, pixelSize, pixelSize);
                        wasSq.Add((x, y));
                        if (color == b.GetPixel(x + pixelSize, y))
                        {
                            //horizontal
                            int lengthX = 1;
                            for (int l = pixelSize; x + l + pixelSize < bitmapImage.Width && b.GetPixel(x + l - pixelSize, y) == b.GetPixel(x + l, y); l += pixelSize)
                            {
                                rect.width += pixelSize;
                                wasSq.Add((x + l, y));
                                lengthX = l;
                            }
                        }
                        else if (color == b.GetPixel(x, y + pixelSize))
                        {
                            //vertical
                            int lengthY = 1;
                            for (int l = pixelSize; y + l + pixelSize < bitmapImage.Height && b.GetPixel(x, y + l - pixelSize) == b.GetPixel(x, y + l); l += pixelSize)
                            {
                                rect.height += pixelSize;
                                wasSq.Add((x, y + l));
                                lengthY = l;
                            }

                        }


                        int row = 0;
                        for (int l = 0; l < recs.Count; l++)
                        {
                            if (recs[l].Tag.ToString() == c.ToString())
                            {
                                row = Grid.GetRow(recs[l]);
                            }
                        }
                        var cb = ColorsChoice.Children.Cast<UIElement>().First(el => Grid.GetRow(el) == row && Grid.GetColumn(el) == 3) as ComboBox;
                        string val = cb.SelectedItem.ToString().ToLower();
                        Colisions cls = Cols(val);
                        rectype[cls].Add(rect);
                    }


                }
            }
        }
        private Colisions Cols(string a)
        {
            foreach (var item in Enum.GetValues(typeof (Colisions)))
            {
                if (item.ToString().ToLower() == a)
                {
                    return (Colisions)item;
                }
            }
            return Colisions.net;
        }
        private void ImageClick(object sender, RoutedEventArgs e)
        {
            Prohod();
            saveFile = new List<dynamic>();
            saveFile.Add(rectype);
            System.Drawing.Bitmap b = new System.Drawing.Bitmap((imageLast.Source as BitmapImage).ToBitmap());
            System.Drawing.Color[,] image2d = new System.Drawing.Color[b.Width, b.Height];
            for (int i = 0; i < b.Width; i++)
            {
                for (int j = 0; j < b.Height; j++)
                {
                    image2d[i, j] = b.GetPixel(i, j);
                }
            }
            saveFile.Add(image2d);
            using (StreamWriter file = File.CreateText(@"C:\Users\bersh\Downloads\hotline_Miami\saves\S1.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, saveFile);
            }
        }
        private void ReadImage()
        {
            System.Drawing.Bitmap b = new System.Drawing.Bitmap(filename);
            int imageWidth = bitmapImage.PixelWidth;
            int imageHeight = bitmapImage.PixelHeight;
            int schet = 0;
            for (int i = 0; i < imageWidth; i+=pixelSize)
            {
                for (int j = 0; j < imageHeight; j+=pixelSize)
                {
                    var color = b.GetPixel(i, j);
                    Color c = (new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B))).Color;
                    if (!IsExist(c))
                    {
                        ColorsChoice.RowDefinitions.Add(new RowDefinition());
                        col.Add(c);
                        Rectangle r = new Rectangle();
                        //r.Margin = new Thickness(30);
                        r.Fill = new SolidColorBrush(c);
                        r.Tag = c.ToString();
                        ColorsChoice.Children.Add(r);
                        r.ToolTip = c.ToString();
                        Grid.SetRow(r, schet);
                        Grid.SetColumn(r, 0);
                        recs.Add(r);
                        
                        Button but = new Button();
                        but.Content = "Browse...";
                        but.Tag = schet;
                        //but.Margin = new Thickness(30, 10, 30, 10);
                        Grid.SetRow(but, schet);
                        Grid.SetColumn(but, 2);
                        ColorsChoice.Children.Add(but);
                        but.Click += BroseClick;
                        
                        ComboBox cb = new ComboBox();
                        foreach (var item in Enum.GetValues(typeof(Colisions)))
                        {
                            cb.Items.Add(item.ToString().ToLower());
                        }
                        cb.SelectedItem = Colisions.wall.ToString().ToLower();
                        cb.Tag = schet;
                        cb.VerticalContentAlignment = VerticalAlignment.Center;
                        cb.HorizontalContentAlignment = HorizontalAlignment.Center;
                        cb.FontSize = 20;
                        Grid.SetRow(cb, schet);
                        Grid.SetColumn(cb, 3);
                        ColorsChoice.Children.Add(cb);

                        schet++;
                    }
                }
            }
        }
        private void OpenImage()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
            if (dialog.ShowDialog() == true)
            {
                filename = dialog.FileName;
                imageFirst = new Image();
                imageLast = new Image();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(dialog.FileName);
                bitmapImage.EndInit();
                imageFirst.Source = bitmapImage;
                imageLast.Source = bitmapImage;
            }
            column1.Children.Add(imageFirst);
            OpenMap.Visibility = Visibility.Hidden;
            column1.Children.Add(imageLast);
            Grid.SetRow(imageLast, 1);
        }
    }

    public class BoundingBox 
    {
        public int posX = 0, posY = 0;
        public int width, height;
        public bool isCircle = false;
        public BoundingBox(int posX, int posY, int width, int height, bool isCircle=false)
        {
            this.posX = posX;
            this.posY = posY;
            this.width = width;
            this.height = height;
            this.isCircle = isCircle;
        }
    }

    

    public static class Addition
    {
        public static void Print(this string text) => WriteLine(text);
        public static int ToInt(this string input) => int.Parse(input);

        public static BitmapImage ToBitmapImage(this System.Drawing.Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                //bitmapImage.Freeze();

                return bitmapImage;
            }
        }

        public static System.Drawing.Bitmap ToBitmap(this BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new System.Drawing.Bitmap(bitmap);
            }
        }
    }
}

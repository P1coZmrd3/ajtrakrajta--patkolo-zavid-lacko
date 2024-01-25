using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.IO;

namespace Color_Finder
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            OBR.MouseLeftButtonDown += ImageControl_MouseLeftButtonDown;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Obrázky|*.jpg;*.jpeg;*.png;*.bmp|Všechny soubory|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                DisplayImage(filePath);
            }
        }

        private void ImageControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point position = e.GetPosition(OBR);
            Color color = GetPixelColor(position);
            INFO.Text = ($"Barva pixelu: R={color.R}, G={color.G}, B={color.B} ");
        }

        private Color GetPixelColor(Point position)
        {
            try
            {
                FormatConvertedBitmap formattedBitmap = new FormatConvertedBitmap();
                formattedBitmap.BeginInit();
                formattedBitmap.Source = (BitmapSource)OBR.Source;
                formattedBitmap.DestinationFormat = PixelFormats.Pbgra32;
                formattedBitmap.EndInit();

                CroppedBitmap croppedBitmap = new CroppedBitmap(formattedBitmap, new Int32Rect((int)position.X, (int)position.Y, 1, 1));
                byte[] pixels = new byte[4];
                croppedBitmap.CopyPixels(pixels, 4, 0);

                Color color = Color.FromArgb(pixels[3], pixels[2], pixels[1], pixels[0]);
                return color;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při získávání barvy pixelu: " + ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                return Colors.Transparent;
            }
        }

        private void DisplayImage(string filePath)
        {
            try
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(filePath, UriKind.RelativeOrAbsolute);
                bitmap.EndInit();
                OBR.Source = bitmap;
            }
            catch
            {
                MessageBox.Show("dej tam správnej soubor troubo");
            }
        }
    }
}
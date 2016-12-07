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
using System.Drawing;

namespace Madera_MMB.View_Crtl
{
    /// <summary>
    /// Logique d'interaction pour Modelisation.xaml
    /// </summary>
    public partial class Modelisation : Page
    {

        private Grid grid = new Grid();

        public Modelisation()
        {
            InitializeComponent();
            initialize();

        }

        private void initialize()
        {
            grid.ShowGridLines = true;
            grid.Margin = new Thickness(7);




            ColumnDefinition col;
            RowDefinition row;

            for (int i = 0; i < 30; i++)
            {
                row = new RowDefinition();
                row.Height = new GridLength(1, GridUnitType.Star);
                grid.RowDefinitions.Add(row);
            }

            for (int i = 0; i < 40; i++)
            {
                col = new ColumnDefinition();
                col.Width = new GridLength(1, GridUnitType.Star);
                grid.ColumnDefinitions.Add(col);
            }

            contenaire.Children.Add(grid);

            initializeImage();
            loadGrid();
        }

        private void initializeImage()
        {


            List<Uri> imagesUri = new List<Uri>();


            Uri Uricroix = new Uri("../Lib/Images/Croix.png", UriKind.Relative);
            Uri Urimurh = new Uri("../Lib/Images/mur_horizontal.png", UriKind.Relative);
            Uri Urimurv = new Uri("../Lib/Images/mur_vertical.png", UriKind.Relative);
            Uri Urianglebd = new Uri("../Lib/Images/angle_bd.png", UriKind.Relative);
            Uri Urianglehd = new Uri("../Lib/Images/angle_bg.png", UriKind.Relative);
            Uri Urianglebg = new Uri("../Lib/Images/angle_hd.png", UriKind.Relative);
            Uri Urianglehg = new Uri("../Lib/Images/angle_hg.png", UriKind.Relative);


            imagesUri.Add(Uricroix);
            imagesUri.Add(Urimurh);
            imagesUri.Add(Urimurv);
            imagesUri.Add(Urianglebd);
            imagesUri.Add(Urianglehd);
            imagesUri.Add(Urianglebg);
            imagesUri.Add(Urianglehg);


            List<BitmapImage> bitmaps = new List<BitmapImage>();


            foreach(Uri uri in imagesUri)
            {
                System
            }

            Uri imageUri;
            Thickness margin;

            croix.Source = new BitmapImage(imageUri);
            croix.MinWidth = 50;
            croix.MinHeight = 50;
            croix.HorizontalAlignment = HorizontalAlignment.Center;
            croix.VerticalAlignment = VerticalAlignment.Center;
            margin = croix.Margin;
            margin.Left = -3;
            margin.Right = -3;
            margin.Bottom = -3;
            margin.Top = -3;
            croix.Margin = margin;





        }

        private void loadGrid()
        {
            Button[,] btns = new Button[30,40];


            for (int i = 0; i < 30; i++)
            {
                for(int y = 0; y < 40; y++) 
                {
                    Button but = new Button();
                    Grid.SetRow(but, i);
                    Grid.SetColumn(but, y);
                    but.Name = "butli" + i + "c" + y;

                    btns[i, y] = but;
                    grid.Children.Add(but);

                   // Console.WriteLine("---------------------------------- " + but.Name + " ----------------------------------");
                }
            }


            //btns[2, 3].Content = murh;
            //btns[2, 4].Content = murh;

            btns[3, 2].Content = murv;
            btns[4, 2].Content = murv;

            /*
            var x = Grid.GetColumn(but1);
            Console.WriteLine("---------------------------------- " + x.ToString() + " ----------------------------------");
             */

        }
    }
}

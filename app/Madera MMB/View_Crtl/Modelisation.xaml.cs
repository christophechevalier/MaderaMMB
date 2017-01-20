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
using Madera_MMB.Lib.Tools;

namespace Madera_MMB.View_Crtl
{
    /// <summary>
    /// Logique d'interaction pour Modelisation.xaml
    /// </summary>
    public partial class Modelisation : Page
    {

        private Grid grid = new Grid();
        private Button[] listB = new Button[30 * 40];
        private ListBox listBox = new ListBox();
        private Image croix = new Image();
        private Image murh = new Image();
        private Image murv = new Image();
        private Image anglebd = new Image();
        private Image anglehd = new Image();
        private Image anglebg = new Image();
        private Image anglehg = new Image();

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


            Grid.SetColumn(listBox, 0);
            Grid.SetRow(listBox, 1);

            Button buttt = new Button();
            buttt.Background = Brushes.Red;
            buttt.Height = 100;
            buttt.Width = 40;
            listBox.Items.Add(buttt);

            MainGrid.Children.Add(listBox);


            initializeImage();
            loadGrid();
        }

        private void initializeImage()
        {
            Uri imageUri;
            Thickness margin;

            imageUri = new Uri("../Lib/Images/Croix.png", UriKind.Relative);
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

            imageUri = new Uri("../Lib/Images/mur_horizontal.png", UriKind.Relative);
            murh.Source = new BitmapImage(imageUri);
            murh.MinWidth = 50;
            murh.MinHeight = 50;
            murh.HorizontalAlignment = HorizontalAlignment.Center;
            murh.VerticalAlignment = VerticalAlignment.Center;
            margin = murh.Margin;
            margin.Left = -3;
            margin.Right = -3;
            margin.Bottom = -3;
            margin.Top = -3;
            murh.Margin = margin;

            imageUri = new Uri("../Lib/Images/mur_vertical.png", UriKind.Relative);
            murv.Source = new BitmapImage(imageUri);
            murv.MinWidth = 50;
            murv.MinHeight = 50;
            murv.HorizontalAlignment = HorizontalAlignment.Center;
            murv.VerticalAlignment = VerticalAlignment.Center;
            margin = murv.Margin;
            margin.Left = -3;
            margin.Right = -3;
            margin.Bottom = -3;
            margin.Top = -3;
            murv.Margin = margin;

            imageUri = new Uri("../Lib/Images/angle_bd.png", UriKind.Relative);
            anglebd.Source = new BitmapImage(imageUri);
            anglebd.MinWidth = 50;
            anglebd.MinHeight = 50;
            anglebd.HorizontalAlignment = HorizontalAlignment.Center;
            anglebd.VerticalAlignment = VerticalAlignment.Center;
            margin = anglebd.Margin;
            margin.Left = -3;
            margin.Right = -3;
            margin.Bottom = -3;
            margin.Top = -3;
            anglebd.Margin = margin;

            imageUri = new Uri("../Lib/Images/angle_bg.png", UriKind.Relative);
            anglebg.Source = new BitmapImage(imageUri);
            anglebg.MinWidth = 50;
            anglebg.MinHeight = 50;
            anglebg.HorizontalAlignment = HorizontalAlignment.Center;
            anglebg.VerticalAlignment = VerticalAlignment.Center;
            margin = anglebg.Margin;
            margin.Left = -3;
            margin.Right = -3;
            margin.Bottom = -3;
            margin.Top = -3;
            anglebg.Margin = margin;

            imageUri = new Uri("../Lib/Images/angle_hd.png", UriKind.Relative);
            anglehd.Source = new BitmapImage(imageUri);
            anglehd.MinWidth = 50;
            anglehd.MinHeight = 50;
            anglehd.HorizontalAlignment = HorizontalAlignment.Center;
            anglehd.VerticalAlignment = VerticalAlignment.Center;
            margin = anglehd.Margin;
            margin.Left = -3;
            margin.Right = -3;
            margin.Bottom = -3;
            margin.Top = -3;
            anglehd.Margin = margin;

            imageUri = new Uri("../Lib/Images/angle_hg.png", UriKind.Relative);
            anglehg.Source = new BitmapImage(imageUri);
            anglehg.MinWidth = 50;
            anglehg.MinHeight = 50;
            anglehg.HorizontalAlignment = HorizontalAlignment.Center;
            anglehg.VerticalAlignment = VerticalAlignment.Center;
            margin = anglehg.Margin;
            margin.Left = -3;
            margin.Right = -3;
            margin.Bottom = -3;
            margin.Top = -3;
            anglehg.Margin = margin;
        }

        private void loadGrid()
        {
            Button[,] btns = new Button[30, 40];

            for (int i = 0; i < 30; i++)
            {
                for (int y = 0; y < 40; y++)
                {
                    Button but = new Button();
                    Grid.SetRow(but, i);
                    Grid.SetColumn(but, y);
                    but.Name = "butlig" + i + "col" + y;
                    but.Background = Brushes.LightGray;
                    but.Click += new RoutedEventHandler(checkAround);

                    btns[i, y] = but;
                    grid.Children.Add(but);
                }
            }

            Button murhaut = new Button();
            Grid.SetColumn(murhaut, 5);
            Grid.SetColumnSpan(murhaut, 20);
            Grid.SetRow(murhaut, 5);
            murhaut.Background = Brushes.Red;
            murhaut.Content = murh;
            grid.Children.Add(murhaut);

            Button murbas = new Button();
            Grid.SetColumn(murbas, 5);
            Grid.SetColumnSpan(murbas, 20);
            Grid.SetRow(murbas, 24);
            murbas.Background = Brushes.Red;
            murbas.Content = murh;
            grid.Children.Add(murbas);

            Button murgauche = new Button();
            Grid.SetColumn(murgauche, 4);
            Grid.SetRowSpan(murgauche, 20);
            Grid.SetRow(murgauche, 5);
            murgauche.Background = Brushes.Red;
            murgauche.Content = murv;
            grid.Children.Add(murgauche);

            Button murdroit = new Button();
            Grid.SetColumn(murdroit, 25);
            Grid.SetRowSpan(murdroit, 20);
            Grid.SetRow(murdroit, 5);
            murdroit.Background = Brushes.Red;
            murdroit.Content = murv;
            grid.Children.Add(murdroit);

            Button slot = new Button();
            Grid.SetColumn(slot, 14);
            Grid.SetRow(slot, 5);
            slot.Background = Brushes.Green;
            grid.Children.Add(slot);

            /*
            btns[2, 2].Content = img;
            btns[4, 4].Content = anglehg;

            btns[2, 3].Content = murh;
            btns[2, 4].Content = murh;

            btns[3, 2].Content = murv;
            btns[4, 2].Content = murv;
            */
        }

        private void checkAround(object sender, RoutedEventArgs e)
        {
            Button but = sender as Button;
            var x = Grid.GetRow(but);
            var y = Grid.GetColumn(but);

            Console.WriteLine("---------------------------------- lig " + x + " col " + y + " ----------------------------------");
            if (but.Background == Brushes.Yellow)
            {
                but.Background = Brushes.Green;
            }
            else if (but.Background == Brushes.Green)
            {
                but.Background = Brushes.LightGray;
            }
            else
            {
                but.Background = Brushes.Yellow;
            }
        }
    }
}

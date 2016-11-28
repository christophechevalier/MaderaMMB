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

            loadGrid();

            /*Button but1 = new Button();
            but1.Name = "But1";
            but1.Background = Brushes.Red;
            grid.Children.Add(but1);

            Button but2 = new Button();
            but2.Name = "But2";
            but2.Background = Brushes.Green;
            //grid.Children.Add(but2);

            var colection = grid.Children.Count;
            var type = 1;
            Console.WriteLine("---------------------------------- " + colection + " ----------------------------------");
            Console.WriteLine("---------------------------------- " + type + " ----------------------------------");*/
        }

        private void loadGrid()
        {
            Image img = new Image();
            Uri imageUri = new Uri("../Lib/Images/Croix.png", UriKind.Relative);
            img.Source = new BitmapImage(imageUri);

            img.MinWidth = 50;
            img.MinHeight = 50;
            img.HorizontalAlignment = HorizontalAlignment.Center;
            img.VerticalAlignment = VerticalAlignment.Center;

            Thickness margin = img.Margin;
            margin.Left = -2;
            margin.Right = -2;
            margin.Bottom = -2;
            margin.Top = -2;
            img.Margin = margin;

            Button but1 = new Button();
            but1.Name = "But1";
            but1.Background = Brushes.Red;

            Button but2 = new Button();
            but2.Name = "But2";
            but2.Background = Brushes.Green;

            but1.Content = img;

            Label lab = new Label();
            lab.Content = "Hello";

            Grid.SetRow(but1, 4);
            Grid.SetColumn(but1, 5);
            grid.Children.Add(but1);
            grid.Children.Add(but2);

            var x = Grid.GetColumn(but1);
            Console.WriteLine("---------------------------------- " + x.ToString() + " ----------------------------------");

        }
    }
}

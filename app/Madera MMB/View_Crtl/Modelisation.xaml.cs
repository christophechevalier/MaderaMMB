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

        //private Grid grid = new Grid();
        //private System.Drawing.Image croix = new Image();
        //private System.Drawing.Image murh = new Image();
        //private System.Drawing.Image murv = new Image();
        //private System.Drawing.Image anglebd = new Image();
        //private System.Drawing.Image anglehd = new Image();
        //private System.Drawing.Image anglebg = new Image();
        //private System.Drawing.Image anglehg = new Image();

        public Modelisation()
        {
            InitializeComponent();
            //initialize();

        }

    //    private void initialize()
    //    {
    //        grid.ShowGridLines = true;
    //        grid.Margin = new Thickness(7);

    //        ColumnDefinition col;
    //        RowDefinition row;

    //        for (int i = 0; i < 30; i++)
    //        {
    //            row = new RowDefinition();
    //            row.Height = new GridLength(1, GridUnitType.Star);
    //            grid.RowDefinitions.Add(row);
    //        }

    //        for (int i = 0; i < 40; i++)
    //        {
    //            col = new ColumnDefinition();
    //            col.Width = new GridLength(1, GridUnitType.Star);
    //            grid.ColumnDefinitions.Add(col);
    //        }

    //        contenaire.Children.Add(grid);

    //        initializeImage();
    //        loadGrid();
    //    }

    //    private void initializeImage()
    //    {
    //        Uri imageUri;
    //        Thickness margin;

    //        imageUri = new Uri("../Lib/Images/Croix.png", UriKind.Relative);
    //        croix.Source = new BitmapImage(imageUri);
    //        croix.MinWidth = 50;
    //        croix.MinHeight = 50;
    //        croix.HorizontalAlignment = HorizontalAlignment.Center;
    //        croix.VerticalAlignment = VerticalAlignment.Center;
    //        margin = croix.Margin;
    //        margin.Left = -3;
    //        margin.Right = -3;
    //        margin.Bottom = -3;
    //        margin.Top = -3;
    //        croix.Margin = margin;

    //        imageUri = new Uri("../Lib/Images/mur_horizontal.png", UriKind.Relative);
    //        murh.Source = new BitmapImage(imageUri);
    //        murh.MinWidth = 50;
    //        murh.MinHeight = 50;
    //        murh.HorizontalAlignment = HorizontalAlignment.Center;
    //        murh.VerticalAlignment = VerticalAlignment.Center;
    //        margin = murh.Margin;
    //        margin.Left = -3;
    //        margin.Right = -3;
    //        margin.Bottom = -3;
    //        margin.Top = -3;
    //        murh.Margin = margin;

    //        imageUri = new Uri("../Lib/Images/mur_vertical.png", UriKind.Relative);
    //        murv.Source = new BitmapImage(imageUri);
    //        murv.MinWidth = 50;
    //        murv.MinHeight = 50;
    //        murv.HorizontalAlignment = HorizontalAlignment.Center;
    //        murv.VerticalAlignment = VerticalAlignment.Center;
    //        margin = murv.Margin;
    //        margin.Left = -3;
    //        margin.Right = -3;
    //        margin.Bottom = -3;
    //        margin.Top = -3;
    //        murv.Margin = margin;

    //        imageUri = new Uri("../Lib/Images/angle_bd.png", UriKind.Relative);
    //        anglebd.Source = new BitmapImage(imageUri);
    //        anglebd.MinWidth = 50;
    //        anglebd.MinHeight = 50;
    //        anglebd.HorizontalAlignment = HorizontalAlignment.Center;
    //        anglebd.VerticalAlignment = VerticalAlignment.Center;
    //        margin = anglebd.Margin;
    //        margin.Left = -3;
    //        margin.Right = -3;
    //        margin.Bottom = -3;
    //        margin.Top = -3;
    //        anglebd.Margin = margin;

    //        imageUri = new Uri("../Lib/Images/angle_bg.png", UriKind.Relative);
    //        anglebg.Source = new BitmapImage(imageUri);
    //        anglebg.MinWidth = 50;
    //        anglebg.MinHeight = 50;
    //        anglebg.HorizontalAlignment = HorizontalAlignment.Center;
    //        anglebg.VerticalAlignment = VerticalAlignment.Center;
    //        margin = anglebg.Margin;
    //        margin.Left = -3;
    //        margin.Right = -3;
    //        margin.Bottom = -3;
    //        margin.Top = -3;
    //        anglebg.Margin = margin;

    //        imageUri = new Uri("../Lib/Images/angle_hd.png", UriKind.Relative);
    //        anglehd.Source = new BitmapImage(imageUri);
    //        anglehd.MinWidth = 50;
    //        anglehd.MinHeight = 50;
    //        anglehd.HorizontalAlignment = HorizontalAlignment.Center;
    //        anglehd.VerticalAlignment = VerticalAlignment.Center;
    //        margin = anglehd.Margin;
    //        margin.Left = -3;
    //        margin.Right = -3;
    //        margin.Bottom = -3;
    //        margin.Top = -3;
    //        anglehd.Margin = margin;

    //        imageUri = new Uri("../Lib/Images/angle_hg.png", UriKind.Relative);
    //        anglehg.Source = new BitmapImage(imageUri);
    //        anglehg.MinWidth = 50;
    //        anglehg.MinHeight = 50;
    //        anglehg.HorizontalAlignment = HorizontalAlignment.Center;
    //        anglehg.VerticalAlignment = VerticalAlignment.Center;
    //        margin = anglehg.Margin;
    //        margin.Left = -3;
    //        margin.Right = -3;
    //        margin.Bottom = -3;
    //        margin.Top = -3;
    //        anglehg.Margin = margin;
    //    }

    //    private void loadGrid()
    //    {
    //        Button[,] btns = new Button[30, 40];

    //        for (int i = 0; i < 30; i++)
    //        {
    //            for (int y = 0; y < 40; y++)
    //            {
    //                Button but = new Button();
    //                Grid.SetRow(but, i);
    //                Grid.SetColumn(but, y);
    //                but.Name = "butli" + i + "c" + y;

    //                btns[i, y] = but;
    //                grid.Children.Add(but);

    //                //Console.WriteLine("---------------------------------- " + but.Name + " ----------------------------------");
    //            }
    //        }

    //        btns[2, 2].Content = anglehg;
    //        btns[4, 4].Content = anglehg;

    //        btns[2, 3].Content = murh;
    //        btns[2, 4].Content = murh;

    //        btns[3, 2].Content = murv;
    //        btns[4, 2].Content = murv;

    //        /*
    //        var x = Grid.GetColumn(but1);
    //        Console.WriteLine("---------------------------------- " + x.ToString() + " ----------------------------------");
    //         */

    //    }
    }
}

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
        private ButtonM[,] listB = new ButtonM[30, 40];
        private ListBox listBox = new ListBox();
        private System.Windows.Controls.Image croix = new System.Windows.Controls.Image();
        private System.Windows.Controls.Image murh = new System.Windows.Controls.Image();
        private System.Windows.Controls.Image murv = new System.Windows.Controls.Image();
        private System.Windows.Controls.Image anglebd = new System.Windows.Controls.Image();
        private System.Windows.Controls.Image anglehd = new System.Windows.Controls.Image();
        private System.Windows.Controls.Image anglebg = new System.Windows.Controls.Image();
        private System.Windows.Controls.Image anglehg = new System.Windows.Controls.Image();

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
            buttt.Background = System.Windows.Media.Brushes.Red;
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
            for (int i = 0; i < 30; i++)
            {
                for (int y = 0; y < 40; y++)
                {
                    ButtonM but = new ButtonM(ButtonM.type.Rien, y, i, 1, 1);
                    Grid.SetRow(but, but.y);
                    Grid.SetColumn(but, but.x);
                    but.Click += new RoutedEventHandler(checkWall);

                    listB[i, y] = but;
                    grid.Children.Add(but);
                }
            }

            ButtonM murhaut = new ButtonM(ButtonM.type.Mur, 5, 5, 20, 1);
            placeComponent(murhaut);

            ButtonM murbas = new ButtonM(ButtonM.type.Mur, 5, 24, 20, 1);
            placeComponent(murbas);

            ButtonM murgauche = new ButtonM(ButtonM.type.Mur, 4, 5, 1, 20);
            placeComponent(murgauche);

            ButtonM murdroit = new ButtonM(ButtonM.type.Mur, 24, 5, 1, 20);
            placeComponent(murdroit);

            ButtonM slot = new ButtonM(ButtonM.type.Slot, 14, 5, 1, 1);
            placeComponent(slot);
        }

        private void placeComponent(ButtonM but)
        {
            Grid.SetColumn(but, but.x);
            Grid.SetColumnSpan(but, but.colspan);
            Grid.SetRow(but, but.y);
            Grid.SetRowSpan(but, but.rowspan);
            //but.Content = murh;
            listB[but.x, but.y] = but;
            grid.Children.Add(but);
        }

        private void checkWall(object sender, RoutedEventArgs e)
        {
            ButtonM but = sender as ButtonM;
            bool around = false;

            for (int x = 0; x < listB.GetLength(0); x++)
            {
                for (int y = 0; y < listB.GetLength(1); y++)
                {
                    if (listB[x, y].letype == ButtonM.type.Mur || listB[x, y].letype == ButtonM.type.MurInt)
                    {
                        ButtonM but2 = listB[x,y];
                        if (but2.rowspan != 1)
                        {
                            for (int span = 0; span < but2.rowspan; span++)
                            {
                                if ((but2.y + span == but.y && but2.x + 1 == but.x) || (but2.y + span == but.y && but2.x - 1 == but.x))
                                {
                                    around = true;
                                }
                            }
                        }
                        else if (but2.colspan !=1 )
                        {
                            for (int span = 0; span < but2.colspan; span++)
                            {
                                if ((but2.x + span == but.x && but2.y + 1 == but.y) || (but2.x + span == but.x && but2.y - 1 == but.y))
                                {
                                    around = true;
                                }
                            } 
                        }
                        else
                        {
                            if ((but2.x == but.x && but2.y + 1 == but.y) || (but2.x == but.x && but2.y - 1 == but.y) || (but2.x + 1 == but.x && but2.y == but.y) || (but2.x - 1 == but.x && but2.y == but.y))
                            {
                                around = true;
                            }
                        }
                        
                    }
                }
            }

            if (around)
            {
                but.letype = ButtonM.type.MurInt;
                but.checkType();
            }
        }
    }
}

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Madera_MMB.Lib;
using Madera_MMB.Lib.Tools;
using Madera_MMB.Model;
using Madera_MMB.CAD;
using System.Windows.Media;

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
        private string mode = "default";

        /*********************************************************** BOUCHON *********************************************************************/
        private Plan plan = new Plan("Plan1", new Projet(), new Plancher("Bois", 50), new Couverture("Tuiles", 200), new CoupePrincipe(1, "Carré", 50, 50, 1000), new Gamme("Aucune", 0, "Aucun", "Aucunes", "Fer"));
        private PlanCAD planCad;
        /*********************************************************** BOUCHON *********************************************************************/
        
        private Brush croix = new ImageBrush(new BitmapImage(new Uri("../../Lib/Images/croix.png", UriKind.RelativeOrAbsolute)));
        private Brush murh = new ImageBrush(new BitmapImage(new Uri("../../Lib/Images/mur_horizontal.png", UriKind.RelativeOrAbsolute)));
        private Brush murv = new ImageBrush(new BitmapImage(new Uri("../../Lib/Images/mur_vertical.png", UriKind.RelativeOrAbsolute)));
        private Brush anglebd = new ImageBrush(new BitmapImage(new Uri("../../Lib/Images/angle_bd.png", UriKind.RelativeOrAbsolute)));
        private Brush anglehd = new ImageBrush(new BitmapImage(new Uri("../../Lib/Images/angle_hd.png", UriKind.RelativeOrAbsolute)));
        private Brush anglebg = new ImageBrush(new BitmapImage(new Uri("../../Lib/Images/angle_bg.png", UriKind.RelativeOrAbsolute)));
        private Brush anglehg = new ImageBrush(new BitmapImage(new Uri("../../Lib/Images/angle_hg.png", UriKind.RelativeOrAbsolute)));
        private Brush tBas = new ImageBrush(new BitmapImage(new Uri("../../Lib/Images/tBas.png", UriKind.RelativeOrAbsolute)));
        private Brush tHaut = new ImageBrush(new BitmapImage(new Uri("../../Lib/Images/tHaut.png", UriKind.RelativeOrAbsolute)));
        private Brush tGauche = new ImageBrush(new BitmapImage(new Uri("../../Lib/Images/tGauche.png", UriKind.RelativeOrAbsolute)));
        private Brush tDroite = new ImageBrush(new BitmapImage(new Uri("../../Lib/Images/tDroite.png", UriKind.RelativeOrAbsolute)));

        private ButtonM slot = new ButtonM(ButtonM.type.SlotMur, 14, 5, 1, 1);
        private ButtonM slot2 = new ButtonM(ButtonM.type.SlotMur, 14, 24, 1, 1);
        private ButtonM murdroit = new ButtonM(ButtonM.type.Mur, 24, 5, 1, 20);
        private ButtonM murgauche = new ButtonM(ButtonM.type.Mur, 4, 5, 1, 20);
        private ButtonM murbas = new ButtonM(ButtonM.type.Mur, 5, 24, 20, 1);
        private ButtonM murhaut = new ButtonM(ButtonM.type.Mur, 5, 5, 20, 1);

        public Modelisation()
        {
            InitializeComponent();

            /*********************************************************** BOUCHON *********************************************************************/
            //planCad = new PlanCAD(new Connexion(), plan.projet, plan.couverture, plan.coupePrincipe, plan.plancher, plan.gamme, new MetamoduleCAD());
            /*********************************************************** BOUCHON *********************************************************************/

            initialize();
        }

        private void initialize()
        {
            grid.ShowGridLines = true;
            grid.Margin = new Thickness(7);

            slot.Click += new RoutedEventHandler(checkType);
            tracer.Click += new RoutedEventHandler(changeMode);
            retirer.Click += new RoutedEventHandler(changeMode);
            Grid.SetColumn(listBox, 0);
            Grid.SetRow(listBox, 1);

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

            //initializeImage();
            loadGrid();
        }

        //private void initializeImage()
        //{
        //    Uri imageUri;
        //    Thickness margin;
            
        //    BitmapImage bit = new BitmapImage(new Uri(@"/Madera MMB;component/Lib/Images/croix.png", UriKind.RelativeOrAbsolute));
        //    croix.Source = bit;
        //    croix.MinWidth = 50;
        //    croix.MinHeight = 50;
        //    croix.HorizontalAlignment = HorizontalAlignment.Center;
        //    croix.VerticalAlignment = VerticalAlignment.Center;
        //    margin = croix.Margin;
        //    margin.Left = -3;
        //    margin.Right = -3;
        //    margin.Bottom = -3;
        //    margin.Top = -3;
        //    croix.Margin = margin;

        //    imageUri = new Uri(@"/Madera MMB;component/Lib/Images/mur_horizontal.png", UriKind.Relative);
        //    murh.Source = new BitmapImage(imageUri);
        //    murh.MinWidth = 50;
        //    murh.MinHeight = 50;
        //    murh.HorizontalAlignment = HorizontalAlignment.Center;
        //    murh.VerticalAlignment = VerticalAlignment.Center;
        //    margin = murh.Margin;
        //    margin.Left = -3;
        //    margin.Right = -3;
        //    margin.Bottom = -3;
        //    margin.Top = -3;
        //    murh.Margin = margin;

        //    imageUri = new Uri(@"/Madera MMB;component/Lib/Images/mur_vertical.png", UriKind.Relative);
        //    murv.Source = new BitmapImage(imageUri);
        //    murv.MinWidth = 50;
        //    murv.MinHeight = 50;
        //    murv.HorizontalAlignment = HorizontalAlignment.Center;
        //    murv.VerticalAlignment = VerticalAlignment.Center;
        //    margin = murv.Margin;
        //    margin.Left = -3;
        //    margin.Right = -3;
        //    margin.Bottom = -3;
        //    margin.Top = -3;
        //    murv.Margin = margin;

        //    imageUri = new Uri(@"/Madera MMB;component/Lib/Images/angle_bd.png", UriKind.Relative);
        //    anglebd.Source = new BitmapImage(imageUri);
        //    anglebd.MinWidth = 50;
        //    anglebd.MinHeight = 50;
        //    anglebd.HorizontalAlignment = HorizontalAlignment.Center;
        //    anglebd.VerticalAlignment = VerticalAlignment.Center;
        //    margin = anglebd.Margin;
        //    margin.Left = -3;
        //    margin.Right = -3;
        //    margin.Bottom = -3;
        //    margin.Top = -3;
        //    anglebd.Margin = margin;

        //    imageUri = new Uri(@"/Madera MMB;component/Lib/Images/angle_bg.png", UriKind.Relative);
        //    anglebg.Source = new BitmapImage(imageUri);
        //    anglebg.MinWidth = 50;
        //    anglebg.MinHeight = 50;
        //    anglebg.HorizontalAlignment = HorizontalAlignment.Center;
        //    anglebg.VerticalAlignment = VerticalAlignment.Center;
        //    margin = anglebg.Margin;
        //    margin.Left = -3;
        //    margin.Right = -3;
        //    margin.Bottom = -3;
        //    margin.Top = -3;
        //    anglebg.Margin = margin;

        //    imageUri = new Uri(@"/Madera MMB;component/Lib/Images/angle_hd.png", UriKind.Relative);
        //    anglehd.Source = new BitmapImage(imageUri);
        //    anglehd.MinWidth = 50;
        //    anglehd.MinHeight = 50;
        //    anglehd.HorizontalAlignment = HorizontalAlignment.Center;
        //    anglehd.VerticalAlignment = VerticalAlignment.Center;
        //    margin = anglehd.Margin;
        //    margin.Left = -3;
        //    margin.Right = -3;
        //    margin.Bottom = -3;
        //    margin.Top = -3;
        //    anglehd.Margin = margin;

        //    imageUri = new Uri(@"/Madera MMB;component/Lib/Images/angle_hg.png", UriKind.Relative);
        //    anglehg.Source = new BitmapImage(imageUri);
        //    anglehg.MinWidth = 50;
        //    anglehg.MinHeight = 50;
        //    anglehg.HorizontalAlignment = HorizontalAlignment.Center;
        //    anglehg.VerticalAlignment = VerticalAlignment.Center;
        //    margin = anglehg.Margin;
        //    margin.Left = -3;
        //    margin.Right = -3;
        //    margin.Bottom = -3;
        //    margin.Top = -3;
        //    anglehg.Margin = margin;
        //}

        private void loadGrid()
        {
            for (int i = 0; i < 30; i++)
            {
                for (int y = 0; y < 40; y++)
                {
                    ButtonM but = new ButtonM(ButtonM.type.Rien, y, i, 1, 1);
                    Grid.SetRow(but, but.y);
                    Grid.SetColumn(but, but.x);
                    but.Click += new RoutedEventHandler(checkType);

                    listB[i, y] = but;
                    grid.Children.Add(but);
                }
            }
            
            slot.Background = croix;
            slot2.Background = croix;

            placeComponent(murhaut);
            placeComponent(murbas);
            placeComponent(murgauche);
            placeComponent(murdroit);
            placeComponent(slot);
            placeComponent(slot2);
        }

        private void placeComponent(ButtonM but)
        {
            Grid.SetColumn(but, but.x);
            Grid.SetColumnSpan(but, but.colspan);
            Grid.SetRow(but, but.y);
            Grid.SetRowSpan(but, but.rowspan);
            listB[but.x, but.y] = but;
            grid.Children.Add(but);
        }

        private void checkType(object sender, RoutedEventArgs e)
        {
            ButtonM but = sender as ButtonM;
            if (but.letype == ButtonM.type.Rien)
            {
                MainGrid.Children.Remove(listBox);
                if (mode == "tracer")
                {
                    placeWall(but);
                }
            } else if (but.letype == ButtonM.type.SlotMur)
            {
                mode = "default";
                MainGrid.Children.Add(listBox);
                ListBoxItem item1 = new ListBoxItem();
                item1.Content = "Yop !";
                listBox.Items.Add(item1);
            } else if (but.letype == ButtonM.type.MurInt)
            {
                if (mode == "retirer")
                {
                    removeWall(but);
                }
            }
        }

        private void changeMode(object sender, RoutedEventArgs e)
        {
            Button but = sender as Button;
            mode = but.Name;
            Console.WriteLine(mode);
        }

        private void placeWall(ButtonM but)
        {
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

            if (around && isInside(but))
            {
                but.letype = ButtonM.type.MurInt;
                //but.checkType();
                for (int i = 1; i < listB.GetLength(0)-1; i++)
                {
                    for (int y = 1; y < listB.GetLength(1)-1; y++)
                    {
                        if (listB[i, y].letype == ButtonM.type.MurInt)
                        {
                            checkImage(listB[i, y]);
                        }
                    }
                }
            }
        }

        private void checkImage(ButtonM but)
        {
            ButtonM butN = listB[but.y - 1, but.x];
            ButtonM butS = listB[but.y + 1, but.x];
            ButtonM butO = listB[but.y, but.x - 1];
            ButtonM butE = listB[but.y ,but.x + 1];

            if (butN.letype == ButtonM.type.MurInt && butS.letype == ButtonM.type.MurInt && butO.letype == ButtonM.type.MurInt && butE.letype == ButtonM.type.MurInt)
            {
                but.Background = croix;
            }
            else if (butN.letype == ButtonM.type.MurInt && butS.letype == ButtonM.type.MurInt && butO.letype == ButtonM.type.MurInt)
            {
                but.Background = tGauche;
            }
            else if (butN.letype == ButtonM.type.MurInt && butS.letype == ButtonM.type.MurInt && butE.letype == ButtonM.type.MurInt)
            {
                but.Background = tDroite;
            }
            else if (butO.letype == ButtonM.type.MurInt && butE.letype == ButtonM.type.MurInt && butS.letype == ButtonM.type.MurInt)
            {
                but.Background = tBas;
            }
            else if (butO.letype == ButtonM.type.MurInt && butE.letype == ButtonM.type.MurInt && butN.letype == ButtonM.type.MurInt)
            {
                but.Background = tHaut;
            }
            else if (butN.letype == ButtonM.type.MurInt && butS.letype == ButtonM.type.MurInt)
            {
                but.Background = murv;
            }
            else if (butO.letype == ButtonM.type.MurInt && butE.letype == ButtonM.type.MurInt)
            {
                but.Background = murh;
            }
            else if (butS.letype == ButtonM.type.MurInt && butO.letype == ButtonM.type.MurInt)
            {
                but.Background = anglehd;
            }
            else if (butS.letype == ButtonM.type.MurInt && butE.letype == ButtonM.type.MurInt)
            {
                but.Background = anglehg;
            }
            else if (butN.letype == ButtonM.type.MurInt && butO.letype == ButtonM.type.MurInt)
            {
                but.Background = anglebd;
            }
            else if (butN.letype == ButtonM.type.MurInt && butE.letype == ButtonM.type.MurInt)
            {
                but.Background = anglebg;
            }
            else if (butO.letype == ButtonM.type.MurInt || butE.letype == ButtonM.type.MurInt)
            {
                but.Background = murh;
            }
            else if (butN.letype == ButtonM.type.MurInt || butS.letype == ButtonM.type.MurInt)
            {
                but.Background = murv;
            }
            else
            {
                but.Background = Brushes.Aqua;
            }
        }

        private void removeWall(ButtonM but)
        {
            Console.WriteLine(but.letype);
            but.letype = ButtonM.type.Rien;
            Console.WriteLine(but.letype);
            but.checkType();
            Console.WriteLine(but.letype);
        }

        private bool isInside (ButtonM but)
        {
            bool inside = false;
            if (but.y > murgauche.y && but.y < (murgauche.y + murgauche.rowspan -1))
            {
                if (but.x > murgauche.x && but.x < murdroit.x)
                {
                    inside = true;
                }
            }

            return inside;
        }
    }
}

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Madera_MMB.Lib;
using Madera_MMB.Lib.Tools;
using Madera_MMB.Model;
using Madera_MMB.CAD;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Controls.Primitives;

namespace Madera_MMB.View_Crtl
{
    /// <summary>
    /// Controle de la vue Modelisation
    /// Features en place :
    ///     Placement de mur
    ///     Retrait de mur
    ///     Selection d'un mode
    ///     Afficher/Masquer liste d'options
    ///     Image des murInt
    /// Restantes :
    ///     Selection type du mur à placer
    ///     Selection module à placer dans un slot
    ///     Generation auto plan
    ///     Sauvegarde plan
    ///     Modelisation initiale
    /// </summary>
    public partial class Modelisation : Page
    {
        #region Attributs
        private Grid grid = new Grid();
        private ButtonM[,] listB = new ButtonM[40, 30];
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

        private Brush mur_beton = new ImageBrush(new BitmapImage(new Uri("../../Lib/Images/mur_beton.jpg", UriKind.RelativeOrAbsolute)));

        private ButtonM slot = new ButtonM(ButtonM.type.SlotMur, 15, 5, 1, 1, null);
        private ButtonM slot2 = new ButtonM(ButtonM.type.SlotMur, 15, 25, 1, 1, null);
        private ButtonM murdroit = new ButtonM(ButtonM.type.Mur, 25, 5, 1, 20, null);
        private ButtonM murgauche = new ButtonM(ButtonM.type.Mur, 5, 6, 1, 20, null);
        private ButtonM murbas = new ButtonM(ButtonM.type.Mur, 6, 25, 20, 1, null);
        private ButtonM murhaut = new ButtonM(ButtonM.type.Mur, 5, 5, 20, 1, null);
        #endregion

        public Modelisation()
        {
            InitializeComponent();
            initialize();
        }

        private void initialize()
        {
            grid.ShowGridLines = true;
            grid.Margin = new Thickness(7);

            /*********************************************************** BOUCHON *********************************************************************/
            //planCad = new PlanCAD(new Connexion(), plan.projet, plan.couverture, plan.coupePrincipe, plan.plancher, plan.gamme, new MetamoduleCAD());
            /*********************************************************** BOUCHON *********************************************************************/

            slot.Click += new RoutedEventHandler(checkType);
            slot2.Click += new RoutedEventHandler(checkType);
            tracer.Click += new RoutedEventHandler(changeMode);
            retirer.Click += new RoutedEventHandler(changeMode);
            Grid.SetColumn(listBox, 0);
            Grid.SetRow(listBox, 2);
            listBox.Margin = new Thickness(7,7,7,7);

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
            loadGridButton();
        }

        private void loadGridButton()
        {
            for (int y = 0; y < 30; y++)
            {
                for (int x = 0; x < 40; x++)
                {
                    ButtonM but = new ButtonM(ButtonM.type.Rien, x, y, 1, 1, null);
                    Grid.SetRow(but, but.y);
                    Grid.SetColumn(but, but.x);
                    but.Click += new RoutedEventHandler(checkType);

                    listB[x, y] = but;
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

            checkImage();
        }

        private void placeComponent(ButtonM but)
        {
            Grid.SetColumn(but, but.x);
            Grid.SetColumnSpan(but, but.colspan);
            Grid.SetRow(but, but.y);
            Grid.SetRowSpan(but, but.rowspan);
            listB[but.x, but.y] = but;

            //Rajouter modification des boutons compris dans les span
            if (but.rowspan > 1 )
            {
                for (int i = 1; i < but.rowspan; i++)
                {
                    listB[but.x, but.y + i].letype = but.letype;
                }
            }
            else if (but.colspan > 1)
            {
                for (int i = 1; i < but.colspan; i++)
                {
                    listB[but.x + i, but.y].letype = but.letype;
                }
            }

            grid.Children.Add(but);
        }

        private void checkType(object sender, RoutedEventArgs e)
        {
            ButtonM but = sender as ButtonM;
            checkMode();
            if (but.letype == ButtonM.type.Rien)
            {
                if (mode == "tracer" && tracer.Background != Brushes.LightGreen)
                {
                    placeWall(but);
                }
                else
                {
                    tracer.IsChecked = false;
                    mode = "default";
                }
            }
            else if (but.letype == ButtonM.type.SlotMur)
            {
                mode = "default";
                MainGrid.Children.Remove(listBox);
                listBox.Items.Clear();
                MainGrid.Children.Add(listBox);
                ListBoxItem item1 = new ListBoxItem();
                item1.Content = "Yop !";
                listBox.Items.Add(item1);
            }
            else if (but.letype == ButtonM.type.MurInt)
            {
                if (mode == "retirer")
                {
                    removeWall(but);
                }
            }
        }

        private void changeMode(object sender, RoutedEventArgs e)
        {
            ToggleButton but = sender as ToggleButton;
            if (mode == but.Name)
            {
                mode = "default";
            }
            else
            {
                mode = but.Name;
            }
            checkMode();
            Console.WriteLine(mode);
        }

        private void checkMode()
        {
            if (mode == "tracer")
            {
                retirer.IsChecked = false;
                loadListMur();
            }
            else if (mode == "retirer" || mode == "default") {
                tracer.IsChecked = false;
                MainGrid.Children.Remove(listBox);
            }
        }

        private void loadListMur()
        {
            MainGrid.Children.Remove(listBox);
            listBox.Items.Clear();
            //Recupérer la liste des possiblités sur la BDD
            ListBoxItem item1 = new ListBoxItem();
            item1.Content = "Yop1 !";
            ListBoxItem item2 = new ListBoxItem();
            item2.Content = "Yop2 !";
            ListBoxItem item3 = new ListBoxItem();
            item3.Content = "Yop3 !";

            listBox.Items.Add(item1);
            listBox.Items.Add(item2);
            listBox.Items.Add(item3);

            MainGrid.Children.Add(listBox);
        }

        private void placeWall(ButtonM but)
        {
            if (checkAround(but) && isInside(but))
            {
                but.letype = ButtonM.type.MurInt;
                but.texture = mur_beton;
                checkImage();
            }
        }

        private void removeWall(ButtonM but)
        {
            if (isInside(but))
            {
                but.letype = ButtonM.type.Rien;
                checkImage();
            }
        }

        private bool checkAround(ButtonM but)
        {
            bool around = false;

            ButtonM butN = listB[but.x, but.y - 1];
            ButtonM butS = listB[but.x, but.y + 1];
            ButtonM butO = listB[but.x - 1, but.y];
            ButtonM butE = listB[but.x + 1, but.y];

            if ((butN.letype == ButtonM.type.Mur || butN.letype == ButtonM.type.MurInt) || (butS.letype == ButtonM.type.Mur || butS.letype == ButtonM.type.MurInt) || (butO.letype == ButtonM.type.Mur || butO.letype == ButtonM.type.MurInt) || (butE.letype == ButtonM.type.Mur || butE.letype == ButtonM.type.MurInt))
            {
                around = true;
            }

            return around;
        }

        private void checkImage()
        {
            for (int i = 1; i < listB.GetLength(0) - 1; i++)
            {
                for (int y = 1; y < listB.GetLength(1) - 1; y++)
                {
                    ButtonM but = listB[i, y];
                    if (but.letype == ButtonM.type.MurInt || but.letype == ButtonM.type.Mur)
                    {
                        ButtonM butN = listB[but.x, but.y - 1];
                        ButtonM butS = listB[but.x, but.y + 1];
                        ButtonM butO = listB[but.x - 1, but.y];
                        ButtonM butE = listB[but.x + 1, but.y];

                        if (but.x == 4 && but.y == 24)
                        {
                            Console.WriteLine("butN " + butN.letype);
                            Console.WriteLine("butS " + butS.letype);
                            Console.WriteLine("butO " + butO.letype);
                            Console.WriteLine("butE " + butE.letype);
                        }

                        if ((butN.letype == ButtonM.type.MurInt || butN.letype == ButtonM.type.Mur) && (butS.letype == ButtonM.type.MurInt || butS.letype == ButtonM.type.Mur) && (butO.letype == ButtonM.type.MurInt || butO.letype == ButtonM.type.Mur) && (butE.letype == ButtonM.type.MurInt || butE.letype == ButtonM.type.Mur))
                        {
                            but.Background = croix;
                            but.back = "croix";
                        }
                        else if ((butN.letype == ButtonM.type.MurInt || butN.letype == ButtonM.type.Mur) && (butS.letype == ButtonM.type.MurInt || butS.letype == ButtonM.type.Mur) && (butO.letype == ButtonM.type.MurInt || butO.letype == ButtonM.type.Mur))
                        {
                            but.Background = tGauche;
                            but.back = "tGauche";
                        }
                        else if ((butN.letype == ButtonM.type.MurInt || butN.letype == ButtonM.type.Mur) && (butS.letype == ButtonM.type.MurInt || butS.letype == ButtonM.type.Mur) && (butE.letype == ButtonM.type.MurInt || butE.letype == ButtonM.type.Mur))
                        {
                            but.Background = tDroite;
                            but.back = "tDroite";
                        }
                        else if ((butO.letype == ButtonM.type.MurInt || butO.letype == ButtonM.type.Mur) && (butE.letype == ButtonM.type.MurInt || butE.letype == ButtonM.type.Mur) &&( butS.letype == ButtonM.type.MurInt || butS.letype == ButtonM.type.Mur))
                        {
                            but.Background = tBas;
                            but.back = "tBas";
                        }
                        else if ((butO.letype == ButtonM.type.MurInt || butO.letype == ButtonM.type.Mur) && (butE.letype == ButtonM.type.MurInt || butE.letype == ButtonM.type.Mur) && (butN.letype == ButtonM.type.MurInt || butN.letype == ButtonM.type.Mur))
                        {
                            but.Background = tHaut;
                            but.back = "tHaut";
                        }
                        else if ((butN.letype == ButtonM.type.MurInt || butN.letype == ButtonM.type.Mur) && (butS.letype == ButtonM.type.MurInt || butS.letype == ButtonM.type.Mur))
                        {
                            but.Background = murv;
                            but.back = "murv";
                        }
                        else if ((butO.letype == ButtonM.type.MurInt || butO.letype == ButtonM.type.Mur) && (butE.letype == ButtonM.type.MurInt || butE.letype == ButtonM.type.Mur))
                        {
                            but.Background = murh;
                            but.back = "murh";
                        }
                        else if ((butS.letype == ButtonM.type.MurInt || butS.letype == ButtonM.type.Mur) && (butO.letype == ButtonM.type.MurInt || butO.letype == ButtonM.type.Mur))
                        {
                            but.Background = anglehd;
                            but.back = "anglehd";
                        }
                        else if ((butS.letype == ButtonM.type.MurInt || butS.letype == ButtonM.type.Mur) && (butE.letype == ButtonM.type.MurInt || butE.letype == ButtonM.type.Mur))
                        {
                            but.Background = anglehg;
                            but.back = "anglehg";
                        }
                        else if ((butN.letype == ButtonM.type.MurInt || butN.letype == ButtonM.type.Mur) && (butO.letype == ButtonM.type.MurInt || butO.letype == ButtonM.type.Mur))
                        {
                            but.Background = anglebd;
                            but.back = "anglebd";
                        }
                        else if ((butN.letype == ButtonM.type.MurInt || butN.letype == ButtonM.type.Mur) && (butE.letype == ButtonM.type.MurInt || butE.letype == ButtonM.type.Mur))
                        {
                            but.Background = anglebg;
                            but.back = "anglebg";
                        }
                        else if (butO.letype == ButtonM.type.MurInt || butE.letype == ButtonM.type.MurInt || butO.letype == ButtonM.type.Mur || butE.letype == ButtonM.type.Mur)
                        {
                            but.Background = murh;
                            but.back = "murh";
                        }
                        else if (butN.letype == ButtonM.type.MurInt || butS.letype == ButtonM.type.MurInt || butN.letype == ButtonM.type.Mur || butS.letype == ButtonM.type.Mur)
                        {
                            but.Background = murv;
                            but.back = "murv";
                        }
                        else
                        {
                            but.Background = croix;
                            but.back = "croix";
                        }
                        if (but.x == 4 && but.y == 24)
                        {
                            Console.WriteLine("but.type " + but.back);
                        }
                    }
                    else if (but.letype == ButtonM.type.Rien)
                    {
                        but.Background = Brushes.LightGray;
                    }
                }
            }
        }

        private bool isInside (ButtonM but)
        {
            bool inside = false;
            bool gauche = false;
            bool droite = false;
            bool haut = false;
            bool bas = false;

            for (int i = 0; i < listB.GetLength(0); i++)
            {
                if (listB[i,but.y].x < but.x && listB[i, but.y].letype != ButtonM.type.Rien)
                {
                    gauche = true;
                }
                if (listB[i, but.y].x > but.x && listB[i, but.y].letype != ButtonM.type.Rien)
                {
                    droite = true;
                }
            }
            for (int i = 0; i < listB.GetLength(1); i++)
            {
                if (listB[but.x, i].y < but.x && listB[but.x, i].letype != ButtonM.type.Rien)
                {
                    haut = true;
                }
                if (listB[but.x, i].y > but.x && listB[but.x, i].letype != ButtonM.type.Rien)
                {
                    bas = true;
                }
            }
            if (haut && bas && gauche && droite)
            {
                inside = true;
            }

            return inside;
        }

        private void afficheTexture(object sender, RoutedEventArgs e)
        {
            for (int i = 1; i < listB.GetLength(0) - 1; i++)
            {
                for (int y = 1; y < listB.GetLength(1) - 1; y++)
                {
                    if (listB[i,y].texture != null)
                    {
                        listB[i, y].Background = listB[i, y].Background = listB[i, y].texture;
                    }
                }
            }
        }
    }
}

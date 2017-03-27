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
using System.Collections.Generic;
using System.Diagnostics;

namespace Madera_MMB.View_Crtl
{
    /// <summary>
    /// Controle de la vue Modelisation
    /// Features Restantes :
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
        private StackPanel stackP = new StackPanel();
        private StackPanel stackList = new StackPanel();
        private string mode = "default";
        private string modeAffich = "tracage";
        List<MetaModule> listMeta;
        private MetaModule metaChoose;


        private Plan plan { get; set; }
        private PlanCAD planCad { get; set; }
        private Connexion con { get; set; }

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
        private Brush slotFH = new ImageBrush(new BitmapImage(new Uri("../../Lib/Images/slotF_horizontal.png", UriKind.RelativeOrAbsolute)));
        private Brush slotFV = new ImageBrush(new BitmapImage(new Uri("../../Lib/Images/slotF_vertical.png", UriKind.RelativeOrAbsolute)));
        private Brush slotPH = new ImageBrush(new BitmapImage(new Uri("../../Lib/Images/slotP_horizontal.png", UriKind.RelativeOrAbsolute)));
        private Brush slotPV = new ImageBrush(new BitmapImage(new Uri("../../Lib/Images/slotP_vertical.png", UriKind.RelativeOrAbsolute)));
        private Brush slotH = new ImageBrush(new BitmapImage(new Uri("../../Lib/Images/slot_horizontal.png", UriKind.RelativeOrAbsolute)));
        private Brush slotV = new ImageBrush(new BitmapImage(new Uri("../../Lib/Images/slot_vertical.png", UriKind.RelativeOrAbsolute)));

        private Brush mur_beton = new ImageBrush(new BitmapImage(new Uri("../../Lib/Images/mur_beton.jpg", UriKind.RelativeOrAbsolute)));

        private ButtonM slot = new ButtonM(ButtonM.type.SlotPorte, 15, 5, 1, 1, null);
        private ButtonM slot2 = new ButtonM(ButtonM.type.SlotFen, 15, 25, 1, 1, null);

        private ButtonM murdroit = new ButtonM(ButtonM.type.Mur, 25, 5, 1, 20, null);
        private ButtonM murgauche = new ButtonM(ButtonM.type.Mur, 5, 6, 1, 20, null);
        private ButtonM murbas = new ButtonM(ButtonM.type.Mur, 6, 25, 20, 1, null);
        private ButtonM murhaut = new ButtonM(ButtonM.type.Mur, 5, 5, 20, 1, null);
        #endregion

        #region Constructeurs
        public Modelisation()
        {
            InitializeComponent();
            initialize();
        }

        public Modelisation(Connexion con, Plan plan, PlanCAD planCad)
        {
            this.con = con;
            this.plan = plan;
            this.planCad = planCad;
            InitializeComponent();
            this.DataContext = this.planCad;
            initialize();
        }
        #endregion

        #region Méthodes initialisation
        private void initialize()
        {
            grid.Margin = new Thickness(7);
            
            planCad = new PlanCAD(new Connexion(), new Projet(new Client(), new Commercial()));
            listMeta = planCad.ListMetaModule;

            //slot.Click += new RoutedEventHandler(checkType);
            //slot2.Click += new RoutedEventHandler(checkType);
            tracer.Click += new RoutedEventHandler(changeMode);
            retirer.Click += new RoutedEventHandler(changeMode);

            Grid.SetColumn(listBox, 0);
            Grid.SetRow(listBox, 2);
            listBox.Margin = new Thickness(7,7,7,7);

            Grid.SetColumn(stackList, 0);
            Grid.SetRow(stackList, 2);
            stackList.Margin = new Thickness(20, 20, 20, 20);

            Grid.SetColumn(stackP, 0);
            Grid.SetRow(stackP, 2);
            stackP.Margin = new Thickness(20, 20, 20, 20);

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
                    but.BorderBrush = Brushes.Silver;
                    but.Background = Brushes.LightGray;
                    Grid.SetRow(but, but.y);
                    Grid.SetColumn(but, but.x);
                    but.Click += new RoutedEventHandler(checkType);

                    listB[x, y] = but;
                    grid.Children.Add(but);
                }
            }

            placeComponent(murhaut);
            placeComponent(murbas);
            placeComponent(murgauche);
            placeComponent(murdroit);
            placeComponent(slot);
            placeComponent(slot2);

            checkImage();
        }
        #endregion

        #region Méthodes privées
        private void placeComponent(ButtonM but)
        {
            /*Grid.SetColumn(but, but.x);
            Grid.SetColumnSpan(but, but.colspan);
            Grid.SetRow(but, but.y);
            Grid.SetRowSpan(but, but.rowspan);*/
            listB[but.x, but.y].letype = but.letype;
            
            if (but.rowspan > 1 )
            {
                for (int i = 0; i < but.rowspan; i++)
                {
                    listB[but.x, but.y + i].letype = but.letype;
                }
            }
            else if (but.colspan > 1)
            {
                for (int i = 0; i < but.colspan; i++)
                {
                    listB[but.x + i, but.y].letype = but.letype;
                }
            }

            //grid.Children.Add(but);
        }

        private void checkType(object sender, RoutedEventArgs e)
        {
            ButtonM but = sender as ButtonM;
            checkMode();
            if (but.letype == ButtonM.type.Rien)
            {
                if (mode == "tracer")
                {
                    placeWall(but);
                }
                else if(mode == "retirer")
                {

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
            else if (but.letype == ButtonM.type.SlotPorte || but.letype == ButtonM.type.SlotFen)
            {
                loadChoiceButton(but);
            }
            else if (but.letype == ButtonM.type.Slot)
            {
                if (mode == "retirer")
                {
                    removeWall(but);
                }
                else if (mode == "tracer")
                {

                }
                else
                {
                    loadChoiceButton(but);
                }
            }
        }

        private void loadChoiceButton (ButtonM but)
        {
            MainGrid.Children.Remove(stackP);

            /*if (but.meta )
            {
                Charger les informations du module pour les afficher
            }*/
            stackP.Children.Clear();

            if (but.letype == ButtonM.type.Slot || but.letype == ButtonM.type.SlotFen)
            {
                Button butFen = new Button();
                butFen.Height = 50;
                butFen.Content = "Fenêtres";
                butFen.Click += new RoutedEventHandler(loadFiltre);
                stackP.Children.Add(butFen);
            }

            if (but.letype == ButtonM.type.Slot || but.letype == ButtonM.type.SlotPorte)
            {
                Button butPor = new Button();
                butPor.Height = 50;
                butPor.Content = "Portes";
                butPor.Click += new RoutedEventHandler(loadFiltre);
                stackP.Children.Add(butPor);
            }

            MainGrid.Children.Add(stackP);
        }

        private void loadFiltre(object sender, RoutedEventArgs e)
        {
            Button but = sender as Button;
            if ((string)but.Content == "Fenêtres")
            {
                // Charger la liste des gammes possibiles pour les fenetres sous forme de liste de boutons dans le stack panel
                // en utilisant le planCAD
            }
            else if ((string)but.Content == "Portes")
            {
                // Charger la liste des gammes possibiles pour les portes sous forme de liste de boutons dans le stack panel
                // en utilisant le planCAD
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
                MainGrid.Children.Remove(stackP);
                loadListMur();
            }
            else if (mode == "retirer" || mode == "default") {
                tracer.IsChecked = false;
                MainGrid.Children.Remove(listBox);
                MainGrid.Children.Remove(stackP);
            }
        }

        private void loadListMur()
        {
            MainGrid.Children.Remove(stackP);
            stackP.Children.Clear();
            
            foreach (MetaModule meta in listMeta)
            {
                // La taille sera de 1 pour des modules de mur à placer
                // Changer le mode d'affichage
                //if (meta.taille == 12 && meta.label.Contains("Mur"))
                //{
                    ToggleButton toggle = new ToggleButton();
                    toggle.Background = Brushes.White;
                    toggle.Width = 150;
                    toggle.Height = 170;

                    Thickness margin = toggle.Margin;
                    margin.Left = 30;
                    margin.Right = 30;
                    margin.Bottom = 10;
                    margin.Top = 10;
                    toggle.Margin = margin;

                    Image img = new Image();
                    img.Source = meta.image;
                    img.Width = 150;
                    img.Height = 130;
                    img.VerticalAlignment = VerticalAlignment.Top;

                    TextBlock tb = new TextBlock();
                    tb.Text = meta.label;
                    tb.VerticalAlignment = VerticalAlignment.Bottom;
                    tb.HorizontalAlignment = HorizontalAlignment.Center;
                    tb.Height = 40;

                    StackPanel sp = new StackPanel();
                    sp.Children.Add(img);
                    sp.Children.Add(tb);

                    toggle.Content = sp;

                    toggle.Click += delegate (object sender, RoutedEventArgs e)
                    {
                        ToggleButton active = sender as ToggleButton;
                        foreach (ToggleButton tgbt in FindVisualChildren<ToggleButton>(stackList))
                        {
                            tgbt.IsChecked = false;
                        }
                        metaChoose = meta;
                        active.IsChecked = true;
                    };

                    stackList.Children.Add(toggle);
                //}
            }
            MainGrid.Children.Add(stackP);
        }

        private void placeWall(ButtonM but)
        {
            if (checkAround(but) && isInside(but))
            {
                but.letype = ButtonM.type.MurInt;
                but.texture = mur_beton;
                checkImage();
                placeSlot();
            }
        }

        private void removeWall(ButtonM but)
        {
            if (isInside(but))
            {
                but.letype = ButtonM.type.Rien;
                but.texture = null;
                checkImage();
                placeSlot();
            }
        }

        private void placeSlot()
        {
            for (int i = 1; i < listB.GetLength(0) - 1; i++)
            {
                for (int y = 1; y < listB.GetLength(1) - 1; y++)
                {
                    ButtonM but = listB[i, y];
                    if (but.letype == ButtonM.type.MurInt || but.letype == ButtonM.type.Slot)
                    {
                        ButtonM butN = listB[but.x, but.y - 1];
                        ButtonM butN2 = listB[but.x, but.y - 2];
                        ButtonM butS = listB[but.x, but.y + 1];
                        ButtonM butS2 = listB[but.x, but.y + 2];
                        ButtonM butO = listB[but.x - 1, but.y];
                        ButtonM butO2 = listB[but.x - 2, but.y];
                        ButtonM butE = listB[but.x + 1, but.y];
                        ButtonM butE2 = listB[but.x + 2, but.y];

                        if ((butN.Background == murv && butS.Background == murv) && (butO.letype == ButtonM.type.Rien && butE.letype == ButtonM.type.Rien))
                        {
                            if (butN2.letype != ButtonM.type.Slot && butS2.letype != ButtonM.type.Slot)
                            {
                                but.letype = ButtonM.type.Slot;
                            }
                        }
                        else if ((butO.Background == murh && butE.Background == murh) && (butN.letype == ButtonM.type.Rien && butS.letype == ButtonM.type.Rien))
                        {
                            if (butO2.letype != ButtonM.type.Slot && butE2.letype != ButtonM.type.Slot)
                            {
                                but.letype = ButtonM.type.Slot;
                            }
                        }
                        else
                        {
                            //if (butN.letype != ButtonM.type.Mur && butS.letype != ButtonM.type.Mur && butO.letype != ButtonM.type.Mur && butE.letype != ButtonM.type.Mur)
                            //{
                                but.letype = ButtonM.type.MurInt;
                            //}
                        }

                        checkImage();
                    }
                }
            }
        }

        private bool checkAround(ButtonM but)
        {
            bool around = false;

            ButtonM butN = listB[but.x, but.y - 1];
            ButtonM butS = listB[but.x, but.y + 1];
            ButtonM butO = listB[but.x - 1, but.y];
            ButtonM butE = listB[but.x + 1, but.y];

            if (butN.letype != ButtonM.type.Rien || butS.letype != ButtonM.type.Rien || butO.letype != ButtonM.type.Rien || butE.letype != ButtonM.type.Rien)
            {
                if (butN.letype != ButtonM.type.SlotPorte && butN.letype != ButtonM.type.SlotFen && butS.letype != ButtonM.type.SlotPorte && butS.letype != ButtonM.type.SlotFen && butE.letype != ButtonM.type.SlotPorte && butE.letype != ButtonM.type.SlotFen && butO.letype != ButtonM.type.SlotPorte && butO.letype != ButtonM.type.SlotFen)
                {
                    around = true;
                }
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
                    ButtonM butN = listB[but.x, but.y - 1];
                    ButtonM butS = listB[but.x, but.y + 1];
                    ButtonM butO = listB[but.x - 1, but.y];
                    ButtonM butE = listB[but.x + 1, but.y];
                    if (but.letype == ButtonM.type.MurInt || but.letype == ButtonM.type.Mur)
                    {

                        if (butN.letype != ButtonM.type.Rien && butS.letype != ButtonM.type.Rien && butO.letype != ButtonM.type.Rien && butE.letype != ButtonM.type.Rien)
                        {
                            but.Background = croix;
                        }
                        else if (butN.letype != ButtonM.type.Rien && butS.letype != ButtonM.type.Rien && butO.letype != ButtonM.type.Rien)
                        {
                            but.Background = tGauche;
                        }
                        else if (butN.letype != ButtonM.type.Rien && butS.letype != ButtonM.type.Rien && butE.letype != ButtonM.type.Rien)
                        {
                            but.Background = tDroite;
                        }
                        else if (butO.letype != ButtonM.type.Rien && butE.letype != ButtonM.type.Rien && butS.letype != ButtonM.type.Rien)
                        {
                            but.Background = tBas;
                        }
                        else if (butO.letype != ButtonM.type.Rien && butE.letype != ButtonM.type.Rien && butN.letype != ButtonM.type.Rien)
                        {
                            but.Background = tHaut;
                        }
                        else if (butN.letype != ButtonM.type.Rien && butS.letype != ButtonM.type.Rien)
                        {
                            but.Background = murv;
                        }
                        else if (butO.letype != ButtonM.type.Rien && butE.letype != ButtonM.type.Rien)
                        {
                            but.Background = murh;
                        }
                        else if (butS.letype != ButtonM.type.Rien && butO.letype != ButtonM.type.Rien)
                        {
                            but.Background = anglehd;
                        }
                        else if (butS.letype != ButtonM.type.Rien && butE.letype != ButtonM.type.Rien)
                        {
                            but.Background = anglehg;
                        }
                        else if (butN.letype != ButtonM.type.Rien && butO.letype != ButtonM.type.Rien)
                        {
                            but.Background = anglebd;
                        }
                        else if (butN.letype != ButtonM.type.Rien && butE.letype != ButtonM.type.Rien)
                        {
                            but.Background = anglebg;
                        }
                        else if (butO.letype != ButtonM.type.Rien || butE.letype != ButtonM.type.Rien)
                        {
                            but.Background = murh;
                        }
                        else if (butN.letype != ButtonM.type.Rien || butS.letype != ButtonM.type.Rien)
                        {
                            but.Background = murv;
                        }
                        else
                        {
                            but.Background = croix;
                        }
                    }
                    else if (but.letype == ButtonM.type.SlotFen)
                    {
                        if ((butN.letype == ButtonM.type.Mur && butS.letype == ButtonM.type.Mur) || (butN.letype == ButtonM.type.MurInt && butS.letype == ButtonM.type.MurInt))
                        {
                            but.Background = slotFV;
                        } else
                        {
                            but.Background = slotFH;
                        }
                    }
                    else if (but.letype == ButtonM.type.SlotPorte)
                    {
                        if ((butN.letype == ButtonM.type.Mur && butS.letype == ButtonM.type.Mur) || (butN.letype == ButtonM.type.MurInt && butS.letype == ButtonM.type.MurInt))
                        {
                            but.Background = slotPV;
                        }
                        else
                        {
                            but.Background = slotPH;
                        }
                    }
                    else if (but.letype == ButtonM.type.Slot)
                    {
                        if ((butN.letype == ButtonM.type.Mur && butS.letype == ButtonM.type.Mur) || (butN.letype == ButtonM.type.MurInt && butS.letype == ButtonM.type.MurInt))
                        {
                            but.Background = slotV;
                        }
                        else
                        {
                            but.Background = slotH;
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
            if (modeAffich == "tracage")
            {
                modeAffich = "texture";
                for (int i = 1; i < listB.GetLength(0) - 1; i++)
                {
                    for (int y = 1; y < listB.GetLength(1) - 1; y++)
                    {
                        if (listB[i, y].texture != null)
                        {
                            listB[i, y].Background = listB[i, y].texture;
                        }
                    }
                }
            }
            else
            {
                modeAffich = "tracage";
                checkImage();
            }
        }

        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
        #endregion
    }
}

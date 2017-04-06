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
    ///     Gestion mur ext
    ///     Chargement auto plan
    /// </summary>
    public partial class Modelisation : Page
    {
        #region Attributs
        private Grid grid = new Grid();
        public Module[,] listB = new Module[40, 30];
        private ListBox listBox = new ListBox();
        private Viewbox viewB = new Viewbox();
        private StackPanel stackP = new StackPanel();
        private StackPanel stackList = new StackPanel();
        private string mode = "default";
        private string modeAffich = "tracage";
        private MetaModule metaChoose;
        private Module butChoose;
        private string type;
        List<MetaModule> listMeta;
        List<Module> listMurExt = new List<Module>();

        public Plan plan { get; set; }
        public PlanCAD planCad { get; set; }
        public Connexion con { get; set; }

        #region Images
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
        private Brush slotPlH = new ImageBrush(new BitmapImage(new Uri("../../Lib/Images/slot_Hplein.png", UriKind.RelativeOrAbsolute)));
        private Brush slotPlV = new ImageBrush(new BitmapImage(new Uri("../../Lib/Images/slot_Vplein.png", UriKind.RelativeOrAbsolute)));
        #endregion

        private Module slot = new Module(Module.type.SlotPorte, 15, 5, 1, 1, null);
        private Module slot2 = new Module(Module.type.SlotFen, 15, 25, 1, 1, null);

        private Module murdroit = new Module(Module.type.Mur, 25, 5, 1, 20, null);
        private Module murgauche = new Module(Module.type.Mur, 5, 6, 1, 20, null);
        private Module murbas = new Module(Module.type.Mur, 6, 25, 20, 1, null);
        private Module murhaut = new Module(Module.type.Mur, 5, 5, 20, 1, null);
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
            loadModules();
        }
        #endregion

        #region Méthodes initialisation
        private void initialize()
        {
            grid.Margin = new Thickness(7);
            
            planCad = new PlanCAD(new Connexion(), new Projet(new Client(), new Commercial()));
            listMeta = planCad.listAllMetaModules();
            
            tracer.Click += new RoutedEventHandler(changeMode);
            retirer.Click += new RoutedEventHandler(changeMode);

            Grid.SetColumn(listBox, 0);
            Grid.SetRow(listBox, 2);
            listBox.Margin = new Thickness(7,7,7,7);

            Grid.SetColumn(viewB, 0);
            Grid.SetRow(viewB, 2);
            viewB.Margin = new Thickness(20, 20, 20, 20);
            MainGrid.Children.Add(viewB);

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
                    Module but = new Module(Module.type.Rien, x, y, 1, 1, null);
                    but.BorderBrush = Brushes.Silver;
                    but.Background = Brushes.LightGray;
                    but.meta = null;
                    Grid.SetRow(but, but.y);
                    Grid.SetColumn(but, but.x);
                    but.Click += new RoutedEventHandler(checkType);

                    listB[x, y] = but;
                    grid.Children.Add(but);
                }
            }

            murhaut.meta = new MetaModule("M406588", "Mur exterieur 2F 1P", 420, null, true, "2017-04-06", new Gamme(), 12, 2);
            murbas.meta = new MetaModule("M406590", "Mur exterieur 2F", 390, null, true, "2017-04-06", new Gamme(), 12, 2);
            murgauche.meta = new MetaModule("M406587", "Mur exterieur 2F", 390, null, true, "2017-04-06", new Gamme(), 12, 2);
            murdroit.meta = new MetaModule("M406587", "Mur exterieur 2F", 390, null, true, "2017-04-06", new Gamme(), 12, 2);

            listMurExt.Add(murhaut);
            listMurExt.Add(murbas);
            listMurExt.Add(murgauche);
            listMurExt.Add(murdroit);

            placeComponent(murhaut);
            placeComponent(murbas);
            placeComponent(murgauche);
            placeComponent(murdroit);

            placeComponent(slot);
            placeComponent(slot2);

            checkImage();
        }

        private void loadModules()
        {
            Trace.WriteLine("############################### Load modules ###############################");
            foreach (Module mod in this.plan.modules)
            {
                Trace.WriteLine("####### " + mod.meta.label + " ####### " + mod.letype + " #######");
                placeComponent(mod);
            }
            Trace.WriteLine("############################### Fin Load modules ###############################");
            checkImage();
        }
        #endregion

        #region Méthodes privées
        private void placeComponent(Module but)
        {
            listB[but.x, but.y].letype = but.letype;
            listB[but.x, but.y].meta = but.meta;
            listB[but.x, but.y].parent = but.parent;
            listB[but.x, but.y].texture = but.texture;

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
        }

        private void checkType(object sender, RoutedEventArgs e)
        {
            Module but = sender as Module;
            butChoose = but;
            checkMode();
            if (but.letype == Module.type.Rien)
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
            else if (but.letype == Module.type.SlotMur)
            {
                mode = "default";
                MainGrid.Children.Remove(listBox);
                listBox.Items.Clear();
                MainGrid.Children.Add(listBox);
                ListBoxItem item1 = new ListBoxItem();
                item1.Content = "Yop !";
                listBox.Items.Add(item1);
            }
            else if (but.letype == Module.type.MurInt)
            {
                if (mode == "retirer")
                {
                    removeWall(but);
                }
            }
            else if (but.letype == Module.type.SlotPorte || but.letype == Module.type.SlotFen)
            {
                if (mode != "retirer" && mode != "tracer")
                {
                    loadChoiceButton(but);
                }
            }
            else if (but.letype == Module.type.Slot)
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
            else if (but.letype == Module.type.Porte || but.letype == Module.type.Fenetre)
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

        private void loadChoiceButton (Module but)
        {
            MainGrid.Children.Remove(stackP);
            stackP.Children.Clear();

            if (but.meta != null)
            {
                ToggleButton toggle = new ToggleButton();
                toggle.Background = Brushes.White;
                toggle.Width = 150;
                toggle.Height = 170;
                toggle.Margin = new Thickness(30, 10, 30, 10);

                Image img = new Image();
                img.Source = but.meta.image;
                img.Width = 150;
                img.Height = 130;
                img.VerticalAlignment = VerticalAlignment.Top;

                TextBlock tb = new TextBlock();
                tb.Text = but.meta.label;
                tb.VerticalAlignment = VerticalAlignment.Bottom;
                tb.HorizontalAlignment = HorizontalAlignment.Center;
                tb.Height = 40;

                StackPanel sp = new StackPanel();
                sp.Children.Add(img);
                sp.Children.Add(tb);

                stackP.Children.Add(sp);
            }

            if (but.letype == Module.type.Slot || but.letype == Module.type.SlotFen || but.letype == Module.type.Fenetre || but.letype == Module.type.Porte)
            {
                Button butFen = new Button();
                butFen.Height = 50;
                butFen.Content = "Fenêtres";
                butFen.Click += new RoutedEventHandler(loadFiltre);
                stackP.Children.Add(butFen);
            }

            if (but.letype == Module.type.Slot || but.letype == Module.type.SlotPorte || but.letype == Module.type.Fenetre || but.letype == Module.type.Porte)
            {
                Button butPor = new Button();
                butPor.Height = 50;
                butPor.Content = "Portes";
                butPor.Click += new RoutedEventHandler(loadFiltre);
                stackP.Children.Add(butPor);
            }

            if (but.meta != null)
            {

                Button vider = new Button();
                vider.Background = Brushes.White;
                vider.Height = 50;
                vider.Content = "Vider le slot";
                vider.Click += delegate (object sender3, RoutedEventArgs e2)
                {
                    foreach (ToggleButton tgbt in FindVisualChildren<ToggleButton>(stackList))
                    {
                        tgbt.IsChecked = false;
                    }
                    if (butChoose.parent != null)
                    {
                        butChoose.meta = butChoose.parent;
                        Brush fond = new ImageBrush(butChoose.meta.image);
                        butChoose.texture = fond;
                        butChoose.parent = null;
                        butChoose.letype = Module.type.Slot;
                    }
                    checkImage();
                };
                stackP.Children.Add(vider);
            }

            viewB.Child = stackP;
            //MainGrid.Children.Add(viewB);
        }

        private void loadFiltre(object sender, RoutedEventArgs e)
        {
            Button but = sender as Button;
            if (mode == "default")
            {
                MainGrid.Children.Remove(scrollView);
                MainGrid.Children.Remove(stackP);
                stackP.Children.Clear();

                List<string> listGammes = new List<string>();

                Trace.WriteLine((string)but.Content);
                if ((string)but.Content == "Fenêtres")
                {
                    listGammes = planCad.listAllGammes("Fen");
                    type = "Fen";
                }
                else if ((string)but.Content == "Portes")
                {
                    listGammes = planCad.listAllGammes("Por");
                    type = "Por";
                }
                Button butG1 = new Button();
                butG1.Height = 50;
                butG1.Content = "Aucun filtre";
                butG1.Click += new RoutedEventHandler(loadListMeta);
                stackP.Children.Add(butG1);

                foreach (string gamme in listGammes)
                {
                    Button butG = new Button();
                    butG.Height = 50;
                    butG.Content = gamme;
                    butG.Click += new RoutedEventHandler(loadListMeta);
                    stackP.Children.Add(butG);
                }
                
                viewB.Child = stackP;
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
                viewB.Child = null;
                loadListMur();
            }
            else if (mode == "retirer" || mode == "default") {
                tracer.IsChecked = false;
                viewB.Child = null;
            }
        }

        private void loadListMur()
        {
            MainGrid.Children.Remove(scrollView);
            stackList.Children.Clear();
            
            
            foreach (MetaModule meta in listMeta)
            {
                if (meta.taille == 1 && meta.label.Contains("Mur"))
                {
                    ToggleButton toggle = new ToggleButton();
                    toggle.Background = Brushes.White;
                    toggle.Width = 150;
                    toggle.Height = 170;
                    toggle.Margin = new Thickness(30, 10, 30, 10);

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

                    if (meta == metaChoose)
                    {
                        toggle.IsChecked = true;
                    }

                    stackList.Children.Add(toggle);
                }
            }
            
            scrollView.Content = stackList;
            viewB.Child = scrollView;
            //MainGrid.Children.Add(scrollView);
        }

        private void loadListMeta(object sender, RoutedEventArgs e)
        {
            Button butSender = sender as Button;
            string gamme = (string)butSender.Content;
            
            stackP.Children.Clear();
            stackList.Children.Clear();

            foreach (MetaModule meta in listMeta)
            {
                if (meta.label.Contains(type))
                {
                    if (meta.taille == 1)
                    {
                        ToggleButton toggle = new ToggleButton();
                        toggle.Background = Brushes.White;
                        toggle.Width = 150;
                        toggle.Height = 170;
                        toggle.Margin = new Thickness(30, 10, 30, 10);

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

                        toggle.Click += delegate (object sender2, RoutedEventArgs e2)
                        {
                            ToggleButton active = sender2 as ToggleButton;
                            foreach (ToggleButton tgbt in FindVisualChildren<ToggleButton>(stackList))
                            {
                                tgbt.IsChecked = false;
                            }
                            butChoose.parent = butChoose.meta;
                            butChoose.meta = meta;
                            Brush fond = new ImageBrush(butChoose.meta.image);
                            butChoose.texture = fond;
                            if (type == "Fen")
                            {
                                butChoose.letype =  Module.type.Fenetre;
                            }
                            else if (type == "Por")
                            {
                                butChoose.letype =  Module.type.Porte;
                            }
                            active.IsChecked = true;
                            checkImage();
                        };

                        if (meta == butChoose.meta)
                        {
                            toggle.IsChecked = true;
                        }

                        if (gamme != "Aucun filtre" && meta.gamme.nom == gamme)
                        {
                            stackList.Children.Add(toggle);
                        } 
                        else if (gamme == "Aucun filtre")
                        {
                            stackList.Children.Add(toggle);
                        }
                    }
                }
            }
            
            scrollView.Content = stackList;
            viewB.Child = scrollView;
            //MainGrid.Children.Add(scrollView);
        }

        private void placeWall(Module but)
        {
            if (isInside(but) && checkAround(but) && metaChoose != null)
            {
                but.letype =  Module.type.MurInt;
                Brush fond = new ImageBrush(metaChoose.image);
                but.texture = fond;
                but.meta = metaChoose;
                checkImage();
                placeSlot();
            }
        }

        private void removeWall(Module but)
        {
            if (isInside(but))
            {
                but.letype =  Module.type.Rien;
                but.texture = null;
                but.meta = null;
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
                     Module but = listB[i, y];
                    if (but.letype ==  Module.type.MurInt || but.letype ==  Module.type.Slot)
                    {
                         Module butN = listB[but.x, but.y - 1];
                         Module butN2 = listB[but.x, but.y - 2];
                         Module butS = listB[but.x, but.y + 1];
                         Module butS2 = listB[but.x, but.y + 2];
                         Module butO = listB[but.x - 1, but.y];
                         Module butO2 = listB[but.x - 2, but.y];
                         Module butE = listB[but.x + 1, but.y];
                         Module butE2 = listB[but.x + 2, but.y];

                        if ((butN.Background == murv && butS.Background == murv) && (butO.letype ==  Module.type.Rien && butE.letype ==  Module.type.Rien))
                        {
                            if (butN2.letype ==  Module.type.MurInt && butS2.letype ==  Module.type.MurInt)
                            {
                                but.letype =  Module.type.Slot;
                            }
                        }
                        else if ((butO.Background == murh && butE.Background == murh) && (butN.letype ==  Module.type.Rien && butS.letype ==  Module.type.Rien))
                        {
                            if (butO2.letype ==  Module.type.MurInt && butE2.letype ==  Module.type.MurInt)
                            {
                                but.letype =  Module.type.Slot;
                            }
                        }
                        else
                        {
                            but.letype =  Module.type.MurInt;
                        }

                        checkImage();
                    }
                }
            }
        }

        private bool checkAround(Module but)
        {
            bool around = false;

             Module butN = listB[but.x, but.y - 1];
             Module butS = listB[but.x, but.y + 1];
             Module butO = listB[but.x - 1, but.y];
             Module butE = listB[but.x + 1, but.y];

            if (butN.letype !=  Module.type.Rien || butS.letype !=  Module.type.Rien || butO.letype !=  Module.type.Rien || butE.letype !=  Module.type.Rien)
            {
                if (butN.letype !=  Module.type.SlotPorte && butN.letype !=  Module.type.SlotFen && butS.letype !=  Module.type.SlotPorte && butS.letype !=  Module.type.SlotFen && butE.letype !=  Module.type.SlotPorte && butE.letype !=  Module.type.SlotFen && butO.letype !=  Module.type.SlotPorte && butO.letype !=  Module.type.SlotFen)
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
                     Module but = listB[i, y];
                     Module butN = listB[but.x, but.y - 1];
                     Module butS = listB[but.x, but.y + 1];
                     Module butO = listB[but.x - 1, but.y];
                     Module butE = listB[but.x + 1, but.y];
                    if (but.letype ==  Module.type.MurInt || but.letype ==  Module.type.Mur)
                    {

                        if (butN.letype !=  Module.type.Rien && butS.letype !=  Module.type.Rien && butO.letype !=  Module.type.Rien && butE.letype !=  Module.type.Rien)
                        {
                            but.Background = croix;
                        }
                        else if (butN.letype !=  Module.type.Rien && butS.letype !=  Module.type.Rien && butO.letype !=  Module.type.Rien)
                        {
                            but.Background = tGauche;
                        }
                        else if (butN.letype !=  Module.type.Rien && butS.letype !=  Module.type.Rien && butE.letype !=  Module.type.Rien)
                        {
                            but.Background = tDroite;
                        }
                        else if (butO.letype !=  Module.type.Rien && butE.letype !=  Module.type.Rien && butS.letype !=  Module.type.Rien)
                        {
                            but.Background = tBas;
                        }
                        else if (butO.letype !=  Module.type.Rien && butE.letype !=  Module.type.Rien && butN.letype !=  Module.type.Rien)
                        {
                            but.Background = tHaut;
                        }
                        else if (butN.letype !=  Module.type.Rien && butS.letype !=  Module.type.Rien)
                        {
                            but.Background = murv;
                        }
                        else if (butO.letype !=  Module.type.Rien && butE.letype !=  Module.type.Rien)
                        {
                            but.Background = murh;
                        }
                        else if (butS.letype !=  Module.type.Rien && butO.letype !=  Module.type.Rien)
                        {
                            but.Background = anglehd;
                        }
                        else if (butS.letype !=  Module.type.Rien && butE.letype != Module.type.Rien)
                        {
                            but.Background = anglehg;
                        }
                        else if (butN.letype != Module.type.Rien && butO.letype != Module.type.Rien)
                        {
                            but.Background = anglebd;
                        }
                        else if (butN.letype != Module.type.Rien && butE.letype != Module.type.Rien)
                        {
                            but.Background = anglebg;
                        }
                        else if (butO.letype != Module.type.Rien || butE.letype != Module.type.Rien)
                        {
                            but.Background = murh;
                        }
                        else if (butN.letype != Module.type.Rien || butS.letype != Module.type.Rien)
                        {
                            but.Background = murv;
                        }
                        else
                        {
                            but.Background = croix;
                        }
                    }
                    else if (but.letype == Module.type.SlotFen)
                    {
                        if ((butN.letype == Module.type.Mur && butS.letype == Module.type.Mur) || (butN.letype == Module.type.MurInt && butS.letype == Module.type.MurInt))
                        {
                            but.Background = slotFV;
                        } else
                        {
                            but.Background = slotFH;
                        }
                    }
                    else if (but.letype == Module.type.SlotPorte)
                    {
                        if ((butN.letype == Module.type.Mur && butS.letype == Module.type.Mur) || (butN.letype == Module.type.MurInt && butS.letype == Module.type.MurInt))
                        {
                            but.Background = slotPV;
                        }
                        else
                        {
                            but.Background = slotPH;
                        }
                    }
                    else if (but.letype == Module.type.Slot )
                    {
                        if ((butN.letype == Module.type.Mur && butS.letype == Module.type.Mur) || (butN.letype == Module.type.MurInt && butS.letype == Module.type.MurInt))
                        {
                            but.Background = slotV;
                        }
                        else
                        {
                            but.Background = slotH;
                        }
                    }
                    else if (but.letype == Module.type.Porte || but.letype == Module.type.Fenetre)
                    {
                        if ((butN.letype == Module.type.Mur && butS.letype == Module.type.Mur) || (butN.letype == Module.type.MurInt && butS.letype == Module.type.MurInt))
                        {
                            but.Background = slotPlV;
                        }
                        else
                        {
                            but.Background = slotPlH;
                        }
                    }
                    else if (but.letype == Module.type.Rien)
                    {
                        but.Background = Brushes.LightGray;
                    }
                }
            }
        }

        private bool isInside (Module but)
        {
            bool inside = false;
            bool gauche = false;
            bool droite = false;
            bool haut = false;
            bool bas = false;

            for (int i = 0; i < listB.GetLength(0); i++)
            {
                if (listB[i, but.y].x < but.x && listB[i, but.y].letype != Module.type.Rien)
                {
                    gauche = true;
                }
                if (listB[i, but.y].x > but.x && listB[i, but.y].letype != Module.type.Rien)
                {
                    droite = true;
                }
            }
            for (int i = 0; i < listB.GetLength(1); i++)
            {
                if (listB[but.x, i].y < but.x && listB[but.x, i].letype != Module.type.Rien)
                {
                    haut = true;
                }
                if (listB[but.x, i].y > but.x && listB[but.x, i].letype != Module.type.Rien)
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

        private void save_Click(object sender, RoutedEventArgs e)
        {
            this.plan.modules.Clear();
            for (int x = 1; x < listB.GetLength(0) - 1; x++)
            {
                for (int y = 1; y < listB.GetLength(1) - 1; y++)
                {
                    if (listB[x, y].letype != Module.type.Rien && listB[x, y].letype != Module.type.Mur && listB[x, y].letype != Module.type.SlotPorte && listB[x, y].letype != Module.type.SlotFen)
                    {
                        this.plan.modules.Add(listB[x, y]);
                    }
                }
            }

            foreach (Module mod in listMurExt)
            {
                this.plan.modules.Add(mod);
            }

            foreach (Module mod in this.plan.modules)
            {
                planCad.insertModule(mod, this.plan.reference);
            }
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Madera_MMB.CAD;
using Madera_MMB.Lib;
using System.Diagnostics;
using System.IO;
using Madera_MMB.Model;

namespace Madera_MMB.View_Crtl
{
    /// <summary>
    /// Logique d'interaction pour ParametresPlan.xaml
    /// </summary>
    public partial class ParametresPlan : Page
    {
        #region Properties

        public Connexion Conn { get; set; }

        private CoupePrincipeCAD coupeCAD { get; set; }
        private CouvertureCAD couvCAD { get; set; }
        private PlancherCAD planchCAD { get; set; }
        private GammeCAD gammCAD { get; set; }
        private string taille_choisie { get; set; }
        private string nom_coupe { get; set; }
        private CoupePrincipe coupeChoisie { get; set; }
        private Couverture couvChoisie { get; set; }
        private Plancher planchChoisi { get; set; }
        private Gamme gammChoisie { get; set; }
        private Projet projet { get; set; }

        #endregion

        #region Constructeur
        public ParametresPlan(Connexion co, Projet projet)
        {
            InitializeComponent();
            Conn = co;
            this.projet = projet;

            if (Conn.MySQLconnected != false)
                Conn.SyncParamPlan();

            coupeCAD = new CoupePrincipeCAD(this.Conn);
            couvCAD = new CouvertureCAD(this.Conn);
            planchCAD = new PlancherCAD(this.Conn);
            gammCAD = new GammeCAD(this.Conn);

            initialize_coupe_wrappers();
            initialize_couv_wrapper();
            initialize_planch_wrapper();
            initialize_gamme_wrapper();
        }
        #endregion

        #region Initialisation des conteneurs
        /// <summary>
        /// Méthode renseigant le formulaire de choix d'une coupe de principe à partir de la base de données SQLite
        /// </summary>
        private void initialize_coupe_wrappers()
        {
            if(coupeCAD.Listecoupeprincipe != null)
            {
                foreach (var coupe in this.coupeCAD.Listecoupeprincipe)
                {
                    if(coupe.label != nom_coupe)
                    {
                        nom_coupe = coupe.label;
                        ToggleButton UneForme = new ToggleButton();
                        UneForme.Background = Brushes.White;
                        UneForme.Width = 100;
                        UneForme.Height = 125;
                        Thickness margin = UneForme.Margin;
                        margin.Left = 20;
                        margin.Right = 20;
                        margin.Bottom = 10;
                        margin.Top = 10;
                        UneForme.Margin = margin;

                        Image img = new Image();
                        img.Source = coupe.image;
                        img.Width = 80;
                        img.Height = 100;
                        img.VerticalAlignment = VerticalAlignment.Top;

                        TextBlock tb = new TextBlock();
                        tb.Text = coupe.label;
                        tb.VerticalAlignment = VerticalAlignment.Bottom;
                        tb.HorizontalAlignment = HorizontalAlignment.Center;
                        tb.Height = 40;

                        StackPanel sp = new StackPanel();
                        sp.Children.Add(img);
                        sp.Children.Add(tb);

                        UneForme.Content = sp;

                        UneForme.Click += delegate (object sender, RoutedEventArgs e)
                        {
                            wraptailles.Children.Clear();
                            ToggleButton active = sender as ToggleButton;
                            foreach (ToggleButton tgbt in FindVisualChildren<ToggleButton>(wrapformes))
                            {
                                tgbt.IsChecked = false;
                            }
                            foreach (var coupeTaille in coupeCAD.Listecoupeprincipe)
                            {
                                if (coupeTaille.label == coupe.label)
                                {
                                    ToggleButton UneTaille = new ToggleButton();
                                    UneTaille.Background = Brushes.White;
                                    UneTaille.Width = 100;
                                    UneTaille.Height = 50;
                                    Thickness margint = UneTaille.Margin;
                                    margin.Left = 20;
                                    margin.Right = 20;
                                    margin.Bottom = 10;
                                    margin.Top = 10;
                                    UneTaille.Margin = margin;

                                    UneTaille.Content = coupeTaille.largeur.ToString() + " X " + coupeTaille.longueur.ToString();

                                    UneTaille.Click += delegate (object send, RoutedEventArgs eventargs)
                                    {
                                        ToggleButton actif = send as ToggleButton;
                                        foreach (ToggleButton tgbt in FindVisualChildren<ToggleButton>(wraptailles))
                                        {
                                            tgbt.IsChecked = false;
                                        }
                                        taille_choisie = UneTaille.Content.ToString();
                                        choix_coupe(coupe.label, taille_choisie);
                                        actif.IsChecked = true;
                                        BoutonChoixCoupe.Content = "";
                                        BoutonChoixCoupe.Content = coupeChoisie.label + " " + coupeChoisie.largeur.ToString() + " X " + coupeChoisie.longueur.ToString();
                                    };
                                    wraptailles.Children.Add(UneTaille);
                                }
                            }
                            active.IsChecked = true;
                            BoutonChoixCoupe.Content = "";
                            if (taille_choisie != null)
                            {
                                choix_coupe(coupe.label, taille_choisie);
                                BoutonChoixCoupe.Content = coupeChoisie.label + " " + coupeChoisie.largeur.ToString() + " X " + coupeChoisie.longueur.ToString();
                            }
                        };
                        wrapformes.Children.Add(UneForme);
                    }                                 
                }
            }
        }

        /// <summary>
        /// Méthode renseigant le formulaire de choix d'une couverture à partir de la base de données SQLite
        /// </summary>
        private void initialize_couv_wrapper()
        {
            if(this.couvCAD.Listecouverture != null)
            {
                foreach (var couv in couvCAD.Listecouverture)
                {
                    ToggleButton UneTuile = new ToggleButton();
                    UneTuile.Background = Brushes.White;
                    UneTuile.Width = 150;
                    UneTuile.Height = 170;
                    Thickness margin = UneTuile.Margin;
                    margin.Left = 30;
                    margin.Right = 30;
                    margin.Bottom = 10;
                    margin.Top = 10;
                    UneTuile.Margin = margin;

                    Image img = new Image();
                    img.Source = couv.image;
                    img.Width = 150;
                    img.Height = 130;
                    img.VerticalAlignment = VerticalAlignment.Top;

                    TextBlock tb = new TextBlock();
                    tb.Text = couv.type;
                    tb.VerticalAlignment = VerticalAlignment.Bottom;
                    tb.HorizontalAlignment = HorizontalAlignment.Center;
                    tb.Height = 40;

                    StackPanel sp = new StackPanel();
                    sp.Children.Add(img);
                    sp.Children.Add(tb);

                    UneTuile.Content = sp;

                    UneTuile.Click += delegate (object sender, RoutedEventArgs e)
                    {
                        ToggleButton active = sender as ToggleButton;
                        foreach (ToggleButton tgbt in FindVisualChildren<ToggleButton>(wrapcouv))
                        {
                            tgbt.IsChecked = false;
                        }
                        couvChoisie = couv;
                        active.IsChecked = true;
                        BoutonChoixCouverture.Content = couvChoisie.type;
                    };

                    wrapcouv.Children.Add(UneTuile);
                }
            } 
        }

        /// <summary>
        /// Méthode renseigant le formulaire de choix d'un plancher à partir de la base de données SQLite
        /// </summary>
        private void initialize_planch_wrapper()
        {
            if(planchCAD.Listeplancher != null)
            {
                foreach (var plancher in planchCAD.Listeplancher)
                {
                    ToggleButton UnPlanch = new ToggleButton();
                    UnPlanch.Background = Brushes.White;
                    UnPlanch.Width = 150;
                    UnPlanch.Height = 170;
                    Thickness margin = UnPlanch.Margin;
                    margin.Left = 30;
                    margin.Right = 30;
                    margin.Bottom = 10;
                    margin.Top = 10;
                    UnPlanch.Margin = margin;

                    Image img = new Image();
                    img.Source = plancher.image;
                    img.Width = 150;
                    img.Height = 130;
                    img.VerticalAlignment = VerticalAlignment.Top;

                    TextBlock tb = new TextBlock();
                    tb.Text = plancher.type;
                    tb.VerticalAlignment = VerticalAlignment.Bottom;
                    tb.HorizontalAlignment = HorizontalAlignment.Center;
                    tb.Height = 40;

                    StackPanel sp = new StackPanel();
                    sp.Children.Add(img);
                    sp.Children.Add(tb);

                    UnPlanch.Content = sp;

                    UnPlanch.Click += delegate (object sender, RoutedEventArgs e)
                    {
                        ToggleButton active = sender as ToggleButton;
                        foreach (ToggleButton tgbt in FindVisualChildren<ToggleButton>(wrapplanch))
                        {
                            tgbt.IsChecked = false;

                        }
                        planchChoisi = plancher;
                        active.IsChecked = true;
                        BoutonChoixPlancher.Content = planchChoisi.type;
                    };

                    wrapplanch.Children.Add(UnPlanch);
                }
            }
        }

        /// <summary>
        /// Méthode renseigant le formulaire de choix d'une gamme à partir de la base de données SQLite
        /// </summary>
        private void initialize_gamme_wrapper()
        {
            if(gammCAD.Listegamme != null)
            {
                foreach (var gamme in gammCAD.Listegamme)
                {
                    ToggleButton UneGam = new ToggleButton();
                    UneGam.Background = Brushes.White;
                    UneGam.Width = 150;
                    UneGam.Height = 170;
                    Thickness margin = UneGam.Margin;
                    margin.Left = 30;
                    margin.Right = 30;
                    margin.Bottom = 10;
                    margin.Top = 10;
                    UneGam.Margin = margin;

                    Image img = new Image();
                    img.Source = gamme.image;
                    img.Width = 150;
                    img.Height = 130;
                    img.VerticalAlignment = VerticalAlignment.Top;

                    TextBlock tb = new TextBlock();
                    tb.Text = gamme.nom;
                    tb.VerticalAlignment = VerticalAlignment.Bottom;
                    tb.HorizontalAlignment = HorizontalAlignment.Center;
                    tb.Height = 40;

                    StackPanel sp = new StackPanel();
                    sp.Children.Add(img);
                    sp.Children.Add(tb);

                    UneGam.Content = sp;

                    UneGam.Click += delegate (object sender, RoutedEventArgs e)
                    {
                        ToggleButton active = sender as ToggleButton;
                        foreach (ToggleButton tgbt in FindVisualChildren<ToggleButton>(wrapgamme))
                        {
                            tgbt.IsChecked = false;

                        }
                        gammChoisie = gamme;
                        active.IsChecked = true;
                        BoutonChoixGamme.Content = gammChoisie.nom;
                    };

                    wrapgamme.Children.Add(UneGam);
                }
            } 
        }

        /// <summary>
        /// méthode permettant la sélection de la coupe de principe selon les critères de forme et de taille
        /// </summary>
        /// <param name="label"></param>
        /// <param name="taille"></param>
        private void choix_coupe(string label, string taille)
        {
            foreach(var coupe in coupeCAD.Listecoupeprincipe)
            {
                if(coupe.label == label && taille.Contains(coupe.largeur.ToString()))
                {
                    coupeChoisie = coupe;
                }
            }
        }
        #endregion

        /// <summary>
        /// Méthodes d'écoute des différents boutons de choix des paramètres du Plan, permettant d'afficher le formulaire correspondant au paramètre désigné
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region listeners
        private void cp_Button_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton btn = sender as ToggleButton;
            if (forme.Visibility == System.Windows.Visibility.Hidden && taille.Visibility == System.Windows.Visibility.Hidden)
            {
                clearAll();
                forme.Visibility = System.Windows.Visibility.Visible;
                taille.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                clearAll();
            }

            disableButtons();
            btn.IsChecked = true;

        }

        private void couv_Button_Click(object sender, RoutedEventArgs e)
        {
            resetBtnChoixCoupe();
            ToggleButton btn = sender as ToggleButton;
            if (couv.Visibility == System.Windows.Visibility.Hidden)
            {
                clearAll();
                couv.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                clearAll();
            }

            disableButtons();
            btn.IsChecked = true;

        }

        private void planc_Button_Click(object sender, RoutedEventArgs e)
        {
            resetBtnChoixCoupe();
            ToggleButton btn = sender as ToggleButton;
            if (planch.Visibility == System.Windows.Visibility.Hidden)
            {
                clearAll();
                planch.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                clearAll();
            }

            disableButtons();
            btn.IsChecked = true;

        }

        private void gamme_Button_Click(object sender, RoutedEventArgs e)
        {
            resetBtnChoixCoupe();
            ToggleButton btn = sender as ToggleButton;
            if (gam.Visibility == System.Windows.Visibility.Hidden)
            {
                clearAll();
                gam.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                clearAll();
            }

            disableButtons();
            btn.IsChecked = true;

        }

        private void BtnConfirmerParamPlan_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnRetour_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        #region public methods
        public bool SetPlan()
        {
            if(this.coupeChoisie != null && this.couvChoisie != null && this.planchChoisi != null)
            {
                string label;
                Plan plan = new Plan(label, this.projet, this.planchChoisi, this.couvChoisie, this.coupeChoisie, this.gammChoisie);
                return true;
            }
            return false;
        }
        #endregion

        #region Tools
        /// <summary>
        /// Méthode permettant de récupérer les éléments enfants d'un conteneur, nécessaire pour désactiver les Togglebuttons
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="depObj"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Méthode désactivant tous les Togglebuttons
        /// </summary>
        private void disableButtons()
        {
            BoutonChoixCoupe.IsChecked = false;
            BoutonChoixCouverture.IsChecked = false;
            BoutonChoixPlancher.IsChecked = false;
            BoutonChoixGamme.IsChecked = false;
        }
        /// <summary>
        /// Méthode cachant tous les formulaires de paramètre de plan
        /// </summary>
        private void clearAll()
        {
            forme.Visibility = Visibility.Hidden;
            taille.Visibility = Visibility.Hidden;
            couv.Visibility = Visibility.Hidden;
            planch.Visibility = Visibility.Hidden;
            gam.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// Méthode restaurant l'indiquation du bouton de choix de coupe de principe dans le cas où une coupe n'est pas choisie
        /// </summary>
        private void resetBtnChoixCoupe()
        {
            if ((string)BoutonChoixCoupe.Content == "")
                BoutonChoixCoupe.Content = " Choisir une coupe ";
        }
        #endregion

    }
}
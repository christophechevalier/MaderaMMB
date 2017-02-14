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
        private CoupePrincipe coupeChoisie { get; set; }
        private Couverture couvChoisie { get; set; }
        private Plancher planchChoisi { get; set; }
        private Gamme gammChoisie { get; set; }

        #endregion

        #region Constructeur
        public ParametresPlan(Connexion co)
        {
            InitializeComponent();
            Conn = co;

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
            if(coupeCAD.listecoupeprincipe != null)
            {
                foreach (var coupe in this.coupeCAD.listecoupeprincipe)
                {
                    ToggleButton Uneforme = new ToggleButton();
                    Uneforme.Background = Brushes.White;
                    Uneforme.Width = 150;
                    Uneforme.Height = 170;
                    Thickness margin = Uneforme.Margin;
                    margin.Left = 30;
                    margin.Right = 30;
                    margin.Bottom = 10;
                    margin.Top = 10;
                    Uneforme.Margin = margin;

                    Image img = new Image();
                    img.Source = coupe.image;
                    img.Width = 150;
                    img.Height = 130;
                    img.VerticalAlignment = VerticalAlignment.Top;

                    TextBlock tb = new TextBlock();
                    tb.Text = coupe.label;
                    tb.VerticalAlignment = VerticalAlignment.Bottom;
                    tb.HorizontalAlignment = HorizontalAlignment.Center;
                    tb.Height = 40;

                    StackPanel sp = new StackPanel();
                    sp.Children.Add(img);
                    sp.Children.Add(tb);

                    Uneforme.Content = sp;

                    Uneforme.Click += delegate (object sender, RoutedEventArgs e)
                    {
                        ToggleButton active = sender as ToggleButton;
                        foreach (ToggleButton tgbt in FindVisualChildren<ToggleButton>(wrapformes))
                        {
                            tgbt.IsChecked = false;
                        }
                        choix_coupe(coupe.label, taille_choisie);
                        active.IsChecked = true;
                        BoutonChoixCoupe.Content = "";
                        BoutonChoixCoupe.Content = coupe.label + " " + taille_choisie;
                    };

                    wrapformes.Children.Add(Uneforme);

                    ToggleButton Unetaille = new ToggleButton();
                    Unetaille.Background = Brushes.White;
                    Unetaille.Width = 100;
                    Unetaille.Height = 50;
                    Thickness margint = Unetaille.Margin;
                    margin.Left = 50;
                    margin.Right = 50;
                    margin.Bottom = 10;
                    margin.Top = 10;
                    Unetaille.Margin = margin;

                    TextBlock tbTaille = new TextBlock();
                    tbTaille.Text = coupe.largeur.ToString() + " X " + coupe.longueur.ToString();
                    tbTaille.VerticalAlignment = VerticalAlignment.Center;
                    tbTaille.HorizontalAlignment = HorizontalAlignment.Center;
                    tbTaille.Height = 50;

                    StackPanel spTaille = new StackPanel();
                    sp.Children.Add(tbTaille);

                    Unetaille.Content = spTaille;

                    Unetaille.Click += delegate (object sender, RoutedEventArgs e)
                    {
                        ToggleButton active = sender as ToggleButton;
                        foreach (ToggleButton tgbt in FindVisualChildren<ToggleButton>(wraptailles))
                        {
                            tgbt.IsChecked = false;
                        }
                        taille_choisie = tbTaille.Text;
                        active.IsChecked = true;
                        BoutonChoixCoupe.Content = "";
                        BoutonChoixCoupe.Content = coupe.label + " " + taille_choisie;
                    };
                    wraptailles.Children.Add(Unetaille);
                }
            }
        }

        /// <summary>
        /// Méthode renseigant le formulaire de choix d'une couverture à partir de la base de données SQLite
        /// </summary>
        private void initialize_couv_wrapper()
        {
            if(this.couvCAD.listecouverture != null)
            {
                foreach (var couv in couvCAD.listecouverture)
                {
                    ToggleButton Unetuile = new ToggleButton();
                    Unetuile.Background = Brushes.White;
                    Unetuile.Width = 150;
                    Unetuile.Height = 170;
                    Thickness margin = Unetuile.Margin;
                    margin.Left = 30;
                    margin.Right = 30;
                    margin.Bottom = 10;
                    margin.Top = 10;
                    Unetuile.Margin = margin;

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

                    Unetuile.Content = sp;

                    Unetuile.Click += delegate (object sender, RoutedEventArgs e)
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

                    wrapcouv.Children.Add(Unetuile);
                }
            } 
        }

        /// <summary>
        /// Méthode renseigant le formulaire de choix d'un plancher à partir de la base de données SQLite
        /// </summary>
        private void initialize_planch_wrapper()
        {
            if(planchCAD.listeplancher != null)
            {
                foreach (var plancher in planchCAD.listeplancher)
                {
                    ToggleButton Unplanch = new ToggleButton();
                    Unplanch.Background = Brushes.White;
                    Unplanch.Width = 150;
                    Unplanch.Height = 170;
                    Thickness margin = Unplanch.Margin;
                    margin.Left = 30;
                    margin.Right = 30;
                    margin.Bottom = 10;
                    margin.Top = 10;
                    Unplanch.Margin = margin;

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

                    Unplanch.Content = sp;

                    Unplanch.Click += delegate (object sender, RoutedEventArgs e)
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

                    wrapplanch.Children.Add(Unplanch);
                }
            }
        }

        /// <summary>
        /// Méthode renseigant le formulaire de choix d'une gamme à partir de la base de données SQLite
        /// </summary>
        private void initialize_gamme_wrapper()
        {
            if(gammCAD.listegamme != null)
            {
                foreach (var gamme in gammCAD.listegamme)
                {
                    ToggleButton Unegam = new ToggleButton();
                    Unegam.Background = Brushes.White;
                    Unegam.Width = 150;
                    Unegam.Height = 170;
                    Thickness margin = Unegam.Margin;
                    margin.Left = 30;
                    margin.Right = 30;
                    margin.Bottom = 10;
                    margin.Top = 10;
                    Unegam.Margin = margin;

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

                    Unegam.Content = sp;

                    Unegam.Click += delegate (object sender, RoutedEventArgs e)
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

                    wrapgamme.Children.Add(Unegam);
                }
            }
            
        }

        private void choix_coupe(string label, string taille)
        {
            foreach(var coupe in coupeCAD.listecoupeprincipe)
            {
                if(coupe.label == label && taille.Contains(coupe.largeur.ToString()))
                {
                    coupeChoisie = coupe;
                }
            }
        }
        #endregion

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

        #region Tools
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
        private void disableButtons()
        {
            BoutonChoixCoupe.IsChecked = false;
            BoutonChoixCouverture.IsChecked = false;
            BoutonChoixPlancher.IsChecked = false;
            BoutonChoixGamme.IsChecked = false;
        }
        private void clearAll()
        {
            forme.Visibility = Visibility.Hidden;
            taille.Visibility = Visibility.Hidden;
            couv.Visibility = Visibility.Hidden;
            planch.Visibility = Visibility.Hidden;
            gam.Visibility = Visibility.Hidden;
        }
        #endregion

    }
}
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

namespace Madera_MMB.View_Crtl
{
    /// <summary>
    /// Logique d'interaction pour ParametresPlan.xaml
    /// </summary>
    public partial class ParametresPlan : Page
    {
        #region Properties
        private string type_forme { get; set; }
        private string taille_choisie { get; set; }
        private string couverture { get; set; }
        private string plancher { get; set; }
        #endregion

        public ParametresPlan()
        {
            InitializeComponent();
            initialize_cp_wrappers();
            initialize_couv_wrapper();
            initialize_planch_wrapper();
            initialize_gamme_wrapper();
        }

        #region Initialisation des conteneurs
        private void initialize_cp_wrappers()
        {
            for (int i = 0; i < 4; i++ )
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
                img.Width = 150;
                img.Height = 130;
                img.VerticalAlignment = VerticalAlignment.Top;
                string source = "../Lib/carre.png";
                Uri imageUri = new Uri(source, UriKind.Relative);
                BitmapImage imageBitmap = new BitmapImage(imageUri);
                img.Source = imageBitmap;

                TextBlock tb = new TextBlock();
                tb.Text = "Carré";
                tb.VerticalAlignment = VerticalAlignment.Bottom;
                tb.HorizontalAlignment = HorizontalAlignment.Center;
                tb.Height = 40;

                StackPanel sp = new StackPanel();
                sp.Children.Add(img);
                sp.Children.Add(tb);
               
                Uneforme.Content = sp;

                Uneforme.Click += delegate(object sender, RoutedEventArgs e)
                {
                    ToggleButton active = sender as ToggleButton;
                    foreach (ToggleButton tgbt in FindVisualChildren<ToggleButton>(wrapformes))
                    {
                        tgbt.IsChecked = false;
                    }
                    type_forme = tb.Text;
                    active.IsChecked = true;
                    btnchoix1.Content = type_forme + " " + taille_choisie;
                };

                wrapformes.Children.Add(Uneforme);
            }

            for(int i = 0; i < 9; i++)
            {
                ToggleButton Unetaille = new ToggleButton();
                Unetaille.Background = Brushes.White;
                Unetaille.Width = 100;
                Unetaille.Height = 50;
                Thickness margin = Unetaille.Margin;
                margin.Left = 50;
                margin.Right = 50;
                margin.Bottom = 10;
                margin.Top = 10;
                Unetaille.Margin = margin;

                TextBlock tb = new TextBlock();
                tb.Text = "50 X 50";
                tb.VerticalAlignment = VerticalAlignment.Center;
                tb.HorizontalAlignment = HorizontalAlignment.Center;
                tb.Height = 50;

                StackPanel sp = new StackPanel();
                sp.Children.Add(tb);

                Unetaille.Content = sp;

                Unetaille.Click += delegate(object sender, RoutedEventArgs e)
                {
                    ToggleButton active = sender as ToggleButton;
                    foreach (ToggleButton tgbt in FindVisualChildren<ToggleButton>(wraptailles))
                    {
                        tgbt.IsChecked = false;
                    }
                    taille_choisie = tb.Text;
                    active.IsChecked = true;
                    btnchoix1.Content = type_forme + " " + taille_choisie;
                };

                wraptailles.Children.Add(Unetaille);
            }

        }
        private void initialize_couv_wrapper()
        {
            for (int i = 0; i < 4; i++)
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
                img.Width = 150;
                img.Height = 130;
                img.VerticalAlignment = VerticalAlignment.Top;
                string source = "../Lib/tuile.jpg";
                Uri imageUri = new Uri(source, UriKind.Relative);
                BitmapImage imageBitmap = new BitmapImage(imageUri);
                img.Source = imageBitmap;

                TextBlock tb = new TextBlock();
                tb.Text = "Tuiles";
                tb.VerticalAlignment = VerticalAlignment.Bottom;
                tb.HorizontalAlignment = HorizontalAlignment.Center;
                tb.Height = 40;

                StackPanel sp = new StackPanel();
                sp.Children.Add(img);
                sp.Children.Add(tb);

                Unetuile.Content = sp;

                Unetuile.Click += delegate(object sender, RoutedEventArgs e)
                {
                    ToggleButton active = sender as ToggleButton;
                    foreach (ToggleButton tgbt in FindVisualChildren<ToggleButton>(wrapcouv))
                    {
                        tgbt.IsChecked = false;
                    }
                    couverture = tb.Text;
                    active.IsChecked = true;
                    btnchoix2.Content = couverture;
                };

                wrapcouv.Children.Add(Unetuile);
            }
        }
        private void initialize_planch_wrapper()
        {
            for (int i = 0; i < 4; i++)
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
                img.Width = 150;
                img.Height = 130;
                img.VerticalAlignment = VerticalAlignment.Top;
                string source = "../Lib/carrelage.jpg";
                Uri imageUri = new Uri(source, UriKind.Relative);
                BitmapImage imageBitmap = new BitmapImage(imageUri);
                img.Source = imageBitmap;

                TextBlock tb = new TextBlock();
                tb.Text = "Carrelage";
                tb.VerticalAlignment = VerticalAlignment.Bottom;
                tb.HorizontalAlignment = HorizontalAlignment.Center;
                tb.Height = 40;

                StackPanel sp = new StackPanel();
                sp.Children.Add(img);
                sp.Children.Add(tb);

                Unplanch.Content = sp;

                Unplanch.Click += delegate(object sender, RoutedEventArgs e)
                {
                    ToggleButton active = sender as ToggleButton;
                    foreach (ToggleButton tgbt in FindVisualChildren<ToggleButton>(wrapplanch))
                    {
                        tgbt.IsChecked = false;

                    }
                    plancher = tb.Text;
                    active.IsChecked = true;
                    btnchoix3.Content = plancher;
                };

                wrapplanch.Children.Add(Unplanch);
            }
        }
        private void initialize_gamme_wrapper()
        {
            for (int i = 0; i < 4; i++)
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
                img.Width = 150;
                img.Height = 130;
                img.VerticalAlignment = VerticalAlignment.Top;
                string source = "../Lib/bois.png";
                Uri imageUri = new Uri(source, UriKind.Relative);
                BitmapImage imageBitmap = new BitmapImage(imageUri);
                img.Source = imageBitmap;

                TextBlock tb = new TextBlock();
                tb.Text = "Tout en bois";
                tb.VerticalAlignment = VerticalAlignment.Bottom;
                tb.HorizontalAlignment = HorizontalAlignment.Center;
                tb.Height = 40;

                StackPanel sp = new StackPanel();
                sp.Children.Add(img);
                sp.Children.Add(tb);

                Unegam.Content = sp;

                Unegam.Click += delegate(object sender, RoutedEventArgs e)
                {
                    ToggleButton active = sender as ToggleButton;
                    foreach (ToggleButton tgbt in FindVisualChildren<ToggleButton>(wrapgamme))
                    {
                        tgbt.IsChecked = false;

                    }
                    plancher = tb.Text;
                    active.IsChecked = true;
                    btnchoix4.Content = plancher;
                };

                wrapgamme.Children.Add(Unegam);
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
            btnchoix1.IsChecked = false;
            btnchoix2.IsChecked = false;
            btnchoix3.IsChecked = false;
            btnchoix4.IsChecked = false;
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

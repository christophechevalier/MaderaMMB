using Madera_MMB.Lib.Tools;
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
    /// Logique d'interaction pour GestionClient.xaml
    /// </summary>
    public partial class GestionPlan : Page
    {
        #region Properties
        private string nom_client { get; set; }
        #endregion

        #region Constructeur
        public GestionPlan()
        {
            InitializeComponent();
            Initialize_Plan_Wrapper();
            Initialize_Labels();
        }
        #endregion

        #region Initialisation Container
        private void Initialize_Plan_Wrapper()
        {
            for (int i = 0; i < 9; i++)
            {
                ToggleButton UnPlan = new ToggleButton();
                UnPlan.Background = Brushes.White;
                UnPlan.Width = 150;
                UnPlan.Height = 150;
                Thickness margin = UnPlan.Margin;
                margin.Left = 20;
                margin.Right = 20;
                margin.Bottom = 20;
                margin.Top = 20;
                UnPlan.Margin = margin;

                Image img = new Image();
                img.Width = 100;
                img.Height = 100;
                img.VerticalAlignment = VerticalAlignment.Top;
                string source = "../Lib/Images/house_black.png";
                Uri imageUri = new Uri(source, UriKind.Relative);
                BitmapImage imageBitmap = new BitmapImage(imageUri);
                img.Source = imageBitmap;

                TextBlock tb = new TextBlock();
                tb.Text = "Plan 1 Base carrée";
                tb.VerticalAlignment = VerticalAlignment.Bottom;
                tb.HorizontalAlignment = HorizontalAlignment.Center;
                tb.Height = 50;

                StackPanel sp = new StackPanel();
                sp.Children.Add(img);
                sp.Children.Add(tb);

                UnPlan.Content = sp;

                UnPlan.Click += delegate(object sender, RoutedEventArgs e)
                {
                    statut.Content = "Statut actuel : ";
                    dateCréation.Content = "Date de création : ";
                    dateMaj.Content = "Dernière mise à jour : ";
                    basePlan.Content = "Base du plan : ";

                    ToggleButton active = sender as ToggleButton;
                    foreach (ToggleButton tgbt in FindVisualChildren<ToggleButton>(WrapPlans))
                    {
                        tgbt.IsChecked = false;
                    }
                    nom_client = tb.Text;
                    active.IsChecked = true;

                    statut.Content += "Devis brouillon";
                    dateCréation.Content += "03/05/2016";
                    dateMaj.Content += "21/06/2016";
                    basePlan.Content += "Base carrée";
                };

                WrapPlans.Children.Add(UnPlan);
            }
        }
        private void Initialize_Labels()
        {
            nomClient.Content += "NOYER Thierry";
            nomCommercial.Content += "José Répas";
        }

        #endregion


        #region Listeners
        private void BtnCréerPlan_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnOuvrirPlan_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnConsulterDevis_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnRetour_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCopierPlan_Click(object sender, RoutedEventArgs e)
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
        #endregion


    }
}

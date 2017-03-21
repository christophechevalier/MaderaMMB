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
using System.Windows.Markup;
using Madera_MMB.Lib;

namespace Madera_MMB.View_Crtl
{
    /// <summary>
    /// Logique d'interaction pour Authentification.xaml
    /// </summary>
    public partial class Authentification : Page, IComponentConnector
    {
        #region Properties

        public Connexion co { get; set; }
        #endregion
        public Authentification(Connexion conn)
        {

            InitializeComponent();
            this.co = conn;
            this.DataContext = co;
        }

        #region listeners
        // Quitter l'application
        private void Btn_Quitter_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        #endregion
    }
}

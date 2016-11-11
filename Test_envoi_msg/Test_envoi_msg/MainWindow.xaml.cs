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
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Test_envoi_msg
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.log

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button bouton = sender as Button;

            IPAddress ipAd = IPAddress.Parse("127.0.0.1");
            log.Text = "... ";
            
            log.Text += "Connexion ... ";
            TcpClient tcpclnt = new TcpClient();

            try
            {
                tcpclnt.Connect(ipAd, 9000);
            
                log.Text += "\n Connecté";


                String str = "POST /RPC2 HTTP/1.1 Host: localhost:9000 Accept-Encoding: gzip User-Agent: xmlrpclib.py/1.0.1 (by www.pythonware.com) Content-Type: text/xml Content-Length: 97 <?xml version='1.0'?> <methodCall> <methodName>now</methodName><params> </params></methodCall>";
                Stream stm = tcpclnt.GetStream();

                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes(str);
                log.Text += "\n Envoi de :  " + str;

                stm.Write(ba, 0, ba.Length);

                byte[] bb = new byte[100];
                int k = stm.Read(bb, 0, 100);

                for (int i = 0; i < k; i++)
                    {
                        log.Text += "\n " + Convert.ToChar(bb[i]);
                    }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                log.Text += "\n" + ex.ToString();
            }

            tcpclnt.Close();
            log.Text += "\n  Déconnecté";

            //float lefloat = (float)String.Format("{0:#,###.##}", zone.Text) as float;

                
        }



        /// removes the client info from the connectedClients enumerable


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        //private void PreviewTextInput(object sender, TextCompositionEventArgs e)
        //{
        //    //e.Handled = !IsValidInput(e.Text);

        //}

        ////private bool IsValidInput(string p)
        ////{
        ////    switch (this.Type)
        ////    {
        ////        case NumericTextBoxType.Float:
        ////            return Regex.Match(p, "^[0-9]*[.][0-9]*$").Success;
        ////        case NumericTextBoxType.Integer:
        ////        default:
        ////            return Regex.Match(p, "^[0-9]*$").Success;
        ////    }
        ////}


        ////public enum NumericTextBoxType
        ////{
        ////    Integer = 0,
        ////    Float = 1
        ////}
    }
}

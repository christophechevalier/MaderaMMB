using Madera_MMB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Madera_MMB.Lib;
using System.Data.SQLite;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Madera_MMB.CAD
{
    public class ProjetCAD : INotifyPropertyChanged
    {
        #region Properties
        public Connexion conn { get; set; }
        public string SQLQuery { get; set; }
        public Commercial commercial { get; set; }
        public Client client { get; set; }
        public ClientCAD clientCAD { get; set; }
        public CommercialCAD commercialCAD { get; set; }
        public ObservableCollection<Client> Clients { get; set; }
        private ObservableCollection<Projet> _projets;
        public ObservableCollection<Projet> Projets
        {
            get
            {
                return this._projets;
            }
            set { _projets = value;}
        }
        #endregion

        #region Events
        private void Projets_CollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("Projets"));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Ctor
        /// <summary>
        /// Constructeur qui prend en paramètre la connexion et le commercial authentifié
        /// </summary>
        /// <param name="laConnexion"></param>
        /// <param name="com"></param>
        public ProjetCAD(Connexion laConnexion, Commercial com, ObservableCollection<Client> Clients)
        {
            // Instanciations
            conn = laConnexion;
            commercial = com;
            Projets = new ObservableCollection<Projet>();
            _projets.CollectionChanged += Projets_CollectionChanged;
            this.Clients = Clients;

            // Appel des méthodes dans le ctor
            ListAllProjects();
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Méthode qui permet de récupérer la liste des projets existant
        /// </summary>
        public void ListAllProjects()
        {
            // Nom du/des champs mis directement dans la requête pour éviter d'avoir à passer par QSqlRecord 
            SQLQuery = "SELECT refProjet, nom, dateCreation, dateModification, refClient, refCommercial FROM projet WHERE refCommercial = '" + commercial.reference + "'";
            //SQLQuery = "SELECT * FROM projet WHERE refCommercial = " + commercial.reference;

            // Ouverture de la connexion
            conn.LiteCo.Open();
            using (SQLiteCommand command = new SQLiteCommand(SQLQuery, conn.LiteCo))
            {
                Trace.WriteLine(SQLQuery);
                try
                {
                    // Execute le lecteur de donnée
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        Trace.WriteLine("#### GET PROJETS DATA ####");
                        while (reader.Read())
                        {
                            Trace.WriteLine(
                                reader.GetString(0) +
                                reader.GetString(1) +
                                reader.GetString(2) +
                                reader.GetString(3) +
                                client +
                                commercial);
                            Projet proj = new Projet
                                (
                                    reader.GetString(0),
                                    reader.GetString(1),
                                    reader.GetString(2),
                                    reader.GetString(3),
                                    getClient(reader.GetString(4)),
                                    commercial
                                );
                            Projets.Add(proj);
                        }
                    }
                    Trace.WriteLine("#### GET PROJETS DATA SUCCESS ####");
                }
                catch (SQLiteException ex)
                {
                    Trace.WriteLine(" \n ################################################# ERREUR RECUPERATION PROJETS ################################################# \n" + ex.ToString() + "\n");
                }
            }
            conn.LiteCo.Close();
        }

        public Client getClient(string id)
        {
            Trace.WriteLine(id);
            foreach (Client cli in Clients)
            {
                Trace.WriteLine(cli.reference);
                if (cli.reference == id)
                {
                    return cli;
                }
            }

            return null;
        }

        /// <summary>
        /// Méthode qui permet de créer un nouveau projet avec un client et un commercial pour ref
        /// </summary>
        /// <param name="projet"></param>
        /// <param name="refClient"></param>
        /// <param name="refCommercial"></param>
        public void InsertProjet(Projet projet)
        {
            string SQLQuery = "INSERT INTO projet(refProjet, nom, dateCreation, dateModification, refClient, refCommercial)" + 
                "VALUES (@refProjet, @nom, @dateCreation, @dateModification, @refClient, @refCommercial)";

            // Ouverture de la connexion
            conn.LiteCo.Open();
            using (SQLiteCommand command = new SQLiteCommand(SQLQuery, conn.LiteCo))
            {
                Trace.WriteLine(SQLQuery);
                try
                {
                    command.Parameters.AddWithValue("@refProjet", projet.reference);
                    command.Parameters.AddWithValue("@nom", projet.nom);
                    command.Parameters.AddWithValue("@dateCreation", DateTime.Today);
                    command.Parameters.AddWithValue("@dateModification", DateTime.Today);
                    command.Parameters.AddWithValue("@refClient", projet.client.reference);
                    command.Parameters.AddWithValue("@refCommercial", projet.commercial.reference);

                    command.ExecuteNonQuery();
                    Trace.WriteLine("#### INSERT NOUVEAU PROJET DATA SUCCESS ####");
                }
                catch (SQLiteException ex)
                {
                    Trace.WriteLine(" \n ################################################# ERREUR INSERTION NOUVEAU PROJET ################################################# \n" + ex.ToString() + "\n");
                }
            }
            conn.LiteCo.Close();
        }

        /// <summary>
        /// Méthode qui permet de compter les plans appartenant à un projet
        /// </summary>
        /// <param name="projet"></param>
        /// <param name="plan"></param>
        public int CountPlansProjet(string refProjet)
        {
            string SQLQuery = "SELECT count(*) FROM plan WHERE refProjet = '" + refProjet + "'";

            // Ouverture de la connexion
            conn.LiteCo.Open();

            int nbPlans = 0;
            using (SQLiteCommand command = new SQLiteCommand(SQLQuery, conn.LiteCo))
            {
                Trace.WriteLine(SQLQuery);
                try
                {
                    // Execute le lecteur de donnée
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        Trace.WriteLine("#### COUNT PLANS PROJET ####");
                        while (reader.Read())
                        {
                            Trace.WriteLine(reader.GetInt32(0));
                            nbPlans = reader.GetInt32(0);
                        }
                    }
                    Trace.WriteLine("#### COUNT PLANS PROJET SUCCESS ####");
                }
                catch (SQLiteException ex)
                {
                    Trace.WriteLine(" \n ################################################# ERREUR RECUPERATION PLANS PROJETS ################################################# \n" + ex.ToString() + "\n");
                }
            }
            conn.LiteCo.Close();
            return nbPlans;
        }
        #endregion
    }
}

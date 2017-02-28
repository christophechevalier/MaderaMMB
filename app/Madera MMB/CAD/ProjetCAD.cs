using Madera_MMB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Madera_MMB.Lib;
using System.Data.SQLite;
using System.Diagnostics;

namespace Madera_MMB.CAD
{
    class ProjetCAD
    {
        #region Properties
        public Connexion conn { get; set; }
        public string SQLQuery { get; set; }
        public Commercial commercial { get; set; }
        public Client client { get; set; }
        public ClientCAD clientCAD { get; set; }
        public CommercialCAD commercialCAD { get; set; }
        public List<Projet> projets { get; set; }
        public List<Client> clients { get; set; }
        #endregion

        #region Ctor
        /// <summary>
        /// Constructeur qui prend en paramètre la connexion et le commercial authentifié
        /// </summary>
        /// <param name="laConnexion"></param>
        /// <param name="com"></param>
        public ProjetCAD(Connexion laConnexion, Commercial com)
        {
            // Instanciations
            conn = laConnexion;
            commercial = com;
            projets = new List<Projet>();
            clients = new List<Client>();

            // Appel des méthodes dans le ctor
            listAllProjects();
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Méthode qui permet de récupérer la liste des projets existant
        /// </summary>
        public void listAllProjects()
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
                                    new Client("CLI001", "Arthur", "Tv", "10 chemin des Albios", "31130", "Balma", "arthur@gmail.com", "06-06-06-06-06", "10-10-2016", "10-10-2016"),
                                    commercial
                                );
                            projets.Add(proj);
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

        // TODO : Faire une méthode pour permettre de créer un nouveau projet en bdd
        //public void insertProjet(Projet projet, string refClient, string refCommercial)
        //{
        //    SQLQuery = "INSERT INTO `projet` (`refProjet`, `nom`, `dateCreation`, `dateModification`, `refClient`, `refCommercial`)" +
        //    "VALUES (" + projet.reference + "," + projet.nom + "," + projet.creation + "," + projet.modification + "," + refClient + "," + refCommercial + ");";
        //    conn.InsertSQliteQuery(SQLQuery);
        //}

        /// <summary>
        /// Méthode qui permet de compter les plans appartenant à un projet
        /// </summary>
        /// <param name="projet"></param>
        /// <param name="plan"></param>
        public int countPlansProjet(String refProjet)
        {
            SQLQuery = "SELECT count(*) FROM plan WHERE refProjet = '" + refProjet + "'";

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
            return nbPlans;
        }
        #endregion
    }
}

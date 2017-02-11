﻿using Madera_MMB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Madera_MMB.Lib;
using System.Data.SQLite;

namespace Madera_MMB.CAD
{
    class ProjetCAD
    {
        #region properties
        public List<Projet> listeProjet { get; set; }
        public Connexion conn { get; set; }
        public string SQLQuery { get; set; }
        public Commercial commercial { get; set; }
        public Client client { get; set; }
        public ClientCAD clientCAD { get; set; }
        public CommercialCAD commercialCAD { get; set; }
        #endregion

        #region Ctor
        public ProjetCAD(Connexion laConnexion, ClientCAD clientCAD, CommercialCAD commercialCAD)
        {
            Connexion conn = laConnexion;
            listeProjet = new List<Projet>();
            this.clientCAD = clientCAD;
            this.commercialCAD = commercialCAD;
        }
        #endregion

        #region privates methods
        private void listAllProjet()
        {
            SQLQuery = "SELECT * FROM Projet WHERE refCommercial AND refClient = " + commercial.reference + client.reference;
            SQLiteCommand command = (SQLiteCommand)conn.LiteCo.CreateCommand();
            command.CommandText = SQLQuery;
            SQLiteDataReader reader = command.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    Projet proj = new Projet
                        (
                            reader.GetString(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetString(3),
                            clientCAD.getClientbyRef(getClient(reader.GetString(4))),
                            commercialCAD.getCommercialbyRef(getCommercial(reader.GetString(5)))
                        );
                    listeProjet.Add(proj);
                }
            }
            finally
            {
                reader.Close();
            }
        }
        private void insertProjet(Projet projet, string refClient, string refCommercial)
        {
            SQLQuery = "INSERT INTO `projet` (`refProjet`, `nom`, `dateCreation`, `dateModification`, `refClient`, `refCommercial`)" +
            "VALUES (" + projet.reference + "," + projet.nom + "," + projet.creation + "," + projet.modification + "," + refClient + "," + refCommercial + ";";
            conn.InsertSQliteQuery(SQLQuery);
        }
        #endregion

        private Client getClient(string reference)
        {
            // TODO : Faire la requête pour sélectionner un client d'un projet

            Client client = new Client();
            return client;
        }

        private Commercial getCommercial(string reference)
        {
            // TODO : Faire la requête pour sélectionner un commercial d'un projet

            Commercial commercial = new Commercial();
            return commercial;
        }
    }
}

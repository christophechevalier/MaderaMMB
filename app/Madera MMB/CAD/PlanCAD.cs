using Madera_MMB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Madera_MMB.Lib;
using System.Data.SQLite;
using System.Diagnostics;
using System.Windows.Media.Imaging;
using System.IO;

namespace Madera_MMB.CAD
{
    class PlanCAD
    {
        #region Properties
        public Connexion conn { get; set; }
        public Projet projet { get; set; }
        public string SQLQuery { get; set; }
        public List<Plan> plans { get; set; }
        public MetaSlotCAD metaslotCAD { get; set; }
        public ComposantCAD compCAD { get; set; }
        #endregion

        #region Ctor
        /// <summary>
        /// Constructeur qui prend en paramètre la connexion et le projet sélectionné
        /// </summary>
        /// <param name="laConnexion"></param>
        /// <param name="unprojet"></param>
        public PlanCAD(Connexion laConnexion, Projet unprojet)
        {
            // Instanciations
            //conn = laConnexion;
            conn = new Connexion();

            projet = unprojet;
            metaslotCAD = new MetaSlotCAD(conn);
            compCAD = new ComposantCAD(conn);
            plans = new List<Plan>();

            // Appel des méthodes dans le ctor
            listAllPlansByProject();
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Méthode qui permet de récupérer la liste des plans par projet
        /// </summary>
        public void listAllPlansByProject()
        {
            // Nom du/des champs mis directement dans la requête pour éviter d'avoir à passer par QSqlRecord 
            //SQLQuery = "SELECT refPlan, label, dateCreation, dateModification, refProjet, typePlancher, typeCouverture, id_coupe, nomGamme FROM plan WHERE refProjet = '" + projet.reference + "';";
            //SQLQuery = "SELECT * FROM `plan` WHERE refProjet = \"" + projet.reference + "\"";

            // TODO : Résoudre l'erreur retourner : SQLite error (1): near "=": syntax error
            SQLQuery = "SELECT * FROM `plan` WHERE refProjet = '" + projet.reference + "';";

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
                        Trace.WriteLine("#### GET PLANS DATA ####");
                        while (reader.Read())
                        {
                            Trace.WriteLine(
                                reader.GetString(0) +
                                reader.GetString(1) +
                                reader.GetString(2) +
                                reader.GetString(3) +
                                projet +
                                getPlancherbyType(reader.GetString(5)) +
                                getCouvbyType(reader.GetString(6)) +
                                getCoupebyId(reader.GetInt32(7)) +
                                getModulesByRefPlan(reader.GetString(8)) +
                                getGammebyNom(reader.GetString(9)));
                            Plan plan = new Plan
                                (
                                    reader.GetString(0),
                                    reader.GetString(1),
                                    reader.GetString(2),
                                    reader.GetString(3),
                                    projet,
                                    getPlancherbyType(reader.GetString(5)),
                                    getCouvbyType(reader.GetString(6)),
                                    getCoupebyId(reader.GetInt32(7)),
                                    getModulesByRefPlan(reader.GetString(8)),
                                    getGammebyNom(reader.GetString(9))
                                );
                            plans.Add(plan);
                        }
                    }
                    Trace.WriteLine("#### GET PLANS DATA SUCCESS ####");
                }
                catch (SQLiteException ex)
                {
                    Trace.WriteLine(" \n ################################################# ERREUR RECUPERATION PLANS ################################################# \n" + ex.ToString() + "\n");
                }
            }
            conn.LiteCo.Close();
        }
        #endregion

        #region Privates methods
        /// <summary>
        /// 
        /// </summary>
        private List<MetaModule> listAllMetaModules()
        {
            SQLQuery = "SELECT * FROM Metamodule";
            SQLiteCommand command = (SQLiteCommand)conn.LiteCo.CreateCommand();
            command.CommandText = SQLQuery;
            List<MetaModule> listMetaModule = new List<MetaModule>();

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                try
                {
                    while (reader.Read())
                    {
                        Byte[] data = (Byte[])reader.GetValue(4);
                        MetaModule metamodule = new MetaModule(
                            reader.GetString(0),
                            reader.GetString(1),
                            reader.GetInt32(2),
                            reader.GetInt32(3),
                            ToImage(data),
                            getGammebyNom(reader.GetString(5)),
                            this.compCAD.listComposantByMetamodule(reader.GetString(0)),
                            this.metaslotCAD.getMetaslotByMetaModule(reader.GetString(0))
                            );
                        listMetaModule.Add(metamodule);
                    }
                }
                catch (SQLiteException ex)
                {
                    Trace.WriteLine(ex.ToString());
                }
                return listMetaModule;
            }
        }

        /// <summary>
        /// Méthode qui permet de récupérer les modules par id/refPlan
        /// </summary>
        /// <param name="refPlan"></param>
        /// <returns></returns>
        private List<Module> getModulesByRefPlan(string refPlan)
        {
            SQLQuery = "SELECT * FROM module WHERE refPlan = " + refPlan;
            SQLiteCommand command = (SQLiteCommand)conn.LiteCo.CreateCommand();
            command.CommandText = SQLQuery;
            SQLiteDataReader reader = command.ExecuteReader();
            List<Module> modules = new List<Module>();
            try
            {
                while (reader.Read())
                {
                    Module module = new Module(
                    reader.GetString(0),
                    reader.GetInt32(1),
                    reader.GetInt32(2),
                    reader.GetInt32(3),
                    reader.GetInt32(4),
                    getMetaModuleByRef(refPlan)
                    );

                    modules.Add(module);
                }
            }
            finally
            {
                reader.Close();
            }
            return modules;
        }

        /// <summary>
        /// Création d'un nouveau plan
        /// </summary>
        /// <param name="plan"></param>
        /// <param name="refClient"></param>
        /// <param name="refCommercial"></param>
        private void insertPlan(Plan plan, string refClient, string refCommercial)
        {
            SQLQuery = "INSERT INTO plan (refPlan, label, dateCreation, dateModification, refProjet, refClient, refCommercial, typeCouverture, id_coupe, typePlancher, nomGamme)" +
            "VALUES (" + plan.reference + "," + plan.label + "," + plan.creation + "," + plan.modification + "," + plan.projet.reference + "," + refClient + "," + refCommercial + "," + plan.couverture.type + "," + plan.coupePrincipe.id + "," + plan.plancher.type + "," + plan.gamme.nom + ";";
            conn.InsertSQliteQuery(SQLQuery);
            foreach (Module module in plan.modules)
            {
                insertModule(module, plan.reference);
            }
        }

        /// <summary>
        /// Création d'un nouveau module
        /// </summary>
        /// <param name="module"></param>
        /// <param name="refplan"></param>
        private void insertModule(Module module, string refplan)
        {
            SQLQuery = "INSERT INTO module (nom, prixHT, nbSlot, coordonneeDebutX , coordonneeDebutY, coordonneeFinX, coordonneeFinY, refMetaModule, refPlan)" +
            "VALUES (" + module.nom + "," + module.getPrixHT() + "," + module.getNbSlot() + "," + module.debutPositionX + "," + module.debutPositionY + "," + module.finPositionX + "," + module.finPositionY + "," + module.getRefMetaModule() + "," + refplan + ";";
            conn.InsertSQliteQuery(SQLQuery);
        }

        /// <summary>
        /// Méthode qui permet de récupérer les coupes de principes par id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private CoupePrincipe getCoupebyId(int id)
        {
            SQLQuery = "SELECT * FROM Coupeprincipe WHERE id_coupe = " + id;
            SQLiteCommand command = (SQLiteCommand)conn.LiteCo.CreateCommand();
            command.CommandText = SQLQuery;
            CoupePrincipe coupe = new CoupePrincipe();

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                try
                {
                    while (reader.Read())
                    {
                        Byte[] data = (Byte[])reader.GetValue(5);
                        coupe = new CoupePrincipe(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetInt32(4), ToImage(data));
                    }
                }
                catch (SQLiteException ex)
                {
                    Trace.WriteLine(ex.ToString());
                }
                return coupe;
            }
        }

        /// <summary>
        /// Méthode qui permet de récupérer les métamodules par id/ref
        /// </summary>
        /// <param name="reference"></param>
        /// <returns></returns>
        private MetaModule getMetaModuleByRef(string reference)
        {
            SQLQuery = "SELECT * FROM Metamodule WHERE refMetaModule = " + reference;
            SQLiteCommand command = (SQLiteCommand)conn.LiteCo.CreateCommand();
            command.CommandText = SQLQuery;
            MetaModule metaModule = new MetaModule();

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                try
                {
                    while (reader.Read())
                    {
                        Byte[] data = (Byte[])reader.GetValue(4);
                        metaModule = new MetaModule(
                        reader.GetString(0),
                        reader.GetString(1),
                        reader.GetInt32(2),
                        reader.GetInt32(3),
                        ToImage(data),
                        getGammebyNom(reader.GetString(5)),
                        this.compCAD.listComposantByMetamodule(reader.GetString(0)),
                        this.metaslotCAD.getMetaslotByMetaModule(reader.GetString(0)));
                        reader.Close();
                    }
                }
                catch (SQLiteException ex)
                {
                    Trace.WriteLine(ex.ToString());
                }
                return metaModule;
            }
        }

        /// <summary>
        /// Méthode qui permet de récupérer les couvertures par id/type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private Couverture getCouvbyType(string type)
        {
            SQLQuery = "SELECT * FROM Couverture WHERE typeCouverture = " + type;
            SQLiteCommand command = (SQLiteCommand)conn.LiteCo.CreateCommand();
            command.CommandText = SQLQuery;
            Couverture couv = new Couverture();

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                try
                {
                    while (reader.Read())
                    {
                        Byte[] data = (Byte[])reader.GetValue(2);
                        couv = new Couverture(reader.GetString(0), reader.GetInt32(1), ToImage(data));
                    }
                }
                catch (SQLiteException ex)
                {
                    Trace.WriteLine(ex.ToString());
                }
                return couv;
            }
        }

        /// <summary>
        /// Méthode qui permet de récupérer une gamme par id/nom
        /// </summary>
        /// <param name="type">nom de la gamme recherchée</param>
        /// <returns></returns>
        private Gamme getGammebyNom(string nom)
        {
            SQLQuery = "SELECT * FROM Gamme WHERE nomGamme = " + nom;
            SQLiteCommand command = (SQLiteCommand)conn.LiteCo.CreateCommand();
            command.CommandText = SQLQuery;
            Gamme gamme = new Gamme();

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                try
                {
                    while (reader.Read())
                    {
                        Byte[] data = (Byte[])reader.GetValue(5);
                        gamme = new Gamme(reader.GetString(0), reader.GetInt32(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), ToImage(data));
                    }

                }
                catch (SQLiteException ex)
                {
                    Trace.WriteLine(ex.ToString());
                }
                return gamme;
            }
        }

        /// <summary>
        /// Méthode qui permet de récupérer les planchers par id/type
        /// </summary>
        /// <param name="type">type du plancher recherché</param>
        /// <returns></returns>
        private Plancher getPlancherbyType(string type)
        {
            SQLQuery = "SELECT * FROM Plancher WHERE typePlancher = " + type;
            SQLiteCommand command = (SQLiteCommand)conn.LiteCo.CreateCommand();
            command.CommandText = SQLQuery;
            Plancher plancher = new Plancher();

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                try
                {
                    while (reader.Read())
                    {
                        Byte[] data = (Byte[])reader.GetValue(2);
                        plancher = new Plancher(reader.GetString(0), reader.GetInt32(1), ToImage(data));
                    }
                }
                catch (SQLiteException ex)
                {
                    Trace.WriteLine(ex.ToString());
                }
                return plancher;
            }
        }
        #endregion

        #region Tools
        /// <summary>
        /// Méthode de conversion de type byte array en BitmapImage
        /// </summary>
        /// <param name="array">tableau d'octets de l'image</param>
        /// <returns></returns>
        private BitmapImage ToImage(byte[] array)
        {
            using (var ms = new System.IO.MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static byte[] GetBytes(SQLiteDataReader reader)
        {
            const int CHUNK_SIZE = 2 * 1024;
            byte[] buffer = new byte[CHUNK_SIZE];
            long bytesRead;
            long fieldOffset = 0;
            using (MemoryStream stream = new MemoryStream())
            {
                while ((bytesRead = reader.GetBytes(0, fieldOffset, buffer, 0, buffer.Length)) > 0)
                {
                    stream.Write(buffer, 0, (int)bytesRead);
                    fieldOffset += bytesRead;
                }
                return stream.ToArray();
            }
        }
        #endregion
    }
}
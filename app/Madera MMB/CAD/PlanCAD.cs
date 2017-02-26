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
            //SQLQuery = "SELECT * FROM plan WHERE refProjet = '" + projet.reference + "';";

            //SQLQuery = "SELECT * FROM `plan` WHERE refProjet = \"" + projet.reference + "\"";


            // Ouverture de la connexion
            conn.LiteCo.Open();
            // CAST(id_coupe AS VARCHAR(255))
            SQLQuery = "SELECT refPlan, label, dateCreation, dateModification, refProjet, typePlancher, typeCouverture, id_coupe, nomGamme FROM plan WHERE refProjet = @refProjet;";

            //SQLQuery = "SELECT * FROM plan WHERE refProjet = @refProjet ;";
            using (SQLiteCommand command = new SQLiteCommand(SQLQuery, conn.LiteCo))
            {
                command.Parameters.AddWithValue("@refProjet", projet.reference);
                Trace.WriteLine(SQLQuery);
                try
                {
                    // Execute le lecteur de donnée

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        Trace.WriteLine("#### GET PLANS DATA ####");
                        while (reader.Read())
                        {
                            //Trace.WriteLine(
                            //    reader.GetString(0) +
                            //    reader.GetString(1) +
                            //    reader.GetString(2) +
                            //    reader.GetString(3) +
                            //    projet +
                            //    getPlancherbyType(reader.GetString(5)) +
                            //    getCouvbyType(reader.GetString(6)) +
                            //    getCoupebyId(reader.GetInt32(7)) +
                            //    getModulesByRefPlan(reader.GetString(8)) +
                            //    getGammebyNom(reader.GetString(9)));

                            Trace.WriteLine(
                                reader.GetString(0).GetType() + " || " +
                                reader.GetString(1).GetType() + " || " +
                                reader.GetString(2).GetType() + " || " +
                                reader.GetString(3).GetType() + " || " +
                                reader.GetValue(4).ToString().GetType() + " || " +
                                reader.GetValue(5).ToString().GetType() + " || " +
                                reader.GetValue(6).ToString().GetType() + " || " +
                                reader.GetValue(7).ToString().GetType() + " || " +
                                reader.GetValue(8).ToString().GetType());
                                //reader.GetValue(9).ToString());
                            Plan plan = new Plan
                                (
                                    reader.GetString(0),
                                    reader.GetString(1),
                                    reader.GetString(2),
                                    reader.GetString(3),
                                    projet,
                                    getPlancherbyType(reader.GetString(8)),
                                    getCouvbyType(reader.GetString(6)),
                                    getCoupebyId(reader.GetInt32(7)),
                                    getModulesByRefPlan(reader.GetString(0)),
                                    getGammebyNom(reader.GetString(9))
                                );

                            plans.Add(plan);
                        }
                    }
                    Trace.WriteLine("#### GET PLANS DATA SUCCESS ####");
                }
                catch (SQLiteException ex)
                {
                    Trace.WriteLine(SQLQuery);
                    Trace.WriteLine(" \n ################################################# ERREUR RECUPERATION PLANS ################################################# \n" + ex.ToString() + "\n");
                }
            }
            conn.LiteCo.Close();
        }
        #endregion

        #region Privates methods
        /// <summary>
        /// Méthode qui permet de récupérer les metamodules
        /// </summary>
        private List<MetaModule> listAllMetaModules()
        {
            List<MetaModule> listMetaModule = new List<MetaModule>();
            SQLQuery = "SELECT * FROM metamodule";
            using (SQLiteCommand command = new SQLiteCommand(SQLQuery, conn.LiteCo))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    try
                    {
                        while (reader.Read())
                        {
                            Byte[] data = (Byte[])reader.GetValue(4);
                            MetaModule metamodule = new MetaModule
                            (
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
                        Trace.WriteLine(SQLQuery);
                        Trace.WriteLine(ex.ToString());
                    }
                    return listMetaModule;
                }
            }
        }

        /// <summary>
        /// Méthode qui permet de récupérer les modules par id/refPlan
        /// </summary>
        /// <param name="refPlan"></param>
        /// <returns></returns>
        private List<Module> getModulesByRefPlan(string refPlan)
        {
            List<Module> modules = new List<Module>();
            SQLQuery = "SELECT * FROM module WHERE refPlan = @reference;";
            using (SQLiteCommand command = new SQLiteCommand(SQLQuery, conn.LiteCo))
            {
                command.Parameters.AddWithValue("@reference", refPlan);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    try
                    {
                        while (reader.Read())
                        {
                            Module module = new Module
                            (
                                reader.GetString(0),
                                //Convert.ToInt32(reader.GetValue(1)),
                                //Convert.ToInt32(reader.GetValue(2)),
                                //Convert.ToInt32(reader.GetValue(3)),
                                //Convert.ToInt32(reader.GetValue(4)),
                                reader.GetInt32(1),
                                reader.GetInt32(2),
                                reader.GetInt32(3),
                                reader.GetInt32(4),
                                getMetaModuleByRef(refPlan)
                            );
                            modules.Add(module);
                        }
                    }
                    catch (SQLiteException ex)
                    {
                        Trace.WriteLine(SQLQuery);
                        Trace.WriteLine(ex.ToString());
                    }
                    return modules;
                }
            }
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
        /// Méthode qui permet de récupérer les métamodules par id/ref
        /// </summary>
        /// <param name="reference"></param>
        /// <returns></returns>
        private MetaModule getMetaModuleByRef(string reference)
        {
            MetaModule metaModule = new MetaModule();
            SQLQuery = "SELECT * FROM metamodule WHERE refMetaModule = @reference;";
            using (SQLiteCommand command = new SQLiteCommand(SQLQuery, conn.LiteCo))
            {
                command.Parameters.AddWithValue("@reference", reference);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    try
                    {
                        while (reader.Read())
                        {
                            Byte[] data = (Byte[])reader.GetValue(4);
                            metaModule = new MetaModule
                            (
                                reader.GetString(0),
                                reader.GetString(1),
                                //Convert.ToInt32(reader.GetValue(2)),
                                //Convert.ToInt32(reader.GetValue(3)),
                                reader.GetInt32(2),
                                reader.GetInt32(3),
                                ToImage(data),
                                getGammebyNom(reader.GetString(5)),
                                this.compCAD.listComposantByMetamodule(reader.GetString(0)),
                                this.metaslotCAD.getMetaslotByMetaModule(reader.GetString(0))
                            );
                        }
                    }
                    catch (SQLiteException ex)
                    {
                        Trace.WriteLine(SQLQuery);
                        Trace.WriteLine(ex.ToString());
                    }
                    return metaModule;
                }
            }
        }

        /// <summary>
        /// Méthode qui permet de récupérer les coupes de principes par id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private CoupePrincipe getCoupebyId(int id)
        {
            CoupePrincipe coupe = new CoupePrincipe();
            SQLQuery = "SELECT * FROM coupeprincipe WHERE id_coupe = @id;";
            using (SQLiteCommand command = new SQLiteCommand(SQLQuery, conn.LiteCo))
            {
                command.Parameters.AddWithValue("@id", id);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    try
                    {
                        while (reader.Read())
                        {
                            Byte[] data = (Byte[])reader.GetValue(5);
                            coupe = new CoupePrincipe
                            (
                                //Convert.ToInt32(reader.GetValue(0)),
                                reader.GetInt32(0),
                                reader.GetString(1),
                                //Convert.ToInt32(reader.GetValue(2)),
                                //Convert.ToInt32(reader.GetValue(3)),
                                //Convert.ToInt32(reader.GetValue(4)),
                                reader.GetInt32(2),
                                reader.GetInt32(3),
                                reader.GetInt32(4),
                                ToImage(data)
                            );
                        }
                    }
                    catch (SQLiteException ex)
                    {
                        Trace.WriteLine(SQLQuery);
                        Trace.WriteLine(ex.ToString());
                    }
                    return coupe;
                }
            }
        }

        /// <summary>
        /// Méthode qui permet de récupérer les couvertures par id/type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private Couverture getCouvbyType(string type)
        {
            Couverture couv = new Couverture();
            SQLQuery = "SELECT * FROM couverture WHERE typeCouverture = @type;";
            using (SQLiteCommand command = new SQLiteCommand(SQLQuery, conn.LiteCo))
            {
                command.Parameters.AddWithValue("@type", type);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    try
                    {
                        while (reader.Read())
                        {
                            Byte[] data = (Byte[])reader.GetValue(2);
                            couv = new Couverture
                            (
                                reader.GetString(0),
                                //Convert.ToInt32(reader.GetValue(1)),
                                reader.GetInt32(1),
                                ToImage(data)
                            );
                        }
                    }
                    catch (SQLiteException ex)
                    {
                        Trace.WriteLine(SQLQuery);
                        Trace.WriteLine(ex.ToString());
                    }
                    return couv;
                }
            }
        }

        /// <summary>
        /// Méthode qui permet de récupérer une gamme par id/nom
        /// </summary>
        /// <param name="type">nom de la gamme recherchée</param>
        /// <returns></returns>
        private Gamme getGammebyNom(string nom)
        {
            Gamme gamme = new Gamme();
            SQLQuery = "SELECT * FROM gamme WHERE nomGamme = @nom;";
            using (SQLiteCommand command = new SQLiteCommand(SQLQuery, conn.LiteCo))
            {
                command.Parameters.AddWithValue("@nom", nom);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    try
                    {
                        while (reader.Read())
                        {
                            Byte[] data = (Byte[])reader.GetValue(5);
                            gamme = new Gamme
                            (
                                reader.GetString(0),
                                //Convert.ToInt32(reader.GetValue(1)),
                                reader.GetInt32(1),
                                reader.GetString(2),
                                reader.GetString(3),
                                reader.GetString(4),
                                ToImage(data)
                            );
                        }
                    }
                    catch (SQLiteException ex)
                    {
                        Trace.WriteLine(SQLQuery);
                        Trace.WriteLine(ex.ToString());
                    }
                    return gamme;
                }
            }
        }

        /// <summary>
        /// Méthode qui permet de récupérer les planchers par id/type
        /// </summary>
        /// <param name="type">type du plancher recherché</param>
        /// <returns></returns>
        private Plancher getPlancherbyType(string type)
        {
            Plancher plancher = new Plancher();
            SQLQuery = "SELECT * FROM plancher WHERE typePlancher = @type;";
            using (SQLiteCommand command = new SQLiteCommand(SQLQuery, conn.LiteCo))
            {
                command.Parameters.AddWithValue("@type", type);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    try
                    {
                        while (reader.Read())
                        {
                            Byte[] data = (Byte[])reader.GetValue(2);
                            plancher = new Plancher
                            (
                                reader.GetString(0),
                                //Convert.ToInt32(reader.GetValue(1)),
                                reader.GetInt32(1),
                                ToImage(data)
                            );
                        }
                    }
                    catch (SQLiteException ex)
                    {
                        Trace.WriteLine(SQLQuery);
                        Trace.WriteLine(ex.ToString());
                    }
                    return plancher;
                }
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
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
using System.Collections.ObjectModel;
using System.ComponentModel;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Madera_MMB.Lib.Tools;

namespace Madera_MMB.CAD
{
    public class PlanCAD : INotifyPropertyChanged
    {
        #region Properties
        public Connexion conn { get; set; }
        public Projet projet { get; set; }
        public string SQLQuery { get; set; }
        public MetaSlotCAD metaslotCAD { get; set; }
        private List<MetaModule> _ListMetaModule { get; set; }
        public List<MetaModule> ListMetaModule { get { return _ListMetaModule; } set { _ListMetaModule = value; RaisePropertyChanged("ListMetaModule"); } }
        private ObservableCollection<Plan> _plans;
        public ObservableCollection<Plan> Plans
        {
            get
            {
                return this._plans;
            }
            set { _plans = value; }
        }
        #endregion

        #region Events
        private void RaisePropertyChanged(String property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        private void Plans_CollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("Plans"));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Ctor
        /// <summary>
        /// Constructeur qui prend en paramètre la connexion et le projet sélectionné
        /// </summary>
        /// <param name="laConnexion"></param>
        /// <param name="unprojet"></param>
        public PlanCAD(Connexion laConnexion, Projet unprojet)
        {
            conn = laConnexion;
            projet = unprojet;
            Plans = new ObservableCollection<Plan>();
            _plans.CollectionChanged += Plans_CollectionChanged;
            metaslotCAD = new MetaSlotCAD(conn);
            ListMetaModule = new List<MetaModule>();

            if (laConnexion.MySQLconnected)
                laConnexion.SynCPlansProj(projet);

            ListAllPlansByProject();
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Méthode qui permet de récupérer la liste des plans par projet
        /// </summary>
        public void ListAllPlansByProject()
        {
            Plans.Clear();
            // Ouverture de la connexion
            conn.LiteCo.Open();
            // Nom du/des champs mis directement dans la requête pour éviter d'avoir à passer par QSqlRecord 
            SQLQuery = "SELECT refPlan, label, dateCreation, dateModification, refProjet, typePlancher, typeCouverture, idCoupe, nomGamme FROM plan WHERE refProjet = @refProjet;";
          
            //SQLQuery = "SELECT * FROM plan WHERE refProjet = @refProjet ;";
            using (SQLiteCommand command = new SQLiteCommand(SQLQuery, conn.LiteCo))
            {
                command.Parameters.AddWithValue("@refProjet", projet.reference);
                try
                {
                    // Execute le lecteur de donnée

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        Trace.WriteLine("#### GET PLANS DATA ####");
                        while (reader.Read())
                        {
                            Plan plan = new Plan
                                (
                                    reader.GetString(0),
                                    reader.GetString(1),
                                    reader.GetString(2),
                                    reader.GetString(3),
                                    projet,
                                    getPlancherByType(reader.GetString(5)),
                                    getCouvByType(reader.GetString(6)),
                                    getCoupeById(reader.GetInt16(7)),
                                    getModulesByRefPlan(reader.GetString(0))
                                );
                            if (reader.GetString(8) != "none")
                            {
                                plan.gamme = getGammebyNom(reader.GetString(8));
                            }
                            Plans.Add(plan);
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

        /// <summary>
        /// Création d'un nouveau plan
        /// </summary>
        /// <param name="plan"></param>
        public void InsertPlan(Plan plan)
        {
            if(plan.gamme != null)
            {
                string SQLQuery = "REPLACE INTO plan(refPlan, label, dateCreation, dateModification, refProjet, typePlancher, typeCouverture, idCoupe, nomGamme)" +
"VALUES (@refPlan, @label, @dateCreation, @dateModification, @refProjet, @typePlancher, @typeCouverture, @idCoupe, @nomGamme)";

                // Ouverture de la connexion
                conn.LiteCo.Open();
                using (SQLiteCommand command = new SQLiteCommand(SQLQuery, conn.LiteCo))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@refPlan", plan.reference);
                        command.Parameters.AddWithValue("@label", plan.label);
                        command.Parameters.AddWithValue("@dateCreation", DateTime.Now.ToString());
                        command.Parameters.AddWithValue("@dateModification", DateTime.Today.ToString());
                        command.Parameters.AddWithValue("@refProjet", plan.projet.reference);
                        command.Parameters.AddWithValue("@typePlancher", plan.plancher.type);
                        command.Parameters.AddWithValue("@typeCouverture", plan.couverture.type);
                        command.Parameters.AddWithValue("@idCoupe", plan.coupePrincipe.id);
                        command.Parameters.AddWithValue("@nomGamme", plan.gamme.nom);

                        command.ExecuteNonQuery();
                        Trace.WriteLine("#### INSERT NOUVEAU PLAN DATA SUCCESS ####");
                    }
                    catch (SQLiteException ex)
                    {
                        Trace.WriteLine(" \n ################################################# ERREUR INSERTION NOUVEAU PLAN ################################################# \n" + ex.ToString() + "\n");
                    }
                }
                conn.LiteCo.Close();
            }
            else
            {
                string SQLQuery = "REPLACE INTO plan(refPlan, label, dateCreation, dateModification, refProjet, typePlancher, typeCouverture, idCoupe, nomGamme)" +
"VALUES (@refPlan, @label, @dateCreation, @dateModification, @refProjet, @typePlancher, @typeCouverture, @idCoupe, @nomGamme)";

                // Ouverture de la connexion
                conn.LiteCo.Open();
                using (SQLiteCommand command = new SQLiteCommand(SQLQuery, conn.LiteCo))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@refPlan", plan.reference);
                        command.Parameters.AddWithValue("@label", plan.label);
                        command.Parameters.AddWithValue("@dateCreation", DateTime.Today.ToString());
                        command.Parameters.AddWithValue("@dateModification", DateTime.Today.ToString());
                        command.Parameters.AddWithValue("@refProjet", plan.projet.reference);
                        command.Parameters.AddWithValue("@typePlancher", plan.plancher.type);
                        command.Parameters.AddWithValue("@typeCouverture", plan.couverture.type);
                        command.Parameters.AddWithValue("@idCoupe", plan.coupePrincipe.id);
                        command.Parameters.AddWithValue("@nomGamme", "none");

                        command.ExecuteNonQuery();
                        Trace.WriteLine("#### INSERT NOUVEAU PLAN DATA SUCCESS ####");
                    }
                    catch (SQLiteException ex)
                    {
                        Trace.WriteLine(" \n ################################################# ERREUR INSERTION NOUVEAU PLAN ################################################# \n" + ex.ToString() + "\n");
                    }
                }
                conn.LiteCo.Close();
            }
            ListAllPlansByProject();

            //conn.InsertSQliteQuery(SQLQuery);
            //foreach (Module module in plan.modules)
            //{
            //    insertModule(module, plan.reference);
            //}
        }

        /// <summary>
        /// Méthode qui permet de récupérer les metamodules
        /// </summary>
        public List<MetaModule> listAllMetaModules()
        {
            conn.LiteCo.Open();
            ListMetaModule = new List<MetaModule>();
            SQLQuery = "SELECT * FROM metamodule WHERE statut = 0";
            using (SQLiteCommand command = new SQLiteCommand(SQLQuery, conn.LiteCo))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    try
                    {
                        while (reader.Read())
                        {                       
                            Byte[] data = (Byte[])reader.GetValue(3);
                            MetaModule metaModule = new MetaModule
                            (
                                reader.GetString(0),
                                reader.GetString(1),
                                reader.GetInt32(2),
                                ToImage(data),
                                reader.GetBoolean(4),
                                reader.GetString(5),
                                getGammebyNom(reader.GetString(6)),
                                reader.GetInt32(7),
                                reader.GetInt32(8)

                            );
                            ListMetaModule.Add(metaModule);
                        }
                    }
                    catch (SQLiteException ex)
                    {
                        Trace.WriteLine(ex.ToString());
                    }
                    conn.LiteCo.Close();
                    return ListMetaModule;
                }
            }
        }

        /// <summary>
        /// Méthode qui permet de récupérer les metaSlot d'un metaModule
        /// </summary>
        public List<MetaSlot> listMetaSlot(string refe)
        {
            List<MetaSlot> listMetaS = new List<MetaSlot>();

            conn.LiteCo.Open();
            SQLQuery = "SELECT * FROM metaslot WHERE refMetaModule = '" + refe + "';";
            using (SQLiteCommand command = new SQLiteCommand(SQLQuery, conn.LiteCo))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    try
                    {
                        while (reader.Read())
                        {
                            MetaSlot metaS = new MetaSlot
                            (
                                reader.GetInt32(0),
                                reader.GetInt32(1),
                                reader.GetString(2),
                                reader.GetString(3)
                            );
                            listMetaS.Add(metaS);
                        }
                    }
                    catch (SQLiteException ex)
                    {
                        Trace.WriteLine(ex.ToString());
                    }
                    conn.LiteCo.Close();
                    return listMetaS;
                }
            }
        }

        /// <summary>
        /// Méthode qui permet de récupérer les gammes de metamodules
        /// </summary>
        public List<String> listAllGammes(string type)
        {
            conn.LiteCo.Open();
            List<String> listGammes = new List<String>();
            if (type != "0")
            {
                SQLQuery = "SELECT distinct(nomGamme) FROM metamodule WHERE statut = 0 AND label LIKE \"%" + type + "%\"";
            }
            else
            {
                SQLQuery = "SELECT distinct(nomGamme) FROM metamodule WHERE statut = 0";
            }
            using (SQLiteCommand command = new SQLiteCommand(SQLQuery, conn.LiteCo))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    try
                    {
                        while (reader.Read())
                        {
                            listGammes.Add(reader.GetString(0));
                        }
                    }
                    catch (SQLiteException ex)
                    {
                        Trace.WriteLine(ex.ToString());
                    }
                    conn.LiteCo.Close();
                    return listGammes;
                }
            }
        }

        /// <summary>
        /// Création d'un nouveau module
        /// </summary>
        /// <param name="module"></param>
        /// <param name="refPlan"></param>
        public void insertModule(Module module, string refPlan)
        {
            if (module.parent != null)
            {
                SQLQuery = "REPLACE INTO module (coordonneeDebutX , coordonneeDebutY, colspan, rowspan, refMetaModule, refMetaparent, refPlan)" +
                "VALUES (" + module.x + "," + module.y + "," + module.colspan + "," + module.rowspan + ",'" + module.meta.reference + "','" + module.parent.reference + "','" + refPlan + "');";
            }
            else
            {
                SQLQuery = "REPLACE INTO module (coordonneeDebutX, coordonneeDebutY, colspan, rowspan, refMetaModule, refMetaparent, refPlan)" +
                "VALUES (" + module.x + "," + module.y + "," + module.colspan + "," + module.rowspan + ",'" + module.meta.reference + "','none','" + refPlan + "');";
            }
            conn.InsertSQliteQuery(SQLQuery);
        }

        #endregion

        #region Privates methods

        /// <summary>
        /// Méthode qui permet de récupérer les modules appartenant à un plan
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
                                  reader.GetInt32(0),
                                  reader.GetInt32(1),
                                  reader.GetInt32(2),
                                  reader.GetInt32(3),
                                  getMetaModuleByRef(reader.GetString(4))
                            );
                            if (reader.GetString(5) != "none")
                            {
                                module.parent = getMetaModuleByRef(reader.GetString(5));
                            }
                            modules.Add(module);
                        }
                    }
                    catch (SQLiteException ex)
                    {
                        Trace.WriteLine(ex.ToString());
                    }
                    return modules;
                }
            }
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
                            Byte[] data = (Byte[])reader.GetValue(3);
                            metaModule = new MetaModule
                            (
                                reader.GetString(0),
                                reader.GetString(1),
                                reader.GetInt32(2),
                                ToImage(data),
                                reader.GetBoolean(4),
                                reader.GetString(5),
                                getGammebyNom(reader.GetString(6)),
                                reader.GetInt32(7),
                                reader.GetInt32(8)
                            );
                        }
                    }
                    catch (SQLiteException ex)
                    {
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
        private CoupePrincipe getCoupeById(int id)
        {
            CoupePrincipe coupe = new CoupePrincipe();
            SQLQuery = "SELECT * FROM coupeprincipe WHERE idCoupe = @id;";
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
                                reader.GetInt16(0),
                                reader.GetString(1),
                                reader.GetInt32(2),
                                reader.GetInt32(3),
                                reader.GetInt32(4),
                                reader.GetBoolean(6),
                                ToImage(data)
                            );
                        }
                    }
                    catch (SQLiteException ex)
                    {
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
        private Couverture getCouvByType(string type)
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
                                reader.GetInt32(1),
                                reader.GetBoolean(3),
                                ToImage(data)
                            );
                        }
                    }
                    catch (SQLiteException ex)
                    {
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
                                reader.GetInt32(1),
                                reader.GetString(2),
                                reader.GetString(3),
                                reader.GetString(4),
                                reader.GetBoolean(6),
                                ToImage(data));
                        }
                    }
                    catch (SQLiteException ex)
                    {
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
        private Plancher getPlancherByType(string type)
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
                                reader.GetInt32(1),
                                reader.GetBoolean(3),
                                ToImage(data)
                            );
                        }
                    }
                    catch (SQLiteException ex)
                    {
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
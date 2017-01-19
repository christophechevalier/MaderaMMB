using Madera_MMB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Madera_MMB.Lib;
using System.Data.SQLite;

namespace Madera_MMB.CAD
{
    class PlanCAD
    {
        #region properties
        public List<Plan> listePlanParProjet { get; set; }
        public Connexion conn { get; set; }
        public Projet projet { get; set; }
        public string SQLQuery { get; set; }
        public CouvertureCAD couvCAD { get; set; }
        public CoupePrincipeCAD coupeCAD { get; set; }
        public PlancherCAD plancherCAD { get; set; }
        public GammeCAD gammeCAD { get; set; }
        public MetamoduleCAD metamodCAD { get; set; }

        #endregion

        #region Ctor
        public PlanCAD(Connexion laConnexion, Projet unprojet, CouvertureCAD couvCAD, CoupePrincipeCAD coupeCAD, PlancherCAD plancherCAD, GammeCAD gammeCAD, MetamoduleCAD metaCAD)
        {
            this.projet = unprojet;
            Connexion conn = laConnexion;
            listePlanParProjet = new List<Plan>();
            this.couvCAD = couvCAD;
            this.coupeCAD = coupeCAD;
            this.plancherCAD = plancherCAD;
            this.gammeCAD = gammeCAD;
            this.metamodCAD = metaCAD;

        }
        #endregion

        #region privates methods
        private void listPlanParProjet()
        {
            SQLQuery = "SELECT * FROM Plan WHERE refProjet = " + projet.reference;
            SQLiteCommand command = (SQLiteCommand)conn.LiteCo.CreateCommand();
            command.CommandText = SQLQuery;
            SQLiteDataReader reader = command.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    Plan plan = new Plan(
                        reader.GetString(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3),
                        this.projet,
                        plancherCAD.getPlancherbyType(reader.GetString(9)),
                        couvCAD.getCouvbyType(reader.GetString(7)),
                        coupeCAD.getCoupebyId(8),
                        gammeCAD.getGammebyNom(reader.GetString(10)),
                        getModulesByRefPlan(reader.GetString(0))
                        );

                    listePlanParProjet.Add(plan);
                }
            }
            finally
            {
                reader.Close();
            }
        }
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
                    this.metamodCAD.getMetaModuleByRef(refPlan)
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
        private void insertModule(Module module, string refplan)
        {
            //SQLQuery = "INSERT INTO module (nom, prixHT, nbSlot, coordonneeDebutX , coordonneeDebutY, coordonneeFinX, coordonneeFinY, refMetaModule, refPlan)" +
            //"VALUES (" + module.nom + "," + module.getPrixHT + "," + module.getNbSlot + "," + module.debutPositionX + "," + module.debutPositionY + "," + module.finPositionX + "," + module.finPositionY + "," + module.getRefMetaModule + "," + refplan + ";";
            //conn.InsertSQliteQuery(SQLQuery);
        }
        #endregion
    }
}
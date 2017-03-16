using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Madera_MMB.Model
{
    public class Module
    {
        #region properties
        public int id { get; set; }
        public int debutPositionX { get;set; }
        public int debutPositionY { get; set; }
        public int finPositionY { get; set; }
        public int finPositionX { get; set; }
        public int nbSlot { get; set; }
        public MetaModule metaModule { get; set; }
        public List<Slot> slotsContenus { get; set; }
        public Slot slotParent { get; set; }
        #endregion

        #region Ctor
        public Module(MetaModule meta)
        {
            this.metaModule = meta;
            foreach(MetaSlot a in meta.metaslots)
            {
                Slot slot = new Slot(a);
                this.slotsContenus.Add(slot);
            }
        }
        public Module(int id, int posXD, int posYD, int posXF, int posYF, MetaModule metaModule) 
        {
            this.id = id;
            this.debutPositionX = posXD;
            this.debutPositionY = posYD;
            this.finPositionY = posYF;
            this.finPositionX = posXF;
            this.metaModule = metaModule;  
        }
        #endregion

        #region public methods
        public int getPrixHT()
        {
            return this.metaModule.prixHT;
        }
        public int getNbSlot()
        {
            return this.metaModule.nbSlot;
        }
        public string getRefMetaModule()
        {
            return this.metaModule.reference;
        }
        #endregion
    }
}

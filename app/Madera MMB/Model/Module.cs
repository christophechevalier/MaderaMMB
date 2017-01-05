using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Madera_MMB.Model
{
    class Module
    {
        #region properties
        public int positionY { get; set; }
        public int positionX { get; set; }
        public MetaModule metaModule { get; set; }
        public List<Slot> slotsContenus { get; set; }
        public Slot slotParent { get; set; }
        #endregion

        #region Ctor
        Module(MetaModule meta)
        {
            this.metaModule = meta;
            foreach(MetaSlot a in meta.metaslots)
            {
                Slot slot = new Slot(a);
                this.slotsContenus.Add(slot);
            }
        }
        public Module() { }
        #endregion

        #region privates methods

        #endregion

        #region public methods
        public int getPrixHT()
        {
            return this.metaModule.prixHT ;
        }
        public int getNbSlot()
        {
            return this.metaModule.nbSlot;
        }
        public string getLabel()
        {
            return this.metaModule.label;
        }
        #endregion
    }
}

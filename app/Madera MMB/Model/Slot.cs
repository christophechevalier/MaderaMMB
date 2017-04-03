using Madera_MMB.Lib.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Madera_MMB.Model
{
    public class Slot
    {
        #region properties
        private int numSlotPosition { get; set; }
        private MetaSlot metaSlot { get; set; }
        private Module moduleEnfant { get; set; }
        #endregion

        #region Ctor

        public Slot(MetaSlot metaslot)
        {
            this.metaSlot = metaslot;
            this.numSlotPosition = this.metaSlot.numMetaSlot;
        }
        // Slot instancié avec un module enfant
        public Slot(MetaSlot metaslot, Module enfant)
        {
            this.metaSlot = metaslot;
            this.moduleEnfant = enfant;
        }
        #endregion

        #region public methods
        public string getLabel()
        {
            return this.metaSlot.label;
        }
        #endregion
    }
}
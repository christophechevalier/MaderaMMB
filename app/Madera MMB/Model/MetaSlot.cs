using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Madera_MMB.Model
{
    public class MetaSlot
    {
        #region properties
        public int id { get; set; }
        public string label { get; set; }
        public int numMetaSlot { get; set; }
        public string refMetaModule { get; set; }
        #endregion

        #region Ctor
        public MetaSlot(int id, string label, int nb, string refMetamodule)
        {
            this.id = id;
            this.label = label;
            this.numMetaSlot = nb;
            this.refMetaModule = refMetamodule;
        }
        #endregion
    }
}
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
        public int posMetaSlot { get; set; }
        public string type { get; set; }
        public string refMetaModule { get; set; }
        #endregion

        #region Ctor
        public MetaSlot(int id, int pos, string type, string refMetamodule)
        {
            this.id = id;
            this.posMetaSlot = pos;
            this.type = type;
            this.refMetaModule = refMetamodule;
        }
        #endregion
    }
}
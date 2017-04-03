using Madera_MMB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Madera_MMB.Lib.Tools
{
    public class Module : Button
    {
        public enum type
        {
            Mur, MurInt, Fenetre, Porte, Rien, SlotMur, SlotFen, SlotPorte, Slot
        }

        public type letype { get; set; }
        public MetaModule parent { get; set; }
        public List<Module> slots { get; set; }
        public int colspan { get; set; }
        public int rowspan { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public Brush texture { get; set; }
        public MetaModule meta { get; set; }

        public Module(MetaModule unparent, type type, int x, int y, int colspan, int rowspan, Brush texture)
        {
            this.parent = unparent;
            this.letype = type;
            this.x = x;
            this.y = y;
            this.colspan = colspan;
            this.rowspan = rowspan;
            this.slots = new List<Module>();
            this.texture = texture;
        }

        public Module(type type, int x, int y, int colspan, int rowspan, Brush texture)
        {
            this.letype = type;
            this.x = x;
            this.y = y;
            this.colspan = colspan;
            this.rowspan = rowspan;
            this.slots = new List<Module>();
            this.texture = texture;
        }
        
        public Module(int i1, int i2, int i3, int i4, int i5, MetaModule s1)
        {

        }
    }
}
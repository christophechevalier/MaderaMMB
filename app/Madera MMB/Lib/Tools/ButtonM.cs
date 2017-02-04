using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Madera_MMB.Lib.Tools
{
    class ButtonM : Button
    {
        public enum type
        {
            Mur, Fenetre, Porte
        }

        public type letype { get; set; }
        public ButtonM parent { get; set; }
        public List<ButtonM> slots { get; set; }

        public ButtonM(type type)
        {
            this.letype = type;
            this.slots = new List<ButtonM>();
        }

        public ButtonM(ButtonM unparent)
        {
            this.parent = unparent;
            this.slots = new List<ButtonM>();
        }

        public ButtonM(ButtonM unparent, type type)
        {
            this.parent = unparent;
            this.letype = type;
            this.slots = new List<ButtonM>();
        }


        public enum verticalite
        {
            vertical, horizontal, none
        }
        public verticalite position { get; set; }
        public ButtonM(type type, verticalite unepos)
        {
            this.letype = type;
            this.position = unepos;
        }
    }
}
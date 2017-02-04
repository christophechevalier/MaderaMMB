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
            Mur, MurInt, Fenetre, Porte, Rien, Slot
        }

        public type letype { get; set; }
        public ButtonM parent { get; set; }
        public List<ButtonM> slots { get; set; }
        public int colspan { get; set; }
        public int rowspan { get; set; }
        public int x { get; set; }
        public int y { get; set; }

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

        public ButtonM(ButtonM unparent, type type,int x, int y, int colspan, int rowspan)
        {
            this.parent = unparent;
            this.letype = type;
            this.x = x;
            this.y = y;
            this.colspan = colspan;
            this.rowspan = rowspan;
            this.slots = new List<ButtonM>();
        }

        public ButtonM(type type, int x, int y, int colspan, int rowspan)
        {
            this.letype = type;
            this.x = x;
            this.y = y;
            this.colspan = colspan;
            this.rowspan = rowspan;
            this.slots = new List<ButtonM>();
            checkType();
        }

        public void checkType()
        {
            switch (this.letype)
            {
                case type.Rien:
                    this.Background = System.Windows.Media.Brushes.LightGray;
                    break;
                case type.Mur:
                    this.Background = System.Windows.Media.Brushes.Brown;
                    break;
                case type.MurInt:
                    this.Background = System.Windows.Media.Brushes.Red;
                    break;
                case type.Fenetre:
                    this.Background = System.Windows.Media.Brushes.Blue;
                    break;
                case type.Porte:
                    this.Background = System.Windows.Media.Brushes.Olive;
                    break;
                case type.Slot:
                    this.Background = System.Windows.Media.Brushes.Cyan;
                    break;
            }
        }
    }
}
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
    class ButtonM : Button
    {
        public enum type
        {
            Mur, MurInt, Fenetre, Porte, Rien, SlotMur, SlotFen, SlotPorte, Slot
        }

        public type letype { get; set; }
        public ButtonM parent { get; set; }
        public List<ButtonM> slots { get; set; }
        public int colspan { get; set; }
        public int rowspan { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public Brush texture { get; set; }
        public MetaModule meta { get; set; }

        public ButtonM(ButtonM unparent, type type, int x, int y, int colspan, int rowspan, Brush texture)
        {
            this.parent = unparent;
            this.letype = type;
            this.x = x;
            this.y = y;
            this.colspan = colspan;
            this.rowspan = rowspan;
            this.slots = new List<ButtonM>();
            this.texture = texture;
        }

        public ButtonM(type type, int x, int y, int colspan, int rowspan, Brush texture)
        {
            this.letype = type;
            this.x = x;
            this.y = y;
            this.colspan = colspan;
            this.rowspan = rowspan;
            this.slots = new List<ButtonM>();
            this.texture = texture;
        }

        public void checkType()
        {
            switch (this.letype)
            {
                case type.Rien:
                    this.Background = Brushes.LightGray;
                    break;
                case type.Mur:
                    this.Background = Brushes.Brown;
                    break;
                case type.MurInt:
                    this.Background = Brushes.Red;
                    break;
                case type.Fenetre:
                    this.Background = Brushes.Blue;
                    break;
                case type.Porte:
                    this.Background = Brushes.Olive;
                    break;
                case type.SlotMur:
                    this.Background = Brushes.Cyan;
                    break;
                case type.SlotFen:
                    this.Background = Brushes.LightGreen;
                    break;
                case type.SlotPorte:
                    this.Background = Brushes.LightBlue;
                    break;
            }
        }

        public void test()
        {

        }
    }
}
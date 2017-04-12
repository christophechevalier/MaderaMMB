using Madera_MMB.Model;
using System.Windows.Controls;
using System.Windows.Media;

namespace Madera_MMB.Lib.Tools
{
    public class Module : Button
    {
        #region Attributs
        public enum type
        {
            Mur, MurInt, Fenetre, Porte, Rien, SlotMur, SlotFen, SlotPorte, Slot
        }

        public type letype { get; set; }
        public MetaModule parent { get; set; }
        public int colspan { get; set; }
        public int rowspan { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public Brush texture { get; set; }
        public MetaModule meta { get; set; }
        #endregion

        #region Constructeurs
        /// <summary>
        /// Constructeur de la classe
        /// </summary>
        /// <param name="unparent"></param>
        /// <param name="type"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="colspan"></param>
        /// <param name="rowspan"></param>
        /// <param name="texture"></param>
        public Module(MetaModule unparent, type type, int x, int y, int colspan, int rowspan, Brush texture)
        {
            this.parent = unparent;
            this.letype = type;
            this.x = x;
            this.y = y;
            this.colspan = colspan;
            this.rowspan = rowspan;
            this.texture = texture;
        }

        /// <summary>
        /// Constructeur de la classe
        /// </summary>
        /// <param name="type"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="colspan"></param>
        /// <param name="rowspan"></param>
        /// <param name="texture"></param>
        public Module(type type, int x, int y, int colspan, int rowspan, Brush texture)
        {
            this.letype = type;
            this.x = x;
            this.y = y;
            this.colspan = colspan;
            this.rowspan = rowspan;
            this.texture = texture;
        }

        /// <summary>
        /// Constructeur de la classe
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="colspan"></param>
        /// <param name="rowspan"></param>
        /// <param name="meta"></param>
        /// <param name="parent"></param>
        public Module(int x, int y, int colspan, int rowspan, MetaModule meta, MetaModule parent = null)
        {
            this.x = x;
            this.y = y;
            this.colspan = colspan;
            this.rowspan = rowspan;
            this.meta = meta;
            this.parent = parent;
            checkType();
        }
        #endregion

        #region Méthode privé
        /// <summary>
        /// Sert à renseigner le type du module en fonction de son meta
        /// et lui renseigne sa texture
        /// </summary>
        private void checkType()
        {
            if (this.meta.label.Contains("Mur exterieur"))
            {
                this.letype = type.Mur;
            }
            else if (this.meta.label.Contains("Mur int"))
            {
                this.letype = type.MurInt;
            }
            else if (this.meta.label.Contains("Porte"))
            {
                this.letype = type.Porte;
            }
            else if (this.meta.label.Contains("Fenetre"))
            {
                this.letype = type.Fenetre;
            }

            Brush fond = new ImageBrush(this.meta.image);
            this.texture = fond;
        }
        #endregion
    }
}
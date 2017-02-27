﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Madera_MMB.Model
{
    class MetaModule
    {
        #region properties
        public string reference { get; set; }
        public string label { get; set; }
        public int prixHT { get; set; }
        public int nbSlot { get; set; }
        public BitmapImage image { get; set; }
        public Gamme gamme { get; set; }
        public List<MetaSlot> metaslots { get; set; }
        public List<Composant> composants { get; set; }
        #endregion

        #region Ctor
        public MetaModule(string reference, string label, int prix, int nbslot, Gamme gamme, List<Composant> composants, List<MetaSlot> metaslots, BitmapImage image = null)
        {
            this.reference = reference;
            this.label = label;
            this.prixHT = prix;
            this.nbSlot = nbslot;
            this.image = image;
            this.gamme = gamme;
            this.composants = composants;
            this.metaslots = metaslots;
        }
        public MetaModule() { }
        #endregion
    }
}

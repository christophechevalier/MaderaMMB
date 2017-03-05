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
        public bool statut { get; set; }
        #endregion

        #region Ctor
        public MetaModule(string reference, string label, int prix, int nbslot, BitmapImage image, Gamme gamme, List<MetaSlot> metaslots, bool statut)
        {
            this.reference = reference;
            this.label = label;
            this.prixHT = prix;
            this.nbSlot = nbslot;
            this.image = image;
            this.gamme = gamme;
            this.metaslots = metaslots;
            this.statut = statut;
        }
        public MetaModule() { }
        #endregion
    }
}

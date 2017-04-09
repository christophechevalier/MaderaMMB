
-- Table Commercial OK

CREATE TABLE commercial (
  refCommercial TEXT PRIMARY KEY NOT NULL,
  nom TEXT NOT NULL,
  prenom TEXT NOT NULL,
  email TEXT NOT NULL,
  motDePasse TEXT NOT NULL
);


-- Table Client OK

CREATE TABLE client (
  refClient TEXT PRIMARY KEY NOT NULL,
  nom TEXT NOT NULL,
  prenom TEXT NOT NULL,
  adresse TEXT NOT NULL,
  codePostal TEXT NOT NULL,
  ville TEXT NOT NULL,
  email TEXT NOT NULL,
  telephone TEXT NOT NULL,
  dateCreation TEXT NOT NULL,
  dateModification TEXT NOT NULL
);

-- Table Projet OK

CREATE TABLE projet (
  refProjet TEXT PRIMARY KEY NOT NULL,
  nom TEXT NOT NULL,
  dateCreation TEXT NOT NULL,
  dateModification TEXT NOT NULL,
  refClient TEXT NOT NULL,
  refCommercial TEXT NOT NULL,
  FOREIGN KEY (refClient) REFERENCES client,
  FOREIGN KEY (refCommercial) REFERENCES commercial
); 

-- Table Couverture OK

CREATE TABLE couverture (
  typeCouverture TEXT PRIMARY KEY NOT NULL,
  prixHT INT NOT NULL,
  image LONG BLOB NOT NULL,
  statut INTEGER NOT NULL,
  dateMaj TEXT NOT NULL
);

-- Table CoupePrincipe OK

CREATE TABLE coupePrincipe (
  idCoupe INTEGER PRIMARY KEY AUTOINCREMENT,
  label TEXT NOT NULL,
  longueur INTEGER NOT NULL,
  largeur INTEGER NOT NULL,
  prixHT INTEGER NOT NULL,
  image LONG BLOB NOT NULL,
  statut INTEGER NOT NULL,
  dateMaj TEXT NOT NULL
);

-- Table Plancher OK

CREATE TABLE plancher (
  typePlancher TEXT PRIMARY KEY NOT NULL,
  prixHT INTEGER NOT NULL,
  image LONG BLOB NOT NULL,
  statut INTEGER NOT NULL,
  dateMaj TEXT NOT NULL
);


-- Table Gamme OK

CREATE TABLE gamme (
  nomGamme TEXT PRIMARY KEY NOT NULL,
  offrePromo INT NOT NULL,
  typeIsolant TEXT NOT NULL,
  typeFinition TEXT NOT NULL,
  qualiteHuisserie TEXT NOT NULL,
  image LONG BLOB NOT NULL,
  statut INTEGER NOT NULL,
  dateMaj TEXT NOT NULL
);

-- Table Plan OK

CREATE TABLE plan (
  refPlan TEXT PRIMARY KEY NOT NULL,
  label TEXT NOT NULL,
  dateCreation TEXT NOT NULL,
  dateModification TEXT NOT NULL,
  refProjet TEXT NOT NULL,
  typePlancher TEXT NOT NULL,
  typeCouverture TEXT NOT NULL,
  idCoupe INT NOT NULL,
  nomGamme TEXT,
  FOREIGN KEY (refProjet) REFERENCES projet,
  FOREIGN KEY (typePlancher) REFERENCES plancher,
  FOREIGN KEY (typeCouverture) REFERENCES couverture,
  FOREIGN KEY (idCoupe) REFERENCES coupePrincipe
);


-- Table Devis OK

CREATE TABLE devis (
  refDevis TEXT PRIMARY KEY NOT NULL,
  etat TEXT NOT NULL,
  dateCreation TEXT NOT NULL,
  prixTotalHT INTEGER NOT NULL,
  prixTotalTTC INTEGER NOT NULL,
  refPlan TEXT NOT NULL
);


-- Table MetaModule OK

CREATE TABLE metamodule (
  refMetaModule TEXT PRIMARY KEY NOT NULL,
  label TEXT NOT NULL,
  prixHT INT NOT NULL,
  image BLOB NULL,
  statut BOOLEAN NOT NULL,
  dateMaj TEXT NOT NULL,
  nomGamme TEXT NOT NULL,
  taille INT NOT NULL,
  ecart INT NOT NULL,
  FOREIGN KEY (nomGamme)REFERENCES gamme
); 

-- Table Module OK

CREATE TABLE module (
  coordonneeDebutX INT NOT NULL,
  coordonneeDebutY INT NOT NULL,
  colspan INT NULL,
  rowspan INT NULL,
  refMetaModule TEXT NOT NULL,
  refMetaparent TEXT,
  refPlan TEXT NOT NULL,
  FOREIGN KEY (refMetaModule) REFERENCES metamodule,
  FOREIGN KEY (refMetaparent) REFERENCES metamodule,
  FOREIGN KEY (refPlan) REFERENCES plan
); 


-- Table MetaSlot OK

CREATE TABLE metaslot (
  idMetaSlot INTEGER PRIMARY KEY AUTOINCREMENT,
  numSlotPosition INTEGER NOT NULL,
  type TEXT NOT NULL,
  refMetaModule TEXT NOT NULL,
  FOREIGN KEY (refMetaModule) REFERENCES metamodule
); 

-- Table Slot OK

CREATE TABLE slot (
  idSlot INTEGER PRIMARY KEY AUTOINCREMENT,
  numSlotPosition INTEGER NOT NULL,
  label TEXT NOT NULL,
  contenu TEXT NOT NULL,
  parent TEXT NOT NULL,
  idMetaSlot INTEGER NOT NULL,
  FOREIGN KEY (contenu) REFERENCES module,
  FOREIGN KEY (parent) REFERENCES module,
  FOREIGN KEY (idMetaSlot) REFERENCES metaslot
); 
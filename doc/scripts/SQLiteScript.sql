
-- Table Commercial OK

CREATE TABLE commercial (
  refCommercial text PRIMARY KEY NOT NULL,
  nom text NOT NULL,
  prenom text NOT NULL,
  motDePasse text NOT NULL
);


-- Table Client OK

CREATE TABLE  client (
  refClient TEXT PRIMARY KEY NOT NULL,
  nom TEXT NOT NULL,
  prenom TEXT NOT NULL,
  adresse TEXT NOT NULL,
  codePostal TEXT NOT NULL,
  ville TEXT NOT NULL,
  email TEXT NOT NULL,
  telephone TEXT NOT NULL
);

-- Table Projet OK

CREATE TABLE  projet (
  refProjet TEXT PRIMARY KEY NOT NULL,
  nom TEXT NOT NULL,
  dateCreation NUMERIC NOT NULL,
  dateModification NUMERIC NOT NULL,
  refClient TEXT NOT NULL,
  refCommercial TEXT NOT NULL,
  FOREIGN KEY (refClient) REFERENCES client,
  FOREIGN KEY (refCommercial) REFERENCES commercial,
); 

-- Table Couverture OK

CREATE TABLE  couverture (
  typeCouverture TEXT PRIMARY KEY NOT NULL,
  prixHT INT NOT NULL
);

-- Table CoupePrincipe OK

CREATE TABLE  coupePrincipe (
  id_coupe INTEGER PRIMARY KEY NOT NULL AUTOINCREMENT,
  label TEXT NOT NULL,
  longueur INTEGER NOT NULL,
  largeur INTEGER NOT NULL,
  prixHT INTEGER NOT NULL
);

-- Table Plancher OK

CREATE TABLE  plancher (
  typePlancher TEXT PRIMARY KEY NOT NULL,
  prixHT INTEGER NOT NULL,
);


-- Table Gamme OK

CREATE TABLE  gamme (
  nom TEXT PRIMARY KEY NOT NULL,
  offrePromo INT NOT NULL,
  typeIsolant TEXT NOT NULL,
  typeFinition TEXT NOT NULL,
  qualiteHuisserie TEXT NOT NULL
);

-- Table Plan OK

CREATE TABLE plan (
  refPlan TEXT PRIMARY KEY NOT NULL,
  label TEXT NOT NULL,
  dateCreation date NOT NULL,
  dateModification date NOT NULL,
  refProjet TEXT NOT NULL,
  refClient TEXT NOT NULL,
  refCommercial TEXT NOT NULL,
  typeCouverture TEXT NOT NULL,
  id_coupe int NOT NULL,
  typePlancher TEXT NOT NULL,
  nomGamme TEXT NOT NULL,
  FOREIGN KEY (refProjet) REFERENCES projet,
  FOREIGN KEY (refClient)REFERENCES client,
  FOREIGN KEY (refCommercial) REFERENCES commercial,
  FOREIGN KEY (typeCouverture)REFERENCES couverture,
  FOREIGN KEY (id_coupe) REFERENCES coupePrincipe,
  FOREIGN KEY (typePlancher) REFERENCES plancher,
  FOREIGN KEY (nomGamme) REFERENCES gamme
);


-- Table Devis OK

CREATE TABLE  devis (
  refDevis TEXT PRIMARY KEY NOT NULL,
  nom TEXT NOT NULL,
  etat TEXT NOT NULL,
  quantite TEXT NOT NULL,
  unite TEXT NOT NULL,
  dateCreation NUMERIC NOT NULL,
  margeCommercial INTEGER NOT NULL,
  margeEntreprise INTEGER NOT NULL,
  prixTotalHT INTEGER NOT NULL,
  prixTotalTTC INTEGER NOT NULL,
  refPlan TEXT NOT NULL,
  refProjet TEXT NOT NULL,
  refClient TEXT NOT NULL,
  refCommercial TEXT NOT NULL,
  FOREIGN KEY (refPlan) REFERENCES plan,
  FOREIGN KEY (refProjet) REFERENCES projet,
  FOREIGN KEY (refClient) REFERENCES client,
  FOREIGN KEY (refCommercial) REFERENCES commercial
);


-- Table MetaModule OK

CREATE TABLE  MetaModule (
  refMetaModule TEXT PRIMARY KEY NOT NULL,
  label TEXT NOT NULL,
  prixHT INT NOT NULL,
  nbSlot INT NOT NULL,
  image BLOB NULL,
  nomGamme TEXT NOT NULL,
  FOREIGN KEY (nomGamme)REFERENCES gamme
); 

-- Table Module OK

CREATE TABLE  module (
  nom TEXT PRIMARY KEY NOT NULL,
  prixHT INT NOT NULL,
  nbSlot INT NOT NULL,
  coordonneeDebutX TEXT NOT NULL,
  coordonneeDebutY NUMERIC NOT NULL,
  coordonneeFinX INT NOT NULL,
  coordonneeFinY INT NOT NULL,
  refMetaModule TEXT NOT NULL,
  refPlan TEXT NOT NULL,
  FOREIGN KEY (refMetaModule) REFERENCES MetaModule,
  FOREIGN KEY (refPlan) REFERENCES plan
); 


-- Table Famille composant OK

CREATE TABLE  familleComposant (
  nom TEXT PRIMARY KEY NOT NULL
); 


-- Table Composant OK

CREATE TABLE  composant (
  id_composant INTEGER PRIMARY KEY NOT NULL AUTOINCREMENT,
  nom TEXT NOT NULL,
  nomFamilleComposant TEXT NOT NULL,
  FOREIGN KEY (nomFamilleComposant) REFERENCES familleComposant
); 


-- Table Composant_has_MetaModule OK

CREATE TABLE  Composant_has_MetaModule (
  refMetaModule TEXT NOT NULL,
  id_composant INT NOT NULL,
  PRIMARY KEY (refMetaModule, id_composant),
  FOREIGN KEY (refMetaModule) REFERENCES Composant_has_MetaModule,
  FOREIGN KEY (id_composant) REFERENCES Composant_has_MetaModule
); 


-- Table MetaSlot OK

CREATE TABLE MetaSlot (
  idMetaSlot INTEGER PRIMARY KEY NOT NULL,
  label TEXT NOT NULL,
  numSlotPosition INT NOT NULL,
  refMetaModule TEXT NOT NULL,
  FOREIGN KEY (refMetaModule) REFERENCES MetaModule
); 

-- Table Slot OK

CREATE TABLE  slot (
  idSlot INT PRIMARY KEY NOT NULL,
  numSlotPosition INT NOT NULL,
  label TEXT NOT NULL,
  contenu TEXT NOT NULL,
  parent TEXT NOT NULL,
  idMetaSlot int NOT NULL,
  FOREIGN KEY (contenu) REFERENCES module,
  FOREIGN KEY (parent) REFERENCES module,
  FOREIGN KEY (idMetaSlot) REFERENCES MetaSlot
); 



-- Table MetaModul_has_MetaSlot OK

CREATE TABLE  MetaModul_has_MetaSlot (
  id_composition INT PRIMARY KEY NOT NULL AUTOINCREMENT,
  refMetaModule TEXT NOT NULL,
  idMetaSlot INT NOT NULL,
  FOREIGN KEY (refMetaModule) REFERENCES MetaModule,
  FOREIGN KEY (idMetaSlot) REFERENCES MetaSlot
); 


  -- ########################### INSERTS ######################################## --
  

--
-- Gammes
--
insert into gamme (nom, offrePromo, typeIsolant, typeFinition, qualiteHuisserie) values ('Bois ancien', 2, 'Amoxicillin and Clavulanate Potassium', 'Amoxicillin and Clavulanate Potassium', 'Metz-Bartoletti');
insert into gamme (nom, offrePromo, typeIsolant, typeFinition, qualiteHuisserie) values ('Dorré chic', 8, 'NITRO-BID', 'Nitroglycerin', 'Armstrong, Barton and Bernhard');
insert into gamme (nom, offrePromo, typeIsolant, typeFinition, qualiteHuisserie) values ('Ebene obscur', 11, 'Glimepiride', 'Glimepiride', 'Murazik-Bailey');
  
  
--
-- Catalogue
--
insert into MetaModule (refMetaModule, label, prixHT, nbSlot, nomGamme) values ('3562989509476444', 'Porte simple battant', 97, 3, 'Bois ancien');
insert into MetaModule (refMetaModule, label, prixHT, nbSlot, nomGamme) values ('30264985106539', 'Porte double battant', 32, 3, 'Dorré chic');
insert into MetaModule (refMetaModule, label, prixHT, nbSlot, nomGamme) values ('3553909364809977', 'Fenêtre carreaux simple', 24, 1, 'Ebene obscur');
insert into MetaModule (refMetaModule, label, prixHT, nbSlot, nomGamme) values ('201455480736282', 'Fenêtre carreaux incurvés', 95, 2, 'Dorré chic');
insert into MetaModule (refMetaModule, label, prixHT, nbSlot, nomGamme) values ('4913679059729102', 'Mur solide 1m', 16, 0, 'Bois ancien');
insert into MetaModule (refMetaModule, label, prixHT, nbSlot, nomGamme) values ('670645194056361597', 'Mur solide long ', 26, 5, 'Dorré chic');
insert into MetaModule (refMetaModule, label, prixHT, nbSlot, nomGamme) values ('4175002133436136', 'Mur léger 1m', 30, 0, 'Bois ancien');
insert into MetaModule (refMetaModule, label, prixHT, nbSlot, nomGamme) values ('201443874685331', 'Mur léger long', 55, 5, 'Dorré chic');
insert into MetaModule (refMetaModule, label, prixHT, nbSlot, nomGamme) values ('5485794656214823', 'Fenêtre mono-vitre simple', 35, 4, 'Bois ancien');
insert into MetaModule (refMetaModule, label, prixHT, nbSlot, nomGamme) values ('30344920947781', 'Fenêtre mono-vitre incurvée', 54, 1, 'Bois ancien');
insert into MetaModule (refMetaModule, label, prixHT, nbSlot, nomGamme) values ('4844260374691931', 'Porte ancienne double battant', 79, 3, 'Ebene obscur');
insert into MetaModule (refMetaModule, label, prixHT, nbSlot, nomGamme) values ('3559747487224406', 'Porte amovible', 39, 5, 'Bois ancien');
insert into MetaModule (refMetaModule, label, prixHT, nbSlot, nomGamme) values ('3559747487225506', 'Double porte simple', 39, 0, 'Bois ancien');
insert into MetaModule (refMetaModule, label, prixHT, nbSlot, nomGamme) values ('3559747487225666', 'Baie vitrée 1m', 39, 0, 'Bois ancien');
insert into MetaModule (refMetaModule, label, prixHT, nbSlot, nomGamme) values ('3559747488225406', 'Porte amovible double', 39, 5, 'Bois ancien');
  
 
--
-- Contenu de la table `commercial`
--

INSERT INTO commercial (refCommercial, nom, prenom, motDePasse) VALUES
('001', 'Schwarze', 'Alexandre', 'titi'),
('002', 'Croco', 'David', 'toto');

--
-- Contenu de la table `couverture`
--
INSERT INTO couverture (typeCouverture, prixHT) VALUES
('Ardoise', '20'),
('Tuiles', '15'),
('Chaume', '5'),
('Bois', '10'),
('Pierre', '30');

--
-- Contenu de la table `coupeprincipe`
--

INSERT INTO coupeprincipe (label, longueur, largeur, prixHT) VALUES
('L', 50, 50, 45000),
('T', 50, 50, 50000),
('carré', 50, 50, 35000),
('rectangle', 50, 50, 35000),
('carré', 50, 50, 35000),
('carré', 50, 50, 35000),
('carré', 50, 50, 35000);

--
-- Contenu de la table `famillecomposant`
--

INSERT INTO famillecomposant (nom) VALUES
('Section'),
('Visseries'),
('Montant'),
('Remplissage');




--
-- Contenu de la table `composant`
--

INSERT INTO composant (nom, nomFamilleComposant) VALUES
('Lysse', 'Section'),
('Contrefort', 'Section' ),
('Sabot assemblage', 'Section'),
('Goujeon de fixation','Section'),
('Equerre','Section'),
('Support de sol','Section'),
('Départ','Montant'),
('Arrivée','Montant'),
('Tasseau','Montant'),
('Isolation','Remplissage'),
('Panneau structurel','Remplissage'),
('Pare-Pluie','Remplissage'),
('Boulon','Visseries'),
('Tire-fond','Visseries'),
('Vis','Visseries'),
('Tire-fond','Visseries');

--
-- Contenu de la table `plancher`
--

INSERT INTO plancher (typePlancher, prixHT) VALUES
('Bois', 50),
('Carrelage', 90);


--
-- Compositions des MetaModules
--
INSERT INTO composant_has_metamodule `refMetaModule, id_composant) VALUES ('201443874685331', '6'), ('201443874685331', '7'), ('201443874685331', '11'), ('3559747487225506', '11'), ('30264985106539', '9'), ('3562989509476444', '11'), ('30344920947781', '6'), ('201443874685331', '15'), ('30344920947781', '15'), ('4844260374691931', '12'), ('3562989509476444', '14'), ('3559747487225506', '12'), ('3559747487225506', '16'), ('30344920947781', '11'), ('201443874685331', '13'), ('4844260374691931', '15'), ('5485794656214823', '14'), ('201455480736282', '4'), ('3559747488225406', '8'), ('4913679059729102', '15');


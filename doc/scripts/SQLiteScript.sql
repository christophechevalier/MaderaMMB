
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
  image LONG BLOB NULL
);

-- Table CoupePrincipe OK

CREATE TABLE coupeprincipe (
  id_coupe INTEGER PRIMARY KEY AUTOINCREMENT,
  label TEXT NOT NULL,
  longueur INT NOT NULL,
  largeur INT NOT NULL,
  prixHT INT NOT NULL,
  image LONG BLOB NULL
);

-- Table Plancher OK

CREATE TABLE plancher (
  typePlancher TEXT PRIMARY KEY NOT NULL,
  prixHT INT NOT NULL,
  image LONG BLOB
);


-- Table Gamme OK

CREATE TABLE gamme (
  nomGamme TEXT PRIMARY KEY NOT NULL,
  offrePromo INT NOT NULL,
  typeIsolant TEXT NOT NULL,
  typeFinition TEXT NOT NULL,
  qualiteHuisserie TEXT NOT NULL,
  image LONG BLOB NULL
);

-- Table Plan OK

CREATE TABLE plan (
  refPlan TEXT PRIMARY KEY NOT NULL,
  label TEXT NOT NULL,
  dateCreation TEXT NOT NULL,
  dateModification TEXT NOT NULL,
  refProjet TEXT NOT NULL,
  typeCouverture TEXT NOT NULL,
  id_coupe INTEGER NOT NULL,
  typePlancher TEXT NOT NULL,
  nomGamme TEXT NOT NULL,
  FOREIGN KEY (refProjet) REFERENCES projet,
  FOREIGN KEY (typeCouverture) REFERENCES couverture,
  FOREIGN KEY (id_coupe) REFERENCES coupeprincipe,
  FOREIGN KEY (typePlancher) REFERENCES plancher,
  FOREIGN KEY (nomGamme) REFERENCES gamme
);


-- Table Devis OK

CREATE TABLE devis (
  refDevis TEXT PRIMARY KEY NOT NULL,
  nom TEXT NOT NULL,
  etat TEXT NOT NULL,
  quantite TEXT NOT NULL,
  unite TEXT NOT NULL,
  dateCreation TEXT NOT NULL,
  margeCommercial INT NOT NULL,
  margeEntreprise INT NOT NULL,
  prixTotalHT INT NOT NULL,
  prixTotalTTC INT NOT NULL,
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

CREATE TABLE metamodule (
  refMetaModule TEXT PRIMARY KEY NOT NULL,
  label TEXT NOT NULL,
  prixHT INT NOT NULL,
  nbSlot INT NOT NULL,
  image BLOB NULL,
  nomGamme TEXT NOT NULL,
  FOREIGN KEY (nomGamme)REFERENCES gamme
); 

-- Table Module OK

CREATE TABLE module (
  nom TEXT PRIMARY KEY NOT NULL,
  prixHT INT NOT NULL,
  nbSlot INT NOT NULL,
  coordonneeDebutX TEXT NOT NULL,
  coordonneeDebutY NUMERIC NOT NULL,
  coordonneeFinX INT NOT NULL,
  coordonneeFinY INT NOT NULL,
  refMetaModule TEXT NOT NULL,
  refPlan TEXT NOT NULL,
  FOREIGN KEY (refMetaModule) REFERENCES metamodule,
  FOREIGN KEY (refPlan) REFERENCES plan
); 


-- Table Famille composant OK

CREATE TABLE famillecomposant (
  nom TEXT PRIMARY KEY NOT NULL
); 


-- Table Composant OK

CREATE TABLE composant (
  id_composant INTEGER PRIMARY KEY AUTOINCREMENT,
  nom TEXT NOT NULL,
  nomFamilleComposant TEXT NOT NULL,
  FOREIGN KEY (nomFamilleComposant) REFERENCES familleComposant
); 


-- Table Composant_Has_MetaModule OK

CREATE TABLE composant_has_metamodule (
  refMetaModule TEXT NOT NULL,
  id_composant INTEGER NOT NULL,
  PRIMARY KEY (refMetaModule, id_composant),
  FOREIGN KEY (refMetaModule) REFERENCES composant_has_metamodule,
  FOREIGN KEY (id_composant) REFERENCES composant_has_metamodule
); 


-- Table MetaSlot OK

CREATE TABLE metaslot (
  idMetaSlot INTEGER PRIMARY KEY AUTOINCREMENT,
  label TEXT NOT NULL,
  numSlotPosition INT NOT NULL,
  refMetaModule TEXT NOT NULL,
  FOREIGN KEY (refMetaModule) REFERENCES metamodule
); 

-- Table Slot OK

CREATE TABLE slot (
  idSlot INTEGER PRIMARY KEY AUTOINCREMENT,
  numSlotPosition INT NOT NULL,
  label TEXT NOT NULL,
  contenu TEXT NOT NULL,
  parent TEXT NOT NULL,
  idMetaSlot INTEGER NOT NULL,
  FOREIGN KEY (contenu) REFERENCES module,
  FOREIGN KEY (parent) REFERENCES module,
  FOREIGN KEY (idMetaSlot) REFERENCES metaslot
); 



-- Table MetaModul_Has_MetaSlot OK

CREATE TABLE metamodul_has_metaslot (
  id_composition INTEGER PRIMARY KEY AUTOINCREMENT,
  refMetaModule TEXT NOT NULL,
  idMetaSlot INTEGER NOT NULL,
  FOREIGN KEY (refMetaModule) REFERENCES metamodule,
  FOREIGN KEY (idMetaSlot) REFERENCES metaslot
); 


  -- ########################### INSERTS ######################################## --
  

--
-- Gammes
--
insert into gamme (nomGamme, offrePromo, typeIsolant, typeFinition, qualiteHuisserie, image) values ('Bois ancien', 2, 'Amoxicillin and Clavulanate Potassium', 'Amoxicillin and Clavulanate Potassium', 'Metz-Bartoletti', '');
insert into gamme (nomGamme, offrePromo, typeIsolant, typeFinition, qualiteHuisserie, image) values ('Dorré chic', 8, 'NITRO-BID', 'Nitroglycerin', 'Armstrong, Barton and Bernhard', '');
insert into gamme (nomGamme, offrePromo, typeIsolant, typeFinition, qualiteHuisserie, image) values ('Ebene obscur', 11, 'Glimepiride', 'Glimepiride', 'Murazik-Bailey', '');
  
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
-- Contenu de la table `Commercial` OK
--

INSERT INTO commercial (refCommercial, nom, prenom, email, motDePasse) VALUES
('COM001', 'Schwarze', 'Alexandre', 'alex@gmail.com', 'titi'),
('COM002', 'Crocco', 'David', 'david@gmail.com', 'toto'),
('COM003', 'Chevalier', 'Christophe', 'monemail@gmail.com', 'mdp');

--
-- Contenu de la table `Client` OK
--

INSERT INTO client (refClient, nom, prenom, adresse, codePostal, ville, email, telephone, dateCreation, dateModification) VALUES
('CLI001', 'Arthur', 'Tv', '10 chemin des Albios', '31130', 'Balma', 'arthur@gmail.com', '06-06-06-06-06', '10-10-2016', '10-10-2016'),
('CLI002', 'Beatrice', 'Tijuana', '9 chemin des iles', '31000', 'Toulouse', 'beatrice@gmail.com', '06-06-06-06-07', '11-10-2016', '11-10-2016'),
('CLI003', 'Marco', 'Polo', '2 rue de la paume', '75000', 'Paris', 'marco@gmail.com', '06-06-06-06-08', '12-11-2016', '12-11-2016'),
('CLI004', 'Jessica', 'Palmer', '69 rue de lalimapo', '33000', 'Bordeaux', 'jess@gmail.com', '06-06-06-06-08', '13-11-2016', '13-11-2016');

--
-- Contenu de la table `Projet` OK
--

INSERT INTO projet (refProjet, nom, dateCreation, dateModification, refClient, refCommercial) VALUES
('PRO001', 'Maison Familiale', '10-10-2016', '10-10-2016', 'CLI001', 'COM003'),
('PRO002', 'Maison Vacance', '11-10-2016', '11-10-2016', 'CLI001', 'COM003'),
('PRO003', 'Maison Montagne', '12-10-2016', '12-10-2016', 'CLI001', 'COM003'),
('PRO004', 'Maison Mer', '13-10-2016', '13-10-2016', 'CLI001', 'COM003'),
('PRO005', 'Maison Secondaire', '14-10-2016', '14-10-2016', 'CLI001', 'COM003');

--
-- Contenu de la table `Plan` OK
--

INSERT INTO plan (refPlan, label, dateCreation, dateModification, refProjet, id_coupe, typeCouverture, typePlancher, nomGamme) VALUES
('PLA001', 'Test 1', '10-10-2016', '10-10-2016', 'PRO001', 1000, 'Ardoise', 'Bois', 'deluxe'),
('PLA002', 'Test 2', '11-10-2016', '11-10-2016', 'PRO001', 1000, 'Ardoise', 'Cailloux', 'deluxe'),
('PLA003', 'Test 3', '12-10-2016', '12-10-2016', 'PRO001', 2000, 'Ardoise', 'Carrelage', 'deluxe'),
('PLA004', 'Test 4', '13-10-2016', '13-10-2016', 'PRO001', 2000, 'Ardoise', 'Moquette', 'deluxe'),
('PLA005', 'Test 5', '14-10-2016', '14-10-2016', 'PRO001', 3000, 'Ardoise', 'Bois', 'deluxe'),
('PLA006', 'Test 6', '15-10-2016', '15-10-2016', 'PRO001', 3000, 'Ardoise', 'Bois', 'deluxe'),
('PLA007', 'Test 7', '16-10-2016', '16-10-2016', 'PRO001', 3000, 'Ardoise', 'Bois', 'deluxe'),
('PLA008', 'Test 8', '17-10-2016', '17-10-2016', 'PRO001', 3000, 'Ardoise', 'Bois', 'deluxe'),
('PLA009', 'Test 9', '18-10-2016', '18-10-2016', 'PRO001', 4000, 'Ardoise', 'Bois', 'deluxe');

--
-- Contenu de la table `Couverture` OK
--

INSERT INTO couverture (typeCouverture, prixHT, image) VALUES
('Ardoise', 20, ''),
('Tuiles', 15, ''),
('Chaume', 5, ''),
('Bois', 10, ''),
('Pierre', 30, '');

--
-- Contenu de la table `CoupePrincipe` OK
--

INSERT INTO coupeprincipe (id_coupe, label, longueur, largeur, prixHT, image) VALUES
(1000, 'L', 50, 50, 45000, ''),
(2000, 'T', 50, 50, 50000, ''),
(3000, 'carré', 50, 50, 35000, ''),
(4000, 'rectangle', 50, 50, 35000, '');


--
-- Contenu de la table `FamilleComposant` OK
--
/*
INSERT INTO famillecomposant (nom) VALUES
('Section'),
('Visseries'),
('Montant'),
('Remplissage');
*/
--
-- Contenu de la table `Composant` OK
--
/*
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
*/
--
-- Contenu de la table `Plancher` OK
--

INSERT INTO plancher (typePlancher, prixHT, image) VALUES
('Bois', 50, ''),
('Cailloux', 60, ''),
('Moquette', 70, ''),
('Carrelage', 90, '');

--
-- Compositions des `MetaModules` OK
--
/*
INSERT INTO composant_has_metamodule (refMetaModule, id_composant) VALUES 
('201443874685331', 6), 
('201443874685331', 7), 
('201443874685331', 11), 
('3559747487225506', 11), 
('30264985106539', 9), 
('3562989509476444', 11), 
('30344920947781', 6), 
('201443874685331', 15), 
('30344920947781', 15), 
('4844260374691931', 12), 
('3562989509476444', 14), 
('3559747487225506', 12), 
('3559747487225506', 16), 
('30344920947781', 11), 
('201443874685331', 13), 
('4844260374691931', 15), 
('5485794656214823', 14), 
('201455480736282', 4), 
('3559747488225406', 8), 
('4913679059729102', 15);
*/

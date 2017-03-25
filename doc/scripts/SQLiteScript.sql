
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
  dateCreation datetime NOT NULL,
  dateModification datetime NOT NULL,
  refProjet TEXT NOT NULL,
  typePlancher TEXT NOT NULL,
  typeCouverture TEXT NOT NULL,
  idCoupe INT NOT NULL,
  nomGamme TEXT NOT NULL,
  FOREIGN KEY (refProjet) REFERENCES projet,
  FOREIGN KEY (typePlancher) REFERENCES plancher,
  FOREIGN KEY (typeCouverture) REFERENCES couverture,
  FOREIGN KEY (idCoupe) REFERENCES coupePrincipe,
  FOREIGN KEY (nomGamme) REFERENCES gamme
);


-- Table Devis OK

CREATE TABLE devis (
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

CREATE TABLE metamodule (
  refMetaModule TEXT PRIMARY KEY NOT NULL,
  label TEXT NOT NULL,
  prixHT INT NOT NULL,
  nbSlot INT NOT NULL,
  image BLOB NULL,
  statut BOOLEAN NOT NULL,
  dateMaj TEXT NOT NULL,
  nomGamme TEXT NOT NULL,
  FOREIGN KEY (nomGamme)REFERENCES gamme
); 

-- Table Module OK

CREATE TABLE module (
  idModule INTEGER PRIMARY KEY AUTOINCREMENT,
  coordonneeDebutX INT NOT NULL,
  coordonneeDebutY INT NOT NULL,
  colspan INT NULL,
  rowspan INT NULL,
  refMetaModule TEXT NOT NULL,
  refPlan TEXT NOT NULL,
  FOREIGN KEY (refMetaModule) REFERENCES metamodule,
  FOREIGN KEY (refPlan) REFERENCES plan
); 


-- Table MetaSlot OK

CREATE TABLE metaslot (
  idMetaSlot INTEGER PRIMARY KEY AUTOINCREMENT,
  label TEXT NOT NULL,
  numSlotPosition INTEGER NOT NULL,
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

-- Table MetaModul_has_MetaSlot OK

CREATE TABLE MetaModul_has_MetaSlot (
  idComposition INTEGER PRIMARY KEY AUTOINCREMENT,
  refMetaModule TEXT NOT NULL,
  idMetaSlot INTEGER NOT NULL,
  FOREIGN KEY (refMetaModule) REFERENCES metamodule,
  FOREIGN KEY (idMetaSlot) REFERENCES MetaSlot
); 

  
--
-- Catalogue
--
/*
insert into metamodule (refMetaModule, label, prixHT, nbSlot, nomGamme, statut, dateModification) values ('3562989509476444', 'Porte simple battant', 97, 3, 'Bois ancien', 1, '2017-03-03 16:54:30.30');
insert into metamodule (refMetaModule, label, prixHT, nbSlot, nomGamme, statut, dateModification) values ('30264985106539', 'Porte double battant', 32, 3, 'Dorré chic', 1, 2017-03-03 16:54:30);
insert into metamodule (refMetaModule, label, prixHT, nbSlot, nomGamme, statut, dateModification) values ('3553909364809977', 'Fenêtre carreaux simple', 24, 1, 'Ebene obscur', 1, 2017-03-03 16:54:30);
insert into metamodule (refMetaModule, label, prixHT, nbSlot, nomGamme, statut, dateModification) values ('201455480736282', 'Fenêtre carreaux incurvés', 95, 2, 'Dorré chic', 1, 2017-03-03 16:54:30);
insert into metamodule (refMetaModule, label, prixHT, nbSlot, nomGamme, statut, dateModification) values ('4913679059729102', 'Mur solide 1m', 16, 0, 'Bois ancien', 1, 2017-03-03 16:54:30);
insert into metamodule (refMetaModule, label, prixHT, nbSlot, nomGamme, statut, dateModification) values ('670645194056361597', 'Mur solide long ', 26, 5, 'Dorré chic', 1, 2017-03-03 16:54:30);
insert into metamodule (refMetaModule, label, prixHT, nbSlot, nomGamme, statut, dateModification) values ('4175002133436136', 'Mur léger 1m', 30, 0, 'Bois ancien', 1, 2017-03-03 16:54:30);
insert into metamodule (refMetaModule, label, prixHT, nbSlot, nomGamme, statut, dateModification) values ('201443874685331', 'Mur léger long', 55, 5, 'Dorré chic', 1, 2017-03-03 16:54:30);
insert into metamodule (refMetaModule, label, prixHT, nbSlot, nomGamme, statut, dateModification) values ('5485794656214823', 'Fenêtre mono-vitre simple', 35, 4, 'Bois ancien', 1, 2017-03-03 16:54:30);
insert into metamodule (refMetaModule, label, prixHT, nbSlot, nomGamme, statut, dateModification) values ('30344920947781', 'Fenêtre mono-vitre incurvée', 54, 1, 'Bois ancien', 1, 2017-03-03 16:54:30);
insert into metamodule (refMetaModule, label, prixHT, nbSlot, nomGamme, statut, dateModification) values ('4844260374691931', 'Porte ancienne double battant', 79, 3, 'Ebene obscur', 1, 2017-03-03 16:54:30);
insert into metamodule (refMetaModule, label, prixHT, nbSlot, nomGamme, statut, dateModification) values ('3559747487224406', 'Porte amovible', 39, 5, 'Bois ancien', 1, 2017-03-03 16:54:30);
insert into metamodule (refMetaModule, label, prixHT, nbSlot, nomGamme, statut, dateModification) values ('3559747487225506', 'Double porte simple', 39, 0, 'Bois ancien', 1, 2017-03-03 16:54:30);
insert into metamodule (refMetaModule, label, prixHT, nbSlot, nomGamme, statut, dateModification) values ('3559747487225666', 'Baie vitrée 1m', 39, 0, 'Bois ancien', 1, 2017-03-03 16:54:30);
insert into metamodule (refMetaModule, label, prixHT, nbSlot, nomGamme, statut, dateModification) values ('3559747488225406', 'Porte amovible double', 39, 5, 'Bois ancien', 1, 2017-03-03 16:54:30);
 */ 
 
--
-- Contenu de la table `commercial`
--
/*
INSERT INTO commercial (refCommercial, nom, prenom, email, motDePasse) VALUES
('COM001', 'Schwarze', 'Alexandre', 'alex@gmail.com', 'titi'),
('COM002', 'Crocco', 'David', 'david@gmail.com', 'toto'),
('COM003', 'Chevalier', 'Christophe', 'monemail@gmail.com', 'mdp');
*/

--
-- Contenu de la table `client`
--

INSERT INTO client (refClient, nom, prenom, adresse, codePostal, ville, email, telephone, dateCreation, dateModification) VALUES
('AT000001', 'Arthur', 'Tv', '10 chemin des Albios', '31130', 'Balma', 'arthur@gmail.com', '06-06-06-06-06', '10-10-2016', '10-10-2016'),
('BT000002', 'Beatrice', 'Tijuana', '9 chemin des iles', '31000', 'Toulouse', 'beatrice@gmail.com', '06-06-06-06-07', '11-10-2016', '11-10-2016'),
('MP000003', 'Marco', 'Polo', '2 rue de la paume', '75000', 'Paris', 'marco@gmail.com', '06-06-06-06-08', '12-11-2016', '12-11-2016'),
('JP000004', 'Jessica', 'Palmer', '69 rue de lalimapo', '33000', 'Bordeaux', 'jess@gmail.com', '06-06-06-06-08', '13-11-2016', '13-11-2016');


--
-- Contenu de la table `projet`
--

INSERT INTO projet (refProjet, nom, dateCreation, dateModification, refClient, refCommercial) VALUES
('CCAT000001', 'Maison Familiale', '10-10-2016', '10-10-2016', 'AT000001', 'COM003'),
('CCMP000002', 'Maison Vacance', '11-10-2016', '11-10-2016', 'AT000001', 'COM003'),
('CCMP000003', 'Maison Montagne', '12-10-2016', '12-10-2016', 'MP000003', 'COM003'),
('CCJP000004', 'Maison Mer', '13-10-2016', '13-10-2016', 'AT000001', 'COM003'),
('CCJP000005', 'Maison Secondaire', '14-10-2016', '14-10-2016', 'AT000001', 'COM003');

--
-- Contenu de la table `plan`
--

INSERT INTO plan (refPlan, label, dateCreation, dateModification, refProjet, typePlancher, typeCouverture, idCoupe, nomGamme) VALUES
('CCAT000001-P01', 'Test 1', '2017-03-03 16:54:30.30', '2017-03-03 16:54:30.30', 'CCAT000001', 'Bois', 'Ardoise pourpre', 2, 'Aluminium'),
('CCAT000001-P02', 'Test 2', '2017-03-03 16:54:30.30', '2017-03-03 16:54:30.30', 'CCAT000001', 'Bois', 'Ardoise pourpre', 3, 'Aluminium'),
('CCAT000001-P03', 'Test 3', '2017-03-03 16:54:30.30', '2017-03-03 16:54:30.30', 'CCAT000001', 'Bois', 'Ardoise pourpre', 4, 'Aluminium'),
('CCAT000002-P01', 'Test 4', '2017-03-03 16:54:30.30', '2017-03-03 16:54:30.30', 'CCMP000002', 'Bois', 'Ardoise pourpre', 2, 'Aluminium'),
('CCAT000002-P02', 'Test 5', '2017-03-03 16:54:30.30', '2017-03-03 16:54:30.30', 'CCMP000002', 'Bois', 'Ardoise pourpre', 3, 'Aluminium'),
('CCAT000002-P03', 'Test 6', '2017-03-03 16:54:30.30', '2017-03-03 16:54:30.30', 'CCMP000002', 'Bois', 'Ardoise pourpre', 4, 'Aluminium');

--
-- Contenu de la table `module`
--

INSERT INTO module (coordonneeDebutX, coordonneeDebutY, colspan, rowspan, refMetaModule, refPlan) VALUES
(25, 35, 5, 0, '201443874685331', 'CCAT000001-P01'),
(44, 44, 4, 0, '201443874685331', 'CCAT000001-P01'),
(33, 33, 0, 3, '201443874685331', 'CCAT000001-P01');




--
-- Contenu de la table `couverture`
--
/*
INSERT INTO couverture (typeCouverture, prixHT, image) VALUES
('Ardoise', '20', ''),
('Tuiles', '15', ''),
('Chaume', '5', ''),
('Bois', '10', ''),
('Pierre', '30', '');
*/
--
-- Contenu de la table `coupeprincipe`
--
/*
INSERT INTO coupeprincipe (label, longueur, largeur, prixHT, image) VALUES
('L', 50, 50, 45000, ''),
('T', 50, 50, 50000, ''),
('carré', 50, 50, 35000, ''),
('rectangle', 50, 50, 35000, '');
*/


--
-- Contenu de la table `plancher`
--
/*
INSERT INTO plancher (typePlancher, prixHT, image) VALUES
('Bois', 50, ''),
('Carrelage', 90, '');
*/


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
  taille INT NOT NULL,
  ecart INT NOT NULL,
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

  
--
-- Catalogue
--


--
-- Contenu de la table `metamodule`
--
insert into metamodule (refMetaModule, label, prixHT, nbSlot, nomGamme, statut, dateMaj, taille, ecart) VALUES 
('M406587', 'Mur exterieur 2F', 390, NULL, 'Aluminium', 1, '2017-03-03 16:54:30.30', 12, 2),
('M406588', 'Mur exterieur 2F 1P', 420, NULL, 'Aluminium', 1, '2017-03-03 16:54:30', 12, 2),
('M406589', 'Mur exterieur 1P', 400, NULL, 'Aluminium', 1, '2017-03-03 16:54:30', 12, 2),
('M406590', 'Mur exterieur 1F', 400, NULL, 'Aluminium', 1, '2017-03-03 16:54:30', 15, 3),
('M406591', 'Mur exterieur 1P 2F', 450, NULL, 'Aluminium', 1, '2017-03-03 16:54:30', 15, 3),
('M406593', 'Mur exterieur NO', 350, NULL, 'Aluminium', 1, '2017-03-03 16:54:30', 12, 2),
('M406594', 'Mur exterieur NO', 370, NULL, 'Aluminium', 1, '2017-03-03 16:54:30', 15, 3),
('M406598', 'Mur exterieur NO', 450, NULL, 'Aluminium', 1, '2017-03-03 16:54:30', 23, 5),
('M406599', 'Mur exterieur 1P 1F', 420, NULL, 'Aluminium', 1, '2017-03-03 16:54:30', 19, 4),
('M406600', 'Mur exterieur 2F 1P', 450, NULL, 'Aluminium', 1, '2017-03-03 16:54:30', 19, 4),
('M406601', 'Mur exterieur 1P 1F', 430, NULL, 'Aluminium', 1, '2017-03-03 16:54:30', 19, 4),
('M406602', 'Mur exterieur 1P 1F', 470, NULL, 'Aluminium', 1, '2017-03-03 16:54:30', 23, 5),
('M406603', 'Mur exterieur 1P 2F', 480, NULL, 'Aluminium', 1, '2017-03-03 16:54:30', 23, 5),
('M406604', 'Mur exterieur 2P 1F', 500, NULL, 'Aluminium', 1, '2017-03-03 16:54:30', 23, 5),
('M406605', 'Mur exterieur 1F 1P', 420, NULL, 'Aluminium', 1, '2017-03-03 16:54:30', 15, 3),
('M40696', 'Mur exterieur NO', 410, NULL, 'Aluminium', 1, '2017-03-03 16:54:30', 19, 4);
 
 
--
-- Contenu de la table `metaslot`
--
 INSERT INTO metaslot (idMetaSlot, numSlotPosition, type, refMetaModule) VALUES
(1, 1, 'F', 'M406587'),
(2, 3, 'F', 'M406587'),
(3, 1, 'F', 'M406588'),
(4, 2, 'P', 'M406588'),
(5, 3, 'F', 'M406588'),
(6, 2, 'P', 'M406589'),
(7, 2, 'F', 'M406590'),
(8, 1, 'P', 'M406591'),
(9, 2, 'F', 'M406591'),
(10, 3, 'F', 'M406591'),
(13, 2, 'P', 'M406599'),
(14, 3, 'F', 'M406599'),
(15, 1, 'F', 'M406600'),
(16, 2, 'F', 'M406600'),
(17, 3, 'P', 'M406600'),
(18, 1, 'F', 'M406601'),
(19, 3, 'P', 'M406601'),
(20, 1, 'F', 'M406602'),
(21, 2, 'P', 'M406602'),
(22, 1, 'F', 'M406603'),
(23, 2, 'P', 'M406603'),
(24, 3, 'F', 'M406603'),
(25, 1, 'P', 'M406604'),
(26, 2, 'F', 'M406604'),
(27, 3, 'P', 'M406604'),
(28, 1, 'F', 'M406605'),
(29, 2, 'P', 'M406605');

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


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
  etat TEXT NOT NULL,
  dateCreation NUMERIC NOT NULL,
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

  
--
-- Contenu de la table `client`
--

INSERT INTO client (refClient, nom, prenom, adresse, codePostal, ville, email, telephone, dateCreation, dateModification) VALUES
('AT000001', 'Arthur', 'Tv', '10 chemin des Albios', '31130', 'Balma', 'arthur@gmail.com', '06-06-06-06-06', '2017-03-03', '2017-03-03'),
('BT000002', 'Beatrice', 'Tijuana', '9 chemin des iles', '31000', 'Toulouse', 'beatrice@gmail.com', '06-06-06-06-07', '2017-03-03', '2017-03-03'),
('MP000003', 'Marco', 'Polo', '2 rue de la paume', '75000', 'Paris', 'marco@gmail.com', '06-06-06-06-08', '2017-03-03', '2017-03-03'),
('JP000004', 'Jessica', 'Palmer', '69 rue de lalimapo', '33000', 'Bordeaux', 'jess@gmail.com', '06-06-06-06-08', '2017-03-03', '2017-03-05'),
('JW000005', 'Johny', 'Walker', '40 rue de la soif', '24000', 'Dordogne', 'johny@gmail.com', '06-06-06-06-09', '2017-03-06', '2017-03-06');


--
-- Contenu de la table `projet`
--

INSERT INTO projet (refProjet, nom, dateCreation, dateModification, refClient, refCommercial) VALUES
('CCAT000001', 'Arthur Tv', '2017-03-04', '2017-03-04', 'AT000001', 'COM003'),
('CCMP000002', 'Marco Polo', '2017-03-05', '2017-03-05', 'MP000003', 'COM003'),
('CCJW000003', 'Johny Walker', '2017-03-06', '2017-03-06', 'JW000005', 'COM003'),
('CCJP000004', 'Jessica Palmer', '2017-03-07', '2017-03-07', 'JP000004', 'COM003'),
('CCBT000005', 'Beatrice Tijuana', '2017-03-08', '2017-03-08', 'BT000002', 'COM003');

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
/*
INSERT INTO module (coordonneeDebutX, coordonneeDebutY, colspan, rowspan, refMetaModule, refMetaparent, refPlan) VALUES
(25, 35, 5, 0, 'M406587','none', 'CCAT000001-P01'),
(44, 44, 4, 0, 'M406587','none', 'CCAT000001-P01'),
(33, 33, 0, 3, 'M406587','none', 'CCAT000001-P01'),
(33, 33, 0, 3, 'F5646', 'M406587', 'CCAT000001-P01'),
(33, 33, 0, 3, 'F5646', 'M406587', 'CCAT000001-P01'),
(33, 33, 0, 3, 'F5646', 'M406587', 'CCAT000001-P01'),
(33, 33, 0, 3, 'F5646', 'M406587', 'CCAT000001-P01');
*/


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

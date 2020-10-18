CREATE DATABASE BibliographicalSourcesIntegratorWarehouseDB
GO
USE BibliographicalSourcesIntegratorWarehouseDB
GO



CREATE TABLE Book(
	ID int primary key NOT NULL,
	
	Title varchar(200) NULL,
	Year varchar(200) NULL,
	Url varchar(200) NULL,
	
	Editorial varchar(200) NULL,
	Pages int NULL
)

CREATE TABLE CongressComunication(
	ID int primary key NOT NULL,
	
	Title varchar(200) NULL,
	Year varchar(200) NULL,
	Url varchar(200) NULL,
	
	Congress varchar(200) NULL,
	Edition int NULL,
	Place varchar(200) NULL,
	InitialPage int NULL,
	FinalPage int NULL
)

CREATE TABLE Journal(
	ID int primary key NOT NULL,
	
	Name varchar(200) NULL
)

CREATE TABLE Exemplar(
	ID int primary key NOT NULL,
	
	Volume int NULL,
	Number int NULL,
	Month int NULL,

	ID_Journal int NULL,
	CONSTRAINT FK_Journal FOREIGN KEY(ID_Journal) REFERENCES Journal(ID)
)

CREATE TABLE Article(
	ID int primary key NOT NULL,
	
	Title varchar(200) NULL,
	Year int NULL,
	Url varchar(200) NULL,
	
	InitialPage int NULL,
	FinalPage int NULL,
	
	ID_Exemplar int NULL,
	CONSTRAINT FK_Exemplar FOREIGN KEY(ID_Exemplar) REFERENCES Exemplar(ID)
)

CREATE TABLE Person(
	ID int primary key NOT NULL,
	
	Name varchar(200) NULL,
	Surnames varchar(200) NULL
)

CREATE TABLE Publication_Person(
	ID int primary key NOT NULL,

	ID_Article int NULL,
	ID_Book int NULL,
	ID_CongressComunication int NULL,
	ID_Person int NULL,

	CONSTRAINT FK_Article FOREIGN KEY(ID_Article) REFERENCES Article(ID),
	CONSTRAINT FK_Book FOREIGN KEY(ID_Book) REFERENCES Book(ID),
	CONSTRAINT FK_CongressComunication FOREIGN KEY(ID_CongressComunication) REFERENCES CongressComunication(ID),
	CONSTRAINT FK_Person FOREIGN KEY(ID_Person) REFERENCES Person(ID)
)



INSERT INTO Article VALUES (1,'Nepe',1999,'nepe.com',3,5,null)
INSERT INTO Person VALUES (88,'Antonio','Carrasco Sanchez')
INSERT INTO Publication_Person VALUES (9,1,null,null,88)
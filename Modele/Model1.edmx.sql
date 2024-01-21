
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/21/2024 18:38:28
-- Generated from EDMX file: C:\c#\test\CiteU\Modele\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [CiteU];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK__Chambres__ID_Bat__398D8EEE]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Chambres] DROP CONSTRAINT [FK__Chambres__ID_Bat__398D8EEE];
GO
IF OBJECT_ID(N'[dbo].[FK_ChambresLits]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Lits] DROP CONSTRAINT [FK_ChambresLits];
GO
IF OBJECT_ID(N'[dbo].[FK_EtudiantsReservations]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Etudiants] DROP CONSTRAINT [FK_EtudiantsReservations];
GO
IF OBJECT_ID(N'[dbo].[FK_ReservationsLits]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Reservations] DROP CONSTRAINT [FK_ReservationsLits];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Batiments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Batiments];
GO
IF OBJECT_ID(N'[dbo].[Chambres]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Chambres];
GO
IF OBJECT_ID(N'[dbo].[Etudiants]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Etudiants];
GO
IF OBJECT_ID(N'[dbo].[Lits]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Lits];
GO
IF OBJECT_ID(N'[dbo].[Reservations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Reservations];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Batiments'
CREATE TABLE [dbo].[Batiments] (
    [ID_Batiment] int  NOT NULL,
    [Nom_Batiment] varchar(100)  NULL,
    [Adresse_Batiment] varchar(255)  NULL,
    [Description_Batiment] varchar(max)  NULL,
    [Nombre_Etages] int  NULL,
    [Nombre_max_chambre] int  NULL
);
GO

-- Creating table 'Chambres'
CREATE TABLE [dbo].[Chambres] (
    [ID_Chambre] int IDENTITY(1,1) NOT NULL,
    [ID_Batiment] int  NULL,
    [Nom_Chambre] nvarchar(max)  NULL,
    [Numero_Batiment] int  NULL,
    [Capacite] int  NULL,
    [Etage] int  NULL,
    [Statut] varchar(20)  NULL,
    [Numero_Chambre] int  NOT NULL
);
GO

-- Creating table 'Etudiants'
CREATE TABLE [dbo].[Etudiants] (
    [ID_Etudiant] int IDENTITY(1,1) NOT NULL,
    [Nom] varchar(50)  NULL,
    [Prenom] varchar(50)  NULL,
    [Date_Naissance] datetime  NULL,
    [Sexe] char(1)  NULL,
    [Telephone] varchar(15)  NULL,
    [Email] varchar(50)  NULL,
    [Handicape] int  NULL,
    [Reservations_ID_Reservation] int  NULL
);
GO

-- Creating table 'Lits'
CREATE TABLE [dbo].[Lits] (
    [id] int IDENTITY(1,1) NOT NULL,
    [ChambresID_Chambre] int  NOT NULL,
    [Reservations_ID_Reservation] int  NULL
);
GO

-- Creating table 'Reservations'
CREATE TABLE [dbo].[Reservations] (
    [ID_Reservation] int IDENTITY(1,1) NOT NULL,
    [ID_Etudiant] int  NULL,
    [ID_Chambre] int  NULL,
    [Date_Debut] datetime  NULL,
    [Date_Fin] datetime  NULL,
    [Statut_Paiement] varchar(20)  NULL,
    [Date_Payement] datetime  NULL,
    [Lits_id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID_Batiment] in table 'Batiments'
ALTER TABLE [dbo].[Batiments]
ADD CONSTRAINT [PK_Batiments]
    PRIMARY KEY CLUSTERED ([ID_Batiment] ASC);
GO

-- Creating primary key on [ID_Chambre] in table 'Chambres'
ALTER TABLE [dbo].[Chambres]
ADD CONSTRAINT [PK_Chambres]
    PRIMARY KEY CLUSTERED ([ID_Chambre] ASC);
GO

-- Creating primary key on [ID_Etudiant] in table 'Etudiants'
ALTER TABLE [dbo].[Etudiants]
ADD CONSTRAINT [PK_Etudiants]
    PRIMARY KEY CLUSTERED ([ID_Etudiant] ASC);
GO

-- Creating primary key on [id] in table 'Lits'
ALTER TABLE [dbo].[Lits]
ADD CONSTRAINT [PK_Lits]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [ID_Reservation] in table 'Reservations'
ALTER TABLE [dbo].[Reservations]
ADD CONSTRAINT [PK_Reservations]
    PRIMARY KEY CLUSTERED ([ID_Reservation] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ID_Batiment] in table 'Chambres'
ALTER TABLE [dbo].[Chambres]
ADD CONSTRAINT [FK__Chambres__ID_Bat__398D8EEE]
    FOREIGN KEY ([ID_Batiment])
    REFERENCES [dbo].[Batiments]
        ([ID_Batiment])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Chambres__ID_Bat__398D8EEE'
CREATE INDEX [IX_FK__Chambres__ID_Bat__398D8EEE]
ON [dbo].[Chambres]
    ([ID_Batiment]);
GO

-- Creating foreign key on [ChambresID_Chambre] in table 'Lits'
ALTER TABLE [dbo].[Lits]
ADD CONSTRAINT [FK_ChambresLits]
    FOREIGN KEY ([ChambresID_Chambre])
    REFERENCES [dbo].[Chambres]
        ([ID_Chambre])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ChambresLits'
CREATE INDEX [IX_FK_ChambresLits]
ON [dbo].[Lits]
    ([ChambresID_Chambre]);
GO

-- Creating foreign key on [Reservations_ID_Reservation] in table 'Etudiants'
ALTER TABLE [dbo].[Etudiants]
ADD CONSTRAINT [FK_EtudiantsReservations]
    FOREIGN KEY ([Reservations_ID_Reservation])
    REFERENCES [dbo].[Reservations]
        ([ID_Reservation])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EtudiantsReservations'
CREATE INDEX [IX_FK_EtudiantsReservations]
ON [dbo].[Etudiants]
    ([Reservations_ID_Reservation]);
GO

-- Creating foreign key on [Lits_id] in table 'Reservations'
ALTER TABLE [dbo].[Reservations]
ADD CONSTRAINT [FK_ReservationsLits]
    FOREIGN KEY ([Lits_id])
    REFERENCES [dbo].[Lits]
        ([id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ReservationsLits'
CREATE INDEX [IX_FK_ReservationsLits]
ON [dbo].[Reservations]
    ([Lits_id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------
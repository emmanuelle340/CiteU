
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/31/2024 05:00:35
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

IF OBJECT_ID(N'[dbo].[FK_dbo_Chambres_dbo_Batiments_ID_Batiment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Chambres] DROP CONSTRAINT [FK_dbo_Chambres_dbo_Batiments_ID_Batiment];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_Lits_dbo_Chambres_ChambresID_Chambre]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Lits] DROP CONSTRAINT [FK_dbo_Lits_dbo_Chambres_ChambresID_Chambre];
GO
IF OBJECT_ID(N'[dbo].[FK_ReservationsEtudiants]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Reservations] DROP CONSTRAINT [FK_ReservationsEtudiants];
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
IF OBJECT_ID(N'[dbo].[Trace_ReservationSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Trace_ReservationSet];
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
    [ID_Chambre] int  NULL,
    [Date_Debut] datetime  NULL,
    [Date_Fin] datetime  NULL,
    [Statut_Paiement] varchar(20)  NULL,
    [Date_Payement] datetime  NULL,
    [Lits_id] int  NOT NULL,
    [Etudiants_ID_Etudiant] int  NOT NULL
);
GO

-- Creating table 'Trace_ReservationSet'
CREATE TABLE [dbo].[Trace_ReservationSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ID_Chambre] int  NOT NULL,
    [Date_Fin] datetime  NOT NULL,
    [Lits_id] int  NOT NULL,
    [Etudiants_ID_Etudiant] int  NOT NULL
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

-- Creating primary key on [Id] in table 'Trace_ReservationSet'
ALTER TABLE [dbo].[Trace_ReservationSet]
ADD CONSTRAINT [PK_Trace_ReservationSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ID_Batiment] in table 'Chambres'
ALTER TABLE [dbo].[Chambres]
ADD CONSTRAINT [FK_dbo_Chambres_dbo_Batiments_ID_Batiment]
    FOREIGN KEY ([ID_Batiment])
    REFERENCES [dbo].[Batiments]
        ([ID_Batiment])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_Chambres_dbo_Batiments_ID_Batiment'
CREATE INDEX [IX_FK_dbo_Chambres_dbo_Batiments_ID_Batiment]
ON [dbo].[Chambres]
    ([ID_Batiment]);
GO

-- Creating foreign key on [ChambresID_Chambre] in table 'Lits'
ALTER TABLE [dbo].[Lits]
ADD CONSTRAINT [FK_dbo_Lits_dbo_Chambres_ChambresID_Chambre]
    FOREIGN KEY ([ChambresID_Chambre])
    REFERENCES [dbo].[Chambres]
        ([ID_Chambre])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_Lits_dbo_Chambres_ChambresID_Chambre'
CREATE INDEX [IX_FK_dbo_Lits_dbo_Chambres_ChambresID_Chambre]
ON [dbo].[Lits]
    ([ChambresID_Chambre]);
GO

-- Creating foreign key on [Etudiants_ID_Etudiant] in table 'Reservations'
ALTER TABLE [dbo].[Reservations]
ADD CONSTRAINT [FK_ReservationsEtudiants]
    FOREIGN KEY ([Etudiants_ID_Etudiant])
    REFERENCES [dbo].[Etudiants]
        ([ID_Etudiant])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ReservationsEtudiants'
CREATE INDEX [IX_FK_ReservationsEtudiants]
ON [dbo].[Reservations]
    ([Etudiants_ID_Etudiant]);
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
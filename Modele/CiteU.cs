using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace CiteU.Modele
{
    public partial class CiteU : DbContext
    {
        public CiteU()
            : base("name=CiteU")
        {
        }

        public virtual DbSet<Batiments> Batiments { get; set; }
        public virtual DbSet<Chambres> Chambres { get; set; }
        public virtual DbSet<Etudiants> Etudiants { get; set; }
        public virtual DbSet<Paiements> Paiements { get; set; }
        public virtual DbSet<Reservations> Reservations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Batiments>()
                .Property(e => e.Nom_Batiment)
                .IsUnicode(false);

            modelBuilder.Entity<Batiments>()
                .Property(e => e.Adresse_Batiment)
                .IsUnicode(false);

            modelBuilder.Entity<Batiments>()
                .Property(e => e.Description_Batiment)
                .IsUnicode(false);

            modelBuilder.Entity<Chambres>()
                .Property(e => e.Statut)
                .IsUnicode(false);

            modelBuilder.Entity<Etudiants>()
                .Property(e => e.Nom)
                .IsUnicode(false);

            modelBuilder.Entity<Etudiants>()
                .Property(e => e.Prenom)
                .IsUnicode(false);

            modelBuilder.Entity<Etudiants>()
                .Property(e => e.Sexe)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Etudiants>()
                .Property(e => e.Telephone)
                .IsUnicode(false);

            modelBuilder.Entity<Etudiants>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Paiements>()
                .Property(e => e.Montant)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Paiements>()
                .Property(e => e.Methode_Paiement)
                .IsUnicode(false);

            modelBuilder.Entity<Reservations>()
                .Property(e => e.Statut_Paiement)
                .IsUnicode(false);
        }
    }
}

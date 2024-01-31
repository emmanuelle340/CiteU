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
        public virtual DbSet<Lits> Lits { get; set; }
        public virtual DbSet<Reservations> Reservations { get; set; }
        public virtual DbSet<Trace_ReservationSet> Trace_ReservationSet { get; set; }

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

            modelBuilder.Entity<Batiments>()
                .HasMany(e => e.Chambres)
                .WithOptional(e => e.Batiments)
                .WillCascadeOnDelete();

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

            modelBuilder.Entity<Etudiants>()
                .HasMany(e => e.Reservations)
                .WithRequired(e => e.Etudiants)
                .HasForeignKey(e => e.Etudiants_ID_Etudiant)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Lits>()
                .HasMany(e => e.Reservations)
                .WithRequired(e => e.Lits)
                .HasForeignKey(e => e.Lits_id);

            modelBuilder.Entity<Reservations>()
                .Property(e => e.Statut_Paiement)
                .IsUnicode(false);
        }
    }
}

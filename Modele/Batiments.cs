namespace CiteU.Modele
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    public partial class Batiments
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Batiments()
        {
            Chambres = new HashSet<Chambres>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_Batiment { get; set; }

        [StringLength(100)]
        public string Nom_Batiment { get; set; }

        [StringLength(255)]
        public string Adresse_Batiment { get; set; }

        public string Description_Batiment { get; set; }

        public int? Nombre_Etages { get; set; }

        public int? Nombre_max_chambre { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Chambres> Chambres { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int NombreChambresVides
        {
            get { return CalculateNombreChambresVides(); }
            //private set { }

        }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int NombreChambresOccupees
        {
            get { return CalculateNombreChambresOccupees(); }
            //private set { }  
        }


        private int CalculateNombreChambresVides()
        {
            using (var context = new CiteU()) // Assurez-vous que le contexte est utilis? localement et est correctement dispos?
            {
                // Recherche des chambres disponibles pour ce b?timent
                int idBatiment = this.ID_Batiment;
                int chambresVides = context.Chambres.Count(c => c.ID_Batiment == idBatiment && c.Statut == "Disponible");

                return chambresVides;
            }
        }

        private int CalculateNombreChambresOccupees()
        {
            // Calcul du nombre de chambres occup?es en soustrayant les chambres vides du nombre total de chambres
            int nombreMaxChambres = (int)this.Nombre_max_chambre;  // Correction du nom de la propri?t?
            int chambresVides = CalculateNombreChambresVides();
            int chambresOccupees = nombreMaxChambres - chambresVides;

            return chambresOccupees;
        }

    }
}

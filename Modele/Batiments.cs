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

        public int? Nombre_max_chambre { get; set; }  // Correction du nom de la propriété

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Chambres> Chambres { get; set; }

        public int NombreChambresVides
        {
            get { return CalculateNombreChambresVides(); }
        }

        public int NombreChambresOccupees
        {
            get { return CalculateNombreChambresOccupees(); }
        }

        private int CalculateNombreChambresVides()
        {
            using (var context = new CiteU()) // Assurez-vous que le contexte est utilisé localement et est correctement disposé
            {
                // Recherche des chambres disponibles pour ce bâtiment
                int idBatiment = this.ID_Batiment;
                int chambresVides = context.Chambres.Count(c => c.ID_Batiment == idBatiment && c.Statut == "Disponible");

                return chambresVides;
            }
        }

        private int CalculateNombreChambresOccupees()
        {
            // Calcul du nombre de chambres occupées en soustrayant les chambres vides du nombre total de chambres
            int nombreMaxChambres = (int)this.Nombre_max_chambre;  // Correction du nom de la propriété
            int chambresVides = CalculateNombreChambresVides();
            int chambresOccupees = nombreMaxChambres - chambresVides;

            return chambresOccupees;
        }
    }
}

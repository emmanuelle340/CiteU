namespace CiteU.Modele
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Chambres
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Chambres()
        {
            Lits = new HashSet<Lits>();
        }

        [Key]
        public int ID_Chambre { get; set; }

        public int? ID_Batiment { get; set; }

        public string Nom_Chambre { get; set; }

        public int? Numero_Batiment { get; set; }

        public int? Capacite { get; set; }

        public int? Etage { get; set; }

        [StringLength(20)]
        public string Statut { get; set; }

        public int Numero_Chambre { get; set; }

        public virtual Batiments Batiments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Lits> Lits { get; set; }
    }
}

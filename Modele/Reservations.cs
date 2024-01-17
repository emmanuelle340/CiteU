namespace CiteU.Modele
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Reservations
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Reservations()
        {
            Lits = new HashSet<Lits>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_Reservation { get; set; }

        public int? ID_Etudiant { get; set; }

        public int? ID_Chambre { get; set; }

        public DateTime? Date_Debut { get; set; }

        public DateTime? Date_Fin { get; set; }

        [StringLength(20)]
        public string Statut_Paiement { get; set; }

        public virtual Etudiants Etudiants { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Lits> Lits { get; set; }
    }
}

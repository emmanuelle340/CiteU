namespace CiteU.Modele
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Etudiants
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Etudiants()
        {
            Reservations = new HashSet<Reservations>();
        }

        [Key]
        public int ID_Etudiant { get; set; }

        [StringLength(50)]
        public string Nom { get; set; }

        [StringLength(50)]
        public string Prenom { get; set; }

        public DateTime? Date_Naissance { get; set; }

        [StringLength(1)]
        public string Sexe { get; set; }

        [StringLength(15)]
        public string Telephone { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        public int? Handicape { get; set; }

        public int? Reservations_ID_Reservation { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reservations> Reservations { get; set; }
    }
}

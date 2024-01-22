namespace CiteU.Modele
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Reservations
    {
        [Key]
        public int ID_Reservation { get; set; }

        public int? ID_Etudiant { get; set; }

        public int? ID_Chambre { get; set; }

        public DateTime? Date_Debut { get; set; }

        public DateTime? Date_Fin { get; set; }

        [StringLength(20)]
        public string Statut_Paiement { get; set; }

        public DateTime? Date_Payement { get; set; }

        public int Lits_id { get; set; }

        public virtual Lits Lits { get; set; }
    }
}

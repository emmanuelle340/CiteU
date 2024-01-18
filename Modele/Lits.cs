namespace CiteU.Modele
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Lits
    {
        public int id { get; set; }

        public int ChambresID_Chambre { get; set; }

        public int? Reservations_ID_Reservation { get; set; }

        public virtual Chambres Chambres { get; set; }

        public virtual Reservations Reservations { get; set; }
    }
}

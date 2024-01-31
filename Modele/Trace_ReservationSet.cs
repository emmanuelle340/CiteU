namespace CiteU.Modele
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Trace_ReservationSet
    {
        public int Id { get; set; }

        public int ID_Chambre { get; set; }

        public DateTime Date_Fin { get; set; }

        public int Lits_id { get; set; }

        public int Etudiants_ID_Etudiant { get; set; }
    }
}

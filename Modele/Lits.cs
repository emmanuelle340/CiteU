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

        [StringLength(10)]
        public string ID_Chambre { get; set; }
    }
}

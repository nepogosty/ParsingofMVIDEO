namespace KP_Gamenotebook.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Reviews
    {
        [Key]
        [Column("ID review")]
        public int ID_review { get; set; }

        [Required]
        [StringLength(50)]
        public string Rating { get; set; }

        [Column("Review text")]
        public string Review_text { get; set; }

        [Column("ID model")]
        public int? ID_model { get; set; }

        public virtual Model Model { get; set; }
    }
}

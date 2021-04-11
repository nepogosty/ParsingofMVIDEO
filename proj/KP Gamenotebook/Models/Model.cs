namespace KP_Gamenotebook.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Model")]
    public partial class Model
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Model()
        {
            Reviews = new HashSet<Reviews>();
        }

        [Key]
        [Column("ID model")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_model { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Href { get; set; }

        [Required]
        public string Price { get; set; }

        [StringLength(50)]
        public string Bonuses { get; set; }

        public string Rating { get; set; }

        [Required]
        public string SSD { get; set; }

        public int RAM { get; set; }

        [Column("ID firm")]
        public int? ID_firm { get; set; }

        public int? ID_GC { get; set; }

        [Column("ID CPU")]
        public int? ID_CPU { get; set; }

        public virtual CPU CPU { get; set; }

        public virtual Firm Firm { get; set; }

        public virtual Graphiccard Graphiccard { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reviews> Reviews { get; set; }
    }
}

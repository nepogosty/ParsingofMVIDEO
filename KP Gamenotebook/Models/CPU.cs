namespace KP_Gamenotebook.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CPU")]
    public partial class CPU
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CPU()
        {
            Model = new HashSet<Model>();
        }

        [Key]
        [Column("ID CPU")]
        public int ID_CPU { get; set; }

        [Required]
        public string Name { get; set; }

        [Column("Count cores")]
        public int? Count_cores { get; set; }

        public string Frequency { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Model> Model { get; set; }
    }
}

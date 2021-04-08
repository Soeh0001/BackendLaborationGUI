namespace LibraryAPI
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Books
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Books()
        {
            Authors = new HashSet<Authors>();
        }

        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [StringLength(50)]
        public string Language { get; set; }

        [StringLength(50)]
        public string Genre { get; set; }

        public int Published { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Authors> Authors { get; set; }
    }
}

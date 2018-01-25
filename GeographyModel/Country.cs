using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeographyModel
{
    [Table("Countries")]
    public class Country
    {
        [Required]
        [StringLength(3)]
        [Column(TypeName = "varchar(2)")]
        public string Code { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "varchar(100)")]
        public string Permalink { get; set; }

        public ICollection<City> Cities { get; set; } 
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeographyModel
{
    [Table("Cities")]
    public class City
    {
        [Required]
        [StringLength(30)]
        [Column(TypeName = "varchar(30)")]
        public string Code { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "varchar(100)")]
        public string Permalink { get; set; }

        [Required]
        [StringLength(3)]
        [Column(TypeName = "varchar(2)")]
        public string CountryCode { get; set; }

        [ForeignKey("CountryCode")]
        public Country Country { get; set; }
    }
}
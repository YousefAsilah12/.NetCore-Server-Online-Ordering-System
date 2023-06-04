

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.DTO
{
    [Table("Supplier")]
    public class SupplierDTO
    {
        [Required]
        public string Image { get; set; }

        [Required]
        [MaxLength(50)]
        public string CompanyTitle { get; set; }


        [Required]
        [MaxLength(50)]
        public string CompanyName { get; set; }

        [Required]
        [MaxLength(50)]
        public string Address { get; set; }

        [Required]
        [MaxLength(20)]
        public string Phone { get; set; }

        [MaxLength(20)]
        public string Fax { get; set; }

        [MaxLength()]
        public string Email { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.DTO
{
    [Table("Category")]
    public class CategoryDTO
    {

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        public string Image { get; set; }

        public string ProductDescription { get; set; }

    }
}

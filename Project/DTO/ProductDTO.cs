

using Project.Moduls;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.DTO
{
    [Table("Product")]
    public class ProductDTO
    {

        public int SupplierId { get; set; }
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(50)]
        public string SerialNumber { get; set; }

        [Required]
        [MaxLength(50)]
        public string ProductName { get; set; }

        [Required]
        [MaxLength(50)]
        public string ProductDescription { get; set; }

        [Required]
        public float ProductPrice { get; set; }

        [Required]
        [MaxLength()]
        public string Image { get; set; }

        [Required]
        public bool Active { get; set; }


        public float Discount { get; set; }

        public int StockQuanitity { get; set; }

/*        public Supplier Supplier { get; set; }
        public Category Category { get; set; }

*/
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.DTO
{
    [Table("Cart")]
    public class CartDTO
    {
        public float TotalAmount { get; set; }
        public int? OrderId { get; set; }

        [Required]
        public string PaymentType { get; set; }

        public float Tax { get; set; }

    }
}

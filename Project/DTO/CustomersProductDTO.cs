
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.DTO
{
    [Table("CustomerProduct")]
    public class CustomersProductDTO
    {
        public int? CartId { get; set; }
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }

        public int Quantity { get; set; }

    }
}

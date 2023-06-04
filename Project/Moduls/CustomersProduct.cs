using System.ComponentModel.DataAnnotations;

namespace Project.Moduls
{
    public class CustomersProduct
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        //ForgienKey
        public int? CartId { get; set; }
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }

        public Product Product { get; set; }
        public Cart Cart { get; set; }

    }
}

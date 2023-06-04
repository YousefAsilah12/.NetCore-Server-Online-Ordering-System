
using System.ComponentModel.DataAnnotations;


namespace Project.Moduls
{
    public class Product
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public float ProductPrice  { get; set; }
        public string Image { get; set; }
        public bool Active { get; set; }
        public float Discount { get; set; }
        public int StockQuanitity { get; set; }

        //ForgienKey
        public int SupplierId { get; set; }
        public int CategoryId { get; set; }

        public Supplier Supplier { get; set; }
        public Category Category { get; set; }


    }
}

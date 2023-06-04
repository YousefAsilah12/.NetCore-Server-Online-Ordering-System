using System.Collections.Generic;

namespace Project.Moduls
{
    public class Cart
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public float TotalAmount { get; set; }
        public string PaymentType { get; set; }
        public float Tax { get; set; }


        //List Of CustomerProducts in the same Cart and order
        public List<CustomersProduct> Products { get; set; }
        public Order Order { get; set; }
    }
}

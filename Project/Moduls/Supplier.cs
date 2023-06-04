
using System.ComponentModel.DataAnnotations;


namespace Project.Moduls
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string CompanyTitle { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }

    }
}

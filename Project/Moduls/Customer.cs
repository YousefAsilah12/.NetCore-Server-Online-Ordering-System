using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project.Moduls
{
    public class Customer
    {
        public int Id {get;set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Image { get; set; }
        public string Password { get; set; }
        public DateTime Birthdate { get; set; }
        public string phone { get; set; }   
        public string Email { get; set; }    
        public int HomeNo { get; set; }
        public string Street { get; set; }
        public string City { get; set; }

/*        public List<Order> Orders { get; set; }
*/

    }
}

using System;
using System.Collections.Generic;

namespace Project.Moduls
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Image { get; set; }
        public string Password { get; set; }
        public int HomeNo { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public List<Order> Orders { get; set; }
    }
}

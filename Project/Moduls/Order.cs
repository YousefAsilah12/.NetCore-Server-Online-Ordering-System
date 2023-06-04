
using LinqToDB.Mapping;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Moduls
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        

        public int OrderNumber { get; set; }

        public DateTime OrderDate { get; set; }

        public float Frieght { get; set; }


        //ForgienKey
        public int? CustomerId { get; set; }
        public int? EmployeeId { get; set; }

        public Customer Customer { get; set; }
        public Employee Employee { get; set; }

        
        public Cart Cart { get; set; }

    }
}

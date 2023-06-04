using Project.Moduls;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.DTO
{
    [Table("Order")]
    public class OrderDTO
    {

        public int? CustomerId { get; set; }
        
        public int? EmployeeId { get; set; }


        public float Frieght { get; set; }



    }
}

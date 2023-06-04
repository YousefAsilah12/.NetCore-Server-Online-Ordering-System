
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Project.Moduls
{
    public class Category
    {
        internal object product;

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Image { get; set; }
        public string ProductDescription { get; set; }
        
        //List Of products in the same Category 
        public List<Product> CategoryProducts { get; set; }

    } 
}

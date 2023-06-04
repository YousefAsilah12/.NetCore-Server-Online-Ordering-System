using Project.DTO;
using Project.Moduls;
using System;

namespace Project.Extentions
{
    public static class DTOsEx
    {


        // cart model To DTO
        public static CartDTO ToDTO(this Cart model)
        {
            CartDTO dto = new CartDTO();
           // dto.CustomerProductId = model.CustomerProductId;
            dto.TotalAmount = model.TotalAmount;
            dto.Tax = model.Tax;
            dto.PaymentType = model.PaymentType;
            return dto;
        }
        //CartDTO TO Model 
        public static Cart ToModel(this CartDTO dto)
        {
            Cart model = new Cart();
            //model.CustomerProductId = dto.CustomerProductId;
            model.TotalAmount = dto.TotalAmount;
            model.Tax = dto.Tax;
            model.PaymentType = dto.PaymentType;
            model.OrderId = dto.OrderId;

            return model;
        }













        // categoryModel TO DTO
        public static CategoryDTO ToDTO(this Category model)
        {
            CategoryDTO dto = new CategoryDTO();
            dto.FirstName = model.FirstName;
            dto.Image = model.Image;
            dto.ProductDescription = model.ProductDescription;
            return dto;
        }
        //CategoryDTO To Model
        public static Category ToModel(this CategoryDTO dto)
        {
            Category model = new Category();
            model.FirstName = dto.FirstName;
            model.Image = dto.Image;
            model.ProductDescription = dto.ProductDescription;
            return model;
        }






        //Customer Model TO DTO
        public static CustomerDTO ToDTO(this Customer model)
        {
            CustomerDTO dto = new CustomerDTO();
            dto.FirstName = model.FirstName;
            dto.LastName = model.LastName;
            dto.Image = model.Image;
            dto.Password = model.Password;
            dto.Birthdate = model.Birthdate;
            dto.phone = model.phone;
            dto.Email = model.Email;
            dto.Street = model.Street;
            dto.City = model.City;
            return dto;
        }


        //CustomerDTO TO Model
        public static Customer ToModel(this CustomerDTO dto)
        {
            Customer model = new Customer();
            model.FirstName = dto.FirstName;
            model.LastName = dto.LastName;
            model.Image = dto.Image;
            model.Birthdate = dto.Birthdate;
            model.phone = dto.phone;
            model.Email = dto.Email;
            model.Street = dto.Street;
            model.City = dto.City;
            return model;
        }








        //CustomersProduct MOdel To DTO
        public static CustomersProductDTO ToDTO(this CustomersProduct model)
        {
            CustomersProductDTO dto = new CustomersProductDTO();
            dto.Quantity = model.Quantity;
            dto.CartId = model.CartId;
            dto.ProductId = model.ProductId;
            dto.OrderId = model.OrderId;
            return dto;
        }
        //CustomersProduct DTO to model
        public static CustomersProduct ToModel(this CustomersProductDTO dto)
        {
            CustomersProduct model = new CustomersProduct();
            model.Quantity = dto.Quantity;
            model.CartId = dto.CartId;
            model.ProductId = dto.ProductId;
            model.OrderId = dto.OrderId;
            return model;
        }







        //EmployeeMOdel To DTO
        public static EmployeeDTO ToDTO(this Employee model)
        {
            EmployeeDTO dto = new EmployeeDTO();
            dto.FirstName = model.FirstName;
            dto.LastName = model.LastName;
            dto.Image = model.Image;
            dto.Password = model.Password;
            dto.HomeNo = model.HomeNo;
            dto.Street = model.Street;
            dto.City = model.City;
            dto.HireDate = model.HireDate;
            dto.BirthDate = model.BirthDate;
            dto.Email = model.Email;
            dto.Role = model.Role;
            return dto;
        }
        //Emplotee DTO to Model
        public static Employee ToModel(this EmployeeDTO dto)
        {
            Employee model = new Employee();
            model.FirstName = dto.FirstName;
            model.LastName = dto.LastName;
            model.Image = dto.Image;
            model.Password = dto.Password;
            model.HomeNo = dto.HomeNo;
            model.Street = dto.Street;
            model.City = dto.City;
            model.HireDate = dto.HireDate;
            model.BirthDate = dto.BirthDate;
            model.Email = dto.Email;
            model.Role = dto.Role;
            return model;
        }






        //OrderModel TO DTO
        public static OrderDTO ToDTO(this Order model)
        {
            OrderDTO dto = new OrderDTO();
            dto.Frieght = model.Frieght;
           dto.CustomerId = model.CustomerId;
            dto.EmployeeId = model.EmployeeId;
            return dto;
        }
        //OrderDTO To Model
        public static Order ToModel(this OrderDTO dto)
        {
            Order model = new Order();
            model.OrderDate = DateTime.Now;
            model.Frieght = dto.Frieght;
            model.CustomerId = dto.CustomerId;
            model.EmployeeId = dto.EmployeeId;

            /*            model.Customer = dto.Customer.ToModel();
                        model.Employee = dto.Employee.ToModel();
            */
            return model;
        }





        //PRoductmodel  To DTO 
        public static ProductDTO ToDTO(this Product model)
        {
            ProductDTO dto = new ProductDTO();
            dto.SupplierId = model.SupplierId;
            dto.CategoryId = model.CategoryId;
            dto.SerialNumber = model.SerialNumber;
            dto.ProductName = model.ProductName;
            dto.ProductDescription = model.ProductDescription;
            dto.ProductPrice = model.ProductPrice;
            dto.Image = model.Image;
            dto.Active = model.Active;
            dto.Discount = model.Discount;
            dto.StockQuanitity = model.StockQuanitity;
            return dto;
        }
        //PoductDTO TO model
        public static Product ToModel(this ProductDTO dto)
        {
            Product model = new Product();
            model.SupplierId = dto.SupplierId;
            model.CategoryId = dto.CategoryId;
            model.SerialNumber = dto.SerialNumber;
            model.ProductName = dto.ProductName;
            model.ProductDescription = dto.ProductDescription;
            model.ProductPrice = dto.ProductPrice;
            model.Image = dto.Image;
            model.Active = dto.Active;
            model.Discount = dto.Discount;
            model.StockQuanitity = dto.StockQuanitity;
            return model;
        }








        //SupplierDTO To Model
        public static SupplierDTO ToDTO(this Supplier model)
        {
            SupplierDTO dto = new SupplierDTO();
            dto.Image = model.Image;
            dto.CompanyTitle = model.CompanyTitle;
            dto.CompanyName = model.CompanyName;
            dto.Address = model.Address;
            dto.Phone = model.Phone;
            dto.Fax = model.Fax;
            dto.Email = model.Email;
            return dto;
        }
        //SupplierModel TO DTO
        public static Supplier ToModel(this SupplierDTO dto)
        {
            Supplier model = new Supplier();
            model.Image = dto.Image;
            model.CompanyTitle = dto.CompanyTitle;
            model.CompanyName = dto.CompanyName;
            model.Address = dto.Address;
            model.Phone = dto.Phone;
            model.Fax = dto.Fax;
            model.Email = dto.Email;
            return model;
        }
    }
}

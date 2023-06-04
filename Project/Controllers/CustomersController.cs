using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Proj_1.Services;
using Project.DataBase;
using Project.DTO;
using Project.Extentions;
using Project.Moduls;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using Project.Services;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class CustomersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly Jwt m_jwtService;
        private readonly PasswordGeneratorClass _passwordGeneretorClass;

        [Obsolete]
        private readonly IHostingEnvironment _host;

        [Obsolete]
        public CustomersController(ApplicationDbContext context, Jwt _jwtService, IHostingEnvironment host, PasswordGeneratorClass passwordGeneretorClass)
        {
            _context = context;
            m_jwtService = _jwtService;
            _host = host;
            _passwordGeneretorClass = passwordGeneretorClass;

        }

        //פונקצית authorization 
        //אם המשתמש הזין פרטים נכונים מחזירה TOKEN 
        [HttpPost]
        [Route("auth")]
        [AllowAnonymous]//כל אחד יכול להזתמש בה 
        public IActionResult Auth([FromBody] AuthRequest request)
        {

            if (string.IsNullOrEmpty(request.email) || string.IsNullOrEmpty(request.password))
            {
                return BadRequest("you Must Enter User Name and Password");
            }
            Customer userFound = PostUser(request.email, request.password);

            if (userFound != null)
            {
                string JwtToken = m_jwtService.GenerateToken(userFound.Id.ToString(), "");

                return Ok(JwtToken);
            }
            return Unauthorized("Not Found");//for request 401 user Not Found 
        }

       



        // קבלת לקוח לפי ID
        [Route("{id?}")]
        [HttpGet]//קבלת נתונים 
        [AllowAnonymous]
        public async Task<ActionResult> GetUserAsync(int id = 0)
        {
            if (id < 1)
            {
                /*var result = _context.Customer.Include(C => C.Orders).Select(ee => new Customer()
                {
                    Id = ee.Id,
                    FirstName = ee.FirstName,
                    LastName = ee.LastName,
                    Image = ee.Image,
                    Password = ee.Password,
                    Birthdate = ee.Birthdate,
                    phone = ee.phone,
                    Email = ee.Email,
                    HomeNo = ee.HomeNo,
                    Street = ee.Street,
                    City = ee.City,
*//*                    Orders = ee.Orders
*//*
                }).ToList();
*/

                List<Customer> result = await _context.Customers.ToListAsync();
                return Ok(result);
            }
            string idFromJwt = m_jwtService.GetTokenClaims();
            id = int.Parse(idFromJwt);
            //check if not deafult

            Customer result2 = await _context.Customers.FindAsync(id);
            return Ok(result2);
        }

        //פונקציה לעדכון סיסמה
        //PUT : api/Customers/5
        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> PutPassword(int id, CustomerDTO customerdto)
        {
            if (id < 1)
            {
                return BadRequest();
            }


            // var Model = customerdto.ToModel();
            //Model.Id = id;
            Customer C = await _context.Customers.FindAsync(id);
            C.FirstName = customerdto.FirstName;
            C.LastName = customerdto.LastName;
            C.Image = customerdto.Image;
            C.Birthdate = customerdto.Birthdate;
            C.phone = customerdto.phone;
            C.Email = customerdto.Email;
            C.HomeNo = customerdto.HomeNo;
            C.Street = customerdto.Street;
            C.City = customerdto.City;
            
            if (C.Password != customerdto.Password)
            {
                C.Password = GetMD5(customerdto.Password);

            }
            //_context.Entry(Model).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        //עדכון פריטי לקוח
        // PUT: api/customer/5
        [HttpPut()]
        [Obsolete]
        public async Task<IActionResult> PutCustomer()
        {
            var id = int.Parse(HttpContext.Request.Form["id"]);
            if (id < 1)
            {
                return BadRequest();
            }
            Customer Customer = new Customer();


            var currDate = HttpContext.Request.Form["birthdate"];
            Customer.Birthdate = DateTime.Parse(currDate);



            Customer.Id = int.Parse(HttpContext.Request.Form["id"]);
            Customer.FirstName = HttpContext.Request.Form["firstName"];
            Customer.LastName = HttpContext.Request.Form["lastName"];
            Customer.Password = HttpContext.Request.Form["password"];
            Customer.phone = HttpContext.Request.Form["phone"];
            Customer.Email = HttpContext.Request.Form["email"];
            Customer.HomeNo = int.Parse(HttpContext.Request.Form["homeNo"]);
            Customer.Street = HttpContext.Request.Form["street"];
            Customer.City = HttpContext.Request.Form["city"];
            var Image = HttpContext.Request.Form.Files["image"];
            Customer.Image = HttpContext.Request.Form["imageNotChanged"];

            if (!string.IsNullOrEmpty(Customer.FirstName) && Image != null && Image.Length > 0)
            {
                var user = await this.PutUserAsync(Customer, Image);
                if (user != null)
                {
                    return Ok();
                }
            }
            else
            {

                _context.Entry(Customer).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok();

            }


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        [Obsolete]
        private async Task<Customer> PutUserAsync(Customer Customer, IFormFile image)
        {

            //שמירת תמונה התקיות של השרת 
            var filePath = Path.Combine(_host.WebRootPath + "/images/users/", image.FileName);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            var Cus = new Customer
            {
                Id = Customer.Id,
                FirstName = Customer.FirstName,
                LastName = Customer.LastName,
                Image = image.FileName,
                Password = Customer.Password,
                Birthdate = Customer.Birthdate,
                phone = Customer.phone,
                Email = Customer.Email,
                HomeNo = Customer.HomeNo,
                Street = Customer.Street,
                City = Customer.City,
/*                Orders = Customer.Orders
*/            };
            if (Customer.Password != Cus.Password)
            {
                Cus.Password = GetMD5(Customer.Password);

            }

            _context.Entry(Cus).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Cus;
        }





        //פונקצית מנפיקה סיסמה חדשה ומעדכנת בבסיס נתונים ושולחת מייל עם הסיסמה החדשה
        [Route("ForgetPassword/{email}")]
        [HttpPost()]
        public async Task<IActionResult> ForgetPasswordAsync(string email)
        {
            //var email = HttpContext.Request.Form["email"];
            var userId = _context.Customers
             .Where(m => m.Email == email)
         .Select(m => m.Id)
             .SingleOrDefault();
            if (userId > 0 && this.CustomerExists(userId))
            {
                Customer c = new Customer();
                c = await _context.Customers.FindAsync(userId);
                String newPass = PasswordGeneratorClass.Main();
                var fromAddress = new MailAddress("yasilah4@gmail.com", "StudioHatnaResturant");
                var toAddress = new MailAddress(c.Email, "Mr." + c.FirstName + "" + c.LastName);
                const string fromPassword = "vtlbjvyepoyumkcv";
                const string subject = "StudioHatna Resturant ForgetPassword";
                string body = "here is Your New Password </br>  <b>'" + newPass + "' </b>  You Can Login In and then change your password from the user sitting page ";
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    message.IsBodyHtml = true;
                    smtp.Send(message);

                }
                CustomerDTO customerdto = c.ToDTO();
                customerdto.Password = newPass;
                this.PutPassword(userId, customerdto);

                return Ok();
            }
            else
            {
                return BadRequest("email Not found");
            }

        }

  



        //הוספת לקוח
        //Post : api/Customers
        [HttpPost]
        [AllowAnonymous]
        [Obsolete]
        public async Task<IActionResult> PostCustomer()
        {
            //receive the object ==>

            CustomerDTO customerdto = new CustomerDTO();


            var currDate = HttpContext.Request.Form["birthdate"];
            customerdto.Birthdate = DateTime.Parse(currDate);




            customerdto.FirstName = HttpContext.Request.Form["firstName"];
            customerdto.LastName = HttpContext.Request.Form["lastName"];
            customerdto.Password = HttpContext.Request.Form["password"];
            customerdto.phone = HttpContext.Request.Form["phone"];
            customerdto.Email = HttpContext.Request.Form["email"];
            customerdto.HomeNo = int.Parse(HttpContext.Request.Form["homeNo"]);
            customerdto.Street = HttpContext.Request.Form["street"];
            customerdto.City = HttpContext.Request.Form["city"];
            var Image = HttpContext.Request.Form.Files["image"];
            if (!string.IsNullOrEmpty(customerdto.FirstName) && Image != null && Image.Length > 0)
            {
                var user = await this.addUserAsync(customerdto, Image);
                if (user != null)
                {
                    return Ok();
                }
            }



            return BadRequest();

            //return CreatedAtAction("GetCustomers", new { id = Model.Id }, Model);
        }

        [Obsolete]
        private async Task<Customer> addUserAsync(CustomerDTO customerdto, IFormFile image)
        {

            //שמירת תמונה בתוך תקיות השרת
            var filePath = Path.Combine(_host.WebRootPath + "/images/users/", image.FileName);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            var Cusomer = new Customer
            {
                FirstName = customerdto.FirstName,
                LastName = customerdto.LastName,
                Image = image.FileName,
                Password = GetMD5(customerdto.Password),
                Birthdate = customerdto.Birthdate,
                phone = customerdto.phone,
                Email = customerdto.Email,
                HomeNo = customerdto.HomeNo,
                Street = customerdto.Street,
                City = customerdto.City,
                
            };
            _context.Customers.Add(Cusomer);
            await _context.SaveChangesAsync();
            return Cusomer;

        }





        //פונקציה שמוחקת לקוח 
        [HttpDelete("{id}")]
        public async Task<ActionResult<Customer>> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return customer;
        }


        //פונקציה שבודקת אם הלקוח קיים או לא 
        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(c => c.Id == id);
        }


        //פונקציה שמחזירה את אוביקא הלקוח כש הוא מזיו מייל וסיסמה נכונים
        private Customer PostUser(string email, string password)
        {
            string passwordAfterMD5 = GetMD5(password);

            return _context.Customers.Where(i => i.Email.ToLower() == email.ToLower() && i.Password == passwordAfterMD5)
                .FirstOrDefault();//lampada
        }

        //פונקציה שמצפינה את הסיסמה של הלקוח 
        //למשל הזין : TTO1234
        //יצא :jij31@123
        private string GetMD5(string input) //123
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.ASCII.GetBytes(input));
                var strResult = BitConverter.ToString(result);
                return strResult.Replace("-", "");
            }

        }
    }


}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proj_1.Services;
using Project.DataBase;
using Project.DTO;
using Project.Extentions;
using Project.Moduls;
namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles="Admin")]//רק עובד עם רמת מנהל יוכל להיכנס
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly Jwt m_jwtService;//שירות של TOKEN


        [Obsolete]
        private readonly IHostingEnvironment _host;//ספרייה שעוזרת לשמור תקיות 

        [Obsolete]
        public EmployeesController(ApplicationDbContext context, Jwt _jwtService, IHostingEnvironment host)
        {
            _context = context;
            m_jwtService = _jwtService;
            _host = host;

        }

        //פונקצית שבודקת אם עובד קיים או לא 
        private Employee PostUser(string email, string password)
        {
            string passwordAfterMD5 = GetMD5(password);

            return _context.Employee.Where(i => i.Email.ToLower() == email.ToLower() && i.Password == passwordAfterMD5)//lampada
                .FirstOrDefault();
        }

        //פונקצית כניסה לאזור המנהל וקבלת TOKEN
        [HttpPost]
        [Route("auth")]
        [AllowAnonymous]//כל אחד יכול להיכנס לאזור הזה
        public IActionResult Auth([FromBody] AuthRequest request)
        {

            if (string.IsNullOrEmpty(request.email) || string.IsNullOrEmpty(request.password))
            {
                return BadRequest("you Must Enter User Name and Password");
            }
            Employee userFound = PostUser(request.email, request.password);

            if (userFound != null)
            {
                string JwtToken = m_jwtService.GenerateToken(userFound.Id.ToString(),userFound.Role);

                return Ok(JwtToken);
            }
            return Unauthorized("Not Found");//for request 401 user Not Found 
        }



        //פונקציה שמביאה כל העובדעים או עובד ספיציפי
        // GET: api/Employees/5
        [HttpGet]
        [Route("{id?}")]
        [AllowAnonymous]
        public async Task<ActionResult<Employee>> GetEmployee(int id=0)
        {
            if (id<1)
            {
                List<Employee> result = await _context.Employee.ToListAsync();
                return Ok(result);

            }
            var employee = await _context.Employee.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }
            
            return employee;
        }


        //פונקצית עדכון פריטי עובד
        // PUT: api/Employees/5
        [HttpPut()]
        [Obsolete]
        public async Task<IActionResult> PutEmployee()
        {
           var  id= int.Parse(HttpContext.Request.Form["id"]);
            if (id < 1)
            {
                return BadRequest();
            }
            Employee Employee = new Employee();

            var birthDate = HttpContext.Request.Form["birthdate"];
            var hireDate = HttpContext.Request.Form["hireDate"];

            Employee.Id = id;
            Employee.FirstName = HttpContext.Request.Form["firstName"];
            Employee.LastName = HttpContext.Request.Form["lastName"];
            Employee.Password = HttpContext.Request.Form["password"];
            Employee.Email = HttpContext.Request.Form["email"];
            Employee.HomeNo = int.Parse(HttpContext.Request.Form["homeNo"]);
            Employee.Street = HttpContext.Request.Form["street"];
            Employee.City = HttpContext.Request.Form["city"];
            Employee.Role = HttpContext.Request.Form["role"];
            var Image = HttpContext.Request.Form.Files["image"];
            Employee.BirthDate = DateTime.Parse(birthDate);
            Employee.HireDate = DateTime.Parse(hireDate);

            if (!string.IsNullOrEmpty(Employee.FirstName) && Image != null && Image.Length > 0)
            {
                var user = await this.PutEmpAsync(Employee, Image);
                if (user != null)
                {
                    return Ok();
                }
            }


            try
            {
                _context.Entry(Employee).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
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
        private async Task<Employee> PutEmpAsync(Employee employee, IFormFile image)
        {
            var filePath = Path.Combine(_host.WebRootPath + "/images/Employees/", image.FileName);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            var emp = new Employee
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Image = image.FileName,
                Password = employee.Password,
                HomeNo = employee.HomeNo,
                Street = employee.Street,
                City = employee.City,
                HireDate = employee.HireDate,
                BirthDate = employee.BirthDate,
                Role = employee.Role,
                Email = employee.Email,
            };
            if (employee.Password != emp.Password)
            {
                emp.Password = GetMD5(employee.Password);

            }

            _context.Entry(emp).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return emp;
        }

        /*
                //פונקצית יצירת עובד חדש
                [HttpPost]
                [Obsolete]
                [AllowAnonymous]
                public async Task<IActionResult> PostEmployee()
                {

                    Employee EmployeeDto = new Employee();
                    EmployeeDto.FirstName = "yousef";
                    EmployeeDto.LastName = "yousef";
                    EmployeeDto.Image = "yousef";
                    EmployeeDto.Password = "yousef";
                    EmployeeDto.HomeNo = "yousef";
                    EmployeeDto.Street = "yousef";
                    EmployeeDto.FirstName = "yousef";
                    EmployeeDto.FirstName = "yousef";


                    var user = await this.addEmpAsync(Employeedto, Image);
                        if (user != null)
                        {
                            return Ok();
                        }




                    return BadRequest();

                    //return CreatedAtAction("GetCustomers", new { id = Model.Id }, Model);
                }*/


        //פונקצית יצירת עובד חדש
        [HttpPost]
        [Obsolete]
        [AllowAnonymous]
        public async Task<IActionResult> PostEmployee()
        {
            //receive the object ==>

            EmployeeDTO Employeedto = new EmployeeDTO();


            var birthDate = HttpContext.Request.Form["birthdate"];
            var hireDate = HttpContext.Request.Form["hireDate"];




            Employeedto.FirstName = HttpContext.Request.Form["firstName"];
            Employeedto.LastName = HttpContext.Request.Form["lastName"];
            Employeedto.Password = HttpContext.Request.Form["password"];
            Employeedto.Email = HttpContext.Request.Form["email"];
            Employeedto.HomeNo = int.Parse(HttpContext.Request.Form["homeNo"]);
            Employeedto.Street = HttpContext.Request.Form["street"];
            Employeedto.City = HttpContext.Request.Form["city"];
            Employeedto.Role = HttpContext.Request.Form["role"];
            var Image = HttpContext.Request.Form.Files["image"];
            Employeedto.BirthDate = DateTime.Parse(birthDate);
            Employeedto.HireDate = DateTime.Parse(hireDate);


            if (!string.IsNullOrEmpty(Employeedto.FirstName) && Image != null && Image.Length > 0)
            {
                var user = await this.addEmpAsync(Employeedto, Image);
                if (user != null)
                {
                    return Ok();
                }
            }



            return BadRequest();

            //return CreatedAtAction("GetCustomers", new { id = Model.Id }, Model);
        }


        [Obsolete]
        private async Task<Employee> addEmpAsync(EmployeeDTO employeedto, IFormFile image)
        {
            var filePath = Path.Combine(_host.WebRootPath + "/images/Employees/", image.FileName);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            var employee = new Employee
            {
                FirstName = employeedto.FirstName,
                LastName = employeedto.LastName,
                Image = image.FileName,
                Password = GetMD5(employeedto.Password),
                HomeNo = employeedto.HomeNo,
                Street = employeedto.Street,
                City = employeedto.City,
                HireDate = employeedto.HireDate,
                BirthDate = employeedto.BirthDate,
                Role = employeedto.Role,
                Email = employeedto.Email,
            };
            _context.Employee.Add(employee);
            await _context.SaveChangesAsync();
            return employee;

        }


        //פונקצית מחיקת עובד
        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Employee>> DeleteEmployee(int id)
        {
            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();

            return employee;
        }

        //פונקציה שבודקת אם עובד קיים או לא 
        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.Id == id);
        }

        //פונקצית הצפנת סיסמה
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
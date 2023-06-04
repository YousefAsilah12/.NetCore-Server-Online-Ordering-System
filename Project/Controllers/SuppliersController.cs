using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.DataBase;
using Project.DTO;
using Project.Extentions;
using Project.Moduls;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        [Obsolete]
        private readonly IHostingEnvironment _host;

        [Obsolete]
        public SuppliersController(ApplicationDbContext context, IHostingEnvironment host)
        {
            _context = context;
            _host = host;
        }

        //מקבל כל הספקים
        // GET: api/Suppliers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetSupplier()
        {
            return await _context.Supplier.ToListAsync();
        }

        // GET: api/Suppliers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Supplier>> GetSupplier(int id)
        {
            var supplier = await _context.Supplier.FindAsync(id);

            if (supplier == null)
            {
                return NotFound();
            }

            return supplier;
        }

        //עדכון פרטי ספק
        // PUT: api/Supplier/5
        [HttpPut()]
        [Obsolete]
        public async Task<IActionResult> PutSupplier()
        {
            var id = int.Parse(HttpContext.Request.Form["id"]);
            if (id < 1)
            {
                return BadRequest();
            }
            Supplier supplier = new Supplier();

            supplier.Id = int.Parse(HttpContext.Request.Form["id"]);
            supplier.CompanyTitle = HttpContext.Request.Form["companyTitle"];
            supplier.CompanyName = HttpContext.Request.Form["companyName"];
            supplier.Address = HttpContext.Request.Form["Address"];
            supplier.Phone = HttpContext.Request.Form["phone"];
            supplier.Fax = HttpContext.Request.Form["fax"];
            supplier.Email = HttpContext.Request.Form["email"];
            var Image = HttpContext.Request.Form.Files["image"];
            supplier.Image = HttpContext.Request.Form["imageNotChanged"];

            if (!string.IsNullOrEmpty(supplier.CompanyTitle) && Image != null && Image.Length > 0)
            {
                var user = await this.PutSupplierAsync(supplier, Image);
                if (user != null)
                {
                    return Ok();
                }
            }
            else
            {

                _context.Entry(supplier).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok();

            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SupplierExists(id))
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
        private async Task<Supplier> PutSupplierAsync(Supplier supplier, IFormFile image)
        {
            var filePath = Path.Combine(_host.WebRootPath + "/images/Suppliers/", image.FileName);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            var Sup = new Supplier
            {
                Id = supplier.Id,
                CompanyTitle = supplier.CompanyTitle,
                CompanyName = supplier.CompanyName,
                Address = supplier.Address,
                Email = supplier.Email,
                Fax = supplier.Fax,
                Phone = supplier.Phone,
                Image = image.FileName

            };

            _context.Entry(Sup).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Sup;
        }



        // Post WIth Image 
        [Obsolete]
        public async Task<IActionResult> PostSupplier()
        {
            //receive the object ==>

            SupplierDTO supplierdto = new SupplierDTO();
            supplierdto.CompanyTitle = HttpContext.Request.Form["companyTitle"];
            supplierdto.CompanyName = HttpContext.Request.Form["companyName"];
            supplierdto.Address = HttpContext.Request.Form["Address"];
            supplierdto.Phone = HttpContext.Request.Form["phone"];
            supplierdto.Fax = HttpContext.Request.Form["fax"];
            supplierdto.Email = HttpContext.Request.Form["email"];
            var Image = HttpContext.Request.Form.Files["image"];

            if (!string.IsNullOrEmpty(supplierdto.CompanyName) && Image != null && Image.Length > 0)
            {
                var category = await this.addSupplierAsync(supplierdto, Image);
                if (category != null)
                {
                    return Ok();
                }
            }



            return BadRequest();

            //return CreatedAtAction("GetCustomers", new { id = Model.Id }, Model);
        }

        [Obsolete]
        private async Task<Supplier> addSupplierAsync(SupplierDTO supplierdto, IFormFile image)
        {
            //שמירת תמונה בתוך תיקיות השרת
            var filePath = Path.Combine(_host.WebRootPath + "/images/Suppliers/", image.FileName);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            var sup = new Supplier
            {
                CompanyTitle = supplierdto.CompanyTitle,
                CompanyName = supplierdto.CompanyName,
                Address = supplierdto.Address,
                Email = supplierdto.Email,
                Fax = supplierdto.Fax,
                Phone = supplierdto.Phone,
                Image = image.FileName
            };
            _context.Supplier.Add(sup);
            await _context.SaveChangesAsync();
            return sup;

        }

        //מחיקת ספק
        // DELETE: api/Suppliers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Supplier>> DeleteSupplier(int id)
        {
            var supplier = await _context.Supplier.FindAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }

            _context.Supplier.Remove(supplier);
            await _context.SaveChangesAsync();

            return supplier;
        }

        //בדיקה אם ספק קיים
        private bool SupplierExists(int id)
        {
            return _context.Supplier.Any(e => e.Id == id);
        }
    }
}

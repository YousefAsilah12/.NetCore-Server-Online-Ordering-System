using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class CustomersProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CustomersProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CustomersProducts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomersProduct>>> GetCustomerProduct()
        {
            return await _context.CustomerProduct.ToListAsync();
        }

        // GET: api/CustomersProducts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomersProduct>> GetCustomersProduct(int id)
        {
            var customersProduct = await _context.CustomerProduct.FindAsync(id);

            if (customersProduct == null)
            {
                return NotFound();
            }

            return customersProduct;
        }

        // PUT: api/CustomersProducts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomersProduct(int id, CustomersProductDTO customersProductdto)
        {
            if (id < 1)
            {
                return BadRequest();
            }


            var Model = customersProductdto.ToModel();
            Model.Id = id;
            _context.Entry(Model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomersProductExists(id))
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

        // POST: api/CustomersProducts
        [HttpPost]
        public async Task<ActionResult<CustomersProductDTO>> PostCustomersProduct(CustomersProductDTO customersProductdto)
         {
            var Model = customersProductdto.ToModel();
            _context.CustomerProduct.Add(Model);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomersProduct", new { id = Model.Id }, Model);
        }

        // DELETE: api/CustomersProducts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CustomersProduct>> DeleteCustomersProduct(int id)
        {
            var customersProduct = await _context.CustomerProduct.FindAsync(id);
            if (customersProduct == null)
            {
                return NotFound();
            }

            _context.CustomerProduct.Remove(customersProduct);
            await _context.SaveChangesAsync();

            return customersProduct;
        }
    
        private bool CustomersProductExists(int id)
        {
            return _context.CustomerProduct.Any(e => e.Id == id);
        }
    }
}

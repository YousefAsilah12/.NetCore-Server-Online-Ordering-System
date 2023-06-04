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
    public class CartsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CartsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Carts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCart()
        {
            //return await _context.Cart.ToListAsync();
            var cart = _context.Cart.Include(C => C.Products).Select(ee => new Cart()
            {
                Id = ee.Id,
                OrderId = ee.OrderId,
                TotalAmount = ee.TotalAmount,
                PaymentType = ee.PaymentType,
                Tax = ee.Tax,
                Products = ee.Products,
                Order=ee.Order

            }).ToList();
            if (cart == null)
            {
                return NotFound();
            }

            return cart;

        }

        // GET: api/Carts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cart>> GetCart(int id)
        {
            var cart = await _context.Cart.FindAsync(id);

            if (cart == null)
            {
                return NotFound();
            }

            return cart;
        }

        // PUT: api/Carts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCart(int id, CartDTO cartDTO)
        {
            if (id < 1)
            {
                return BadRequest();
            }
            var Model = cartDTO.ToModel();
            Model.Id = id;
            _context.Entry(Model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartExists(id))
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

        // POST: api/Carts
        [HttpPost]
        public async Task<ActionResult<CartDTO>> PostCart(CartDTO cart)
        {
            var model = cart.ToModel();
            _context.Cart.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCart", new { Id = model.Id }, model);
        }

        // DELETE: api/Carts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Cart>> DeleteCart(int id)
        {
            var cart = await _context.Cart.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }

            _context.Cart.Remove(cart);
            await _context.SaveChangesAsync();

            return cart;
        }



        private bool CartExists(int id)
        {
            return _context.Cart.Any(e => e.Id == id);
        }
    }
}

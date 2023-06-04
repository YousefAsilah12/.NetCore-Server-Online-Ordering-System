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
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        [Obsolete]
        private readonly IHostingEnvironment _host;

        [Obsolete]
        public ProductsController(ApplicationDbContext context, IHostingEnvironment host)
        {
            _context = context;
            _host = host;

        }


        //קבלת אבלאות מוצרים כל מוצר עם טבלת קטגוקיה שלו ו הספק 
        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct()
        {

            var e = _context.Product.Include(C => C.Category).Select(ee => new Product()
            {
                Id = ee.Id,
                SupplierId = ee.SupplierId,
                CategoryId = ee.CategoryId,
                SerialNumber = ee.SerialNumber,
                ProductName = ee.ProductName,
                Discount = ee.Discount,
                ProductDescription = ee.ProductDescription,
                ProductPrice = ee.ProductPrice,
                Image = ee.Image,
                Active = ee.Active,
                StockQuanitity = ee.StockQuanitity,
                Supplier = ee.Supplier,
                Category = ee.Category,

            }).ToList();
            return e; 

           
        }

        //קבךת מוצר לפי הקוד הקטגוריה
        // GET: api/Products/ByCatagoryId/id
        [HttpGet("ByCatagoryId/{catId}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCatagoryId(int catId)
        {
            return await _context.Product.Where(p => p.CategoryId == catId).ToListAsync();
        }


        //קבלת מוצר לפי קוד מוצר
        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Product.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }


        //עדכון פרטי מוצר
        // PUT: api/customer/5
        [HttpPut()]
        [Obsolete]
        public async Task<IActionResult> PutProduct()
        {
            var id = int.Parse(HttpContext.Request.Form["id"]);
            if (id < 1)
            {
                return BadRequest();
            }
            Product product = new Product();




            product.Id = int.Parse(HttpContext.Request.Form["id"]);
            product.ProductName = HttpContext.Request.Form["productName"];
            product.SupplierId = int.Parse(HttpContext.Request.Form["supplierId"]);
            product.CategoryId = int.Parse(HttpContext.Request.Form["categoryId"]);
            product.ProductDescription = HttpContext.Request.Form["productDescription"];
            product.ProductPrice = int.Parse(HttpContext.Request.Form["productPrice"]);
            product.Active = Boolean.Parse(HttpContext.Request.Form["active"]);
            product.Discount = float.Parse(HttpContext.Request.Form["discount"]);
            product.StockQuanitity = int.Parse(HttpContext.Request.Form["stockQuantity"]);
            product.SerialNumber = HttpContext.Request.Form["serialNumber"];
            var Image = HttpContext.Request.Form.Files["image"];
            product.Image = HttpContext.Request.Form["imageNotChanged"];

            if (!string.IsNullOrEmpty(product.ProductName) && Image != null && Image.Length > 0)
            {
                var user = await this.PutUserAsync(product, Image);
                if (user != null)
                {
                    return Ok();
                }
            }
            else
            {

                _context.Entry(product).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok();

            }


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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
        private async Task<Product> PutUserAsync(Product product, IFormFile image)
        {
            var filePath = Path.Combine(_host.WebRootPath + "/images/Products/", image.FileName);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            var Pr = new Product
            {
                Id=product.Id,
                ProductName = product.ProductName,
                SupplierId = product.SupplierId,
                Image = image.FileName,
                CategoryId = product.CategoryId,
                ProductDescription = product.ProductDescription,
                ProductPrice = product.ProductPrice,
                Discount = product.Discount,
                Active = product.Active,
                StockQuanitity = product.StockQuanitity,
                SerialNumber = product.SerialNumber,

            };

            _context.Entry(Pr).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Pr;
        }


        //יצירת מוצר חדש
        //Post : api/Customers
        [HttpPost]
        [Obsolete]
        public async Task<IActionResult> PostProduct()
        {
            //receive the object ==>

            ProductDTO productdto = new ProductDTO();

            productdto.ProductName = HttpContext.Request.Form["productName"];
            productdto.SupplierId =int.Parse( HttpContext.Request.Form["supplierId"]);
            productdto.CategoryId =int.Parse( HttpContext.Request.Form["categoryId"]);
            productdto.ProductDescription = HttpContext.Request.Form["productDescription"];
            productdto.ProductPrice = int.Parse(HttpContext.Request.Form["productPrice"]);
            productdto.Active = Boolean.Parse(HttpContext.Request.Form["active"]);
            productdto.Discount =float.Parse( HttpContext.Request.Form["discount"]);
            productdto.StockQuanitity =int.Parse( HttpContext.Request.Form["stockQuantity"]);
            productdto.SerialNumber = HttpContext.Request.Form["serialNumber"];

            var Image = HttpContext.Request.Form.Files["image"];
            if (!string.IsNullOrEmpty(productdto.ProductName) && Image != null && Image.Length > 0)
            {
                var user = await this.addProductAsync(productdto, Image);
                if (user != null)
                {
                    return Ok();
                }
            }



            return BadRequest();

        }

        [Obsolete]
        private async Task<Product> addProductAsync(ProductDTO productdto, IFormFile image)
        {
            //שמירת תמונה בתוך תקיות השרת
            var filePath = Path.Combine(_host.WebRootPath + "/images/Products/", image.FileName);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            var product = new Product
            {
                ProductName = productdto.ProductName,
                SupplierId = productdto.SupplierId,
                Image = image.FileName,
                CategoryId = productdto.CategoryId,
                ProductDescription = productdto.ProductDescription,
                ProductPrice = productdto.ProductPrice,
                Discount = productdto.Discount,
                Active = productdto.Active,
                StockQuanitity = productdto.StockQuanitity,
                SerialNumber = productdto.SerialNumber,

            };
            _context.Product.Add(product);
            await _context.SaveChangesAsync();
            return product;

        }






        //מחיקת מוצר
        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return product;
        }

        //בדיקת מטצר קיים או לא 
        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}

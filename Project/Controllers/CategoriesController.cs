using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using Project.DataBase;
using Project.DTO;
using Project.Extentions;
using Project.Moduls;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        [Obsolete]
        private readonly IHostingEnvironment _host;

        [Obsolete]
        public CategoriesController(ApplicationDbContext context, IHostingEnvironment host)
        {
            _context = context;
            _host = host;

        }


        //מחזיר רשימה עם כל הקטיגוריות
        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategory()
        {
            
            var e = _context.Category.Include(C => C.CategoryProducts).Select(ee => new Category()
            {
                Id = ee.Id,
                FirstName = ee.FirstName,
                Image = ee.Image,
                ProductDescription = ee.ProductDescription,
                CategoryProducts = ee.CategoryProducts

            }).ToList();
            return e;



        }


        //מחזיר קטיגוריה עם קטיגוריה עם ID
        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<Category> GetCategory(int id)
        {
            var category =  _context.Category.Where(I => I.Id == id).Include(CP => CP.CategoryProducts).Select(ee => new Category()
            {
                Id = ee.Id,
                FirstName = ee.FirstName,
                Image = ee.Image,
                ProductDescription = ee.ProductDescription,
                CategoryProducts = ee.CategoryProducts
            }).FirstOrDefault();

           // var category = await _context.Category.Include(C => C.CategoryProducts).Where(p => p.Id == id).First();

            if (category == null)
            {
                return null;
            }

            return category;
        }

        
        //מעדכן קטגוריה
        // PUT: api/customer/5
        [HttpPut()]
        [Obsolete]
        public async Task<IActionResult> PutCategory()
        {
            var id = int.Parse(HttpContext.Request.Form["id"]);
            if (id < 1)
            {
                return BadRequest();
            }
            Category category = new Category();




            category.Id = int.Parse(HttpContext.Request.Form["id"]);
            category.FirstName = HttpContext.Request.Form["firstName"];
            category.ProductDescription = HttpContext.Request.Form["ProductDescription"];
            category.ProductDescription = HttpContext.Request.Form["ProductDescription"];
            var Image = HttpContext.Request.Form.Files["image"];
            category.Image = HttpContext.Request.Form["imageNotChanged"];

            if (!string.IsNullOrEmpty(category.FirstName) && Image != null && Image.Length > 0)
            {
                var user = await this.PutCategoryAsync(category, Image);
                if (user != null)
                {
                    return Ok();
                }
            }
            else
            {

                _context.Entry(category).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok();

            }


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
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
        private async Task<Category> PutCategoryAsync(Category category, IFormFile image)
        {
            var filePath = Path.Combine(_host.WebRootPath + "/images/Categories/", image.FileName);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            var Cat = new Category
            {
                Id = category.Id,
                FirstName = category.FirstName,
                ProductDescription = category.ProductDescription,
                Image = image.FileName

            };

            _context.Entry(Cat).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Cat;
        }

      


        // Post WIth Image 
        [Obsolete]
        public async Task<IActionResult> PostCategory()
        {
            //receive the object ==>

            CategoryDTO categorydto = new CategoryDTO();






            categorydto.FirstName = HttpContext.Request.Form["firstName"];
            categorydto.ProductDescription= HttpContext.Request.Form["ProductDescription"];
            var Image = HttpContext.Request.Form.Files["image"];
            if (!string.IsNullOrEmpty(categorydto.FirstName) && Image != null && Image.Length > 0)
            {
                var category = await this.addUserAsync(categorydto, Image);
                if (category != null)
                {
                    return Ok();
                }
            }



            return BadRequest();

            //return CreatedAtAction("GetCustomers", new { id = Model.Id }, Model);
        }

        [Obsolete]
        private async Task<Category> addUserAsync(CategoryDTO customerdto, IFormFile image)
        {
            
            var filePath = Path.Combine(_host.WebRootPath + "/images/Categories/", image.FileName);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            var category = new Category
            {
                FirstName = customerdto.FirstName,
                ProductDescription = customerdto.ProductDescription,
                Image = image.FileName
            };
            _context.Category.Add(category);
            await _context.SaveChangesAsync();
            return category;

        }


        //מחיקת קטיגוריה
        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Category>> DeleteCategory(int id)
        {
            Category category = await GetCategory(id);
            if (category == null)
            {
                return NotFound();
            }

             List<Product> products;
              products= category.CategoryProducts;
            
            
            _context.Category.Remove(category);
            for (int i = 0; i < products.Count; i++)
            {
                _context.Product.Remove(products[i]);
            }
            await _context.SaveChangesAsync();

            return category;
        }

        //פונקצית מחיקת קטגוריה החלפת מוצרים עם קטגוריה אחרת
        // DELETE: api/Categories/5
        [Route("{deleteWithOutProducts}/{DeleteId}/{ChangeId}")]
        [HttpDelete]
        public async Task<ActionResult<Category>> DeleteCategory(int DeleteId, int ChangeId )
        {

            Category category = await GetCategory(DeleteId);

            if (category == null)
            {
                return NotFound();
            }

            List<Product> products;
            products = category.CategoryProducts;


            for (int i = 0; i < products.Count; i++)
            {

                //update category id for products we need to delete old category 
                Product Pr = products[i];
                Pr.CategoryId = ChangeId;
                _context.Product.Update(Pr);

            }


            category.CategoryProducts = null;
            _context.Category.Remove(category);
            await _context.SaveChangesAsync();

            return category;

        }
        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.Id == id);
        }
    } 
}

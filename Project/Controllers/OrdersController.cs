using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        [Obsolete]
        private readonly IHostingEnvironment _host;

        [Obsolete]
        public OrdersController(ApplicationDbContext context, IHostingEnvironment host)
        {
            _host = host;
            _context = context;
        }


        //קבלת כל ההזמנות 
        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrder()
        {
            //return await _context.Order.ToListAsync();
            var order = _context.Order.Include(C => C.Cart).Select(ee => new Order()
            {
                Id = ee.Id,
                CustomerId = ee.CustomerId,
                EmployeeId = ee.EmployeeId,
                //OrderNumber = ee.OrderNumber,
                OrderDate = ee.OrderDate,
                Frieght = ee.Frieght,
               // Customer = ee.Customer,
                Employee = ee.Employee,
                Cart=ee.Cart

            }).ToList();
            if (order == null)
            {
                return NotFound();
            }

            return order;


        }


        //שליחת קבלה למייל של הלקוח
        [Route("SentRecipt/{OrderId}")]
        [HttpPost("SentRecipt/{OrderId}")]
        [Obsolete]
        public async Task<ActionResult> SentRecipt(int OrderId)
        {

            if (OrderId > 0 && this.OrderExists(OrderId))
            {
                //Fetching Email Body Text from EmailTemplate File.  
                string FilePath = Path.Combine(_host.WebRootPath + "\\EmailTemplates\\Recipt\\Recipt.html");
                StreamReader str = new StreamReader(FilePath);
                string MailText = str.ReadToEnd();
                str.Close();

                Order o = new Order();
                o = await _context.Order.FindAsync(OrderId);
                Employee Employee = new Employee();
                Employee = await _context.Employee.FindAsync(o.EmployeeId);
                Cart c = new Cart();
                c = await _context.Cart.FindAsync(OrderId);

                Customer customer = new Customer();
                customer = await _context.Customers.FindAsync(o.CustomerId);

                var fromAddress = new MailAddress("yasilah4@gmail.com", "StudioHatnaResturant");
                var toAddress = new MailAddress(customer.Email, "Mr." + customer.FirstName + "" + customer.LastName);
                const string fromPassword = "vtlbjvyepoyumkcv";
                const string subject = "StudioHatna Resturant Order Recipt";

                MailText = MailText.Replace("#cname", Employee.FirstName.Trim()+" "+ Employee.LastName.Trim());
                MailText = MailText.Replace("#orderNo", o.Id.ToString().Trim() );
                MailText = MailText.Replace("#cname", Employee.FirstName.Trim() + " " + Employee.LastName.Trim());
                MailText = MailText.Replace("#cAddress", customer.City.ToString().Trim() + customer.Street.ToString().Trim() + customer.HomeNo.ToString().Trim());
                MailText = MailText.Replace("#cOrderDate", o.OrderDate.ToString().Trim());
                MailText = MailText.Replace("#cOrderPrice", c.TotalAmount.ToString().Trim());



                string body = MailText;
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
                    smtp.EnableSsl = true;
                    smtp.Send(message);

                }

                return Ok();
            }
            else
            {
                return BadRequest("email Not found");
            }

        }

        //קבלת הזמנה
        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {

            var order = await _context.Order.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }


        // עדכון הזמנה 
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, OrderDTO orderdto)
        {
            if (id < 1)
            {
                return BadRequest();
            }

            var Model = orderdto.ToModel();
            Model.Id = id;
            _context.Entry(Model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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


        //יצירת הזמנה חדה 
        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<OrderDTO>> PostOrder(OrderDTO orderdto)
        {
            var Model = orderdto.ToModel();
            Model.OrderDate = DateTime.Now;

            _context.Order.Add(Model);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = Model.Id }, Model);
        }




        //מחיקת הזמנה 
        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Order>> DeleteOrder(int id)
            {
            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            order.CustomerId = null;
            order.EmployeeId = null;
            order.Cart = null;
            order.Customer = null;
            order.Employee = null;
            //----------------------------------
            var cart = await _context.Cart.FindAsync(id);
            cart.OrderId = 0;
            cart.Order = null; 
            _context.Cart.Remove(cart);
            //----------------------------------
            var cp =await _context.CustomerProduct.Where(cp => cp.OrderId == id).FirstOrDefaultAsync();
            cp.OrderId = null;
            cp.CartId = null;
            cp.ProductId = null;
            cp.Cart = null;
             _context.CustomerProduct.Remove(cp);
            //----------------------------------
            //remove order
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();

            return order;
        }


        // בדיקת הזצמנה קיימת או לא 
        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.Id == id);
        }
    }
}

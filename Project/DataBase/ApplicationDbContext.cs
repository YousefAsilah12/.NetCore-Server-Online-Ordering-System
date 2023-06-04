using Microsoft.EntityFrameworkCore;
using Project.Moduls;
using Project.DTO;


namespace Project.DataBase
{
    public class ApplicationDbContext :DbContext
    {
       // internal object CustomerDTO;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                // Products child
                entity
                .HasMany<Product>(u => u.CategoryProducts)
                .WithOne(m => m.Category)
                .HasForeignKey(m => m.CategoryId).OnDelete(DeleteBehavior.ClientCascade);
            });




            modelBuilder.Entity<Cart>(entity =>
            {
                // Cart child
                entity
                .HasMany<CustomersProduct>(u => u.Products)
                .WithOne(m => m.Cart)
                .HasForeignKey(m => m.CartId).OnDelete(DeleteBehavior.ClientCascade);
            });

            //between Employee and Order 
            modelBuilder.Entity<Employee>(entity =>
            {
                // Cart child
                entity
                .HasMany<Order>(u => u.Orders)
                .WithOne(m => m.Employee)
                .HasForeignKey(m => m.EmployeeId).OnDelete(DeleteBehavior.ClientCascade);
            });

           /* //between customer and Order 
            modelBuilder.Entity<Customer>(entity =>
            {
                // Cart child
                entity
                .HasMany<Order>(u => u.Orders)
                .WithOne(m => m.Customer)
                .HasForeignKey(m => m.EmployeeId).OnDelete(DeleteBehavior.ClientCascade);
            });*/


            modelBuilder.Entity<Order>(entity =>
            {
                // Cart child
                entity
                .HasOne<Cart>(u => u.Cart)
                .WithOne(m => m.Order)
                .HasForeignKey<Cart>(ad => ad.OrderId);

            });





        }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomersProduct> CustomerProduct { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Supplier> Supplier { get; set; }
    }
}

using MangoFusion_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MangoFusion_API.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<MenuItem>().HasData(
                new MenuItem
                {
                    Id = 1,
                    Name = "Samosa",
                    Description = "Crispy and savory Indian pastry filled with spiced potatoes and peas.",
                    Image = "images/confirm.jpg",
                    Price = 15.0,
                    Category = "Appetizer",
                    SpecialTag = "Chef's Special"
                },
                new MenuItem
                {
                    Id = 2,
                    Name = "Paneer Tikka",
                    Description = "Marinated cubes of paneer cheese grilled to perfection, served with mint chutney.",
                    Category = "Appetizer",
                    Price = 13.99,
                    Image = "images/hero.jpg",
                    SpecialTag = ""
                },
                new MenuItem
                {
                    Id = 3,
                    Name = "Chicken Tikka Masala",
                    Description = "Tender chicken pieces cooked in a creamy tomato-based sauce, served with basmati rice.",
                    Category = "Main Course",
                    Price = 20.99,
                    Image = "images/project2-min.jpg",
                    SpecialTag = "Chef's Special"
                },
                new MenuItem
                {
                    Id = 4,
                    Name = "Garlic Naan",
                    Description = "Soft and fluffy Indian bread topped with garlic and butter, perfect for scooping up curries.",
                    Category = "Bread",
                    Price = 5.99,
                    Image = "images/project3-min.jpg",
                    SpecialTag = ""
                },
                new MenuItem
                {
                    Id = 5,
                    Name = "Mango Lassi",
                    Description = "A refreshing yogurt-based drink blended with ripe mangoes and a hint of cardamom.",
                    Category = "Beverage",
                    Price = 7.99,
                    Image = "images/service1-min.jpg",
                    SpecialTag = ""
                }
            );
        }
    }
}

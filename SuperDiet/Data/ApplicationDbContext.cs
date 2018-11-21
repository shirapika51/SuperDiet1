using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SuperDiet.Models;

namespace SuperDiet.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<ItemOrder>()
                .HasKey(po => new { po.ItemID, po.OrderID });
        }

        public DbSet<SuperDiet.Models.Item> Item { get; set; }

        public DbSet<SuperDiet.Models.Order> Order { get; set; }

        public DbSet<SuperDiet.Models.ItemOrder> ItemOrder { get; set; }

        public DbSet<SuperDiet.Models.Branch> Branch { get; set; }
    }
}

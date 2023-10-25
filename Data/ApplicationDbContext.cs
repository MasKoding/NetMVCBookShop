using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace bookshop.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Models.Book> Books { get; set; }

    public DbSet<Models.Genre> Genres { get; set; }

    public DbSet<Models.Order> Orders { get; set; }

    public DbSet<Models.ShoppingCart> ShoppingCarts { get; set; }

    public DbSet<Models.CartDetail> CartDetails { get; set; }

    public DbSet<Models.OrderDetail> OrderDetails { get; set; }

    public DbSet<Models.OrderStatus> OrderStatuses { get; set; }

}


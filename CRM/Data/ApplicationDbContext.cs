using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CRM.Models;

namespace CRM.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<CRM.Models.Customer> Customer { get; set; } = default!;
    public DbSet<CRM.Models.Call> Call { get; set; } = default!;
}
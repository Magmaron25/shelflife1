using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace shelflife.data;

/// <summary>
/// Factory for creating the ApplicationDbContext at design time.  This is
/// required by EF Core tools when running commands such as migrations from
/// outside of the application.
/// </summary>
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlite("Data Source=shelflife.db");
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
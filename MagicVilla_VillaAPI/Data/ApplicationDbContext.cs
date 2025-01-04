using MagicVilla_VillaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        // We need to create a constructor that takes DbContextOptions as a parameter.
        // A constructor is a special method that is called when an instance of the class is created.
        // The constructor initializes the object.
        // The DbContextOptions parameter is used to pass additional configuration options to the DbContext.
        // : base(options) syntax is used to call the constructor of the base class (DbContext) and pass the options parameter to it.
        // This ensures that the DbContext is configured with the provided options.
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // We need to create individual DbSet properties inside the DbContext for each model class that we want to store in the database (e.g. Villa entity).
        public DbSet<Villa> Villas { get; set; }
        // When we will be querying the DbSet, we will be using LINQ statements.
        // Those LINQ (Language-Integrated Query) statements will be automatically translated into SQL queries by Entity Framework Core.
        // That way, we don't have to write SQL queries manually.

        
    }

}
// Add connection string to the SQL server in the appsettings.json file.
// We need to link this ApplicationDbContext class to use the connection string.
// We have to register this to dependency injection.


// We are passing the connection string to the ApplicationDbContext class.
// We have to pass that connection string to the DbContext as well - since we will be using the basic features of the DbContext most of the time.
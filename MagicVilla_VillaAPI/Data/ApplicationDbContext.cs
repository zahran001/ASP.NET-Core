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

        // We automatically want to seed this table with couple of records with the migration.
        // We will override one method for migration.

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa()
                {
                    Id = 1,
                    Name = "Castle Villa",
                    Details = "Labore exerci in sadipscing wisi enim facilisis dolores nulla exerci kasd magna voluptua et tation sanctus eleifend no amet consequat",
                    Rate = 1000,
                    ImageUrl = "https://www.istockphoto.com/en/photo/moorish-castle-sintra-lisbon-in-portugal-gm1487324369-512841745",
                    Occupacy = 10,
                    Sqft = 5000,
                    Amenity = "Pool, Gym, Spa, Garden",
                    CreatedDate = new DateTime(2025, 1, 4, 12, 0, 0), // January 4, 2025, at 12:00 PM,
                },
                new Villa()
                {
                    Id = 2,
                    Name = "Oceanview Villa",
                    Details = "Sea-facing luxury villa with breathtaking views, perfect for a tranquil getaway.",
                    Rate = 1500,
                    ImageUrl = "https://www.istockphoto.com/en/photo/moorish-castle-sintra-lisbon-in-portugal-gm1487324369-512841745",
                    Occupacy = 8,
                    Sqft = 4000,
                    Amenity = "Infinity Pool, Private Beach Access, Sauna, Outdoor Barbecue",
                    CreatedDate = new DateTime(2025, 1, 5, 12, 0, 0), // January 5, 2025, at 12:00 PM,
                },
                new Villa()
                {
                    Id = 3,
                    Name = "Mountain Retreat Villa",
                    Details = "Nestled in the mountains, enjoy peaceful surroundings and a cozy atmosphere.",
                    Rate = 1200,
                    ImageUrl = "https://www.istockphoto.com/en/photo/moorish-castle-sintra-lisbon-in-portugal-gm1487324369-512841745",
                    Occupacy = 6,
                    Sqft = 3500,
                    Amenity = "Fireplace, Hiking Trails, Hot Tub, Ski-In/Ski-Out",
                    CreatedDate = new DateTime(2025, 1, 6, 12, 0, 0), // January 6, 2025, at 12:00 PM,
                },
                new Villa()
                {
                    Id = 4,
                    Name = "Urban Luxury Villa",
                    Details = "Modern villa in the heart of the city with top-notch amenities and stunning interiors.",
                    Rate = 2000,
                    ImageUrl = "https://www.istockphoto.com/en/photo/moorish-castle-sintra-lisbon-in-portugal-gm1487324369-512841745",
                    Occupacy = 5,
                    Sqft = 3000,
                    Amenity = "Home Theater, Rooftop Deck, Smart Home Features, Wine Cellar",
                    CreatedDate = new DateTime(2025, 1, 7, 12, 0, 0), // January 7, 2025, at 12:00 PM,
                },
                new Villa()
                {
                    Id = 5,
                    Name = "Tropical Paradise Villa",
                    Details = "Experience tropical luxury with lush greenery and serene surroundings.",
                    Rate = 1800,
                    ImageUrl = "https://www.istockphoto.com/en/photo/moorish-castle-sintra-lisbon-in-portugal-gm1487324369-512841745",
                    Occupacy = 7,
                    Sqft = 4500,
                    Amenity = "Private Pool, Hammocks, Outdoor Kitchen, Exotic Garden",
                    CreatedDate = new DateTime(2025, 1, 8, 12, 0, 0), // January 8, 2025, at 12:00 PM,
                }
            );
        }


    }

}
// Add connection string to the SQL server in the appsettings.json file.
// We need to link this ApplicationDbContext class to use the connection string.
// We have to register this to dependency injection.


// We are passing the connection string to the ApplicationDbContext class.
// We have to pass that connection string to the DbContext as well - since we will be using the basic features of the DbContext most of the time.

// You will not delete any migration unless you're confident about what you're doing.
// If you delete a migration, you will lose the ability to roll back to that migration.
// Just add a new migration - call it something else.



// Debug
// First try: Didn't work
// Clean and Rebuild the Project
// Delete and Recreate the Migration

// Second try:
// Followed this approach.
// Delete all migration files in the Migrations folder, including ModelSnapshot.
// Drop the database: drop-database
// Recreate the initial migration: add-migration InitialMigration
// Update the database: update-database
// Hardcoding the date fixed the issue.
// Best Practice: Use a hardcoded value for seeding data to avoid migration conflicts. Reserve DateTime.Now or DateTime.UtcNow for runtime-generated timestamps (e.g., in application logic or database defaults).
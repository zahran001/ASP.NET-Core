using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicVilla_VillaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddVillaTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Villas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sqft = table.Column<int>(type: "int", nullable: false),
                    Occupacy = table.Column<int>(type: "int", nullable: false),
                    Rate = table.Column<double>(type: "float", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amenity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Villas", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenity", "CreatedDate", "Details", "ImageUrl", "Name", "Occupacy", "Rate", "Sqft", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "Pool, Gym, Spa, Garden", new DateTime(2025, 1, 4, 12, 0, 0, 0, DateTimeKind.Unspecified), "Labore exerci in sadipscing wisi enim facilisis dolores nulla exerci kasd magna voluptua et tation sanctus eleifend no amet consequat", "https://www.istockphoto.com/en/photo/moorish-castle-sintra-lisbon-in-portugal-gm1487324369-512841745", "Castle Villa", 10, 1000.0, 5000, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Infinity Pool, Private Beach Access, Sauna, Outdoor Barbecue", new DateTime(2025, 1, 5, 12, 0, 0, 0, DateTimeKind.Unspecified), "Sea-facing luxury villa with breathtaking views, perfect for a tranquil getaway.", "https://www.istockphoto.com/en/photo/moorish-castle-sintra-lisbon-in-portugal-gm1487324369-512841745", "Oceanview Villa", 8, 1500.0, 4000, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Fireplace, Hiking Trails, Hot Tub, Ski-In/Ski-Out", new DateTime(2025, 1, 6, 12, 0, 0, 0, DateTimeKind.Unspecified), "Nestled in the mountains, enjoy peaceful surroundings and a cozy atmosphere.", "https://www.istockphoto.com/en/photo/moorish-castle-sintra-lisbon-in-portugal-gm1487324369-512841745", "Mountain Retreat Villa", 6, 1200.0, 3500, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "Home Theater, Rooftop Deck, Smart Home Features, Wine Cellar", new DateTime(2025, 1, 7, 12, 0, 0, 0, DateTimeKind.Unspecified), "Modern villa in the heart of the city with top-notch amenities and stunning interiors.", "https://www.istockphoto.com/en/photo/moorish-castle-sintra-lisbon-in-portugal-gm1487324369-512841745", "Urban Luxury Villa", 5, 2000.0, 3000, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "Private Pool, Hammocks, Outdoor Kitchen, Exotic Garden", new DateTime(2025, 1, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), "Experience tropical luxury with lush greenery and serene surroundings.", "https://www.istockphoto.com/en/photo/moorish-castle-sintra-lisbon-in-portugal-gm1487324369-512841745", "Tropical Paradise Villa", 7, 1800.0, 4500, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Villas");
        }
    }
}

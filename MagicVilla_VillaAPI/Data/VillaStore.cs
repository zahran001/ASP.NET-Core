using MagicVilla_VillaAPI.Models.Dto;

namespace MagicVilla_VillaAPI.Data
{
    // The VillaStore class is static to provide a simple, globally accessible, and consistent in-memory data store for VillaDTO objects.
    // Since the data is stored in a static field, it remains in memory for the lifetime of the application.
    public static class VillaStore
    {
        public static List<VillaDTO> villaList = new List<VillaDTO> {
                new VillaDTO { Id = 1, Name="Daytona Villa" , Occupacy=4, Sqft=2000},
                new VillaDTO { Id = 2, Name="Palm Beach Villa", Occupacy=3, Sqft=1500 }

            };
    }
}
// This is a on-the-fly data store.
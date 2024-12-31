using MagicVilla_VillaAPI.Models.Dto;

namespace MagicVilla_VillaAPI.Data
{
    public static class VillaStore
    {
        public static List<VillaDTO> villaList = new List<VillaDTO> {
                new VillaDTO { Id = 1, Name="Daytona Villa" },
                new VillaDTO { Id = 2, Name="Palm Beach Villa" }

            };
    }
}
// This is a on-the-fly data store.
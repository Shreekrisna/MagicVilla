using MagicVilla_VillaAPI.Models.Dto;

namespace MagicVilla_VillaAPI.Data
{
    public static class VillaStore
    {
        public static List<VillaDto> villalist = new List<VillaDto>
        {
                new VillaDto{Id=1,Name="Pool View",Sqrft=100,Occupancy=5},
                new VillaDto{Id=2,Name="Beach View",Sqrft=200,Occupancy=10}
        };
    }
}

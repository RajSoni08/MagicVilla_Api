using MagicVilla_VillaAPI.Models.Dto;

namespace MagicVilla_VillaAPI.Data
{
    public static class VillaStore
    {
        public static List<VillaDTO> villaList = new List<VillaDTO>
        {
            new VillaDTO() {Id =1 , Name= "Beach View",Sqft=100,Occupancy=5},
            new VillaDTO() {Id =2 ,Name="Mountain View",Sqft=500,Occupancy=6}
        };
    };

}

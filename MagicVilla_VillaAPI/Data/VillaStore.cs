using MagicVilla_VillaAPI.Models.Dto;

namespace MagicVilla_VillaAPI.Data
{
    public  static class VillaStore
    {
        public static List<VillaDTO> villaList = new List<VillaDTO>
        {
            new VillaDTO() {id =1 , Name= "Beach View"},
            new VillaDTO() {id =2 ,Name="Mpontain View"}
        };
    };
    }
}

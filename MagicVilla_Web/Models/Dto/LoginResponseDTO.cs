using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace MagicVilla_VillaAPI.Models.Dto
{
    
    public  class LoginResponseDTO
    {
       // [JsonProperty(PropertyName = "User")]
        public UserDTO User { get; set; }

        //[JsonProperty(PropertyName = "Token")]
        public string Token { get; set; }

    }
}

using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace MagicVilla_Web.Models.Dto
{
    [JsonConverter(typeof(UserDTO))]
    public class UserDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        
    }
}

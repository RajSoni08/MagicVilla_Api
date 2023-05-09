using static MagicVilla_Utility.SD;

namespace MagicVilla_Web.Models
{
    public class APIRequest
    {
        internal string Url;

        public string Token { get; set; }

        public ApiType ApiType { get; set; } = ApiType.GET;
        public string URL { get; set; }
        public  object Data { get; set; }
    }
}

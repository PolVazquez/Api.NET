using System.Net;

namespace MinimalAPI.Models
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public Object Result { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public List<string> Errores { get; set; } = new();
    }
}

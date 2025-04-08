using System.Net;

namespace ApiCurso.Model
{
    public class ResponseAPI
    {
        public ResponseAPI()
        {
        }

        public List<string> ErrorMessages { get; set; } = new();
        public HttpStatusCode StatusCode { get; set; }
        public bool Success { get; set; } = true;
        public Object Result { get; set; }
    }
}

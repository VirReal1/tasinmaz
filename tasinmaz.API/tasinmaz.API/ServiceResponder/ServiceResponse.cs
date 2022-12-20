using System.Collections.Generic;

namespace tasinmaz.API.ServiceResponder
{
    public class ServiceResponse<T>
    {
        public string Process { get; set; }
        public string Message { get; set; }
        public bool Warning { get; set; }
        public bool Error { get; set; }
        public T Data { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}
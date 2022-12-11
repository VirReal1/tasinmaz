using System.Collections.Generic;

namespace tasinmaz.API.ServiceResponder
{
    public class ServiceResponse<T>
    {
        public string Process { get; set; }
        public string Message { get; set; } = null;
        public bool Success { get; set; } = true;
        public T Data { get; set; }
        public List<string> ErrorMessages { get; set; } = null;
    }
}
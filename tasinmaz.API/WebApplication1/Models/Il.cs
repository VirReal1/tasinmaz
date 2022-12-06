using System.Collections.Generic;

namespace tasinmaz.API.Models
{
    public class Il
    {
        public Il()
        {
            Ilceler = new List<Ilce>();
        }
        public int IlId { get; set; }
        public string IlAdi { get; set; }
        public List<Ilce> Ilceler { get; set; }
    }
}
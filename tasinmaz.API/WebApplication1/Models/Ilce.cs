using System.Collections.Generic;

namespace tasinmaz.API.Models
{
    public class Ilce
    {
        public Ilce()
        {
            Mahalleler = new List<Mahalle>();
        }

        public int IlceId { get; set; }
        public string IlceAdi { get; set; }
        public Il Il { get; set; }
        public List<Mahalle> Mahalleler { get; set; }
    }
}
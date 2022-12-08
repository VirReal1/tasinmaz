using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using tasinmaz.API.Models;

namespace tasinmaz.API.Dtos.Tasinmaz
{
    public class TasinmazDto
    {
        public int Id { get; set; }
        public string Adi { get; set; }
        public string IlAdi { get; set; }
        public string IlceAdi { get; set; }
        public string MahalleAdi { get; set; }
        public string Ada { get; set; }
        public string Parsel { get; set; }
        public string Nitelik { get; set; }
        public string KoordinatBilgileri { get; set; }
        public int KullaniciId { get; set; }
    }
}
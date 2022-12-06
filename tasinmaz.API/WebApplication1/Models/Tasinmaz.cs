using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tasinmaz.API.Models
{
    [Table("Tasinmazlar")]
    public class Tasinmaz
    {
        [Key]
        public int TasinmazId { get; set; }
        public string IlAdi { get; set; }
        public string IlceAdi { get; set; }
        public string MahalleAdi { get; set; }
        public string Ada { get; set; }
        public string Parsel { get; set; }
        public string Nitelik { get; set; }
        public string KoordinatBilgileri { get; set; }
        [ForeignKey("KullaniciId")]
        public Kullanici Kullanici { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using tasinmaz.API.Entities.Abstract;

namespace tasinmaz.API.Models
{
    [Table("Tasinmazlar")]
    public class Tasinmaz : IEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Adi { get; set; }
        public string IlAdi { get; set; }
        public string IlceAdi { get; set; }
        public string MahalleAdi { get; set; }
        public string Ada { get; set; }
        public string Parsel { get; set; }
        public string Nitelik { get; set; }
        public string KoordinatBilgileri { get; set; }
        [ForeignKey("Id")]
        public Kullanici Kullanici { get; set; }
    }
}
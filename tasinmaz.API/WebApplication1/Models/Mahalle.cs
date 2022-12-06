using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tasinmaz.API.Models
{
    [Table("Mahalleler")]
    public class Mahalle
    {
        [Key]
        public int MahalleId { get; set; }
        public string MahalleAdi { get; set; }
        [ForeignKey("IlceId")]
        public Ilce Ilce { get; set; }
    }
}
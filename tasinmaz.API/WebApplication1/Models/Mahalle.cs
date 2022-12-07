using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tasinmaz.API.Models
{
    [Table("Mahalleler")]
    public class Mahalle
    {
        [Key]
        public int Id { get; set; }
        public string Adi { get; set; }
        [ForeignKey("Id")]
        public Ilce Ilce { get; set; }
    }
}
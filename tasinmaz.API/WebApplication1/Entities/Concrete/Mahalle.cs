using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using tasinmaz.API.Entities.Abstract;

namespace tasinmaz.API.Models
{
    [Table("Mahalleler")]
    public class Mahalle : IEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Adi { get; set; }
        [ForeignKey("Id")]
        public Ilce Ilce { get; set; }
    }
}
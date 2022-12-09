using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using tasinmaz.API.Entities.Abstract;

namespace tasinmaz.API.Models
{
    [Table("Ilceler")]
    public class Ilce : IEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Adi { get; set; }
        [ForeignKey("Id")]
        public Il Il { get; set; }
    }
}
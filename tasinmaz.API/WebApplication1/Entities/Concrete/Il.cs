using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using tasinmaz.API.Entities.Abstract;

namespace tasinmaz.API.Models
{
    [Table("Iller")]
    public class Il : IEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Adi { get; set; }
    }
}
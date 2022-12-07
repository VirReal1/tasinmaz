using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tasinmaz.API.Models
{
    [Table("Iller")]
    public class Il
    {
        [Key]
        public int Id { get; set; }
        public string Adi { get; set; }
    }
}
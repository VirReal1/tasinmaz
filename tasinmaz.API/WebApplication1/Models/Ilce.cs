using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tasinmaz.API.Models
{
    [Table("Ilceler")]
    public class Ilce
    {
        [Key]
        public int Id { get; set; }
        public string Adi { get; set; }
        [ForeignKey("Id")]
        public Il Il { get; set; }
    }
}
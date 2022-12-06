using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tasinmaz.API.Models
{
    [Table("Iller")]
    public class Il
    {
        [Key]
        public int IlId { get; set; }
        public string IlAdi { get; set; }
    }
}
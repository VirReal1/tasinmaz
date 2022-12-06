using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tasinmaz.API.Models
{
    [Table("Ilceler")]
    public class Ilce
    {
        [Key]
        public int IlceId { get; set; }
        public string IlceAdi { get; set; }
        [ForeignKey("IlId")]
        public Il Il { get; set; } }
}
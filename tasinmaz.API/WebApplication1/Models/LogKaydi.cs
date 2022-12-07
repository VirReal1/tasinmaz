using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tasinmaz.API.Models
{
    [Table("LogKayitlari")]
    public class LogKaydi
    {
        [Key]
        public int Id { get; set; }
        public string KullaniciIp { get; set; }
        public DateTime Tarih { get; set; }
        public string Durum { get; set; }
        public string IslemTipi { get; set; }
        public string Aciklama { get; set; }
        [ForeignKey("Id")]
        public Kullanici Kullanici { get; set; }
    }
}
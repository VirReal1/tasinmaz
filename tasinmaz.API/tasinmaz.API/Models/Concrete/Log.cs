using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using tasinmaz.API.Entities.Abstract;
using tasinmaz.API.Models;

namespace tasinmaz.API.Entities.Concrete
{
    [Table("Loglar")]
    public class Log: IEntity
    {
        [Key]
        public int Id { get; set; }
        public string KullaniciIp { get; set; } = null;
        public DateTime Tarih { get; set; }
        public string Durum { get; set; } = null;
        public string Islem { get; set; } = null;
        public string Aciklama { get; set; } = null;
        [ForeignKey("Kullanici")]
        public int KullaniciId { get; set; }
        public Kullanici Kullanici { get; set; }
    }
}
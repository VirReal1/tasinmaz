using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace tasinmaz.API.Entities.Concrete
{
    public class LogKaydi<T>
    {
        [Key]
        public int Id { get; set; }
        public string KullaniciIp { get; set; } = null;
        public DateTime Tarih { get; set; }
        public string Durum { get; set; } = null;
        public string Islem { get; set; } = null;
        public string Aciklama { get; set; } = null;
        public T Veri { get; set; }
        public List<string> HataMesajlari { get; set; } = null;
    }
}
using System;

namespace tasinmaz.API.Models
{
    public class LogKaydi
    {
        public int LogId { get; set; }
        public string KullaniciIp { get; set; }
        public DateTime Tarih { get; set; }
        public string Durum { get; set; }
        public string IslemTipi { get; set; }
        public string Aciklama { get; set; }
        public Kullanici Kullanici { get; set; }
    }
}
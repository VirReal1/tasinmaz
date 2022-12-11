using System;
using tasinmaz.API.Models;

namespace tasinmaz.API.Dtos.Log
{
    public class LogDto
    {
        public int Id { get; set; }
        public string KullaniciIp { get; set; } = null;
        public DateTime Tarih { get; set; }
        public string Durum { get; set; } = null;
        public string Islem { get; set; } = null;
        public string Aciklama { get; set; } = null;
        public int KullaniciId { get; set; }
    }
}

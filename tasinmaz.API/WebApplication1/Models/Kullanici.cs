using System.Collections.Generic;

namespace tasinmaz.API.Models
{
    public class Kullanici
    {
        public Kullanici()
        {
            LogKayitlari = new List<LogKaydi>();
            Tasinmazlar = new List<Tasinmaz>();
        }

        public int KullaniciId { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public List<LogKaydi> LogKayitlari { get; set; }
        public List<Tasinmaz> Tasinmazlar { get; set; }
    }
}
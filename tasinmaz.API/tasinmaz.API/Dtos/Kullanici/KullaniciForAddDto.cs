namespace tasinmaz.API.Dtos
{
    public class KullaniciForAddDto
    {
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool AdminMi { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public int LogKullaniciId { get; set; }
    }
}
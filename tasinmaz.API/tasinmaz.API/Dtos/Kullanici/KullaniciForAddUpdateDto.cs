namespace tasinmaz.API.Dtos
{
    public class KullaniciForAddUpdateDto
    {
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool AdminMi { get; set; }
    }
}

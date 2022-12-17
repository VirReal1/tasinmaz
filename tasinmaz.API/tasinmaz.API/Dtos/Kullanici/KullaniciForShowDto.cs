namespace tasinmaz.API.Dtos
{
    public class KullaniciForShowDto
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Email { get; set; }
        public bool AdminMi { get; set; }
        public bool? SearchAdminMi { get; set; }
        public int LogKullaniciId { get; set; }
    }
}
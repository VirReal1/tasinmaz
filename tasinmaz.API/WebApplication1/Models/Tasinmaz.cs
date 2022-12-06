namespace tasinmaz.API.Models
{
    public class Tasinmaz
    {
        public int TasinmazId { get; set; }
        public string IlAdi { get; set; }
        public string IlceAdi { get; set; }
        public string MahalleAdi { get; set; }
        public string Ada { get; set; }
        public string Parsel { get; set; }
        public string Nitelik { get; set; }
        public string KoordinatBilgileri { get; set; }
        public Kullanici Kullanici { get; set; }
    }
}
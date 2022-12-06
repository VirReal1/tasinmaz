namespace tasinmaz.API.Models
{
    public class Mahalle
    {
        public int MahalleId { get; set; }
        public string MahalleAdi { get; set; }
        public Ilce ilce { get; set; }
    }
}
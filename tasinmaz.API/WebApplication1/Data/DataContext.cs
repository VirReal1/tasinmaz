using Microsoft.EntityFrameworkCore;
using tasinmaz.API.Models;

namespace tasinmaz.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Tasinmaz> Tasinmazlar { get; set; }
        public DbSet<Il> Iller { get; set; }
        public DbSet<Ilce> Ilceler { get; set; }
        public DbSet<Mahalle> Mahalleler { get; set; }
        public DbSet<LogKaydi> LogKayitlari { get; set; }
    }
}

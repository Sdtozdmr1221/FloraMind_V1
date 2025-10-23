using Microsoft.EntityFrameworkCore;
using FloraMind_V1.Models;

namespace FloraMind_V1.Data
{

    
    public class FloraMindDbContext : DbContext    //veritabanı ile site arasındaki köprü
    {
        public FloraMindDbContext(DbContextOptions<FloraMindDbContext> options) // Yapıcı metod
            : base(options){}
        public DbSet<User> Users { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<Content> Contents { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder) //veritabanı ilişkilerini tanımlama
        {
            base.OnModelCreating(modelBuilder);

            // Kullanıcı -> Bitki (1:N)
                modelBuilder.Entity<User>()
        .HasMany(u => u.Plants)
        .WithOne(p => p.User)
        .OnDelete(DeleteBehavior.NoAction);

            // Kullanıcı -> İçerik (1:N)
            modelBuilder.Entity<User>()
    .HasMany(u => u.Contents)
    .WithOne(c => c.User)
    .OnDelete(DeleteBehavior.Cascade);

            // Bitki -> İçerik (1:N)
            modelBuilder.Entity<Plant>()
    .HasMany(p => p.Contents)
    .WithOne(c => c.Plant)
    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}


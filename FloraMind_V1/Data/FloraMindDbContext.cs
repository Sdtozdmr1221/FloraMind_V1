using Microsoft.EntityFrameworkCore;
using FloraMind_V1.Models;

namespace FloraMind_V1.Data
{

    
    public class FloraMindDbContext : DbContext    //veritabanı ile site arasındaki köprü sınıf
    {
        public FloraMindDbContext(DbContextOptions<FloraMindDbContext> options) // Yapıcı metod
            : base(options){}
        public DbSet<User> Users { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<UserPlant> UserPlants { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder) // Veritabanı ilişkilerini tanımlama
        {
            base.OnModelCreating(modelBuilder);

            
            // User-Plant İlişkisi
            
            modelBuilder.Entity<Plant>()
                .HasOne(p => p.User)
                .WithMany(u => u.Plants)
                .HasForeignKey(p => p.UserID)
                .IsRequired(false)             // Katalog bitkisi kimseye ait olmayabilir (NULL)
                .OnDelete(DeleteBehavior.Restrict); // Kullanıcı silinse bile katalog bitkisi kalır

            
            // User-Content) İlişkisi

            modelBuilder.Entity<Content>()
                .HasOne(c => c.User)
                .WithMany(u => u.Contents)
                .HasForeignKey(c => c.UserID)
                .IsRequired(true)             // Veritabanı seviyesinde zorunlu kılındı (NOT NULL)
                .OnDelete(DeleteBehavior.Restrict); // Admin silinse bile içerikler korunur

            
            //Plant-Content İlişkisi
            
            modelBuilder.Entity<Content>()
                .HasOne(c => c.Plant)
                .WithMany(p => p.Contents)
                .HasForeignKey(c => c.PlantID)
                .IsRequired(true)             // İçeriğin mutlaka bir bitkiye ait olması zorunlu kılındı
                .OnDelete(DeleteBehavior.Cascade); // Bitki silinirse, ona ait tüm içerikler de silinir

           
            //User-UserPlant
            
            modelBuilder.Entity<UserPlant>()
                .HasOne(up => up.User)
                .WithMany(u => u.UserPlants)
                .HasForeignKey(up => up.UserID)
                .OnDelete(DeleteBehavior.Cascade); // Kullanıcı silinirse koleksiyon kayıtları silinir

            
            //Plant-UserPlant İlişkisi
            
            modelBuilder.Entity<UserPlant>()
                .HasOne(up => up.Plant)
                
                .WithMany(p => p.UserPlants) // Plant modelinde bu koleksiyonun var olduğunu varsayıyoruz
                .HasForeignKey(up => up.PlantID)
                .OnDelete(DeleteBehavior.Restrict); // Katalogdaki bitki silinirken, kullanıcıların koleksiyon kayıtları korunur.

        }
    }
}


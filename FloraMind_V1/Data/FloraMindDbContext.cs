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

            // =========================================================================
            // 1. Kullanıcı (User) <--> Bitki (Plant) İlişkisi (Katalog Aitliği)
            // =========================================================================
            // Not: Plant modelindeki UserID zaten int? olduğu için IsRequired(false)
            modelBuilder.Entity<Plant>()
                .HasOne(p => p.User)
                .WithMany(u => u.Plants)
                .HasForeignKey(p => p.UserID)
                .IsRequired(false)             // Katalog bitkisi kimseye ait olmayabilir (NULL)
                .OnDelete(DeleteBehavior.Restrict); // Kullanıcı silinse bile katalog bitkisi kalır

            // =========================================================================
            // 2. Kullanıcı (User) <--> İçerik (Content) İlişkisi (1:N)
            // =========================================================================
            // DİKKAT: Content.UserID artık int? olduğu için, ilişki varsayılan olarak opsiyoneldir.
            // Ancak bu, Admin'in bir içeriği olması gerektiği için veritabanında zorunlu kalmalıdır.
            modelBuilder.Entity<Content>()
                .HasOne(c => c.User)
                .WithMany(u => u.Contents)
                .HasForeignKey(c => c.UserID)
                .IsRequired(true)             // Veritabanı seviyesinde zorunlu kılındı (NOT NULL)
                .OnDelete(DeleteBehavior.Restrict); // Admin silinse bile içerikler korunur (Kullanıcı verisi hassas olduğu için CASCADE yerine RESTRICT tercih edilir)

            // =========================================================================
            // 3. Bitki (Plant) <--> İçerik (Content) İlişkisi (1:N)
            // =========================================================================
            // DİKKAT: Content.PlantID artık int? olduğu için, ilişki varsayılan olarak opsiyoneldir.
            modelBuilder.Entity<Content>()
                .HasOne(c => c.Plant)
                .WithMany(p => p.Contents)
                .HasForeignKey(c => c.PlantID)
                .IsRequired(true)             // İçeriğin mutlaka bir bitkiye ait olması zorunlu kılındı
                .OnDelete(DeleteBehavior.Cascade); // Bitki silinirse, ona ait tüm içerikler de silinir

            // =========================================================================
            // 4. Kullanıcı (User) <--> Kullanıcının Bitkisi (UserPlant) İlişkisi (1:N)
            // =========================================================================
            modelBuilder.Entity<UserPlant>()
                .HasOne(up => up.User)
                .WithMany(u => u.UserPlants)
                .HasForeignKey(up => up.UserID)
                .OnDelete(DeleteBehavior.Cascade); // Kullanıcı silinirse koleksiyon kayıtları silinir

            // =========================================================================
            // 5. Bitki (Plant) <--> Kullanıcının Bitkisi (UserPlant) İlişkisi (1:N)
            // =========================================================================
            modelBuilder.Entity<UserPlant>()
                .HasOne(up => up.Plant)
                // DİKKAT: Plant modelinizdeki navigasyon özelliği UserPlants olarak tanımlandı
                .WithMany(p => p.UserPlants) // Plant modelinde bu koleksiyonun var olduğunu varsayıyoruz
                .HasForeignKey(up => up.PlantID)
                .OnDelete(DeleteBehavior.Restrict); // Katalogdaki bitki silinirken, kullanıcıların koleksiyon kayıtları korunur.

            // --- BİRLEŞİK ANAHTAR TANIMI ---
            // UserPlant modelinde birincil anahtar yoksa veya çoklu anahtar kullanılıyorsa:
            // modelBuilder.Entity<UserPlant>().HasKey(up => new { up.UserID, up.PlantID });

            // --- Identity Kullanımı ---
            // base.OnModelCreating(modelBuilder); satırı Identity tablolarını zaten çağırır.
        }
    }
}


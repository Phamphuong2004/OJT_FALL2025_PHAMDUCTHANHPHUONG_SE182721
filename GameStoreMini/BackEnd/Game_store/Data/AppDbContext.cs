using Microsoft.EntityFrameworkCore;
using GameStoreMini.Models;
using Game_store.Models;

namespace GameStoreMini.Data
{
    // The EF Core DbContext holds DbSet<TEntity> properties for each table.
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Application tables
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Game> Games { get; set; } = null!;
        public DbSet<Cart> Carts { get; set; } = null!;
        public DbSet<CartItem> CartItems { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Location> Locations { get; set; } = null!;
        
        public DbSet<Address> Addresses { get; set; } = null!;
        public DbSet<Game_store.Models.Promotion> Promotions { get; set; } = null!;
        public DbSet<Game_store.Models.PromotionGame> PromotionGames { get; set; } = null!;
        public DbSet<Game_store.Models.PromotionClaim> PromotionClaims { get; set; } = null!;
        public DbSet<ReviewGame> ReviewGames { get; set; }
        public DbSet<ReviewHelpful> ReviewHelpfuls { get; set; }
        public DbSet<Game_store.Models.Wishlist> Wishlists { get; set; } = null!;
        public DbSet<Game_store.Models.ViewHistory> ViewHistories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ensure email is unique
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Configure one-to-one between User and Cart (User may have a Cart)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Cart)
                .WithOne(c => c.User)
                .HasForeignKey<Cart>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure max lengths to match annotations to ensure consistent migrations
            modelBuilder.Entity<User>().Property(u => u.Email).HasMaxLength(200);
            modelBuilder.Entity<User>().Property(u => u.UserName).HasMaxLength(100);
            modelBuilder.Entity<User>().Property(u => u.FullName).HasMaxLength(200);
            modelBuilder.Entity<User>().Property(u => u.Avatar).HasMaxLength(500);

            // Category configuration
            modelBuilder.Entity<Category>().Property(c => c.Name).HasMaxLength(100);
            modelBuilder.Entity<Category>().Property(c => c.Slug).HasMaxLength(100);
            modelBuilder.Entity<Category>().HasIndex(c => c.Name).IsUnique(false);
            modelBuilder.Entity<Category>().HasIndex(c => c.Slug).IsUnique(true).HasFilter(null);

            // Many-to-many Game <-> Category (EF Core creates join table automatically)
            modelBuilder.Entity<Game>()
                .HasMany(g => g.Categories)
                .WithMany(c => c.Games)
                .UsingEntity<Dictionary<string, object>>(
                    "GameCategory",
                    j => j.HasOne<Category>().WithMany().HasForeignKey("CategoryId").HasConstraintName("FK_GameCategory_Category").OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<Game>().WithMany().HasForeignKey("GameId").HasConstraintName("FK_GameCategory_Game").OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.HasKey("GameId", "CategoryId");
                        j.ToTable("GameCategories");
                    }
                );

            // Promotion configurations
            modelBuilder.Entity<Game_store.Models.Promotion>()
                .HasIndex(p => p.Slug)
                .IsUnique();

            modelBuilder.Entity<Game_store.Models.PromotionGame>()
                .HasKey(pg => new { pg.PromotionId, pg.GameId });

            modelBuilder.Entity<Game_store.Models.PromotionGame>()
                .HasOne(pg => pg.Promotion)
                .WithMany(p => p.PromotionGames)
                .HasForeignKey(pg => pg.PromotionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Game_store.Models.PromotionGame>()
                .HasOne(pg => pg.Game)
                .WithMany()
                .HasForeignKey(pg => pg.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            // PromotionClaim configuration: each claim belongs to a Promotion and a User
            modelBuilder.Entity<Game_store.Models.PromotionClaim>()
                .HasOne(pc => pc.Promotion)
                .WithMany(p => p.PromotionClaims)
                .HasForeignKey(pc => pc.PromotionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Game_store.Models.PromotionClaim>()
                .HasOne<GameStoreMini.Models.User>(pc => pc.User)
                .WithMany()
                .HasForeignKey(pc => pc.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Game_store.Models.PromotionClaim>()
                .HasIndex(pc => new { pc.UserId });

            // Review configuration
            modelBuilder.Entity<ReviewGame>()
                .HasOne(r => r.Game)
                .WithMany(g => g.Reviews)
                .HasForeignKey(r => r.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReviewGame>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            // ReviewHelpful configuration
            modelBuilder.Entity<ReviewHelpful>()
                .HasOne(rh => rh.Review)
                .WithMany()
                .HasForeignKey(rh => rh.ReviewId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReviewHelpful>()
                .HasIndex(rh => new { rh.ReviewId, rh.UserId })
                .IsUnique(); // Một user chỉ vote helpful 1 lần cho 1 review

            // Wishlist configuration
            modelBuilder.Entity<Game_store.Models.Wishlist>()
                .HasOne(w => w.User)
                .WithMany()
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Game_store.Models.Wishlist>()
                .HasOne(w => w.Game)
                .WithMany()
                .HasForeignKey(w => w.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Game_store.Models.Wishlist>()
                .HasIndex(w => new { w.UserId, w.GameId })
                .IsUnique(); // Một user chỉ có thể thêm 1 game 1 lần vào wishlist

            // ViewHistory configuration
            modelBuilder.Entity<Game_store.Models.ViewHistory>()
                .HasOne(vh => vh.User)
                .WithMany()
                .HasForeignKey(vh => vh.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Game_store.Models.ViewHistory>()
                .HasOne(vh => vh.Game)
                .WithMany()
                .HasForeignKey(vh => vh.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Game_store.Models.ViewHistory>()
                .HasIndex(vh => new { vh.UserId, vh.GameId })
                .IsUnique(); // Một user chỉ có 1 record cho mỗi game

            modelBuilder.Entity<Game_store.Models.ViewHistory>()
                .HasIndex(vh => vh.LastViewedAt);

            // Order configuration: ensure Id is auto-generated
            modelBuilder.Entity<Order>()
                .Property(o => o.Id)
                .ValueGeneratedOnAdd();

            // OrderItem configuration: ensure Id is auto-generated
            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.Id)
                .ValueGeneratedOnAdd();

            // Location data already exists via migration 20251104044210_AddLocations
            
            // Configure Address
            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(a => a.Id);

                // Index để tìm địa chỉ theo UserId nhanh hơn
                entity.HasIndex(a => a.UserId);

                // Index để tìm địa chỉ default nhanh hơn
                entity.HasIndex(a => new { a.UserId, a.IsDefault });

                // Relationship với User
                entity.HasOne(a => a.User)
                    .WithMany(u => u.Addresses)
                    .HasForeignKey(a => a.UserId)
                    .OnDelete(DeleteBehavior.Cascade); // Xóa User → xóa tất cả Address

                // Constraints
                entity.Property(a => a.FullName).IsRequired().HasMaxLength(100);
                entity.Property(a => a.PhoneNumber).IsRequired().HasMaxLength(15);
                entity.Property(a => a.Street).IsRequired().HasMaxLength(200);
                entity.Property(a => a.Ward).IsRequired().HasMaxLength(100);
                entity.Property(a => a.District).IsRequired().HasMaxLength(100);
                entity.Property(a => a.City).IsRequired().HasMaxLength(100);
                entity.Property(a => a.PostalCode).HasMaxLength(10);
            });
        }
    }
}
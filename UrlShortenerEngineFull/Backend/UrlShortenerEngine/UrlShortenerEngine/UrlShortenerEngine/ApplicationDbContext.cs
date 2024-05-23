using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UrlShortenerEngine.Entities;
using UrlShortenerEngine.Services;

namespace UrlShortenerEngine
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<ShortenedUrl> shortenedUrls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ShortenedUrl>(builder =>
            {
                builder.Property(c => c.CodeAdded).HasMaxLength(ShortenUrlService.NumberOfCharsInShortenLink);
                builder.HasIndex(c => c.CodeAdded).IsUnique();

            });
        }
    }
}

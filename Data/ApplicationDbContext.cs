using DemoLinkedIn.Server.Entities;
using DemoLinkedInApi.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DemoLinkedInApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<RefreshTokenInfo> RefreshTokenInfos { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Relation One To Many between User and Post
            builder.Entity<Post>()
                .HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId)
                .IsRequired(true);

            // Relation One To Many between Post and Comment
            builder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .IsRequired(false);

            // Relation One To Many between User and Comment
            builder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .IsRequired(false);

            // Relation Many To Many between Post and User through PostLike
            builder.Entity<PostLike>()
                .HasKey(pl => new { pl.PostId, pl.UserId }); // Composite key

            builder.Entity<PostLike>()
                .HasOne(pl => pl.Post)
                .WithMany(p => p.PostLikes)
                .HasForeignKey(pl => pl.PostId);

            builder.Entity<PostLike>()
                .HasOne(pl => pl.User)
                .WithMany(u => u.PostLikes)
                .HasForeignKey(pl => pl.UserId);

            // Explicit configuration for IdentityUserLogin<string>
            builder.Entity<IdentityUserLogin<string>>()
                .HasKey(login => new { login.LoginProvider, login.ProviderKey }); // Composite key
            
            
            // Apply global filter "IsActive" for specific entities
            builder.Entity<Post>().HasQueryFilter(p => p.IsActive);
            builder.Entity<Comment>().HasQueryFilter(c => c.IsActive);
            builder.Entity<ApplicationUser>().HasQueryFilter(a => a.IsActive);

        }
    }
}

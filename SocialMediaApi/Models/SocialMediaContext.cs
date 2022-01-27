using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace SocialMediaApi.Models
{
    public partial class SocialMediaContext : DbContext
    {
        public SocialMediaContext()
        {
        }

        public SocialMediaContext(DbContextOptions<SocialMediaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Follow> Follows { get; set; }
        public virtual DbSet<Like> Likes { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-5L1QPDH\\SQLEXPRESS;Database=SocialMedia;User ID=suphi;Password=Suphi102030;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Follow>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("follow");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.FollowerId).HasColumnName("follower_id");

                entity.Property(e => e.FollowingId).HasColumnName("following_id");

                entity.HasOne(d => d.Follower)
                    .WithMany()
                    .HasForeignKey(d => d.FollowerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_follow_user1");

                entity.HasOne(d => d.Following)
                    .WithMany()
                    .HasForeignKey(d => d.FollowingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_follow_user");
            });

            modelBuilder.Entity<Like>(entity =>
            {
                entity.ToTable("like");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Likes)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK_like_post");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Likes)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_like_user");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("post");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.Description)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Image)
                    .IsUnicode(false)
                    .HasColumnName("image");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_post_user");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Bio)
                    .IsUnicode(false)
                    .HasColumnName("bio");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.FullName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("full_name");

                entity.Property(e => e.ProfilePicture)
                    .IsUnicode(false)
                    .HasColumnName("profile_picture");

                entity.Property(e => e.Username)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("username");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebBoard.DataAccess.EntityFramework.DbContextModel
{
    public partial class webboarddbContext : DbContext
    {
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<Post> Post { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"Server=DESKTOP-KR0EJIO;Database=webboarddb;Persist Security Info=True;User ID=sa;Password=123456789");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("COMMENT");

                entity.Property(e => e.CommentId)
                    .HasColumnName("comment_id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CommentDetail)
                    .IsRequired()
                    .HasColumnName("comment_detail");

                entity.Property(e => e.CommentTimeStamp).HasColumnName("comment_time_stamp");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.HasOne(d => d.CommentNavigation)
                    .WithOne(p => p.InverseCommentNavigation)
                    .HasForeignKey<Comment>(d => d.CommentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_COMMENT_COMMENT");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_COMMENT_post");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("POST");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.Property(e => e.PostDetail)
                    .IsRequired()
                    .HasColumnName("post_detail");

                entity.Property(e => e.PostSubject)
                    .IsRequired()
                    .HasColumnName("post_subject")
                    .HasMaxLength(500);

                entity.Property(e => e.PostTimeStamp).HasColumnName("post_time_stamp");
            });
        }
    }
}

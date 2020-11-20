using AngularProjectAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularProjectAPI.Data
{
    public class NewsContext : DbContext
    {
        public NewsContext(DbContextOptions<NewsContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<ArticleStatus> ArticleStatuses { get; set; }

        public DbSet<Article> Articles { get; set; }
        
        public DbSet<Like> Likes { get; set; }

        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // prevents likes from throwing multiple cascade errors. 
            modelBuilder.Entity<Like>().HasOne(l => l.Article).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Like>().HasOne(l => l.User).WithMany().OnDelete(DeleteBehavior.Restrict);
            
            //same for comments
            modelBuilder.Entity<Comment>().HasOne(c => c.Article).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Comment>().HasOne(c => c.User).WithMany().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Role>().ToTable("Role");
            modelBuilder.Entity<Tag>().ToTable("Tag");
            modelBuilder.Entity<ArticleStatus>().ToTable("ArticleStatus");
            modelBuilder.Entity<Article>().ToTable("Article");
            modelBuilder.Entity<Like>().ToTable("Like");
            modelBuilder.Entity<Comment>().ToTable("Comment");
        }
    }
}

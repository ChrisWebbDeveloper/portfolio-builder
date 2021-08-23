using System;
using System.Collections.Generic;
using System.Text;
using PortfolioBuilder.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace PortfolioBuilder.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<About> AboutDetails { get; set; }
        public DbSet<AboutPhoto> AboutPhotos { get; set; }
        public DbSet<CarouselPhoto> CarouselPhotos { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Contact> ContactDetails { get; set; }
        public DbSet<FeaturedPortfolio> FeaturedPortfolios { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<PortfolioCategory> PortfolioCategories { get; set; }
        public DbSet<PortfolioPhoto> PortfolioPhotos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // About
            modelBuilder.Entity<About>().ToTable("AboutDetails");

            // AboutPhoto
            modelBuilder.Entity<AboutPhoto>().ToTable("AboutPhotos");
            modelBuilder.Entity<AboutPhoto>().HasIndex(ap => ap.PhotoId).IsUnique();

            // CarouselPhoto
            modelBuilder.Entity<CarouselPhoto>().ToTable("CarouselPhotos");
            modelBuilder.Entity<CarouselPhoto>().HasIndex(cp => cp.PhotoId).IsUnique();

            // Category
            modelBuilder.Entity<Category>().ToTable("Categories");
            modelBuilder.Entity<Category>().HasIndex(ct => ct.Name).IsUnique();
            modelBuilder.Entity<Category>().HasOne(ct => ct.FeaturedPhoto).WithMany(ph => ph.FeaturedInCategories).OnDelete(DeleteBehavior.SetNull);

            // Contact
            modelBuilder.Entity<Contact>().ToTable("ContactDetails");

            // FeaturedPortfolio
            modelBuilder.Entity<FeaturedPortfolio>().ToTable("FeaturedPortfolios");
            modelBuilder.Entity<FeaturedPortfolio>().HasIndex(fp => fp.PortfolioId).IsUnique();

            // Photo
            modelBuilder.Entity<Photo>().ToTable("Photos");
            modelBuilder.Entity<Photo>().HasIndex(ph => ph.Name).IsUnique();
            modelBuilder.Entity<Photo>().HasIndex(ph => ph.FilePath).IsUnique();

            // Portfolio
            modelBuilder.Entity<Portfolio>().ToTable("Portfolios").Ignore(pr => pr.UnencryptedPassword);
            modelBuilder.Entity<Portfolio>().HasIndex(pr => pr.Name).IsUnique();
            modelBuilder.Entity<Portfolio>().HasOne(pr => pr.FeaturedPhoto).WithMany(ph => ph.FeaturedInPortfolios).OnDelete(DeleteBehavior.SetNull);

            // PortfolioCategory
            modelBuilder.Entity<PortfolioCategory>().ToTable("PortfolioCategories");
            modelBuilder.Entity<PortfolioCategory>().HasIndex(pc => new { pc.PortfolioId, pc.CategoryId }).IsUnique();

            // PortfolioPhoto
            modelBuilder.Entity<PortfolioPhoto>().ToTable("PortfolioPhotos");
            modelBuilder.Entity<PortfolioPhoto>().HasIndex(pr => new { pr.PortfolioId, pr.PhotoId }).IsUnique();

            modelBuilder.Initialise();
        }
    }
}

using PortfolioBuilder.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PortfolioBuilder.Data
{
    public static class DbInitialiser
    {
        public static void Initialise(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<About>().HasData(
                new About { Id = 1, Title = "About", Description = "About Me" }
            );

            modelBuilder.Entity<Contact>().HasData(
                new Contact { Id = 1, Name = "Chris Webb", Location = "Nottingham", Email = "chris@chriswebbdeveloper.co.uk" }
            );

            modelBuilder.Entity<Photo>().HasData(
                new Photo { Id = 1, Name = "Photo 1", FilePath = "img_1.jpg" },
                new Photo { Id = 2, Name = "Photo 2", FilePath = "img_2.jpg" },
                new Photo { Id = 3, Name = "Photo 3", FilePath = "img_3.jpg" },
                new Photo { Id = 4, Name = "Photo 4", FilePath = "img_4.jpg" },
                new Photo { Id = 5, Name = "Photo 5", FilePath = "img_5.jpg" },
                new Photo { Id = 6, Name = "Photo 6", FilePath = "img_6.jpg" },
                new Photo { Id = 7, Name = "Photo 7", FilePath = "img_7.jpg" },
                new Photo { Id = 8, Name = "Photo 8", FilePath = "img_8.jpg" }
            );

            modelBuilder.Entity<Portfolio>().HasData(
                new Portfolio { Id = 1, Name = "Portfolio 1", Private = false, Published = true, Position = 1, FeaturedPhotoId = 1 },
                new Portfolio { Id = 2, Name = "Portfolio 2", Private = false, Published = true, Position = 0, FeaturedPhotoId = 5 }
            );

            modelBuilder.Entity<PortfolioPhoto>().HasData(
                new PortfolioPhoto { Id = 1, PortfolioId = 1, PhotoId = 1, Position = 2 },
                new PortfolioPhoto { Id = 2, PortfolioId = 1, PhotoId = 2, Position = 1 },
                new PortfolioPhoto { Id = 3, PortfolioId = 1, PhotoId = 3, Position = 0 },
                new PortfolioPhoto { Id = 4, PortfolioId = 1, PhotoId = 4, Position = 3 },
                new PortfolioPhoto { Id = 5, PortfolioId = 2, PhotoId = 5, Position = 0 },
                new PortfolioPhoto { Id = 6, PortfolioId = 2, PhotoId = 6, Position = 2 },
                new PortfolioPhoto { Id = 7, PortfolioId = 2, PhotoId = 7, Position = 3 },
                new PortfolioPhoto { Id = 8, PortfolioId = 2, PhotoId = 8, Position = 1 }
            );

            modelBuilder.Entity<AboutPhoto>().HasData(
                new AboutPhoto { Id = 1, PhotoId = 1, Position = 0 },
                new AboutPhoto { Id = 2, PhotoId = 2, Position = 1 }
            );

            modelBuilder.Entity<FeaturedPortfolio>().HasData(
                new FeaturedPortfolio { Id = 1, PortfolioId = 1, Position = 0 },
                new FeaturedPortfolio { Id = 2, PortfolioId = 2, Position = 1 }
            );

            modelBuilder.Entity<CarouselPhoto>().HasData(
                new CarouselPhoto { Id = 1, PhotoId = 3, Position = 0 },
                new CarouselPhoto { Id = 2, PhotoId = 6, Position = 1 }
            );

            IdentityUser superuserAcc = new IdentityUser
            {
                Id = "27d346c3-8648-426d-9295-270cb3817ba4",
                UserName = "system@portfoliobuilder.com",
                NormalizedUserName = "system@portfoliobuilder.com".ToUpper(),
                Email = "system@portfoliobuilder.com",
                NormalizedEmail = "system@portfoliobuilder.com".ToUpper(),
                EmailConfirmed = true,
                TwoFactorEnabled = false
            };

            PasswordHasher<IdentityUser> ph = new PasswordHasher<IdentityUser>();
            superuserAcc.PasswordHash = ph.HashPassword(superuserAcc, "System123");

            IdentityUser userManagerAcc = new IdentityUser
            {
                Id = "9484141c-4f0a-4536-946d-834b6b662454",
                UserName = "usermanager@portfoliobuilder.com",
                NormalizedUserName = "usermanager@portfoliobuilder.com".ToUpper(),
                Email = "usermanager@portfoliobuilder.com",
                NormalizedEmail = "usermanager@portfoliobuilder.com".ToUpper(),
                EmailConfirmed = true,
                TwoFactorEnabled = false
            };

            userManagerAcc.PasswordHash = ph.HashPassword(userManagerAcc, "User123");

            modelBuilder.Entity<IdentityUser>().HasData(
                superuserAcc,
                userManagerAcc
            );

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "00b0bd19-dab1-4be5-a56d-4ee893ce349d", Name = "Superuser", NormalizedName = "Superuser".ToUpper() },
                new IdentityRole { Id = "e1f72ad0-7d23-46ba-b670-7c7195dfd4fa", Name = "User Manager", NormalizedName = "User Manager".ToUpper() }
            );

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { UserId = "27d346c3-8648-426d-9295-270cb3817ba4", RoleId = "00b0bd19-dab1-4be5-a56d-4ee893ce349d" },
                new IdentityUserRole<string> { UserId = "9484141c-4f0a-4536-946d-834b6b662454", RoleId = "e1f72ad0-7d23-46ba-b670-7c7195dfd4fa" }
            );













        }
    }
}

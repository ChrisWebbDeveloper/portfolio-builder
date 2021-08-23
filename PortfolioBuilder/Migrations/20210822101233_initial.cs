using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace portfoliobuilder.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AboutDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Description = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    Name = table.Column<string>(type: "varchar(256) CHARACTER SET utf8mb4", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "varchar(256) CHARACTER SET utf8mb4", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    UserName = table.Column<string>(type: "varchar(256) CHARACTER SET utf8mb4", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "varchar(256) CHARACTER SET utf8mb4", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "varchar(256) CHARACTER SET utf8mb4", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "varchar(256) CHARACTER SET utf8mb4", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PasswordHash = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    SecurityStamp = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    PhoneNumber = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Message = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Location = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Email = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    PhoneNo = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Facebook = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Instagram = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    LinkedIn = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true),
                    Description = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    FilePath = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    ClaimType = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ClaimValue = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    ClaimType = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ClaimValue = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "varchar(128) CHARACTER SET utf8mb4", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "varchar(128) CHARACTER SET utf8mb4", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    UserId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    RoleId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    LoginProvider = table.Column<string>(type: "varchar(128) CHARACTER SET utf8mb4", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "varchar(128) CHARACTER SET utf8mb4", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AboutPhotos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AboutId = table.Column<int>(type: "int", nullable: false),
                    PhotoId = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutPhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AboutPhotos_AboutDetails_AboutId",
                        column: x => x.AboutId,
                        principalTable: "AboutDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AboutPhotos_Photos_PhotoId",
                        column: x => x.PhotoId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CarouselPhotos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PhotoId = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: true),
                    ShowName = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    ShowDescription = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarouselPhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarouselPhotos_Photos_PhotoId",
                        column: x => x.PhotoId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true),
                    Published = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: true),
                    Width = table.Column<int>(type: "int", nullable: true),
                    FeaturedPhotoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Photos_FeaturedPhotoId",
                        column: x => x.FeaturedPhotoId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Portfolios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true),
                    Description = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Private = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Published = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Featured = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: true),
                    Width = table.Column<int>(type: "int", nullable: true),
                    PasswordHash = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CustomHtml = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    FeaturedPhotoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Portfolios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Portfolios_Photos_FeaturedPhotoId",
                        column: x => x.FeaturedPhotoId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "FeaturedPortfolios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PortfolioId = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeaturedPortfolios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeaturedPortfolios_Portfolios_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PortfolioCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PortfolioId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: true),
                    Width = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortfolioCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PortfolioCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PortfolioCategories_Portfolios_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PortfolioPhotos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PortfolioId = table.Column<int>(type: "int", nullable: false),
                    PhotoId = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: true),
                    Width = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortfolioPhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PortfolioPhotos_Photos_PhotoId",
                        column: x => x.PhotoId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PortfolioPhotos_Portfolios_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AboutDetails",
                columns: new[] { "Id", "Description", "Title" },
                values: new object[] { 1, "About Me", "About" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "00b0bd19-dab1-4be5-a56d-4ee893ce349d", "d6f69075-d9b1-48b9-a498-7af47c026e5b", "Superuser", "SUPERUSER" },
                    { "e1f72ad0-7d23-46ba-b670-7c7195dfd4fa", "febd306c-9c3e-47a4-8f18-5ccaa2955c3d", "User Manager", "USER MANAGER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "27d346c3-8648-426d-9295-270cb3817ba4", 0, "e45e019c-8c8e-4ccf-8d2c-95dcb457c11d", "system@portfoliobuilder.com", true, false, null, "SYSTEM@PORTFOLIOBUILDER.COM", "SYSTEM@PORTFOLIOBUILDER.COM", "AQAAAAEAACcQAAAAEFP4zPX2LcsnhjDIjYWCMNnRANmyBDyYb098707ropDwJMkXTyv8cnGrHLH2AJpYKg==", null, false, "3904a66b-b9ad-426f-9ad2-436d3caa2863", false, "system@portfoliobuilder.com" },
                    { "9484141c-4f0a-4536-946d-834b6b662454", 0, "8a0df11b-c98c-4a4e-aaf7-16e2aa89397a", "usermanager@portfoliobuilder.com", true, false, null, "USERMANAGER@PORTFOLIOBUILDER.COM", "USERMANAGER@PORTFOLIOBUILDER.COM", "AQAAAAEAACcQAAAAEOtyq4vvBVS2HsyvbiZ2WibuowNY2lLXQaZ6noTs1Ej91St27+TrmCAl7+KU6a+AyQ==", null, false, "f3d4e01f-a55b-46db-a22f-0cb36643ae26", false, "usermanager@portfoliobuilder.com" }
                });

            migrationBuilder.InsertData(
                table: "ContactDetails",
                columns: new[] { "Id", "Email", "Facebook", "Instagram", "LinkedIn", "Location", "Message", "Name", "PhoneNo" },
                values: new object[] { 1, "chris@chriswebbdeveloper.co.uk", null, null, null, "Nottingham", null, "Chris Webb", null });

            migrationBuilder.InsertData(
                table: "Photos",
                columns: new[] { "Id", "Description", "FilePath", "Name" },
                values: new object[,]
                {
                    { 1, null, "img_1.jpg", "Photo 1" },
                    { 2, null, "img_2.jpg", "Photo 2" },
                    { 3, null, "img_3.jpg", "Photo 3" },
                    { 4, null, "img_4.jpg", "Photo 4" },
                    { 5, null, "img_5.jpg", "Photo 5" },
                    { 6, null, "img_6.jpg", "Photo 6" },
                    { 7, null, "img_7.jpg", "Photo 7" },
                    { 8, null, "img_8.jpg", "Photo 8" }
                });

            migrationBuilder.InsertData(
                table: "AboutPhotos",
                columns: new[] { "Id", "AboutId", "PhotoId", "Position" },
                values: new object[,]
                {
                    { 1, 1, 1, 0 },
                    { 2, 1, 2, 1 }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "00b0bd19-dab1-4be5-a56d-4ee893ce349d", "27d346c3-8648-426d-9295-270cb3817ba4" },
                    { "e1f72ad0-7d23-46ba-b670-7c7195dfd4fa", "9484141c-4f0a-4536-946d-834b6b662454" }
                });

            migrationBuilder.InsertData(
                table: "CarouselPhotos",
                columns: new[] { "Id", "PhotoId", "Position", "ShowDescription", "ShowName" },
                values: new object[,]
                {
                    { 1, 3, 0, false, false },
                    { 2, 6, 1, false, false }
                });

            migrationBuilder.InsertData(
                table: "Portfolios",
                columns: new[] { "Id", "CustomHtml", "Description", "Featured", "FeaturedPhotoId", "Name", "PasswordHash", "Position", "Private", "Published", "Width" },
                values: new object[,]
                {
                    { 1, null, null, false, 1, "Portfolio 1", null, 1, false, true, null },
                    { 2, null, null, false, 5, "Portfolio 2", null, 0, false, true, null }
                });

            migrationBuilder.InsertData(
                table: "FeaturedPortfolios",
                columns: new[] { "Id", "PortfolioId", "Position" },
                values: new object[,]
                {
                    { 1, 1, 0 },
                    { 2, 2, 1 }
                });

            migrationBuilder.InsertData(
                table: "PortfolioPhotos",
                columns: new[] { "Id", "PhotoId", "PortfolioId", "Position", "Width" },
                values: new object[,]
                {
                    { 1, 1, 1, 2, null },
                    { 2, 2, 1, 1, null },
                    { 3, 3, 1, 0, null },
                    { 4, 4, 1, 3, null },
                    { 5, 5, 2, 0, null },
                    { 6, 6, 2, 2, null },
                    { 7, 7, 2, 3, null },
                    { 8, 8, 2, 1, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AboutPhotos_AboutId",
                table: "AboutPhotos",
                column: "AboutId");

            migrationBuilder.CreateIndex(
                name: "IX_AboutPhotos_PhotoId",
                table: "AboutPhotos",
                column: "PhotoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CarouselPhotos_PhotoId",
                table: "CarouselPhotos",
                column: "PhotoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_FeaturedPhotoId",
                table: "Categories",
                column: "FeaturedPhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FeaturedPortfolios_PortfolioId",
                table: "FeaturedPortfolios",
                column: "PortfolioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Photos_FilePath",
                table: "Photos",
                column: "FilePath",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Photos_Name",
                table: "Photos",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioCategories_CategoryId",
                table: "PortfolioCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioCategories_PortfolioId_CategoryId",
                table: "PortfolioCategories",
                columns: new[] { "PortfolioId", "CategoryId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioPhotos_PhotoId",
                table: "PortfolioPhotos",
                column: "PhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioPhotos_PortfolioId_PhotoId",
                table: "PortfolioPhotos",
                columns: new[] { "PortfolioId", "PhotoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Portfolios_FeaturedPhotoId",
                table: "Portfolios",
                column: "FeaturedPhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_Portfolios_Name",
                table: "Portfolios",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AboutPhotos");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CarouselPhotos");

            migrationBuilder.DropTable(
                name: "ContactDetails");

            migrationBuilder.DropTable(
                name: "FeaturedPortfolios");

            migrationBuilder.DropTable(
                name: "PortfolioCategories");

            migrationBuilder.DropTable(
                name: "PortfolioPhotos");

            migrationBuilder.DropTable(
                name: "AboutDetails");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Portfolios");

            migrationBuilder.DropTable(
                name: "Photos");
        }
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using PortfolioBuilder.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FluentValidation.AspNetCore;
using FluentValidation;
using PortfolioBuilder.Models;
using PortfolioBuilder.ModelValidators;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using PortfolioBuilder.ViewModels;

namespace PortfolioBuilder
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(
                    Configuration.GetConnectionString("MySqlConnection"),
                    new MySqlServerVersion(new Version(10, 3, 21))
                    )
                );
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
            services.AddRazorPages();

            //Fluent Validation
            services.AddMvc().AddFluentValidation();
            services.AddTransient<IValidator<AboutPhoto>, AboutPhotoValidator>();
            services.AddTransient<IValidator<About>, AboutValidator>();
            services.AddTransient<IValidator<CarouselPhoto>, CarouselPhotoValidator>();
            services.AddTransient<IValidator<Category>, CategoryValidator>();
            services.AddTransient<IValidator<CreateUserViewModel>, CreateUserVMValidator>();
            services.AddTransient<IValidator<Contact>, ContactValidator>();
            services.AddTransient<IValidator<FeaturedPortfolio>, FeaturedPortfolioValidator>();
            services.AddTransient<IValidator<Photo>, PhotoValidator>();
            services.AddTransient<IValidator<PortfolioCategory>, PortfolioCategoryValidator>();
            services.AddTransient<IValidator<PortfolioPhoto>, PortfolioPhotoValidator>();
            services.AddTransient<IValidator<Portfolio>, PortfolioValidator>();
            services.AddTransient<IValidator<RoleViewModel>, RoleVMValidator>();
            services.AddTransient<IValidator<UploadPhotosViewModel>, UploadPhotosVMValidator>();
            services.AddTransient<IValidator<UserViewModel>, UserVMValidator>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
            });

            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = int.MaxValue;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStatusCodePagesWithReExecute("/Error/{0}");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(a =>
                {
                    a.Run(ctx =>
                    {
                        ctx.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        return Task.CompletedTask;
                    });
                });
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{name?}");
                endpoints.MapRazorPages();
            });
        }
    }
}

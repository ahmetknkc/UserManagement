using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Domain.Models.Authentication.Login;
using System;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Domain.Models;
using System.Text;
//using Application.DynamicRole;

internal class Program
{
    public const int swaggerPort = 7149;
    private static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();
        builder.Services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        });
         

        builder.Services.AddHttpClient();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            c.DocInclusionPredicate((docName, apiDesc) =>
            {

                var actionApiVersionModel = apiDesc.ActionDescriptor?.GetApiVersionModel();
                if (actionApiVersionModel == null)
                {
                    return true;
                }
                if (actionApiVersionModel.DeclaredApiVersions.Any())
                {
                    return actionApiVersionModel.DeclaredApiVersions.Any(v => $"v{v}" == docName);
                }
                return actionApiVersionModel.ImplementedApiVersions.Any(v => $"v{v}" == docName);
            });
        });

        //builder.Services.AddDbContext<IdentityDbContext>();
        builder.Services.Configure<CookiePolicyOptions>(options =>
        {
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });

        string connStr = DbConnections.IdentityDB;

        builder.Services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(connStr));


        builder.Services.AddDbContext<EfDbContext>(options => options.UseSqlServer(DbConnections.EntityDB));

        builder.Services.AddHttpContextAccessor();

        builder.Services.Configure<SecurityStampValidatorOptions>(options =>
        {
            options.ValidationInterval = TimeSpan.Zero;
        });

        builder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

            options.LoginPath = "/Auth/Login";
            options.AccessDeniedPath = "/Home/Index";
            options.SlidingExpiration = true;
        });

        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();

        //builder.Services.AddScoped<RoleManager<IdentityRole>>();



        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
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
            endpoints.MapDefaultControllerRoute();
        });


        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");


        app.Run();
    }
}
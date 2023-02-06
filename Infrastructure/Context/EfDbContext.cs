using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Models.EntityFramework;
using Domain.Models;

namespace Infrastructure.Context
{
    public class EfDbContext : DbContext
    {
        public EfDbContext(DbContextOptions<EfDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer(DbConnections.EntityDB, b => b.MigrationsAssembly("API"));


        public static void Seed(ModelBuilder context)
        {


            List<Menu> menus = new List<Menu>() {
                    new Menu
                    {
                        ID = Guid.NewGuid().ToString(),
                        MenuName = "accounting Department",
                        AuthorizeRole = "accounting",
                        Title = "Accounting Department: Ensuring Safe and Accurate Management of Financial Transactions",
                        Content = "The Accounting Department handles financial transactions and record-keeping. " +
                                    "They provide financial analysis and ensure compliance with laws and ethics. " +
                                    "The team is crucial to the financial stability of the organization."
                    },
                    new Menu
                    {
                        ID = Guid.NewGuid().ToString(),
                        MenuName = "software Department",
                        AuthorizeRole = "software",
                        Title = "Software Department: Driving Innovation and Efficiency through Technology",
                        Content = "The Software Department creates and maintains software for innovation and efficiency. " +
                                    "They use latest technology and deliver high-quality products. " +
                                    "The team is essential for the success and growth of the company."
                    }
             };
            context.Entity<Menu>().HasData(menus);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Menu>().ToTable("Menu");
            Seed(modelBuilder);
        }

        public DbSet<Menu> Menu { get; set; }
    }
}

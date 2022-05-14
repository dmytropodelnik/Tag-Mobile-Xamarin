using App1.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace App1.Database
{
    public class ApplicationContext : DbContext
    {
        private string _databasePath;

        public DbSet<User> Users { get; set; }

        public ApplicationContext(string databasePath)
        {
            _databasePath = databasePath;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={_databasePath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User[]
                {
                    new User { Id = 1, Username = "Admin", Password = "123", },
                });

            modelBuilder.Entity<Result>().HasData(
                new Result[]
                {
                    new Result { Id = 1, UserId = 1, Steps = 10, },
                    new Result { Id = 2, UserId = 1, Steps = 15, },
                    new Result { Id = 3, UserId = 1, Steps = 20, },
                    new Result { Id = 4, UserId = 1, Steps = 25, },
                    new Result { Id = 5, UserId = 1, Steps = 35, },
                });
        }
    }
}

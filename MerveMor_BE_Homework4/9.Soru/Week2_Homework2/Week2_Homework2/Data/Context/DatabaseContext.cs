using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Week2_Homework2.Data.Entity;

namespace Week2_Homework2.Data.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }
        public DbSet<Movie> Movie { get; set; }
        public DbSet<MovieType> MovieType { get; set; }
        public DbSet<MovieTypeRelation> MovieTypeRelation { get; set; }
        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new User
            {
                UserId = 1,
                UserName = "admin",
                IsAdmin = true
            });

            modelBuilder.Entity<MovieType>().HasData(new MovieType
            {
                MovieTypeId = 1,
                Name = "Action"

            });

            modelBuilder.Entity<MovieType>().HasData(new MovieType
            {
                MovieTypeId = 2,
                Name = "Comedy"

            });

            modelBuilder.Entity<MovieType>().HasData(new MovieType
            {
                MovieTypeId = 3,
                Name = "Horror"

            });
        }
    }
}

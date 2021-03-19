using HotelFinder.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelFinder.DataAccess
{
    public class HotelDbContext : DbContext //kullanabilmek için nuget kısmından Microsoft.EntityFrameworkCore.SqlServer ekledil
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=LAPTOP-4QDJIGKS\\SQLEXPRESS; Database=HotelDb; uid=sa; pwd=saburo1997");
        }

            public DbSet<Hotel> Hotels { get; set; }
    }
}

using Hotels.API.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotels.API.Contexts
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider service)
        {
            await AddSampleData(service.GetRequiredService<HotelApiDbContext>());
        }
        public static async Task AddSampleData(HotelApiDbContext dbContext)
        {
            if (!dbContext.Rooms.Any())
            { 

                dbContext.Rooms.Add(new RoomEntity
                {
                    Id = Guid.Parse("fbdf9514-f6e1-4fff-8718-3965638af6f6"),
                    Name = "Standart Oda",
                    Rate = 35698,
                    IsMigrate = false
                });

                dbContext.Rooms.Add(new RoomEntity
                {
                    Id = Guid.Parse("167eb96a-01d0-4db9-b316-0676cb34b44c"),
                    Name = "Suit Oda",
                    Rate = 35665,
                    IsMigrate = false
                });
            }

            if(!dbContext.Users.Any())
            {
                dbContext.Users.Add(new UserEntity
                {
                    Id  = 1,
                    Name = "Merve",
                    SurName = "Mor",
                    LoginName = "MM",
                    Password = "1234",
                    Phone = "05500000000"

                });
            }


            await dbContext.SaveChangesAsync();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Enums;

namespace Persistence
{
    public class DataSeeder
    {
        public static void SeedDatabase(WebApiDbContext context)
        {
            if (!context.Users.Any())
            {
                var users = new List<User>
                {
                    new User{ Username = "John Doe", Email = "johndoe@gmail.com", CreatedAt = DateTime.Now, Active = true, Password = "123456", Role = Role.Admin},
                    new User{ Username = "Jane Doe", Email = "janedoe@gmail.com", CreatedAt = DateTime.Now, Active = true, Password = "123456", Role = Role.Manager},
                    new User{ Username = "Dunce Way", Email = "dunceway@gmail.com", CreatedAt = DateTime.Now, Active = true, Password = "123456", Role = Role.Supervisor},
                    new User{ Username = "Mark Bent", Email = "markbent@gmail.com", CreatedAt = DateTime.Now, Active = true, Password = "123456", Role = Role.Employee},
                    new User{ Username = "Pitty Part", Email = "pittypart@gmail.com", CreatedAt = DateTime.Now, Active = true, Password = "123456", Role = Role.Employee},
                };
                context.Users.AddRange(users);
                context.SaveChanges();
            }
        }
    }
}

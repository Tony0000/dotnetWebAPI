using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Model;

namespace Data
{
    public class DataSeeder
    {
        public static void SeedDatabase(WebApiDbContext context)
        {
            if (!context.Users.Any())
            {
                var users = new List<User>
                {
                    new User{ Username = "John Doe", Email = "johndoe@gmail.com", CreatedAt = DateTime.Now, Active = true, Password = "123456"},
                    new User{ Username = "Jane Doe", Email = "janedoe@gmail.com", CreatedAt = DateTime.Now, Active = true, Password = "123456"},
                    new User{ Username = "Dunce Way", Email = "dunceway@gmail.com", CreatedAt = DateTime.Now, Active = true, Password = "123456"},
                    new User{ Username = "Mark Bent", Email = "markbent@gmail.com", CreatedAt = DateTime.Now, Active = true, Password = "123456"},
                    new User{ Username = "Pitty Part", Email = "markbent@gmail.com", CreatedAt = DateTime.Now, Active = true, Password = "123456"},
                };
                context.Users.AddRange(users);
                context.SaveChanges();
            }
        }
    }
}

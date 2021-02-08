using System;
using Domain.Common;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class User : AuditableEntity
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public Role Role { get; set; }

        private string _password;

        public string Password
        {
            get => _password;
            set
            {
                if(string.IsNullOrEmpty(value)) 
                    throw new ArgumentNullException(nameof(Password), "New Password cannot be null.");
                var passwordHasher = new PasswordHasher<User>();
                _password = passwordHasher.HashPassword(this, value);
            }
        }
        public bool ConfirmPassword(string password)
        {
            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(this, Password, password);
            return result != PasswordVerificationResult.Failed;
        }
    }
}

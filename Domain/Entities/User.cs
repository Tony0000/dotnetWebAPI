using System;
using System.ComponentModel.DataAnnotations;
using Domain.Entities.Base;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        [MinLength(6), MaxLength(20), Required]
        public string Username { get; set; }
        [EmailAddress, Required]
        public string Email { get; set; }
        public bool Status { get; set; } = true;
        
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

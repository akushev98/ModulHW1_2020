using System;
using WebApplication.DTO;

namespace WebApplication.Models
{
    public class User
    {
        public User(UserCredentialsDTO user)
        {
            Id = Guid.NewGuid();
            Username = user.Username;
            _password = new Password(user.Password);
            Email = user.Email;
        }

        private Password _password;
        public string Password => _password.PasswordHash;
        public string Salt => _password.Salt;
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
using System;
using System.Collections.Generic;
using WebApplication.DTO;
using WebApplication.Models;

namespace WebApplication.Services
{
    public interface IUserService
    {
        bool IsValidUser(string email, string password);
        void CreateUserItem(UserCredentialsDTO user);
        bool CheckUniqueUsername(String username);
        bool CheckUniqueEmail(String email);
        bool IsValidEmail(string email);
        UserCredentialsDTO GetUserItem(string email);
        List<UserCredentialsDTO> GetAllUsers();
        void DeleteUser(int id);
        void UpdateUserItem(User user);
    }
}
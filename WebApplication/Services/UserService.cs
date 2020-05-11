using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Npgsql;
using WebApplication.DTO;
using WebApplication.Models;

namespace WebApplication.Services
{
    public class UserService : IUserService
    {
        public bool IsValidEmail(string email)
        {
            try {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch {
                return false;
            }
        }

        public bool IsValidUser(string email, string password)
        {
            var testSalt = GetSalt(email);
            var testPassword = GetPassHash(email);
            var flag = Password.CheckPassword(password, testSalt, testPassword);
            return flag;
        }

        public bool CheckUniqueUsername(string username)
        {
            using (var conn = Connection.CreateConnection())
            {
                conn.Open();
                var result = conn.QueryFirstOrDefault<string>("SELECT username FROM employees WHERE username = @username;",
                    new
                    {
                        username
                    });
                if (result == null) return true;
            }
            return false;
        }
        
        public bool CheckUniqueEmail(string email)
        {
            using (var conn = Connection.CreateConnection())
            {
                conn.Open();
                var result = conn.QueryFirstOrDefault<string>("SELECT email FROM employees WHERE email = @email;",
                    new
                    {
                        email
                    });
                if (result == null) return true;
            }
            return false;
        }

        public string GetPassHash(string email)
        {
            using (var conn = Connection.CreateConnection())
            {
                conn.Open();
                var result = conn.QueryFirstOrDefault<string>("SELECT passwordhash FROM employees WHERE email = @email;",
                new
                {
                    email
                });
                return result ?? "";
            }
        }

        public string GetSalt(string email)
        {
            using (var conn = Connection.CreateConnection())
            {
                conn.Open();
                var result = conn.QueryFirstOrDefault<string>("SELECT salt FROM employees WHERE email = @email;",
                    new
                    {
                        email
                    });
                return result ?? "";
            }
        }

        public List<UserCredentialsDTO> GetAllUsers()
        {
            using (var conn = Connection.CreateConnection())
            {
                conn.Open();
                var result = conn.QueryMultiple("SELECT username, email FROM employees;");
                return result.Read<UserCredentialsDTO>().ToList();
            }
        }

        public UserCredentialsDTO GetUserItem(string email)
        {
            using (var conn = Connection.CreateConnection())
            {
                conn.Open();
                var result = conn.QueryFirst<UserCredentialsDTO>(
                    "SELECT email, passwordhash, salt FROM employees WHERE email = @email;",
                    new
                    {
                        email,
                    });
                return result;
            }
        }

        public void CreateUserItem(UserCredentialsDTO user)
        {
            using (var conn = Connection.CreateConnection())
            {
                conn.Open();

                var employee = new User(user);

                conn.Execute(
                    "INSERT INTO employees (id, username, email, passwordHash, salt) VALUES(@id, @username, @email, @passwordHash, @salt);",
                    new
                    {
                        id = employee.Id,
                        username = employee.Username,
                        email = employee.Email,
                        passwordHash = employee.Password,
                        salt = employee.Salt,
                    });
            }
        }
        
        //Пока не используемые методы
        public void UpdateUserItem(User employee)
        {
            using (var conn = Connection.CreateConnection())
            {
                conn.Open();

                conn.Execute(
                    "UPDATE employees SET firstname = @firstname, lastname = @lastname, position = @position WHERE id = @id;",
                    new
                    {
                        id = employee.Id,
                        firstname = employee.Username,
                        lastname = employee.Password,
                        position = employee.Salt,
                    });
            }
        }

        public void DeleteUser(int employeeId)
        {
            using (var conn = Connection.CreateConnection())
            {
                conn.Open();

                conn.Execute("DELETE FROM employees WHERE id = @id;",
                    new
                    {
                        id = employeeId,
                    });
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.DTO;
using WebApplication.Models;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserValueController : Controller
    {
        private static IUserService _repository; 
        
        public UserValueController(IUserService userRepository)
        {
            _repository = userRepository;
        }
        
        [HttpGet]
        public List<UserCredentialsDTO> Get()
        {
            return  _repository.GetAllUsers();
        }
        
        [HttpPost]
        public  List<UserCredentialsDTO> Post([FromBody]UserCredentialsDTO user)
        {
            _repository.CreateUserItem(user);
            return  _repository.GetAllUsers();
        }
        
        [HttpDelete("{id}")]
        public List<UserCredentialsDTO> Delete(int id)
        {
            _repository.DeleteUser(id);
            return  _repository.GetAllUsers();
        }
        
        [HttpPut("{id}")]
        public void  UpdateUserItem(Guid id, [FromBody]User user)
        {
            user.Id = id;
            _repository.UpdateUserItem(user);
        }
    }
}
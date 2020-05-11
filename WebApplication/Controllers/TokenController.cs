using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApplication.DTO;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : Controller
    {
        private readonly IUserService _service;
        private readonly AuthOptions _authOptions;

        public TokenController(IUserService service, IOptions<AuthOptions> authOptionsAccessor)
        {
            _service = service;
            _authOptions = authOptionsAccessor.Value;
        }

        private JwtSecurityToken GetToken(String email)
        {
            var authClaims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                _authOptions.Issuer,
                _authOptions.Audience,
                authClaims,
                DateTime.Now, DateTime.Now.AddMinutes(_authOptions.ExpiresInMinute),
                new SigningCredentials
                (new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authOptions.SecureKey)),
                    SecurityAlgorithms.HmacSha256Signature)
            );
            return token;
        }

        [HttpPost("signin")]
        public IActionResult Authorisation([FromBody] UserCredentialsDTO user)
        {
            if (_service.IsValidUser(user.Email, user.Password))
            {
                var token = GetToken(user.Email);
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(token);

                var response = new
                {
                    access_token = encodedJwt,
                    expiration = token.ValidTo
                };
                return new OkObjectResult(response);
            }
            return new UnauthorizedResult();
        }

        [HttpPost("signup")]
        public IActionResult Registration([FromBody] UserCredentialsDTO user)
        {
            if (_service.IsValidEmail(user.Email))
            {
                if (user.Username.Length >= 3)
                {
                    if (_service.CheckUniqueUsername(user.Username))
                    {
                        if (_service.CheckUniqueEmail(user.Email))
                        {
                            _service.CreateUserItem(user);

                            var token = GetToken(user.Email);
                            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(token);

                            return new OkObjectResult(new
                            {
                                access_token = encodedJwt,
                                expiration = token.ValidTo
                            });
                        }
                        else return new BadRequestObjectResult("A user with this mail is already registered");
                    }
                    else return new BadRequestObjectResult("Username is already taken");
                }
                else return new BadRequestObjectResult("Enter username (Must be at least 3 characters)");
            }
            else return new BadRequestObjectResult("Invalid Email Address");
        }
    }
}
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiProject.DAL;
using WebApiProject.Models;

namespace WebApiProject.ServiceLayer
{
    public class UserService: IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        public UserService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }



        public async Task<string> RegisterUserAsync(User user, string password)
        {
            try
            {
                if (user == null || string.IsNullOrEmpty(password))
                    throw new ArgumentNullException(nameof(password), "Password cannot be null or empty.");

                var passwordHasher = new PasswordHasher<User>();
                user.PasswordHash = passwordHasher.HashPassword(user, password);

                // add user ti data base
                await _unitOfWork.Users.AddUserAsync(user);
                await _unitOfWork.SaveAsync();

                return GenerateJwtToken(user);
            }
            catch (ArgumentNullException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while registering the user.", ex);
            }
        }

        public async Task<string> LoginUserAsync(string userName, string password)
        {
            try
            {
                var user = await _unitOfWork.Users.GetUserByUsernameAsync(userName);
                if (user == null) return null;

                var passwordHasher = new PasswordHasher<User>();
                var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
                if (result != PasswordVerificationResult.Success) return null;

                return GenerateJwtToken(user);
            }
            catch (Exception ex) 
            {
                return null;
            }

        }
        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}

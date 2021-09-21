using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using UserAthemtication.DTOs;
using UserAthentication.Model;

namespace UserAthentication.BusinessLogic
{
    public class Authentication : IAuthentication
    {
        private readonly UserManager<Users> _userManager;
        private readonly ITokenGenerator _tokenGenerator;

        public Authentication(UserManager<Users> userManager, ITokenGenerator tokenGenerator)
        {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<UserResponse> Login(UserLogin userLogin)
        {
            Users user = await _userManager.FindByEmailAsync(userLogin.Email);
            if (user != null)
            {
                if (await _userManager.CheckPasswordAsync(user, userLogin.Password))
                {
                    var response =  UserMappings.GetResponse(user);
                    response.Token = await _tokenGenerator.GetToken(user);
                    
                    return response;
                }
                throw new AccessViolationException("Invalid Credentials");
            }
            throw new AccessViolationException("Invalid Credentials");
        }

        public async Task<UserResponse> Register(string role, RegistrationRequest registrationRequest)
        {
            Users customer = UserMappings.RegisterUser(registrationRequest);
            var result = await _userManager.CreateAsync(customer, registrationRequest.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(customer, role);

                return UserMappings.GetResponse(customer);
            }
            string errors = string.Empty;
            foreach (var error in result.Errors)
            {
                errors += error.Description + Environment.NewLine;
            }
            throw new MissingFieldException(errors);
        }
    }
}

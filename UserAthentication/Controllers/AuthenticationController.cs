using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using UserAthemtication.DTOs;
using UserAthentication.BusinessLogic;
using UserAthentication.Common;
using System.Linq;

namespace UserAthentication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthentication _authentication;
        public AuthenticationController(IAuthentication authentication)
        {
            _authentication = authentication;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
        {
            try
            {
                var user = await _authentication.Login(userLogin);
                //HttpContext.Session.SetString("Token", user.Token);
                return Ok(user);
            }
            catch (AccessViolationException)
            {
                return BadRequest();
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegistrationRequest registrationRequest)
        {
            var user = HttpContext.User.IsInRole(UserRole.Admin.ToString());

            var role = string.Empty;
            try
            {
                if (user)
                {
                    role = UserRole.Admin.ToString();
                }
                else
                    role = UserRole.Customer.ToString();

                var result = await _authentication.Register(role, registrationRequest);
                return Created("", result);
            }
            catch (MissingFieldException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

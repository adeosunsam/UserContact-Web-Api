using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using UserAthemtication.DTOs;
using UserAthentication.BusinessLogic;
using UserAthentication.Common;
using UserAthentication.DTOs;
using UserAthentication.Model;

namespace UserAthentication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IContactService _contactService;
        private readonly IImageService _imageService;
        private readonly UserManager<Users> _userManager;

        public UserController(IContactService contactService,
            IImageService imageService, UserManager<Users> userManager)
        {
            _contactService = contactService;
            _imageService = imageService;
            _userManager = userManager;
        }

        [HttpGet(Name = "GetUser")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllUser([FromQuery] Pagination pagination)
        {
            try
            {
                var allUser = _contactService.GetAll(pagination);
                
                var previousPage = allUser.HasPrevious ? UserUri(pagination, ResourceUriType.PreviousPage) : null;
                var nextPage = allUser.HasNext ? UserUri(pagination, ResourceUriType.NextPage) : null;

                var paginationMetaData = new
                {
                    totalCount = allUser.TotalCount,
                    pageSize = allUser.PageSize,
                    currentPage = allUser.CurrentPage,
                    totalPage = allUser.TotalPage,
                    previousPage = previousPage?.Replace(@"\u0026", "&"),
                    nextPage = (nextPage != null) ? nextPage.Replace(@"\u0026", "&") : null,
                };

                Response.Headers.Add("X-Pagination",
                    JsonSerializer.Serialize(paginationMetaData));
                return Ok(allUser);
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("id")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var user = await _contactService.GetUserById(id);
                return Ok(user);
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("email")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            try
            {
                var user = await _contactService.GetUserByEmail(email);
                return Ok(user);
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPatch]
        [Route("update")]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> UpdateUser(UpdateRequest updateRequest)
        {
            string userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
            try
            {
                await _contactService.UpdateUser(userId, updateRequest);
                return NoContent();
            }
            catch (MissingFieldException)
            {
                return BadRequest();
            }
            catch (ArgumentException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPatch]
        [Route("updateAd")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(UpdateRequest updateRequest, string userId)
        {
            try
            {
                var user = await _contactService.UpdateUser(userId, updateRequest);
                return NoContent();
            }
            catch (MissingFieldException)
            {
                return BadRequest();
            }
            catch (ArgumentException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                var user = await _contactService.DeleteUserById(id);
                return NoContent();
            }
            catch (MissingFieldException)
            {
                return BadRequest();
            }
            catch (ArgumentException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("photo")]
        [Authorize(Roles = "Customer, Admin")]
        public async Task<IActionResult> Image([FromForm] ImageDTOs image)
        {
            var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var user = _userManager.Users.FirstOrDefault(x => x.Id == userId);

            try
            {
                var upload = await _imageService.UploadImage(image.Image);

                user.ProfilePics = upload.Url.ToString();
                await _userManager.UpdateAsync(user);

                return NoContent();
            }
            catch (NotSupportedException)
            {
                return BadRequest();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private string UserUri(Pagination pagination, ResourceUriType type)
        {
            if (type == ResourceUriType.PreviousPage)
            {
                return Url.Link("GetUser",
                    new
                    {
                        pageNumber = pagination.PageNumber - 1,
                        pageSize = pagination.PageSize
                    });
            }
            else if (type == ResourceUriType.NextPage)
            {
                if (pagination.PageNumber < 1)
                {
                    return Url.Link("GetUser",
                    new
                    {
                        pageNumber = 1,
                        pageSize = pagination.PageSize
                    });
                }
                return Url.Link("GetUser",
                    new
                    {
                        pageNumber = pagination.PageNumber + 1,
                        pageSize = pagination.PageSize
                    });
            }
            else
            {
                return Url.Link("GetUser",
                    new
                    {
                        pageNumber = pagination.PageNumber,
                        pageSize = pagination.PageSize
                    });
            }

        }
    }
}

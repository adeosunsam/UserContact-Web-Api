using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAthemtication.DTOs;
using UserAthentication.Common;
using UserAthentication.Model;

namespace UserAthentication.BusinessLogic
{
    public class ContactService : IContactService
    {
        public ContactService(UserManager<Users> userManager)
        {
            _userManager = userManager;
        }
        private readonly UserManager<Users> _userManager;

        public async Task<bool> UpdateUser(string userId, UpdateRequest updateRequest)
        {
            Users user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.FirstName = string.IsNullOrWhiteSpace(updateRequest.FirstName) ? user.FirstName : updateRequest.FirstName;
                user.LastName = string.IsNullOrWhiteSpace(updateRequest.LastName) ? user.LastName : updateRequest.LastName;
                user.PhoneNumber = string.IsNullOrWhiteSpace(updateRequest.PhoneNumber) ? user.PhoneNumber : updateRequest.PhoneNumber;
                user.Last_Updated = DateTime.Now;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return true;
                }
                string errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += error.Description + Environment.NewLine;
                }
                throw new MissingFieldException(errors);
            }
            throw new ArgumentException("Invalid records");
        }

        public async Task<bool> DeleteUserById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return true;
                }
                string errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += error.Description + Environment.NewLine;
                }
                throw new MissingFieldException(errors);
            }
            throw new ArgumentException("Invalid records");
        }

        public async Task<UserResponse> GetUserById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                return UserMappings.GetResponse(user);
            }
            throw new ArgumentException("Invalid records");
        }

        public async Task<UserResponse> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return UserMappings.GetResponse(user);
            }
            throw new ArgumentException("Invalid records");
        }

        public PagedList<Users> GetAll(Pagination pagination)
        {
            var user =  _userManager.Users;
            if (user != null)
            {
                /*if ((pagination.PageSize * (pagination.PageNumber - 1)) > user.ToList().Count)
                {
                    var totalPage = Math.Ceiling(user.ToList().Count / (double)pagination.PageSize);

                    var item = user.Skip(pagination.PageSize * (int)(totalPage - 1))
                                .Take(pagination.PageSize);

                    var paging = PagedList<Users>.Create(user, (int)totalPage, pagination.PageSize);
                    return paging;
                }*/

                var setPage = PagedList<Users>.Create(user, pagination.PageNumber, pagination.PageSize);
                //return UserMappings.GetResponse(setPage);
                return setPage;
            }
            throw new ArgumentException("Invalid records");
        }
    }
}

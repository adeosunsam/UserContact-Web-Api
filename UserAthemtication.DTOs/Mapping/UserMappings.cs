using System.Collections.Generic;
using UserAthentication.Common;
using UserAthentication.Model;

namespace UserAthemtication.DTOs
{
    public class UserMappings
    {
        public static UserResponse GetResponse(Users user)
        {
            return new UserResponse
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                ProfilePics = user.ProfilePics,
                Id = user.Id,
                PassWord = user.PasswordHash
            };
        }

        public static Users RegisterUser(RegistrationRequest registrationRequest)
        {
            return new Users
            {
                FirstName = registrationRequest.FirstName,
                LastName = registrationRequest.LastName,
                Email = registrationRequest.Email,
                UserName = string.IsNullOrWhiteSpace(registrationRequest.UserName) ? registrationRequest.Email : registrationRequest.UserName,
                PasswordHash = registrationRequest.Password,
                PhoneNumber = registrationRequest.PhoneNumber
            };
        }

        public static List<UserResponse> GetResponse(PagedList<Users> user)
        {
            var newList = new List<UserResponse>();

            for (int i = 0; i < user.Count; i++)
            {
                var userResponse = new UserResponse
                {
                    FirstName = user[i].FirstName,
                    LastName = user[i].LastName,
                    Email = user[i].Email,
                    PhoneNumber = user[i].PhoneNumber,
                    ProfilePics = user[i].ProfilePics,
                    Id = user[i].Id,
                    PassWord = user[i].PasswordHash
                };
                newList.Add(userResponse);
            }
            return newList;
        }
    }
}

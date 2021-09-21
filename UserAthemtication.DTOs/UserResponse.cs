using Microsoft.AspNetCore.Http;
using System;

namespace UserAthemtication.DTOs
{
    public class UserResponse
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PassWord { get; set; }
        public string ProfilePics { get; set; }
        public string PhoneNumber { get; set; }
        public string Token { get; set; }
    }
}

using Microsoft.AspNetCore.Http;

namespace UserAthemtication.DTOs
{
    public class UpdateRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;
using System;

namespace UserAthentication.Model
{
    public class Users : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePics { get; set; } = "https://png.pngtree.com/png-vector/20190710/ourmid/pngtree-user-vector-avatar-png-image_1541962.jpg";
        public DateTime Created_at { get; set; } = DateTime.Now;
        public DateTime Last_Updated { get; set; } = DateTime.Now;
    }
}

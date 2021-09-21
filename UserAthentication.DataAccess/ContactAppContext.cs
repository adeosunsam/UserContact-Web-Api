using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using UserAthentication.Model;

namespace UserAthentication.DataAccess
{
    public class ContactAppContext : IdentityDbContext<Users>
    {
        public ContactAppContext(DbContextOptions<ContactAppContext> option) : base(option)
        {

        }
    }
}

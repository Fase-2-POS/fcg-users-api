using Fcg.Users.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fcg.Users.Infrastructure.Data;

public class UsersDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
{
    public UsersDbContext(DbContextOptions<UsersDbContext> options)
        : base(options)
    {
    }
}

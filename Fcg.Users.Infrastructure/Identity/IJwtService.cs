using System.Collections.Generic;

namespace Fcg.Users.Infrastructure.Identity;

public interface IJwtService
{
    string GenerateToken(
        ApplicationUser user,
        IList<string> roles);
}
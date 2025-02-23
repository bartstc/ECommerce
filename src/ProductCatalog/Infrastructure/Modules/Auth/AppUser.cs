using Microsoft.AspNetCore.Identity;

namespace ProductCatalog.Infrastructure.Auth;

public class AppUser : IdentityUser
{
    public string DisplayName { get; set; }
    public string Bio { get; set; }
}

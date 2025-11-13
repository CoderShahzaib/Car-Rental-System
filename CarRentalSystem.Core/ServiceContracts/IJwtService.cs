
using CarRentalSystem.Core.DTOs;
using CarRentalSystem.Core.Identity;

namespace CarRentalSystem.Core.ServiceContracts
{
    public interface IJwtService
    {
        AuthenticationResponse CreateJwtToken(ApplicationUser user);
    }
}

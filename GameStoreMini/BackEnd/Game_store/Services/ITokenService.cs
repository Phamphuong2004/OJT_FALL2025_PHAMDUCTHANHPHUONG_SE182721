using GameStoreMini.Models;

namespace GameStoreMini.Services
{
    // Token service interface - creates JWT tokens for authenticated users.
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
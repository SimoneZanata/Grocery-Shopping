using Server.Entities;

namespace Server.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
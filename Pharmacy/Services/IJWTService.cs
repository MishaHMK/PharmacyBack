using Pharmacy.Entities;

namespace Pharmacy.Services
{
    public interface IJWTService
    {
        Token Authenticate(string id, string name, IEnumerable<string> roles);
    }
}

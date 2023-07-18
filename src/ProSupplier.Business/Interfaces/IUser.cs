using System.Security.Claims;

namespace ProSupplier.Business.Interfaces
{
    public interface IUser
    {
        string Name { get; }    
        Guid GetUserId();
        string GetUserEmail();
        bool IsAuthenticated();
        bool IsInRole(string role);
        IEnumerable<Claim> GetClaimsIdentity();
    }
}

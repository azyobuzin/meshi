using System.Security.Claims;
using System.Threading.Tasks;

namespace MeshiRoulette.Services
{
    public interface IPlaceCollectionAuthorization
    {
        Task<bool> IsAdministrator(string placeCollectionId, ClaimsPrincipal user);
        Task<bool> IsEditable(string placeCollectionId, ClaimsPrincipal user);
    }
}

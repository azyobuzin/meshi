using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MeshiRoulette.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MeshiRoulette.Services
{
    public class PlaceCollectionAuthorization : IPlaceCollectionAuthorization
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public PlaceCollectionAuthorization(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            this._dbContext = dbContext;
            this._userManager = userManager;
        }

        private async Task<PlaceCollectionParticipantType?> GetParticipantAsync(string placeCollectionId, ClaimsPrincipal user)
        {
            var userId = this._userManager.GetUserId(user);
            if (userId == null) return null;

            var record = await this._dbContext.PlaceCollectionParticipants
                .Where(x => x.PlaceCollectionId == placeCollectionId && x.UserId == userId)
                .Select(x => new { x.ParticipantType })
                .SingleOrDefaultAsync()
                .ConfigureAwait(false);
            return record?.ParticipantType;
        }

        public async Task<bool> IsAdministrator(string placeCollectionId, ClaimsPrincipal user)
        {
            return (await this.GetParticipantAsync(placeCollectionId, user).ConfigureAwait(false))
                == PlaceCollectionParticipantType.Administrator;
        }

        public async Task<bool> IsEditable(string placeCollectionId, ClaimsPrincipal user)
        {
            switch (await this.GetParticipantAsync(placeCollectionId, user).ConfigureAwait(false))
            {
                case PlaceCollectionParticipantType.Administrator:
                case PlaceCollectionParticipantType.Editor:
                    return true;
                default:
                    return false;
            }
        }
    }
}

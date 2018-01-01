using System.Linq;
using System.Threading.Tasks;
using MeshiRoulette.Data;
using MeshiRoulette.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeshiRoulette.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public HomeController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var placeCollections = await this._dbContext.PlaceCollections
                .Where(x => x.Accessibility == PlaceCollectionAccessibility.Public)
                .OrderByDescending(x => x.CreatedAt)
                .Select(placeCollection => new HomePlaceCollectionViewModel()
                {
                    Id = placeCollection.Id,
                    Name = placeCollection.Name,
                    Description = placeCollection.Description,
                    CreatedAt = placeCollection.CreatedAt,
                    CreatorScreenName = placeCollection.Creator.ScreenName,
                    CreatorProfileImage = placeCollection.Creator.ProfileImage,
                    PlaceCount = placeCollection.Places.Count
                })
                .ToListAsync();

            return View(placeCollections);
        }
    }
}

using System;
using System.Linq;
using System.Threading.Tasks;
using MeshiRoulette.Data;
using MeshiRoulette.Services;
using MeshiRoulette.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeshiRoulette.Controllers
{
    public class PlaceCollectionsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPlaceCollectionAuthorization _placeCollectionAuthorization;
        private readonly IHostingEnvironment _env;

        public PlaceCollectionsController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager,
            IPlaceCollectionAuthorization placeCollectionAuthorization, IHostingEnvironment env)
        {
            this._dbContext = dbContext;
            this._userManager = userManager;
            this._placeCollectionAuthorization = placeCollectionAuthorization;
            this._env = env;
        }

        public async Task<IActionResult> Index()
        {
            if (this._env.IsDevelopment())
            {
                return this.View(await this._dbContext.PlaceCollections.AsNoTracking().ToListAsync());
            }
            else
            {
                return this.NotFound();
            }
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return this.NotFound();

            var placeCollection = await this._dbContext.PlaceCollections
                .AsNoTracking()
                .Include(x => x.Creator)
                .Include(x => x.Places)
                .ThenInclude(x => x.TagAssociations)
                .ThenInclude(x => x.Tag)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (placeCollection == null) return this.NotFound();

            return this.View(placeCollection);
        }

        [Authorize]
        public IActionResult Create()
        {
            return this.View(new EditPlaceCollectionViewModel() { Accessibility = PlaceCollectionAccessibility.Public });
        }

        [HttpPost, Authorize, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EditPlaceCollectionViewModel viewModel)
        {
            if (this.ModelState.IsValid)
            {
                var userId = this._userManager.GetUserId(this.User);
                var placeCollection = new PlaceCollection(viewModel.Name, userId, viewModel.Description ?? "", viewModel.Accessibility, DateTimeOffset.Now);
                this._dbContext.Add(placeCollection);

                this._dbContext.Add(new PlaceCollectionParticipant(placeCollection.Id, userId, PlaceCollectionParticipantType.Administrator));

                await this._dbContext.SaveChangesAsync();

                return this.RedirectToAction(nameof(Details), new { placeCollection.Id });
            }

            return this.View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            if (!await this._placeCollectionAuthorization.IsEditable(id, this.User))
            {
                return this.Unauthorized();
            }

            var placeCollection = await this._dbContext.PlaceCollections
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.Id == id);
            if (placeCollection == null)
            {
                return this.NotFound();
            }

            return this.View(new EditPlaceCollectionViewModel(placeCollection));
        }

        [HttpPost, Authorize, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, EditPlaceCollectionViewModel viewModel)
        {
            if (id != viewModel.Id) return this.NotFound();

            var placeCollection = await this._dbContext.PlaceCollections.SingleOrDefaultAsync(x => x.Id == id);
            if (placeCollection == null) return this.NotFound();

            if (!await this._placeCollectionAuthorization.IsEditable(id, this.User))
                return this.Unauthorized();

            if (this.ModelState.IsValid)
            {
                viewModel.ApplyTo(placeCollection);
                await this._dbContext.SaveChangesAsync();

                return this.RedirectToAction(nameof(Details), new { id });
            }

            viewModel.ExistingName = placeCollection.Name;
            return this.View(viewModel);
        }

        [HttpPost, Authorize, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (!await this._placeCollectionAuthorization.IsAdministrator(id, this.User))
                return this.Unauthorized();

            var placeCollection = await this._dbContext.PlaceCollections.SingleAsync(m => m.Id == id);
            this._dbContext.PlaceCollections.Remove(placeCollection);
            await this._dbContext.SaveChangesAsync();

            return this.RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> RouletteData(string id)
        {
            var places = await this._dbContext.Places
                .AsNoTracking()
                .Where(x => x.PlaceCollectionId == id)
                .Include(x => x.TagAssociations)
                .ToArrayAsync();

            var tagIds = places.SelectMany(x => x.TagAssociations)
                .Select(x => x.TagId)
                .Distinct()
                .ToArray();

            var tags = await this._dbContext.PlaceTags
                .Where(x => tagIds.Contains(x.Id))
                .OrderBy(x => x.Name)
                .Select(x => new { id = x.Id, name = x.Name })
                .ToArrayAsync();

            return this.Json(new
            {
                places = places.Select(x => new { id = x.Id, name = x.Name, tags = x.TagAssociations.Select(y => y.TagId) }),
                tags
            });
        }
    }
}

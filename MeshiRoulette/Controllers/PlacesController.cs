using System;
using System.Threading.Tasks;
using MeshiRoulette.Data;
using MeshiRoulette.Services;
using MeshiRoulette.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeshiRoulette.Controllers
{
    public class PlacesController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IPlaceCollectionAuthorization _placeCollectionAuthorization;
        private readonly IHostingEnvironment _env;

        public PlacesController(ApplicationDbContext dbContext, IPlaceCollectionAuthorization placeCollectionAuthorization, IHostingEnvironment env)
        {
            this._dbContext = dbContext;
            this._placeCollectionAuthorization = placeCollectionAuthorization;
            this._env = env;
        }

        public async Task<IActionResult> Index()
        {
            if (this._env.IsDevelopment())
            {
                var places = await this._dbContext.Places
                    .AsNoTracking()
                    .Include(p => p.PlaceCollection)
                    .ToArrayAsync();
                return this.View(places);
            }
            else
            {
                return this.NotFound();
            }
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return this.NotFound();

            var place = await this._dbContext.Places
                .AsNoTracking()
                .Include(p => p.PlaceCollection)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (place == null) return this.NotFound();

            return this.View(place);
        }

        [Authorize]
        public async Task<IActionResult> Create(string placeCollectionId)
        {
            if (!await this._placeCollectionAuthorization.IsEditable(placeCollectionId, this.User))
                return this.Unauthorized();

            return this.View(new EditPlaceViewModel() { PlaceCollectionId = placeCollectionId });
        }

        [HttpPost, Authorize, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string placeCollectionId, EditPlaceViewModel viewModel, string tags)
        {
            if (placeCollectionId != viewModel.PlaceCollectionId)
                return this.NotFound();

            if (!await this._placeCollectionAuthorization.IsEditable(placeCollectionId, this.User))
                return this.Unauthorized();

            if (this.ModelState.IsValid)
            {
                var place = new Place(viewModel.Name, viewModel.Latitude, viewModel.Longitude, viewModel.Address, placeCollectionId, DateTimeOffset.Now);
                this._dbContext.Add(place);

                switch (await PlaceTagManager.SetTagsToPlaceAsync(this._dbContext, place.Id, tags))
                {
                    case PlaceTagsActionResult.Success: break;
                    case PlaceTagsActionResult.InvalidTagsData: return this.BadRequest();
                    default: throw new Exception();
                }

                await this._dbContext.SaveChangesAsync();

                return this.RedirectToAction(nameof(Details), new { place.Id });
            }

            return this.View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null) return this.NotFound();

            var place = await this._dbContext.Places
                .AsNoTracking()
                .Include(x => x.TagAssociations)
                .ThenInclude(x => x.Tag)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (place == null) return this.NotFound();

            if (!await this._placeCollectionAuthorization.IsEditable(place.PlaceCollectionId, this.User))
                return this.Unauthorized();

            return this.View(new EditPlaceViewModel(place));
        }

        [HttpPost, Authorize, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, EditPlaceViewModel viewModel, string tags)
        {
            if (id != viewModel.Id) return this.NotFound();

            if (this.ModelState.IsValid)
            {
                var place = await this._dbContext.Places.SingleOrDefaultAsync(x => x.Id == id);

                if (place == null) return this.NotFound();
                if (!await this._placeCollectionAuthorization.IsEditable(place.PlaceCollectionId, this.User))
                    return this.Unauthorized();

                viewModel.ApplyTo(place);

                switch (await PlaceTagManager.SetTagsToPlaceAsync(this._dbContext, place.Id, tags))
                {
                    case PlaceTagsActionResult.Success: break;
                    case PlaceTagsActionResult.InvalidTagsData: return this.BadRequest();
                    default: throw new Exception();
                }

                await this._dbContext.SaveChangesAsync();

                return this.RedirectToAction(nameof(Details), new { id });
            }

            return this.View(viewModel);
        }

        [HttpPost, Authorize, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            var place = await this._dbContext.Places.SingleOrDefaultAsync(m => m.Id == id);

            if (place == null) return this.NotFound();
            if (!await this._placeCollectionAuthorization.IsEditable(place.PlaceCollectionId, this.User))
                return this.Unauthorized();

            this._dbContext.Places.Remove(place);
            await this._dbContext.SaveChangesAsync();

            return this.RedirectToPlaceCollection(place.PlaceCollectionId);
        }

        private IActionResult RedirectToPlaceCollection(string placeCollectionId)
        {
            return this.RedirectToAction("Details", "PlaceCollections", new { id = placeCollectionId });
        }
    }
}

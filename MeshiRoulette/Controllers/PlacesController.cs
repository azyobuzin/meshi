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
                    .ToListAsync();
                return this.View(places);
            }
            else
            {
                return this.NotFound();
            }
        }

        public async Task<IActionResult> Details(string placeCollectionId, long? id)
        {
            if (placeCollectionId == null || id == null) return this.NotFound();

            var place = await this._dbContext.Places
                .AsNoTracking()
                .Include(p => p.PlaceCollection)
                .Include(x => x.TagAssociations)
                .ThenInclude(x => x.Tag)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (place == null || place.PlaceCollectionId != placeCollectionId)
                return this.NotFound();

            return this.View(place);
        }

        [Authorize]
        public async Task<IActionResult> Create(string placeCollectionId)
        {
            if (placeCollectionId == null) return this.BadRequest();

            if (!await this._placeCollectionAuthorization.IsEditable(placeCollectionId, this.User))
                return this.Unauthorized();

            var placeCollection = await this._dbContext.PlaceCollections
                .AsNoTracking()
                .SingleAsync(x => x.Id == placeCollectionId);

            return this.View(new EditPlaceViewModel(placeCollection));
        }

        [HttpPost, Authorize, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EditPlaceViewModel viewModel, string tags)
        {
            var placeCollectionId = viewModel.PlaceCollectionId;
            if (placeCollectionId == null) return this.BadRequest();

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

                return this.RedirectToAction(nameof(Details), new { placeCollectionId, place.Id });
            }

            viewModel.Tags = PlaceTagManager.ParseTagsData(tags); // 不正なデータのチェックも兼ねて一度 parse

            var placeCollection = await this._dbContext.PlaceCollections
               .AsNoTracking()
               .SingleAsync(x => x.Id == placeCollectionId);

            viewModel.PlaceCollectionName = placeCollection.Name;

            return this.View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null) return this.NotFound();

            var place = await this._dbContext.Places
                .AsNoTracking()
                .Include(x => x.PlaceCollection)
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

            var place = await this._dbContext.Places.SingleOrDefaultAsync(x => x.Id == id);
            if (place == null) return this.NotFound();

            if (!await this._placeCollectionAuthorization.IsEditable(place.PlaceCollectionId, this.User))
                return this.Unauthorized();

            if (this.ModelState.IsValid)
            {
                viewModel.ApplyTo(place);

                switch (await PlaceTagManager.SetTagsToPlaceAsync(this._dbContext, place.Id, tags))
                {
                    case PlaceTagsActionResult.Success: break;
                    case PlaceTagsActionResult.InvalidTagsData: return this.BadRequest();
                    default: throw new Exception();
                }

                await this._dbContext.SaveChangesAsync();

                return this.RedirectToAction(nameof(Details), new { place.PlaceCollectionId, id });
            }

            viewModel.ExistingName = place.Name;
            viewModel.Tags = PlaceTagManager.ParseTagsData(tags);

            var placeCollection = await this._dbContext.PlaceCollections
                .AsNoTracking()
                .SingleAsync(x => x.Id == place.PlaceCollectionId);

            viewModel.PlaceCollectionName = placeCollection.Name;

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

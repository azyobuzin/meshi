﻿using System;
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
                var places = this._dbContext.Places.Include(p => p.PlaceCollection);
                return this.View(await places.ToListAsync());
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

            return this.View();
        }

        [HttpPost, Authorize, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string placeCollectionId, EditPlaceViewModel viewModel)
        {
            if (placeCollectionId != viewModel.PlaceCollectionId)
                return this.NotFound();

            if (!await this._placeCollectionAuthorization.IsEditable(placeCollectionId, this.User))
                return this.Unauthorized();

            if (this.ModelState.IsValid)
            {
                this._dbContext.Add(new Place(viewModel.Name, viewModel.Latitude, viewModel.Longitude, placeCollectionId, DateTimeOffset.Now));
                await this._dbContext.SaveChangesAsync();

                return this.RedirectToPlaceCollection(placeCollectionId);
            }

            return this.View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null) return this.NotFound();

            var place = await this._dbContext.Places.SingleOrDefaultAsync(m => m.Id == id);
            if (place == null) return this.NotFound();

            if (!await this._placeCollectionAuthorization.IsEditable(place.PlaceCollectionId, this.User))
                return this.Unauthorized();

            return this.View(new EditPlaceViewModel(place));
        }

        [HttpPost, Authorize, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, EditPlaceViewModel viewModel)
        {
            if (id != viewModel.Id) return this.NotFound();

            if (this.ModelState.IsValid)
            {
                var place = await this._dbContext.Places.SingleOrDefaultAsync(x => x.Id == id);

                if (place == null) return this.NotFound();
                if (!await this._placeCollectionAuthorization.IsEditable(place.PlaceCollectionId, this.User))
                    return this.Unauthorized();

                place.Name = viewModel.Name;
                place.Latitude = viewModel.Latitude;
                place.Longitude = viewModel.Longitude;

                await this._dbContext.SaveChangesAsync();

                return this.RedirectToPlaceCollection(place.PlaceCollectionId);
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

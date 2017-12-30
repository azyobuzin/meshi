﻿using System;
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
                return this.View(await this._dbContext.PlaceCollections.ToListAsync());
            }
            else
            {
                return this.NotFound();
            }
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var placeCollection = await this._dbContext.PlaceCollections
                .SingleOrDefaultAsync(m => m.Id == id);
            if (placeCollection == null)
            {
                return this.NotFound();
            }

            return this.View(placeCollection);
        }

        [Authorize]
        public IActionResult Create()
        {
            return this.View();
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

                return this.RedirectToAction(nameof(Index));
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

            var placeCollection = await this._dbContext.PlaceCollections.SingleOrDefaultAsync(m => m.Id == id);
            if (placeCollection == null)
            {
                return this.NotFound();
            }

            return this.View(new EditPlaceCollectionViewModel(placeCollection));
        }

        [HttpPost, Authorize, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, EditPlaceCollectionViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return this.NotFound();
            }

            if (!await this._placeCollectionAuthorization.IsEditable(id, this.User))
            {
                return this.Unauthorized();
            }

            if (this.ModelState.IsValid)
            {
                var placeCollection = await this._dbContext.PlaceCollections
                    .SingleOrDefaultAsync(x => x.Id == id);

                if (placeCollection == null) return this.NotFound();

                placeCollection.Name = viewModel.Name;
                placeCollection.Description = viewModel.Description ?? "";
                placeCollection.Accessibility = viewModel.Accessibility;

                await this._dbContext.SaveChangesAsync();

                return this.RedirectToAction(nameof(Index));
            }

            return this.View(viewModel);
        }

        [HttpPost, Authorize, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (!await this._placeCollectionAuthorization.IsAdministrator(id, this.User))
            {
                return this.Unauthorized();
            }

            var placeCollection = await this._dbContext.PlaceCollections.SingleOrDefaultAsync(m => m.Id == id);
            this._dbContext.PlaceCollections.Remove(placeCollection);
            await this._dbContext.SaveChangesAsync();

            return this.RedirectToAction(nameof(Index));
        }
    }
}

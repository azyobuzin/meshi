using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using MeshiRoulette.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MeshiRoulette.Services
{
    public class ApplicationUserStore : IUserStore<ApplicationUser>
    {
        public ApplicationDbContext DbContext { get; }
        private readonly IdentityErrorDescriber _errorDescriber;

        public ApplicationUserStore(ApplicationDbContext dbContext, IdentityErrorDescriber errorDescriber)
        {
            this.DbContext = dbContext;
            this._errorDescriber = errorDescriber ?? new IdentityErrorDescriber();
        }

        public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString(CultureInfo.InvariantCulture));
        }

        public Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Name);
        }

        public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken)
        {
            user.Name = userName;
            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            await this.DbContext.AddAsync(user, cancellationToken).ConfigureAwait(false);

            try
            {
                await this.DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException)
            {
                return IdentityResult.Failed(this._errorDescriber.ConcurrencyFailure());
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            this.DbContext.Update(user);

            try
            {
                await this.DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException)
            {
                return IdentityResult.Failed(this._errorDescriber.ConcurrencyFailure());
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            this.DbContext.Remove(user);

            try
            {
                await this.DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException)
            {
                return IdentityResult.Failed(this._errorDescriber.ConcurrencyFailure());
            }

            return IdentityResult.Success;
        }

        public Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var id = long.Parse(userId, CultureInfo.InvariantCulture);
            return this.DbContext.Users.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        public void Dispose() { }
    }
}

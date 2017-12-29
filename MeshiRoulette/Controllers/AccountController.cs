using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MeshiRoulette.Data;
using Microsoft.AspNetCore.Authentication.Twitter;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MeshiRoulette.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOptionsSnapshot<TwitterOptions> _twitterOptions;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IOptionsSnapshot<TwitterOptions> twitterOptions)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._twitterOptions = twitterOptions;
        }

        public IActionResult Login(string returnUrl = null)
        {
            var provider = TwitterDefaults.AuthenticationScheme;
            var properties = this._signInManager.ConfigureExternalAuthenticationProperties(
                provider,
                this.Url.Action(nameof(LoginCallback), new { returnUrl })
            );
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> LoginCallback(string returnUrl = null, string remoteError = null)
        {
            IActionResult LoginFailed(params string[] messages) => this.View(messages);

            IdentityResult identityResult;

            if (remoteError != null) return LoginFailed(remoteError);

            var info = await this._signInManager.GetExternalLoginInfoAsync();
            if (info == null) return LoginFailed("もう一度ログインし直してください。");

            var screenName = info.Principal.FindFirstValue(MeshiRouletteClaimTypes.TwitterScreenName);
            var profileImage = info.Principal.FindFirstValue(MeshiRouletteClaimTypes.TwitterProfileImage);

            if (string.IsNullOrEmpty(screenName) || string.IsNullOrEmpty(profileImage))
                return LoginFailed("ユーザー情報が取得できませんでした。");

            var user = await this._userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            if (user == null)
            {
                // 新規登録
                user = new ApplicationUser()
                {
                    ScreenName = screenName,
                    ProfileImage = profileImage,
                    CreatedAt = DateTimeOffset.Now
                };
                user.UserName = user.Id; // Id は Guid なのでそれを流用

                identityResult = await this._userManager.CreateAsync(user);
                if (!identityResult.Succeeded) goto IdentityError;

                identityResult = await this._userManager.AddLoginAsync(user, info);
                if (!identityResult.Succeeded) goto IdentityError;
            }
            else
            {
                // ユーザー情報更新
                user.ScreenName = screenName;
                user.ProfileImage = profileImage;
                identityResult = await this._userManager.UpdateAsync(user);
                if (!identityResult.Succeeded) goto IdentityError;
            }

            await this._signInManager.SignInAsync(user, true);

            return string.IsNullOrEmpty(returnUrl)
                  ? (IActionResult)this.RedirectToAction("Index", "Home")
                  : this.LocalRedirect(returnUrl);

            IdentityError:
            return this.View(identityResult.Errors.Select(x => x.Description).ToArray());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await this._signInManager.SignOutAsync();
            return this.RedirectToAction("Index", "Home");
        }
    }
}

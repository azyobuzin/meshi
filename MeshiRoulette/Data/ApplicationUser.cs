using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MeshiRoulette.Data
{
    public class ApplicationUser : IdentityUser
    {
        /// <summary>ユーザーの表示名</summary>
        /// <remarks>Twitter しか使わない間は screen_name で</remarks>
        [Required]
        public string ScreenName { get; set; }

        /// <summary>プロフィール画像の URI</summary>
        [Required]
        public string ProfileImage { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public ApplicationUser() : base() { }

        public ApplicationUser(string screenName, string profileImage, DateTimeOffset createdAt)
            : base()
        {
            this.UserName = this.Id; // Id は Guid なのでそれを流用
            this.ScreenName = screenName;
            this.ProfileImage = profileImage;
            this.CreatedAt = createdAt;
        }
    }
}

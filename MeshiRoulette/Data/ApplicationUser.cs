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
    }
}

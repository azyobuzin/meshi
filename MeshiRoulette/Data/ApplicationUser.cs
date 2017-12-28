using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeshiRoulette.Data
{
    public class ApplicationUser
    {
        public long Id { get; set; }

        /// <summary>ユーザーの表示名</summary>
        /// <remarks>Twitter しか使わない間は screen_name で</remarks>
        [Required]
        public string Name { get; set; }

        /// <summary>プロフィール画像の URI</summary>
        [Required]
        public string ProfileImage { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTimeOffset CreatedAt { get; set; }
    }
}

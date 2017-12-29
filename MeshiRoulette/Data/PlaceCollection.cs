using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MeshiRoulette.Data
{
    /// <summary>いわゆるルーレット</summary>
    public class PlaceCollection
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string CreatorId { get; set; }
        public ApplicationUser Creator { get; set; }

        [Required]
        public string Description { get; set; }

        public PlaceCollectionAccessibility Accessibility { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public List<Place> Places { get; set; }

        public PlaceCollection() { }

        public PlaceCollection(string name, string creatorId, string description, PlaceCollectionAccessibility accessibility, DateTimeOffset createdAt)
        {
            this.Id = Guid.NewGuid().ToString("N");
            this.Name = name;
            this.CreatorId = creatorId;
            this.Description = description;
            this.Accessibility = accessibility;
            this.CreatedAt = createdAt;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MeshiRoulette.Data
{
    public class Place
    {
        /// <remarks><see cref="Place"/> にアクセスするときは <see cref="PlaceCollectionId"/> とセットで合っているか確認すること！</remarks>
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public List<PlaceTagAssociation> TagAssociations { get; set; }

        public string PlaceCollectionId { get; set; }
        public PlaceCollection PlaceCollection { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public Place() { }

        public Place(string name, double? latitude, double? longitude, string placeCollectionId, DateTimeOffset createdAt)
        {
            this.Name = name;
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.PlaceCollectionId = placeCollectionId;
            this.CreatedAt = createdAt;
        }
    }
}

using System;
using System.Collections.Generic;

namespace MeshiRoulette.Data
{
    /// <summary>いわゆるルーレット</summary>
    public class PlaceCollection
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public PlaceCollectionAccessibility Accessibility { get; set; }

        public List<Place> Places { get; set; }

        public List<PlaceTag> PlaceTags { get; set; }

        public PlaceCollection()
        {
            this.Id = Guid.NewGuid().ToString("N");
        }
    }
}

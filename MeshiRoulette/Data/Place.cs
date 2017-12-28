using System.Collections.Generic;

namespace MeshiRoulette.Data
{
    public class Place
    {
        /// <remarks><see cref="Place"/> にアクセスするときは <see cref="PlaceCollectionId"/> とセットで合っているか確認すること！</remarks>
        public long Id { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public List<PlaceTagAssociation> TagAssociations { get; set; }

        public string PlaceCollectionId { get; set; }
        public PlaceCollection PlaceCollection { get; set; }
    }
}

using System;

namespace MeshiRoulette.ViewModels
{
    public class HomePlaceCollectionViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string CreatorScreenName { get; set; }
        public string CreatorProfileImage { get; set; }
        public int PlaceCount { get; set; }
    }
}

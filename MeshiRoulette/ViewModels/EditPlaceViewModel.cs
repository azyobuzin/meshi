using System.ComponentModel.DataAnnotations;
using MeshiRoulette.Data;

namespace MeshiRoulette.ViewModels
{
    public class EditPlaceViewModel
    {
        public long Id { get; set; }

        [Display(Name = "場所名"), Required]
        public string Name { get; set; }

        // TODO: Latitude, Longitude がセットで代入されているか検証
        [Display(Name = "緯度")]
        public double? Latitude { get; set; }

        [Display(Name = "経度")]
        public double? Longitude { get; set; }

        public string PlaceCollectionId { get; set; }

        public EditPlaceViewModel() { }

        public EditPlaceViewModel(Place place)
        {
            this.Id = place.Id;
            this.Name = place.Name;
            this.Latitude = place.Latitude;
            this.Longitude = place.Longitude;
            this.PlaceCollectionId = place.PlaceCollectionId;
        }
    }
}

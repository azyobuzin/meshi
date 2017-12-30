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

        [Display(Name = "住所")]
        public string Address { get; set; }

        public string PlaceCollectionId { get; set; }

        public EditPlaceViewModel() { }

        public EditPlaceViewModel(Place place)
        {
            this.Id = place.Id;
            this.Name = place.Name;
            this.Latitude = place.Latitude;
            this.Longitude = place.Longitude;
            this.Address = place.Address;
            this.PlaceCollectionId = place.PlaceCollectionId;
        }

        public void ApplyTo(Place place)
        {
            place.Name = this.Name;
            place.Latitude = this.Latitude;
            place.Longitude = this.Longitude;
            place.Address = this.Address;
        }
    }
}

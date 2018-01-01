using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MeshiRoulette.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace MeshiRoulette.ViewModels
{
    public class EditPlaceViewModel
    {
        // [BindNever] が指定されているものはサーバーサイドで用意

        public long Id { get; set; }

        /// <summary>DB に保存されている名前</summary>
        [BindNever]
        public string ExistingName { get; set; }

        [Display(Name = "場所名"), Required]
        public string Name { get; set; }

        // TODO: Latitude, Longitude がセットで代入されているか検証
        [Display(Name = "緯度")]
        public double? Latitude { get; set; }

        [Display(Name = "経度")]
        public double? Longitude { get; set; }

        [Display(Name = "住所")]
        public string Address { get; set; }

        [BindNever]
        public List<string> Tags { get; set; }

        public string PlaceCollectionId { get; set; }

        [BindNever]
        public string PlaceCollectionName { get; set; }

        public EditPlaceViewModel() { }

        public EditPlaceViewModel(Place place)
        {
            this.Id = place.Id;
            this.ExistingName = place.Name;
            this.Name = place.Name;
            this.Latitude = place.Latitude;
            this.Longitude = place.Longitude;
            this.Address = place.Address;
            this.Tags = place.TagAssociations.ConvertAll(x => x.Tag.Name);
            this.PlaceCollectionId = place.PlaceCollectionId;
            this.PlaceCollectionName = place.PlaceCollection.Name;
        }

        public EditPlaceViewModel(PlaceCollection placeCollection)
        {
            this.PlaceCollectionId = placeCollection.Id;
            this.PlaceCollectionName = placeCollection.Name;
        }

        public void ApplyTo(Place place)
        {
            place.Name = this.Name;
            place.Latitude = this.Latitude;
            place.Longitude = this.Longitude;
            place.Address = this.Address;
        }

        public string GetTagsJson() => JsonConvert.SerializeObject(this.Tags);
    }
}

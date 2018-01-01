using System.ComponentModel.DataAnnotations;
using MeshiRoulette.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MeshiRoulette.ViewModels
{
    public class EditPlaceCollectionViewModel
    {
        public string Id { get; set; }

        /// <summary>DB に保存されている名前</summary>
        [BindNever]
        public string ExistingName { get; set; }

        [Display(Name = "ルーレット名"), Required]
        public string Name { get; set; }

        [Display(Name = "説明")]
        public string Description { get; set; }

        [Display(Name = "公開範囲")] // TODO: validation
        public PlaceCollectionAccessibility Accessibility { get; set; }

        public EditPlaceCollectionViewModel() { }

        public EditPlaceCollectionViewModel(PlaceCollection placeCollection)
        {
            this.Id = placeCollection.Id;
            this.ExistingName = placeCollection.Name;
            this.Name = placeCollection.Name;
            this.Description = placeCollection.Description;
            this.Accessibility = placeCollection.Accessibility;
        }

        public void ApplyTo(PlaceCollection placeCollection)
        {
            placeCollection.Name = this.Name;
            placeCollection.Description = this.Description ?? "";
            placeCollection.Accessibility = this.Accessibility;
        }
    }
}

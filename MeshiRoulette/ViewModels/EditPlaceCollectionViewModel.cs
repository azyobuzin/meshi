using System.ComponentModel.DataAnnotations;
using MeshiRoulette.Data;

namespace MeshiRoulette.ViewModels
{
    public class EditPlaceCollectionViewModel
    {
        public string Id { get; set; }

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
            this.Name = placeCollection.Name;
            this.Description = placeCollection.Description;
            this.Accessibility = placeCollection.Accessibility;
        }
    }
}

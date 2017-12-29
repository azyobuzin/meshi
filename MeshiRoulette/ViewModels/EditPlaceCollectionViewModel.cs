using System.ComponentModel.DataAnnotations;
using MeshiRoulette.Data;

namespace MeshiRoulette.ViewModels
{
    public class EditPlaceCollectionViewModel
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Range(1, 2)]
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

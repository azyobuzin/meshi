using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MeshiRoulette.Data
{
    public class PlaceTag
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        public List<PlaceTagAssociation> Associations { get; set; }
    }
}

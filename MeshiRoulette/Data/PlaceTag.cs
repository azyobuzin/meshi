﻿using System.Collections.Generic;

namespace MeshiRoulette.Data
{
    public class PlaceTag
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string PlaceCollectionId { get; set; }
        public PlaceCollection PlaceCollection { get; set; }

        public List<PlaceTagAssociation> Associations { get; set; }
    }
}
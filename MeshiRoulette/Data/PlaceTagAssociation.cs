namespace MeshiRoulette.Data
{
    /// <summary>
    /// <see cref="Place"/> と <see cref="PlaceTag"/> の中間テーブル
    /// </summary>
    public class PlaceTagAssociation
    {
        public long PlaceId { get; set; }
        public Place Place { get; set; }

        public long TagId { get; set; }
        public PlaceTag Tag { get; set; }

        public PlaceTagAssociation() { }

        public PlaceTagAssociation(long placeId, long tagId)
        {
            this.PlaceId = placeId;
            this.TagId = tagId;
        }
    }
}

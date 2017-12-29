using System.ComponentModel.DataAnnotations;

namespace MeshiRoulette.Data
{
    public class PlaceCollectionParticipant
    {
        public long Id { get; set; }

        [Required]
        public string PlaceCollectionId { get; set; }
        public PlaceCollection PlaceCollection { get; set; }

        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public PlaceCollectionParticipantType ParticipantType { get; set; }

        public PlaceCollectionParticipant() { }

        public PlaceCollectionParticipant(string placeCollectionId, string userId, PlaceCollectionParticipantType participantType)
        {
            this.PlaceCollectionId = placeCollectionId;
            this.UserId = userId;
            this.ParticipantType = participantType;
        }
    }
}

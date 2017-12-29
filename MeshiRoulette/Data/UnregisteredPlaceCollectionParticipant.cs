using System.ComponentModel.DataAnnotations;

namespace MeshiRoulette.Data
{
    /// <summary>
    /// ユーザー登録されていないアカウントの編集権限を保存しておくところ
    /// </summary>
    public class UnregisteredPlaceCollectionParticipant
    {
        public long Id { get; set; }

        [Required]
        public string PlaceCollectionId { get; set; }
        public PlaceCollection PlaceCollection { get; set; }

        public ExternalIdType ExternalIdType { get; set; }

        [Required]
        public string ExternalId { get; set; }

        public PlaceCollectionParticipantType ParticipantType { get; set; }
    }
}

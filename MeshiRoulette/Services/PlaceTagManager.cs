using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MeshiRoulette.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace MeshiRoulette.Services
{
    public static class PlaceTagManager
    {
        /// <returns>不正なデータなら <c>null</c></returns>
        private static List<string> ParseTagsData(string tagsData)
        {
            try
            {
                return JsonConvert.DeserializeObject<List<string>>(tagsData) ?? new List<string>();
            }
            catch (JsonException)
            {
                return null;
            }
        }

        public static async Task<PlaceTagsActionResult> SetTagsToPlaceAsync(ApplicationDbContext dbContext, long placeId, string tagsData)
        {
            var clientTags = ParseTagsData(tagsData);
            if (clientTags == null) return PlaceTagsActionResult.InvalidTagsData;

            var serverTagAssociations = await dbContext.PlaceTagAssociations
                .Where(x => x.PlaceId == placeId)
                .Include(x => x.Tag)
                .ToDictionaryAsync(x => x.Tag.Name)
                .ConfigureAwait(false);

            foreach (var clientTag in clientTags.Distinct())
            {
                if (!serverTagAssociations.Remove(clientTag))
                {
                    // 追加
                    var tagEntity = await dbContext.PlaceTags
                        .AsNoTracking()
                        .SingleOrDefaultAsync(x => x.Name == clientTag)
                        .ConfigureAwait(false);

                    if (tagEntity == null)
                    {
                        tagEntity = new PlaceTag(clientTag);
                        dbContext.Add(tagEntity);
                    }

                    dbContext.Add(new PlaceTagAssociation(placeId, tagEntity.Id));
                }
            }

            // 削除
            dbContext.RemoveRange(serverTagAssociations.Values);

            return PlaceTagsActionResult.Success;
        }
    }

    public enum PlaceTagsActionResult
    {
        Success,
        InvalidTagsData
    }
}

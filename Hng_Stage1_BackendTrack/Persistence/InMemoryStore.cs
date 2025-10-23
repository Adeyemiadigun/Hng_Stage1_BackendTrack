using Hng_Stage1_BackendTrack.Models;
using StringModel = Hng_Stage1_BackendTrack.Models.String;
namespace Hng_Stage1_BackendTrack.Persistence
{
    public class InMemoryStore
    {
        public static List<StringModel> InMemoryStores { get; set; } = new List<StringModel>();
    }
}

using Storage.Repository;

namespace Storage.Data
{
    public class StoredStageOverview : StageOverview, IEntity
    {
        public int Id { get; set; }
    }
}

using Storage.Repository;

namespace Storage.Data
{
    public class StoredConflictingData : ConflictingData, IEntity
    {
        public int Id { get; set; }
    }
}

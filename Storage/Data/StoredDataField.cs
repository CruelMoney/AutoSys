using Storage.Repository;

namespace Storage.Data
{
    public class StoredDataField : DataField, IEntity
    {
        public int Id { get; set; }
    }
}

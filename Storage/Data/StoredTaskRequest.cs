using Storage.Repository;

namespace Storage.Data
{
    public class StoredTaskRequest : TaskRequest, IEntity
    {
        public int Id { get; set; }
    }
}

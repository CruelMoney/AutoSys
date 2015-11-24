using Storage.Repository;

namespace Storage.Data
{
    public class StoredTaskSubmission : TaskSubmission, IEntity
    {
        public int Id { get; set; }
    }
}

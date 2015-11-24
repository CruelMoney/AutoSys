using Storage.Repository;

namespace Storage.Data
{
    public class StoredStudyOverview : StudyOverview, IEntity
    {
        public int Id { get; set; }
    }
}

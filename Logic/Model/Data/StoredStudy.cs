using Logic.Model.DTO;
using Storage.Repository;

namespace Logic.Model.Data
{
    class StoredStudy : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int teamId { get; set; }
    }
}

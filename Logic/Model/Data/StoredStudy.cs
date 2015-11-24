using Logic.Model.DTO;
using Storage.Repository;
using System.Collections.Generic;

namespace Logic.Model.Data
{
    public class StoredStudy : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int teamId { get; set; }
        public Team ascociatedTeam { get; set; }
        public List<Item> studyData { get; set; }
    }
}

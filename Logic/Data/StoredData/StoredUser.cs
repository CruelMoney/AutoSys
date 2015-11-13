using System.Data.Entity;
using Storage.Repository;

namespace Logic.Data
{
    public class StoredUser : IEntity
    {
        public int Id { get; set; }
    }

}

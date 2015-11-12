using System.Data.Entity;
using Storage.Repository;

namespace Logic.Data
{
    public class User : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class UserContext : DbContext
    {
        public DbSet<User> users { get; set; }
    }

}

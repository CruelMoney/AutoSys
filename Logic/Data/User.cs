using System.Data.Entity;
using Storage.Repository;

namespace Logic.Data
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MetaData { get; set; }

        public class Role
        {
        }
    }

        public enum Role
        {
            Reviewer,
            Validator
        }
}



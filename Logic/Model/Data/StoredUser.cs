using Logic.Model.DTO;
using Storage.Repository;

namespace Logic.Model.Data
{
    public class StoredUser : User, IEntity
    {
        public StoredUser(User GivenUser)
        {
            this.Id = GivenUser.Id;
            this.Name = GivenUser.Name;
            this.Metadata = GivenUser.Metadata;
        }
    }
}

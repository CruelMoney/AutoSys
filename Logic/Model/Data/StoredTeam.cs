using Logic.Model.DTO;
using Storage.Repository;

namespace Logic.Model.Data
{
    public class StoredTeam : Team, IEntity
    {
        public StoredTeam(Team GivenTeam)
        {
            this.Id = GivenTeam.Id;
            this.Name = GivenTeam.Name;
            this.Metadata = GivenTeam.Metadata;
            this.UserIDs = GivenTeam.UserIDs;
        }
    }
}

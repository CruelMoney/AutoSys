using Logic.Model.DTO;
using Storage.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Model
{
    public class TeamLogic : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int[] UserIDs { get; set; }
        public String MetaData { get; set; }

        public TeamLogic(Team team)
        {
            this.Id = team.Id;
            this.Name = team.Name;
            this.UserIDs = team.UserIDs;
            this.MetaData = team.Metadata;
        }
    }

    
}

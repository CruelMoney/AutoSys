using Logic.Model.DTO;
using Storage.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Model
{
    public class Team : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int[] UserIDs { get; set; } 
        public virtual List<Study> Studies { get;  set;}  // reference til Study (one to many)
        public virtual List<User> Users { get; set; }     // reference til Users (one to many)
        public string Metadata { get; set; }

        public Team(TeamDTO teamDto)
        {
            this.Id = teamDto.Id;
            this.Name = teamDto.Name;
            this.UserIDs = teamDto.UserIDs;
            this.Metadata = teamDto.Metadata;
        }
        public Team()
        {

        }
    }

    
}

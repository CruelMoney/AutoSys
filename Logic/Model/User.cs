using Logic.Model.DTO;
using Storage.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Model
{
    public class User : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<Team> Teams { get; set; }     // reference til Teams (one to many)
        public string Metadata { get; set; }
        public virtual List<StudyTask> Tasks { get; set; } 

        public User(UserDTO userDto)
        {
            this.Id = userDto.Id;
            this.Name = userDto.Name;
            this.Metadata = userDto.Metadata;
        }
        public User()
        {

        }
    }
}

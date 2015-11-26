using Logic.Model.DTO;
using Storage.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Model
{
    public class UserLogic : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public String Metadata { get; set; }

        public UserLogic(User user)
        {
            this.Id = user.Id;
            this.Name = user.Name;
            this.Metadata = user.Metadata;
        }
        public UserLogic()
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Data;
using Storage.Repository;

namespace Logic.StorageManagement
{
    class UserManager
    {
        IRepository _userRepo;
        public UserManager()
        {
        }

        public UserManager(IRepository repo)
        {
            _userRepo = repo;
        }

        public void CreateUser(String UserName, String MetaData)
        {
            throw new NotImplementedException();
        }
        public void RemoveUser(int UserID)
        {
            throw new NotImplementedException();
        }
        public void UpdateUser(int UserID, String UpdatedName, String UpdatedMetaData)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<User> SearchUsers(String TeamName)
        {
            throw new NotImplementedException();
        }
        public User GetUser(int UserID)
        {
            throw new NotImplementedException();
        }

    }
}

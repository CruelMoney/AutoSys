using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.StorageManagement;
using Logic.Model.DTO;

namespace Logic.TeamCRUD
{
    public class UserManagement
    {
        UserStorageManager _userStorageManager;
        public UserManagement()
        {
            _userStorageManager = new UserStorageManager();
        }

        public UserManagement(UserStorageManager storageManager)
        {
            _userStorageManager = storageManager;
        }
        public void CreateUser(string UserName, string MetaData)
        {
            throw new NotImplementedException();
            //_userStorageManager.SaveUser(new User()); Opret ny User
        }
        public void RemoveUser(int UserID)
        {
            throw new NotImplementedException();
            _userStorageManager.RemoveUser(UserID);
        }
        public void UpdateUser(int UserID, String UpdatedName, String UpdatedMetaData)
        {
            throw new NotImplementedException();
            //_userStorageManager.RemoveUser(new User()); fjern bruger skabt af det man ved
        }
        public IEnumerable<User> SearchUsers(String UserName)
        {
            throw new NotImplementedException();
            _userStorageManager.SearchUsers(UserName);
        }
        public User GetUser(int UserID)
        {
            throw new NotImplementedException();
            _userStorageManager.GetUser(UserID);
        }
    }
}

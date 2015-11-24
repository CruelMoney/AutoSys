using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.StorageManagement;
using Logic.Model.DTO;

namespace Logic.UserCRUD
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
        public void CreateUser(String UserName, String MetaData)
        {
            var UserToAdd = new User();
            UserToAdd.Name = UserName;
            UserToAdd.Metadata = MetaData;

            _userStorageManager.SaveUser(UserToAdd);
        }

        public void RemoveUser(int UserID)
        {
            _userStorageManager.RemoveUser(UserID);
        }

        public void UpdateUser(int UserID, String UpdatedName, String UpdatedMetaData)
        {
            var UserToUpdate = _userStorageManager.GetUser(UserID);         //gets the user to save unupdated fields.
            UserToUpdate.Name = UpdatedName;
            UserToUpdate.Metadata = UpdatedMetaData;
            _userStorageManager.UpdateUser(UserToUpdate);
        }

        public IEnumerable<User> SearchUsers(String UserName)
        {
            return _userStorageManager.SearchUsers(UserName);
        }

        public User GetUser(int UserID)
        {
            return _userStorageManager.GetUser(UserID);
        }
    }
}

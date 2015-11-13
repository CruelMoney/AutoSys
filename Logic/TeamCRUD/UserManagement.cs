using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.StorageManagement;

namespace Logic.TeamCRUD
{
    public class UserManagement
    {
        UserStorageManager _userStorageManager;
        public UserManagement()
        {
            _userStorageManager = new UserStorageManager();
        }
        public void CreateUser() 
        {
            //_userStorageManager.CreateUser();
            throw new NotImplementedException();
        }

        public void UpdateUser()
        {
            //_userStorageManager.UpdateUser();
            throw new NotImplementedException();
        }

        public void GetUser()
        {
            //_userStorageManager.GetUser();
            throw new NotImplementedException();
        }

        public void DeleteUser()
        {
            //_userStorageManager.RemoveUser();
            throw new NotImplementedException();
        }
    }
}

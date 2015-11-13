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
        UserSaver _userSaver;
        UserRequester _userRequester;
        public UserManagement()
        {
            _userSaver = new UserSaver();
            _userRequester = new UserRequester();
        }
        public void CreateUser() 
        {
            //_userSaver.CreateUser();
            throw new NotImplementedException();
        }

        public void UpdateUser()
        {
            //_userSaver.UpdateUser();
            throw new NotImplementedException();
        }

        public void GetUser()
        {
            //_userRequester.GetUser();
            throw new NotImplementedException();
        }

        public void DeleteUser()
        {
            //_userSaver.RemoveUser();
            throw new NotImplementedException();
        }
    }
}

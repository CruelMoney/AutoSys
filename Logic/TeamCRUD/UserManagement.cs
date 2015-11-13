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
        UserManager _userManager;
        public UserManagement()
        {
            _userManager = new UserManager();
        }
        public void CreateUser() 
        {
            //_userManager.CreateUser();
            throw new NotImplementedException();
        }

        public void UpdateUser()
        {
            //_userManager.UpdateUser();
            throw new NotImplementedException();
        }

        public void GetUser()
        {
            //_userManager.GetUser();
            throw new NotImplementedException();
        }

        public void DeleteUser()
        {
            //_userManager.RemoveUser();
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using Logic.Model.DTO;
using Storage.Repository;
using Logic.Model.Data;
using Logic.Model;

namespace Logic.StorageManagement
{
    public class UserStorageManager
    {
        IRepository _userRepo;
        public UserStorageManager()
        {
        }

        public UserStorageManager(IRepository repo)
        {
            _userRepo = repo;
        }

        public void SaveUser(User UserToSave)
        {
            var UserLogicToSave = new UserLogic(UserToSave);
            _userRepo.Create(UserLogicToSave);
        }

        public void RemoveUser(int UserWithIDToDelete)
        {
            _userRepo.Delete(_userRepo.Read<UserLogic>(UserWithIDToDelete));
        }

        public void UpdateUser(User UserToUpdate)
        {
            var UserLogicToUpdate = new UserLogic(UserToUpdate);
            _userRepo.Update<UserLogic>(UserLogicToUpdate);
        }

        public IEnumerable<UserLogic> SearchUsers(String UserName)
        {
            foreach (UserLogic u in (_userRepo.Read<UserLogic>()))
                if (t.Name.Contains(UserName.ToLower()))
                {
                    yield return u;
                }
            yield break;
        }


        public UserLogic GetUser(int UserID)
        {
            return _userRepo.Read<UserLogic>(UserID);
        }

    }
}

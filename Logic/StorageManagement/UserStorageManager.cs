using System;
using System.Collections.Generic;
using Logic.Model.DTO;
using Storage.Repository;
using Logic.Model.Data;

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
            var StoredUserToSave = new StoredUser(UserToSave);
            _userRepo.Create(StoredUserToSave);
        }

        public void RemoveUser(int UserWithIDToDelete)
        {
            _userRepo.Delete(_userRepo.Read<StoredUser>(UserWithIDToDelete));
        }

        public void UpdateUser(User UserToUpdate)
        {
            var StoredUserToUpdate = new StoredUser(UserToUpdate);
            _userRepo.Update<StoredUser>(StoredUserToUpdate);
        }

        public IEnumerable<User> SearchUsers(String UserName)
        {
            foreach (StoredUser t in (_userRepo.Read<StoredUser>()))
                if (t.Name.Contains(UserName.ToLower()))
                {
                    yield return new User(t);
                }
            yield break;
        }



        public User GetUser(int UserID)
        {
            return new User(_userRepo.Read<StoredUser>(UserID));
        }

    }
}

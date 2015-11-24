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
            throw new NotImplementedException();
            //var StoredUserToSave = new StoredUser(UserToSave);
            //_userRepo.Create<StoredUser>(StoredUserToSave);
        }

        public void RemoveUser(int UserWithIDToDelete)
        {
            throw new NotImplementedException();
            //var StoredUserToDelete = new StoredUser(UserToDelete);
            _userRepo.Delete<StoredUser>(_userRepo.Read<StoredUser>(UserWithIDToDelete));
        }

        public void UpdateUser(int UserID, String UpdatedName, String UpdatedMetaData)
        {
            throw new NotImplementedException();
            //var StoredUserToUpdate = new StoredUser(UserToUpdate);
            //_userRepo.Update<StoredUser>(StoredUserToUpdate);
        }

        public IEnumerable<User> SearchUsers(String UserName)
        {
            throw new NotImplementedException();
            //foreach(StoredUser u in (_userRepo.Read<StoredUser>()){
            //    yield u.Name.equals(UserName);                        
            //}
        }

        public User GetUser(int UserID)
        {
            throw new NotImplementedException();
            //_userRepo.Read<StoredUser>(UserID);
        }

    }
}

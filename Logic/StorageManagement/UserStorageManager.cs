using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Logic.Model.DTO;
using Storage.Repository;
using Logic.Model;
using Logic.Model.Data;

namespace Logic.StorageManagement
{
    public class UserStorageManager
    {
        private readonly IGenericRepository _userRepo;
        public UserStorageManager()
        {
        }

        public UserStorageManager(IGenericRepository repo)
        {
            _userRepo = repo;
        }

        public int SaveUser(UserLogic userToSave)
        {
            return _userRepo.Create(userToSave);
        }

        public bool RemoveUser(int userWithIdToDelete)
        {
           return _userRepo.Delete(_userRepo.Read<UserLogic>(userWithIdToDelete));
        }

        public bool UpdateUser(UserLogic user)
        {
           return _userRepo.Update(user);
        }

        public IEnumerable<UserLogic> GetAllUsers()
        {
            return _userRepo.Read<UserLogic>().Include(u=>u.Id);
        }


        public UserLogic GetUser(int userId)
        {
           return _userRepo.Read<UserLogic>(userId); 
        }

    }
}

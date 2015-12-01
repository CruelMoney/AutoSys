﻿using System;
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

        public int SaveUser(User userToSave)
        {
            return _userRepo.Create(userToSave);
        }

        public bool RemoveUser(int userWithIdToDelete)
        {
           return _userRepo.Delete(_userRepo.Read<User>(userWithIdToDelete));
        }

        public bool UpdateUser(User user)
        {
           return _userRepo.Update(user);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userRepo.Read<User>().Include(u=>u.Id);
        }

        public User GetUser(int userId)
        {
           return _userRepo.Read<User>(userId); 
        }

    }
}

using Storage.Repository;
using System;
using System.Collections.Generic;
using Logic.Model.DTO;
using Logic.Model.Data;
using Logic.Model;

namespace Logic.StorageManagement
{
    public class StudyStorageManager
    {
        IRepository _studyRepo;
        public IEnumerable<StoredStudy> Tasks => _studyRepo.Read<StoredStudy>();
        public StudyStorageManager()
        {
        }

        public StudyStorageManager(IRepository repo)
        {
            _studyRepo = repo;
        }

        public StoredStudy saveStudy(string name, Team team, List<Item> studyData)
        {
            
            var newStudy = new StoredStudy() { Name = name, ascociatedTeam = team, studyData = studyData };     
            _studyRepo.Create(newStudy);
            return newStudy;
        }

        public List<Study>

    }
}

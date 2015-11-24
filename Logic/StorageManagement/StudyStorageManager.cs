using Storage.Repository;
using System;
using System.Collections.Generic;
using Logic.Model.DTO;

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

        public Study saveStudy(string name, Team team, List<Item> studyDatay)
        {
            
            var newStudy = new Study() { Name = name, AssociatedTeam = team, StudyData = studyDatay };
            var toStore = new StoredStudyOverview();
            toStore.Update(newStudy);
            _studyRepo.Create(toStore);
            return newStudy;
        }

    }
}

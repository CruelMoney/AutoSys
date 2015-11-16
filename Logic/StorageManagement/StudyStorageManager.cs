using Storage.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Data;

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
            throw new NotImplementedException();
            var newStudy = new Study() { Name = name, AssociatedTeam = team, StudyData = studyDatay };
            var toStore = new StoredStudy();
            toStore.Update(newStudy);
            _studyRepo.Create(toStore);
            return newStudy;
        }

    }
}

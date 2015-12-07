using System;
using System.Collections.Generic;
using System.Linq;
using Storage.Repository;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.Data;

namespace StudyConfigurationServer.Logic.StorageManagement
{
    public class StudyStorageManager : IObservable<Study>
    {

        IGenericRepository _studyRepo;
        List<IObserver<Study>> observers;


        /// <summary>
        /// Code created using observer design pattern tutorial at https://msdn.microsoft.com/
        /// </summary>
        private class Unsubscriber : IDisposable
        {
            private List<IObserver<Study>> _observers;
            private IObserver<Study> _observer;

            public Unsubscriber(List<IObserver<Study>> observers, IObserver<Study> observer)
            {
                this._observers = observers;
                this._observer = observer;
            }

            public void Dispose()
            {
                if (!(_observer == null)) _observers.Remove(_observer);
            }
        }

        public StudyStorageManager()
        {
            _studyRepo = new EntityFrameworkGenericRepository<StudyContext>();
            observers = new List<IObserver<Study>>();
        }

        public StudyStorageManager(IGenericRepository repo)
        {
            _studyRepo = repo;
        }

        public int SaveStudy(Study study)
        {
            var returnValue = _studyRepo.Create(study);
            
            return returnValue;
        }

        public bool RemoveStudy(int studyWithIdToDelete)
        {
            return _studyRepo.Delete(_studyRepo.Read<Study>(studyWithIdToDelete));
        }
        public bool UpdateStudy(Study study)
        {



            return _studyRepo.Update(study);
        }

        public IEnumerable<Study> GetAllStudies()
        {
            return _studyRepo.Read<Study>();
        }

        public Study GetStudy(int studyId)
        {
            return _studyRepo.Read<Study>(studyId);
        }

        public IDisposable Subscribe(IObserver<Study> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);

            return new Unsubscriber(observers, observer);
        }



    }


}

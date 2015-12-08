﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Ajax.Utilities;
using Storage.Repository;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.Data;

namespace StudyConfigurationServer.Logic.StorageManagement
{
    public class StudyStorageManager : IObservable<Study>
    {
        readonly IGenericRepository _studyRepo;
        readonly List<IObserver<Study>> observers;


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
                if (_observer != null) _observers.Remove(_observer);
            }
        }

        public StudyStorageManager()
        {
            _studyRepo = new EntityFrameworkGenericRepository<StudyContext>();
            observers = new List<IObserver<Study>>();
        }

        public StudyStorageManager(IGenericRepository repo, List<IObserver<Study>> observe)
        {
            _studyRepo = repo;
            observers = observe;
        }

        public int SaveStudy(Study study)
        {
            var returnValue = _studyRepo.Create(study);

            foreach (var observer in observers)
            {
                observer.OnNext(study);
            }
            
            return returnValue;
        }

        public bool RemoveStudy(int studyWithIdToDelete)
        {
            return _studyRepo.Delete(_studyRepo.Read<Study>(studyWithIdToDelete));
        }
        public bool UpdateStudy(Study study)
        {
            var returnValue = _studyRepo.Update(study);

            foreach (var observer in observers)
            {
                observer.OnNext(study);
            }

            return returnValue;
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

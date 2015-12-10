using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.Data;

namespace StudyConfigurationServer.Logic.StorageManagement
{
    public class TaskRepo
    {
        private readonly StudyContext _context;

        public TaskRepo()
        {
            _context = new StudyContext();
        }

        public TaskRepo(StudyContext context)
        {
            _context = context;
        }

        public int Create(StudyTask entity)
        {
            _context.Set<StudyTask>().Add(entity);
            _context.SaveChanges();
            return entity.Id;
        }

        public bool Delete(StudyTask task)
        {
            var found = _context.Set<StudyTask>().FindAsync(task.Id);

            if (found == null)
            {
                return false;
            }

            _context.Set<StudyTask>().Remove(task);
            _context.SaveChanges();
            return true;
        }

        public IQueryable<StudyTask> Read()
        {
            return _context.Set<StudyTask>();
        }

        public StudyTask Read(int teamID)
        {
            return _context.Set<StudyTask>().Find(teamID);
        }

        public bool Update(StudyTask entity)
        {

            var found = _context.Set<StudyTask>().Find(entity.Id);

            if (found == null)
            {
                return false;
            }

            _context.Set<StudyTask>().Attach(entity);

            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();

            return true;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
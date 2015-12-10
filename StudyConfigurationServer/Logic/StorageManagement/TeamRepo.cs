using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Storage.Repository;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.Data;

namespace StudyConfigurationServer.Logic.StorageManagement
{
    public class TeamRepo
    {
        private readonly StudyContext _context;

        public TeamRepo()
        {
            _context = new StudyContext();
        }

        public TeamRepo(StudyContext context)
        {
            _context = context;
        }

        public int CreateTeam(Team entity)
        {
            _context.Set<Team>().Add(entity);
            _context.SaveChanges();
            return entity.Id;
        }

        public bool Delete(Team team)
        {
            var found = _context.Set<Team>().FindAsync(team.Id);

            if (found == null)
            {
                return false;
            }

            _context.Set<Team>().Remove(team);
            _context.SaveChanges();
            return true;
        }

        public IQueryable<Team> Read()
        {
            return _context.Set<Team>();
        }

        public Team Read(int teamID)
        {
            return _context.Set<Team>().Find(teamID);
        }

        public bool UpdateTeam(Team entity)
        {

            var found = _context.Set<Team>().Find(entity.Id);

            if (found == null)
            {
                return false;
            }

            _context.Set<Team>().Attach(entity);
            foreach (var user in entity.Users)
            {
                _context.Set<User>().Attach(user);
            }
            _context.Entry(entity.Users).State = EntityState.Unchanged;
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
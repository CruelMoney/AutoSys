using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.DTO;

namespace StudyConfigurationServer.Logic.StudyManagement
{
    public interface IStudyManager
    {
        int CreateStudy(StudyDto studyDto);
        
        bool RemoveStudy(int studyId);

        bool UpdateStudy(int studyId, StudyDto studyDto);

        IEnumerable<StudyDto> SearchStudies(string studyName);

        StudyDto GetStudy(int studyId);

        IEnumerable<StudyDto> GetAllStudies();

        StudyOverviewDto GetStudyOverview(int id);
    }
}
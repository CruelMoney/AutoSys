using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.DTO;

namespace StudyConfigurationServer.Logic.TeamUserManagement
{
    public interface IUserManager
    {
        int CreateUser(UserDto userDto);
        bool RemoveUser(int userId);
        bool UpdateUser(int userId, UserDto newUserDto);
        IEnumerable<UserDto> SearchUserDtOs(string userName);
        UserDto GetUserDto(int userId);
        IEnumerable<UserDto> GetAllUserDtOs();

        IEnumerable<int> GetStudyIds(int userId);
    }
}
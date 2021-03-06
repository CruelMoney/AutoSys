﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Navigation;
using StudyConfigurationUIibrary.Model;
using StudyConfigurationUILibrary.Data;

namespace StudyConfigurationUILibrary.Logic
{
    public class Logic
    {
        public StudyDTO _StudyToWorkOn { get; set; }
        public TeamDTO _TeamAssociated { get; set; }
        public List<UserDTO> _Users { get; set; }
        public PageStackEntry _Origin { get; set; }
        public bool _IsNewStudy { get; set; }

        /// <summary>
        /// Sets up the logic object from a given team id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task SetUpFromTeam(int id)
        {
            _TeamAssociated = await Service.GetTeam(id);
            _StudyToWorkOn = new StudyDTO
            {
                Team = _TeamAssociated
            };
            _Users = new List<UserDTO>();
            _Users.AddRange(await GetUserNames(_TeamAssociated.UserIDs));
        }
        /// <summary>
        /// Sets up the logic object from a given study id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task SetUpFromStudy(int id)
        {
            _StudyToWorkOn = await Service.GetStudy(id);
            _TeamAssociated = await Service.GetTeam(_StudyToWorkOn.Team.Id);
            _Users = new List<UserDTO>();
            _Users.AddRange(await GetUserNames(_TeamAssociated.UserIDs));
        }
        /// <summary>
        /// Get all users names
        /// </summary>
        /// <param name="IDs"></param>
        /// <returns></returns>
        private async Task<UserDTO[]> GetUserNames(int[] IDs)
        {
            return await Service.GetUsers(IDs);
        }
        /// <summary>
        /// Open the file selector and return the selected object.
        /// </summary>
        /// <returns></returns>
        public async Task<StorageFile> OpenPicker()
        {
            var picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation =
                PickerLocationId.Desktop;
            picker.FileTypeFilter.Add(".csv");
            picker.FileTypeFilter.Add(".bib");
            picker.FileTypeFilter.Add(".txt");
            return await picker.PickSingleFileAsync();
        }
        /// <summary>
        /// Adds the bytearray from the selected file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task AddResources(StorageFile file)
        {
            try
            {
                var buffer = await FileIO.ReadBufferAsync(file);
                _StudyToWorkOn.Items = buffer.ToArray();
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// Remove a selected phase from the logic.
        /// </summary>
        /// <param name="stageToRemove"></param>
        public void RemoveStage(StageDTO stageToRemove)
        {
            var stages = _StudyToWorkOn.Stages;
            _StudyToWorkOn.Stages = stages.Where(val => val != stageToRemove).ToArray();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using StudyConfigurationUI.Data;

namespace StudyConfigurationUI.ViewModels
{
    class ViewModel
    {
        private Study _study;

        public ViewModel()
        {
            _study = new Study();
        }
        public ViewModel(int TeamID)
        {
            throw new NotImplementedException();
            //TODO HENT FRA API
        }

        public void setStudy(int studyId)
        {
            throw new NotImplementedException();
            //TODO hent fra api
        }

        public Team getTeam(int? id)
        {
            return new Team()
            {
                Id = 1,
                Name = "TestTeam"
            };
            
        }

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
    }
}

using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using StudyConfigurationUI.Data;
using StudyConfigurationUI.Logic.BibliographyParser;
using StudyConfigurationUI.Logic.BibliographyParser.bibTex;

namespace StudyConfigurationUI.Logic
{
    public class Logic
    {
        public Study _StudyToWorkOn { get; set; }

        public Logic()
        {
            _StudyToWorkOn = new Study();
        }
        public Logic(int TeamID)
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

        public async Task AddResources( StorageFile file)
        {
            var parser = new BibTexParser(new ItemValidator());
            try
            {
                var parsedStringFromFile = await FileIO.ReadTextAsync(file);
                _StudyToWorkOn.Items = parser.Parse(parsedStringFromFile);
            }
            catch (Exception)
            {
                return;
            }
            
        }
    }
}

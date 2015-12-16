using System;
using System.Linq;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using StudyConfigurationUIibrary.Model;
using StudyConfigurationUILibrary.Data;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StudyConfigurationUILibrary
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ManageStudyPage : Page
    {
        private Logic.Logic _logic;

        public ManageStudyPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs args)
        {
            _logic = new Logic.Logic();
            _logic._Origin = Frame.BackStack.FirstOrDefault();
            if (_logic._Origin == null)
            {
                throw new InvalidOperationException("The ManageStudyPage should be navigated to from another page.");
            }

            if (args.Parameter.GetType() == typeof (Logic.Logic))
            {
                _logic = args.Parameter as Logic.Logic;
                SetUpFromLogic(_logic);
                return;
            }
            // Make sure either a team, or study ID is passed, so that a suitable UI to manage the study can be created.
            var studyArgs = args.Parameter as ManageStudyPageArgs;
            if (studyArgs == null)
            {
                throw new ArgumentException("This page needs to receive either a team or study ID as parameter.",
                    nameof(args));
            }

            // Based on the parameter passed to this page, either load an existing study, or create a new one.
            try
            {
                if (studyArgs.StudyId != null)
                {
                    await _logic.SetUpFromStudy((int) studyArgs.StudyId);
                    _logic._IsNewStudy = false;
                    SetUpFromLogic(_logic);
                }
                else if (studyArgs.TeamId != null)
                {
                    await _logic.SetUpFromTeam((int) studyArgs.TeamId);
                    _logic._IsNewStudy = true;
                    SetUpFromLogic(_logic);
                }
            }
            catch (Exception)
            {
                var dialog = new MessageDialog("Error retrieving from Database") {Title = "Error"};
                await dialog.ShowAsync();
                Frame.Navigate(_logic._Origin.SourcePageType);
            }
        }

        private async void BibtexInputButton_OnClick(object sender, RoutedEventArgs e)
        {
            var file = await _logic.OpenPicker();
            bibtexOutput.Text = file.Path;
            await _logic.AddResources(file);
        }

        private void onNewPhase(object sender, RoutedEventArgs e)
        {
            _logic._StudyToWorkOn.Name = nameInput.Text;
            Frame.Navigate(typeof (ManagePhasePage), _logic);
        }

        private async void onDeletePhase(object sender, RoutedEventArgs e)
        {
            if ((StageDTO) phaseComboBox.SelectionBoxItem == null) return;
            var dialog = new MessageDialog("Are you sure?") {Title = "Really?"};
            dialog.Commands.Add(new UICommand {Label = "Yes - Delete", Id = 0});
            dialog.Commands.Add(new UICommand {Label = "Cancel", Id = 1});
            var res = await dialog.ShowAsync();
            if ((int) res.Id == 0)
            {
                _logic.RemoveStage((StageDTO) phaseComboBox.SelectionBoxItem);
            }
            SetUpCombobox(_logic._StudyToWorkOn.Stages);
        }

        private void SetUpFromLogic(Logic.Logic logic)
        {
            if (logic._StudyToWorkOn.Name != null)
            {
                nameInput.Text = logic._StudyToWorkOn.Name;
            }
            SetUpCombobox(logic._StudyToWorkOn.Stages);
            teamOutput.Text = logic._TeamAssociated.Name;

            if (logic._StudyToWorkOn.Items == null)
            {
                bibtexOutput.Text = "No items selected yet";
            }
            else
            {
                bibtexOutput.Text = "Items have already been selected";
                bibtexInputButton.IsEnabled = false;
            }
            if (!_logic._IsNewStudy)
            {
                deletePhasebutton.IsEnabled = false;
            }
        }

        private void SetUpCombobox(StageDTO[] stages)
        {
            if (stages != null)
            {
                phaseComboBox.Items.Clear();
                foreach (var s in stages)
                {
                    phaseComboBox.Items.Add(s);
                }
                phaseComboBox.DisplayMemberPath = "Name";
            }
        }

        private async void SaveAndClose(object sender, RoutedEventArgs e)
        {
            if (_logic._StudyToWorkOn.Items == null)
            {
                var dialog = new MessageDialog("Remember to add items") {Title = "Error"};
                await dialog.ShowAsync();
                return;
            }
            _logic._StudyToWorkOn.Name = nameInput.Text;
            if (_logic._IsNewStudy)
            {
                await Service.PostStudy(_logic._StudyToWorkOn);
            }
            else
            {
                await Service.UpdateStudy(_logic._StudyToWorkOn.Id, _logic._StudyToWorkOn);
            }

            Frame.Navigate(_logic._Origin.SourcePageType);
        }

        private async void DeleteAndReturn(object sender, RoutedEventArgs e)
        {
            if (_logic._IsNewStudy)
            {
                Frame.Navigate(_logic._Origin.SourcePageType);
            }
            else
            {
                var success = await Service.RemoveStudy(_logic._StudyToWorkOn.Id);
                if (success)
                {
                    var dialog = new MessageDialog("Successfully deleted study the study, will now return")
                    {
                        Title = "Success"
                    };
                    await dialog.ShowAsync();
                    Frame.Navigate(_logic._Origin.SourcePageType);
                }
                else
                {
                    var dialog = new MessageDialog("Error deleting from the database, check your connection")
                    {
                        Title = "Error"
                    };
                    await dialog.ShowAsync();
                    Frame.Navigate(_logic._Origin.SourcePageType);
                }
            }
        }
    }
}
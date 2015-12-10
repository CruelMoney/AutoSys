using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using StudyConfigurationUI.Data;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StudyConfigurationUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ManageStudyPage : Page
    {
        private Logic.Logic _logic;
        public ManageStudyPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs args)
        {
            _logic = new Logic.Logic();
            if (args.Parameter.GetType() == typeof (Study))
            {
                _logic._StudyToWorkOn = args.Parameter as Study;
                SetUpFromStudy(_logic._StudyToWorkOn);
                return;
            }
            // Make sure either a team, or study ID is passed, so that a suitable UI to manage the study can be created.
            var studyArgs = args.Parameter as ManageStudyPageArgs;
            if (studyArgs == null)
            {
                throw new ArgumentException("This page needs to receive either a team or study ID as parameter.", nameof(args));
            }

            // Based on the parameter passed to this page, either load an existing study, or create a new one.
            if (studyArgs.StudyId != null)
            {
                // TODO: Initialize the UI by loading data for the given study.
            }
            else if (studyArgs.TeamId != null)
            {
                teamOutput.Text = _logic.getTeam(studyArgs.TeamId).Name;
            }
        }

        private async void BibtexInputButton_OnClick(object sender, RoutedEventArgs e)
        {
                var file = await _logic.OpenPicker();
                bibtexOutput.Text = file.Path;
                _logic._StudyToWorkOn.Items = new List<Item>();
               await _logic.AddResources(file);
            

            if (_logic._StudyToWorkOn.Items.Count < 1)
                {
                    var dialog = new MessageDialog("No items were added from this file.") { Title = "No Items Added" };
                    var res = await dialog.ShowAsync();
                }
                else
            {
                bibtexInputButton.IsEnabled = false;
                    var dialog = new MessageDialog(_logic._StudyToWorkOn.Items.Count +"Items were added from the selected file") { Title = "Items added" };
                    var res = await dialog.ShowAsync();
                }
        }

        private void onNewPhase(object sender, RoutedEventArgs e)
        {
            
            this.Frame.Navigate(typeof(ManagePhasePage),_logic._StudyToWorkOn);
        }

        private async void onDeletePhase(Object sender, RoutedEventArgs e)
        {
            if ((Stage) phaseComboBox.SelectionBoxItem == null) return;
            var dialog = new MessageDialog("Are you sure?") {Title = "Really?"};
            dialog.Commands.Add(new UICommand { Label = "Yes - Delete", Id = 0 });
            dialog.Commands.Add(new UICommand { Label = "Cancel", Id = 1 });
            var res = await dialog.ShowAsync();
            if ((int)res.Id == 0)
            {
                _logic._StudyToWorkOn.Stages.Remove((Stage)phaseComboBox.SelectionBoxItem);
            }
            SetUpCombobox(_logic._StudyToWorkOn);
        }   

        private void SetUpFromStudy(Study study)
        {
            nameInput.Text = study.Name;
            SetUpCombobox(study);
            teamOutput.Text = study.Team.Name;

            if (study.Items == null || study.Items.Count < 1)
            {
                bibtexOutput.Text = "No items selected yet";
            }
            else
            {
                bibtexOutput.Text = "Items have already been selected";
                bibtexInputButton.IsEnabled = false;
            }
        }

        private void SetUpCombobox(Study study)
        {
            phaseComboBox.Items.Clear();
            foreach (Stage s in study.Stages)
            {
                phaseComboBox.Items.Add(s);
            }
            phaseComboBox.DisplayMemberPath = "Name";
        }
    }
}

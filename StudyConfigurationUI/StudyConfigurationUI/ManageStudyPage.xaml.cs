using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using StudyConfigurationUI.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StudyConfigurationUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ManageStudyPage : Page
    {
        private ViewModel _viewModel;
        public ManageStudyPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs args)
        {
            if (args.Parameter.GetType() == typeof (ViewModel))
            {
                var viewArgs = args.Parameter as ViewModel;
                ReturnFromStageManager(viewArgs);
                return;
            }

            _viewModel = new ViewModel();
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
                teamOutput.Text = _viewModel.getTeam(studyArgs.TeamId).Name;
            }
        }

        private async void BibtexInputButton_OnClick(object sender, RoutedEventArgs e)
        {
           var t =  await _viewModel.OpenPicker();
            bibtexOutput.Text = t.Path;
        }

        private void ReturnFromStageManager(ViewModel vm)
        {
            
        }

        private void onNewPhase(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ManagePhasePage));
        }
    }
}

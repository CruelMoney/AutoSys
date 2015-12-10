﻿using System;
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
using StudyConfigurationUI.Data;
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
            _viewModel = new ViewModel();
            if (args.Parameter.GetType() == typeof (Study))
            {
                _viewModel.studyToWorkOn = args.Parameter as Study;
                SetUpFromStudy(_viewModel.studyToWorkOn);
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
                teamOutput.Text = _viewModel.getTeam(studyArgs.TeamId).Name;
            }
        }

        private async void BibtexInputButton_OnClick(object sender, RoutedEventArgs e)
        {
           var t =  await _viewModel.OpenPicker();
            bibtexOutput.Text = t.Path;
        }

        private void onNewPhase(object sender, RoutedEventArgs e)
        {
            var teststudy = new Study();
            teststudy.Team = new Team();
            teststudy.Team.Users = new List<User>();
            teststudy.Team.Users.Add(new User() {Name = "Thomas",Id = 1});
            teststudy.Team.Users.Add(new User() { Name = "Ramos",Id = 2});
            teststudy.Team.Users.Add(new User() { Name = "Timothy",Id = 3});
            teststudy.Team.Users.Add(new User() { Name = "Kathrin",Id = 4});
            teststudy.Team.Users.Add(new User() { Name = "Dengsø",Id = 5});
            teststudy.Team.Users.Add(new User() { Name = "Mads",Id = 6});
            teststudy.Team.Users.Add(new User() { Name = "Tor",Id = 7});
            this.Frame.Navigate(typeof(ManagePhasePage),teststudy);
        }

        private void SetUpFromStudy(Study study)
        {
            nameInput.Text = study.Name;
            foreach (Stage s in study.Stages)
            {
                phaseComboBox.Items.Add(s.Name);
            }
            phaseComboBox.SelectedIndex = 1;
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
    }
}

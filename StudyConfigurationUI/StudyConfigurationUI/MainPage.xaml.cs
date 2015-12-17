using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using StudyConfigurationUILibrary;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace StudyConfigurationUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void OnOpenStudy(object sender, RoutedEventArgs e)
        {
            int teamsid = Int32.Parse(teamid.Text);
            var args = ManageStudyPageArgs.CreateForExistingTeam(teamsid);
            this.Frame.Navigate(typeof(ManageStudyPage), args);
        }

        private void OnOpenStudyWithStudy(object sender, RoutedEventArgs e)
        {
            int studysid = Int32.Parse(studyid.Text);
            var args = ManageStudyPageArgs.CreateForExistingStudy(studysid);
            this.Frame.Navigate(typeof(ManageStudyPage), args);
        }

    }
}
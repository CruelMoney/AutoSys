using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using StudyConfigurationUI;
using StudyConfigurationUI.Data;
using StudyConfigurationUI.Model;
using StudyConfigurationUI.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace StudyConfiguration
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
            var args = ManageStudyPageArgs.CreateForExistingTeam(1);
            this.Frame.Navigate(typeof(ManageStudyPage), args);
        }

        private void OnOpenStudyWithStudy(object sender, RoutedEventArgs e)
        {
            Study s = new Study();
            s.Name = "This test Study";
            s.Stages = new List<Stage>();
            s.Stages.Add(new Stage() {Name = "stage1 LOL"});
            s.Stages.Add(new Stage() { Name = "stage441 LOL" });
            s.Team = new Team() {Name = "vinderholdet"};
            this.Frame.Navigate(typeof (ManageStudyPage), s);
        }

        private async void PostStudy(object sender, RoutedEventArgs e)
        {
            var study = new Study();
            study.Name = "Superhold";
            study.Id = 1;
            study.Team = new Team() {Name = "SuperHoldet"};
            await Service.PostStudy(study);

        }

        private async void callAPI(object sender, RoutedEventArgs e)
        {
            var study = new Study();
            study.Name = "Superhold";
            study.Team = new Team() { Name = "SuperHoldet" };
            await Service.PostStudy(study);

        }

    }
}
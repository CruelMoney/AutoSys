using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using StudyConfigurationUI.Data;
using StudyConfigurationUI.Model;

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
            var args = ManageStudyPageArgs.CreateForExistingTeam(0);
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
            s.Stages = new List<Stage>();
            s.Team.Users = new List<User>();
            s.Team.Users.Add(new User() { Name = "Thomas", Id = 1 });
            s.Team.Users.Add(new User() { Name = "Ramos", Id = 2 });
            s.Team.Users.Add(new User() { Name = "Timothy", Id = 3 });
            s.Team.Users.Add(new User() { Name = "Kathrin", Id = 4 });
            s.Team.Users.Add(new User() { Name = "Dengsø", Id = 5 });
            s.Team.Users.Add(new User() { Name = "Mads", Id = 6 });
            s.Team.Users.Add(new User() { Name = "Tor", Id = 7 });
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
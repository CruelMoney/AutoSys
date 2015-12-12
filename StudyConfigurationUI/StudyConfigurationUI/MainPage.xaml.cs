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
            var args = ManageStudyPageArgs.CreateForExistingTeam(1);
            this.Frame.Navigate(typeof(ManageStudyPage), args);
        }

        private void OnOpenStudyWithStudy(object sender, RoutedEventArgs e)
        {
            
        }

        private async void PostStudy(object sender, RoutedEventArgs e)
        {
            
        }

        private async void callAPI(object sender, RoutedEventArgs e)
        {

        }

    }
}
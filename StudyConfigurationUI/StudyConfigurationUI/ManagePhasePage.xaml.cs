﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
using System.Diagnostics;
using System.Reflection;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StudyConfigurationUI
{

    public sealed partial class ManagePhasePage : Page
    {
        private Logic.Logic _viewModel;
        private Stage _stageToWorkOn;
        private List<User> _users;
        private List<UserStudies> _reviewers;
        private List<UserStudies> _validators;
        public ManagePhasePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs args)
        {
            _viewModel = new Logic.Logic();
            var studyArgs = args.Parameter as Study;
            if (studyArgs == null)
            {
                throw new ArgumentException("this page needs to recieve a study as parameter", nameof(args));
            }
            
            _viewModel._StudyToWorkOn = studyArgs;
            SetUpFromStudy(_viewModel._StudyToWorkOn);
            SetUpCriteria();
        }

        private void SetUpCriteria()
        {
            foreach (Enum en in Enum.GetValues(typeof(DataField.DataType)))
            {
                CriteriaDataType.Items.Add(en);
            }

        }

        public void SaveCriteria()
        {
            _stageToWorkOn.Criteria = new List<Criteria>();
            var criteriaToAdd = new Criteria
            {
                Name = CriteriaName.Text,
                Description = CriteriaDescription.Text,
                Stage = _stageToWorkOn,
                DataType = (DataField.DataType) CriteriaDataType.SelectionBoxItem,
                Rule = (Criteria.CriteriaRule) CriteriaRule.SelectionBoxItem
            };
            if (criteriaToAdd.DataType == DataField.DataType.Enumeration ||
                criteriaToAdd.DataType == DataField.DataType.Flags)
            {
                criteriaToAdd.TypeInfo = dataMatchDescription.Text.Split(',');
            }
            else
            {
                criteriaToAdd.DataMatch = new string[]
                {
                    dataMatchDescription.Text
                };
            }
            _stageToWorkOn.Criteria.Add(criteriaToAdd);
        }

        

        public void SelectionChanged(object sender, Object e)
        {
            CriteriaRule.Items.Clear();
            var selected = CriteriaDataType.SelectionBoxItem as DataField.DataType?;
            
            switch (selected)
            {
                case DataField.DataType.String:
                    foreach (Enum en in Enum.GetValues(typeof(Criteria.CriteriaRule)))
                    {
                        CriteriaRule.Items.Add(en);
                    }
                    break;

                case DataField.DataType.Boolean:
                    CriteriaRule.Items.Add(Criteria.CriteriaRule.Equals);
                    break;

                case DataField.DataType.Enumeration:
                    CriteriaRule.Items.Add(Criteria.CriteriaRule.Equals);
                    break;

                case DataField.DataType.Flags:
                    CriteriaRule.Items.Add(Criteria.CriteriaRule.Equals);
                    CriteriaRule.Items.Add(Criteria.CriteriaRule.Contains);
                    break;
                case DataField.DataType.Resource:
                    break;
                case null:
                    break;
                default:
                    return;
            }
        }

        private void SetUpFromStudy(Study study)
        {
            _stageToWorkOn = new Stage();
            _stageToWorkOn.Study = _viewModel._StudyToWorkOn;
            _stageToWorkOn.Users = new List<UserStudies>();
            _stageToWorkOn.VisibleFields = new List<Item.FieldType>();
            _users = new List<User>();
            _users.AddRange(_viewModel._StudyToWorkOn.Team.Users);
            _reviewers = new List<UserStudies>();
            _validators = new List<UserStudies>();
            SetUpBoxes();
        }


        private void MakeReviewer_OnClick(object sender, RoutedEventArgs e)
        {
            var takenUser = UserListBox.SelectedItem as User;
            if (takenUser == null)
            {
                return;
            }
            UserStudies userToMakeReviewer = new UserStudies
            {
                Stage = _stageToWorkOn, User = takenUser, Id = takenUser.Id, StudyRole = UserStudies.Role.Reviewer
            };
            _reviewers.Add(userToMakeReviewer);
            _users.Remove(takenUser);
            SetUpBoxes();
        }

        private void MakeValidator_OnClick(object sender, RoutedEventArgs e)
        {
            var takenUser = UserListBox.SelectedItem as User;
            if (takenUser == null)
            {
                return;
            }
            UserStudies userToMakeValidator = new UserStudies
            {
                Stage = _stageToWorkOn, User = takenUser, Id = takenUser.Id, StudyRole = UserStudies.Role.Validator
            };
            _validators.Add(userToMakeValidator);
            _users.Remove(takenUser);
            SetUpBoxes();
        }

        private void RemoveValidator_Click(object sender, RoutedEventArgs e)
        {
            var takenValidator = ValidatorListBox.SelectedItem as UserStudies;
            if (takenValidator == null)
            {
                return;
            }
            _users.Add(takenValidator.User);
            _validators.Remove(takenValidator);
            SetUpBoxes();
        }

        private void RemoveReviewer_Click(object sender, RoutedEventArgs e)
        {
            var takenReviewer = ReviewerListBox.SelectedItem as UserStudies;
            if (takenReviewer == null)
            {
                return;
            }
            _users.Add(takenReviewer.User);
            _reviewers.Remove(takenReviewer);
            SetUpBoxes();
        }

        private void SetUpBoxes()
        {
            UserListBox.ItemsSource = null;
            UserListBox.ItemsSource = _users;
            UserListBox.DisplayMemberPath = "Name";
            ReviewerListBox.ItemsSource = null;
            ReviewerListBox.ItemsSource = _reviewers;
            ReviewerListBox.DisplayMemberPath = "User.Name";
            ValidatorListBox.ItemsSource = null;
            ValidatorListBox.ItemsSource = _validators;
            ValidatorListBox.DisplayMemberPath = "User.Name";
        }

        private void AddReviewersAndValidators()
        {
            _stageToWorkOn.Users.AddRange(_reviewers);
            _stageToWorkOn.Users.AddRange(_validators);
        }


        private void AddDistribution()
        {
            if (HundredOverlap.IsChecked.Value)
            {
                _stageToWorkOn.DistributionRule = Stage.Distribution.HundredPercentOverlap;
                return;
            }
            if (FiftyOverlap.IsChecked.Value)
            {
                _stageToWorkOn.DistributionRule = Stage.Distribution.FiftyPercentOverlap;
                return;
            }
            if (NoOverlap.IsChecked.Value)
            {
                _stageToWorkOn.DistributionRule = Stage.Distribution.NoOverlap;
                return;
            }
        }


        private void AddVisibleFields()
        {
            _stageToWorkOn.VisibleFields.Clear();
            if (AddressCheckbox.IsChecked.Value)
            {
                _stageToWorkOn.VisibleFields.Add(Item.FieldType.Address);
            }
            if (AnnoteCheckbox.IsChecked.Value)
            {
                _stageToWorkOn.VisibleFields.Add(Item.FieldType.Annote);
            }
            if (AuthorCheckbox.IsChecked.Value)
            {
                _stageToWorkOn.VisibleFields.Add(Item.FieldType.Author);
            }
            if (BooktitleCheckbox.IsChecked.Value)
            {
                _stageToWorkOn.VisibleFields.Add(Item.FieldType.Booktitle);
            }
            if (ChapterCheckbox.IsChecked.Value)
            {
                _stageToWorkOn.VisibleFields.Add(Item.FieldType.Chapter);
            }
            if (CrossrefCheckbox.IsChecked.Value)
            {
                _stageToWorkOn.VisibleFields.Add(Item.FieldType.Crossref);
            }
            if (EditionCheckbox.IsChecked.Value)
            {
                _stageToWorkOn.VisibleFields.Add(Item.FieldType.Edition);
            }
            if (EditorCheckbox.IsChecked.Value)
            {
                _stageToWorkOn.VisibleFields.Add(Item.FieldType.Editor);
            }
            if (HowPublishedCheckbox.IsChecked.Value)
            {
                _stageToWorkOn.VisibleFields.Add(Item.FieldType.HowPublished);
            }
            if (InstritutionCheckbox.IsChecked.Value)
            {
                _stageToWorkOn.VisibleFields.Add(Item.FieldType.Instritution);
            }
            if (JournalCheckbox.IsChecked.Value)
            {
                _stageToWorkOn.VisibleFields.Add(Item.FieldType.Journal);
            }
            if (KeyCheckbox.IsChecked.Value)
            {
                _stageToWorkOn.VisibleFields.Add(Item.FieldType.Key);
            }
            if (MonthCheckbox.IsChecked.Value)
            {
                _stageToWorkOn.VisibleFields.Add(Item.FieldType.Month);
            }
            if (NoteCheckbox.IsChecked.Value)
            {
                _stageToWorkOn.VisibleFields.Add(Item.FieldType.Note);
            }
            if (NumberCheckbox.IsChecked.Value)
            {
                _stageToWorkOn.VisibleFields.Add(Item.FieldType.Number);
            }
            if (OrganizationCheckbox.IsChecked.Value)
            {
                _stageToWorkOn.VisibleFields.Add(Item.FieldType.Organization);
            }
            if (PagesCheckbox.IsChecked.Value)
            {
                _stageToWorkOn.VisibleFields.Add(Item.FieldType.Pages);
            }
            if (PublisherCheckbox.IsChecked.Value)
            {
                _stageToWorkOn.VisibleFields.Add(Item.FieldType.Publisher);
            }
            if (SchoolCheckbox.IsChecked.Value)
            {
                _stageToWorkOn.VisibleFields.Add(Item.FieldType.School);
            }
            if (SeriesCheckbox.IsChecked.Value)
            {
                _stageToWorkOn.VisibleFields.Add(Item.FieldType.Series);
            }
            if (TitleCheckbox.IsChecked.Value)
            {
                _stageToWorkOn.VisibleFields.Add(Item.FieldType.Title);
            }
            if (TypeCheckbox.IsChecked.Value)
            {
                _stageToWorkOn.VisibleFields.Add(Item.FieldType.Type);
            }
            if (VolumeCheckbox.IsChecked.Value)
            {
                _stageToWorkOn.VisibleFields.Add(Item.FieldType.Volume);
            }
            if (YearCheckbox.IsChecked.Value)
            {
                _stageToWorkOn.VisibleFields.Add(Item.FieldType.Year);
            }
            if (URLCheckbox.IsChecked.Value)
            {
                _stageToWorkOn.VisibleFields.Add(Item.FieldType.URL);
            }
            if (ISBNCheckbox.IsChecked.Value)
            {
                _stageToWorkOn.VisibleFields.Add(Item.FieldType.ISBN);
            }
            if (ISSNCheckbox.IsChecked.Value)
            {
                _stageToWorkOn.VisibleFields.Add(Item.FieldType.ISSN);
            }
            if (LCCNCheckbox.IsChecked.Value)
            {
                _stageToWorkOn.VisibleFields.Add(Item.FieldType.LCCN);
            }
            if (AbstractCheckbox.IsChecked.Value)
            {
                _stageToWorkOn.VisibleFields.Add(Item.FieldType.Abstract);
            }
            if (KeywordsCheckbox.IsChecked.Value)
            {
                _stageToWorkOn.VisibleFields.Add(Item.FieldType.Keywords);
            }
            if (PriceCheckbox.IsChecked.Value)
            {
                _stageToWorkOn.VisibleFields.Add(Item.FieldType.Price);
            }
            if (CopyrightCheckbox.IsChecked.Value)
            {
                _stageToWorkOn.VisibleFields.Add(Item.FieldType.Copyright);
            }
            if (LanguageCheckbox.IsChecked.Value)
            {
                _stageToWorkOn.VisibleFields.Add(Item.FieldType.Language);
            }
            if (ContentsCheckbox.IsChecked.Value)
            {
                _stageToWorkOn.VisibleFields.Add(Item.FieldType.Contents);
            }
            if (DoiCheckbox.IsChecked.Value)
            {
                _stageToWorkOn.VisibleFields.Add(Item.FieldType.Doi);
            }
        }



        private void SaveAndReturn_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _stageToWorkOn.Name = nameInput.Text;
                SaveCriteria();
                AddReviewersAndValidators();
                AddDistribution();
                AddVisibleFields();
                _viewModel._StudyToWorkOn.Stages.Add(_stageToWorkOn);
                this.Frame.Navigate(typeof (ManageStudyPage), _viewModel._StudyToWorkOn);
            }
            catch (NullReferenceException)
            {
                ErrorBox.Text = " ERROR : Make sure that all fields are filled out.";
            }


        }
    }
}
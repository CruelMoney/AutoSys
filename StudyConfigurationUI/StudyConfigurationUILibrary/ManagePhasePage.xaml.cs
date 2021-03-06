﻿using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using StudyConfigurationUILibrary.Data;

namespace StudyConfigurationUILibrary
{
    public sealed partial class ManagePhasePage : Page
    {
        private Logic.Logic _logic;
        private List<UserDTO> _reviewers;
        private StageDTO _stageToWorkOn;
        private List<UserDTO> _users;
        private List<UserDTO> _validators;

        public ManagePhasePage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Navigating to this page with a logic object. 
        /// </summary>
        /// <param name="args"></param>
        protected override void OnNavigatedTo(NavigationEventArgs args)
        {
            _logic = new Logic.Logic();
            var studyArgs = args.Parameter as Logic.Logic;
            if (studyArgs == null)
            {
                throw new ArgumentException("this page needs to recieve a logic as parameter", nameof(args));
            }

            _logic = studyArgs;
            SetUpFromStudy(_logic._StudyToWorkOn);
            SetUpCriteria();
        }
        /// <summary>
        /// Populates teh criteria datatype dropdown
        /// </summary>
        private void SetUpCriteria()
        {
            foreach (Enum en in Enum.GetValues(typeof (DataFieldDTO.DataType)))
            {
                CriteriaDataType.Items.Add(en);
            }
        }
        /// <summary>
        /// Takes all the input from the criteria and adds it to the criteria object stored in the study in the logic.
        /// </summary>
        public void SaveCriteria()
        {
            var criteriaToAdd = new CriteriaDTO
            {
                Name = CriteriaName.Text,
                Description = CriteriaDescription.Text,
                DataType = (DataFieldDTO.DataType) CriteriaDataType.SelectionBoxItem,
                Rule = (CriteriaDTO.CriteriaRule) CriteriaRule.SelectionBoxItem
            };
            if (criteriaToAdd.DataType == DataFieldDTO.DataType.Enumeration ||
                criteriaToAdd.DataType == DataFieldDTO.DataType.Flags)
            {
                criteriaToAdd.TypeInfo = dataMatchDescription.Text.Split(',');
            }
            else
            {
                criteriaToAdd.DataMatch = new[]
                {
                    dataMatchDescription.Text
                };
            }
            _stageToWorkOn.Criteria = criteriaToAdd;
        }

        /// <summary>
        /// Adds the proper datafield for the criteria rule when changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SelectionChanged(object sender, object e)
        {
            CriteriaRule.Items.Clear();
            var selected = CriteriaDataType.SelectionBoxItem as DataFieldDTO.DataType?;

            switch (selected)
            {
                case DataFieldDTO.DataType.String:
                    foreach (Enum en in Enum.GetValues(typeof (CriteriaDTO.CriteriaRule)))
                    {
                        CriteriaRule.Items.Add(en);
                    }
                    break;

                case DataFieldDTO.DataType.Boolean:
                    CriteriaRule.Items.Add(CriteriaDTO.CriteriaRule.Equals);
                    break;

                case DataFieldDTO.DataType.Enumeration:
                    CriteriaRule.Items.Add(CriteriaDTO.CriteriaRule.Equals);
                    break;

                case DataFieldDTO.DataType.Flags:
                    CriteriaRule.Items.Add(CriteriaDTO.CriteriaRule.Equals);
                    CriteriaRule.Items.Add(CriteriaDTO.CriteriaRule.Contains);
                    break;
                case DataFieldDTO.DataType.Resource:
                    break;
                case null:
                    break;
                default:
                    return;
            }
        }
        /// <summary>
        /// Sets up the page from the logic object.
        /// </summary>
        /// <param name="study"></param>
        private void SetUpFromStudy(StudyDTO study)
        {
            _stageToWorkOn = new StageDTO();
            _stageToWorkOn.StudyID = _logic._StudyToWorkOn.Id;
            _users = new List<UserDTO>();
            _validators = new List<UserDTO>();
            _reviewers = new List<UserDTO>();
            _users.AddRange(_logic._Users);
            SetUpBoxes();
        }

        /// <summary>
        /// When a user is highlighted, make him a reviewer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MakeReviewer_OnClick(object sender, RoutedEventArgs e)
        {
            var takenUser = UserListBox.SelectedItem as UserDTO;
            if (takenUser == null)
            {
                return;
            }
            _reviewers.Add(takenUser);
            _users.Remove(takenUser);
            SetUpBoxes();
        }
        /// <summary>
        /// when a user is highlighted, make him a validator
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MakeValidator_OnClick(object sender, RoutedEventArgs e)
        {
            var takenUser = UserListBox.SelectedItem as UserDTO;
            if (takenUser == null)
            {
                return;
            }
            _validators.Add(takenUser);
            _users.Remove(takenUser);
            SetUpBoxes();
        }
        /// <summary>
        /// remove a selected validator
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveValidator_Click(object sender, RoutedEventArgs e)
        {
            var takenValidator = ValidatorListBox.SelectedItem as UserDTO;
            if (takenValidator == null)
            {
                return;
            }
            _users.Add(takenValidator);
            _validators.Remove(takenValidator);
            SetUpBoxes();
        }
        /// <summary>
        /// Remove a selected reviewer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveReviewer_Click(object sender, RoutedEventArgs e)
        {
            var takenReviewer = ReviewerListBox.SelectedItem as UserDTO;
            if (takenReviewer == null)
            {
                return;
            }
            _users.Add(takenReviewer);
            _reviewers.Remove(takenReviewer);
            SetUpBoxes();
        }
        /// <summary>
        /// refresh the user boxes.
        /// </summary>
        private void SetUpBoxes()
        {
            UserListBox.ItemsSource = null;
            UserListBox.ItemsSource = _users;
            UserListBox.DisplayMemberPath = "Name";
            ReviewerListBox.ItemsSource = null;
            ReviewerListBox.ItemsSource = _reviewers;
            ReviewerListBox.DisplayMemberPath = "Name";
            ValidatorListBox.ItemsSource = null;
            ValidatorListBox.ItemsSource = _validators;
            ValidatorListBox.DisplayMemberPath = "Name";
        }
        /// <summary>
        /// add reviewers and validators to the logic object.
        /// </summary>
        private void AddReviewersAndValidators()
        {
            _stageToWorkOn.ValidatorIDs = _validators.Select(user => user.Id).ToArray();
            _stageToWorkOn.ReviewerIDs = _reviewers.Select(user => user.Id).ToArray();
        }

        /// <summary>
        /// Adds teh distribution rule
        /// </summary>
        private void AddDistribution()
        {
            if (HundredOverlap.IsChecked.Value)
            {
                _stageToWorkOn.DistributionRule = StageDTO.Distribution.HundredPercentOverlap;
                return;
            }
            if (FiftyOverlap.IsChecked.Value)
            {
                _stageToWorkOn.DistributionRule = StageDTO.Distribution.FiftyPercentOverlap;
                return;
            }
            if (NoOverlap.IsChecked.Value)
            {
                _stageToWorkOn.DistributionRule = StageDTO.Distribution.NoOverlap;
            }
        }

        /// <summary>
        /// runs through the visible fields and if the box is checked adds it to the logic object.
        /// </summary>
        private void AddVisibleFields()
        {
            var stageFields = new List<StageDTO.FieldType>();
            stageFields.Clear();
            if (AddressCheckbox.IsChecked.Value)
            {
                stageFields.Add(StageDTO.FieldType.Address);
            }
            if (AnnoteCheckbox.IsChecked.Value)
            {
                stageFields.Add(StageDTO.FieldType.Annote);
            }
            if (AuthorCheckbox.IsChecked.Value)
            {
                stageFields.Add(StageDTO.FieldType.Author);
            }
            if (BooktitleCheckbox.IsChecked.Value)
            {
                stageFields.Add(StageDTO.FieldType.Booktitle);
            }
            if (ChapterCheckbox.IsChecked.Value)
            {
                stageFields.Add(StageDTO.FieldType.Chapter);
            }
            if (CrossrefCheckbox.IsChecked.Value)
            {
                stageFields.Add(StageDTO.FieldType.Crossref);
            }
            if (EditionCheckbox.IsChecked.Value)
            {
                stageFields.Add(StageDTO.FieldType.Edition);
            }
            if (EditorCheckbox.IsChecked.Value)
            {
                stageFields.Add(StageDTO.FieldType.Editor);
            }
            if (HowPublishedCheckbox.IsChecked.Value)
            {
                stageFields.Add(StageDTO.FieldType.HowPublished);
            }
            if (InstritutionCheckbox.IsChecked.Value)
            {
                stageFields.Add(StageDTO.FieldType.Instritution);
            }
            if (JournalCheckbox.IsChecked.Value)
            {
                stageFields.Add(StageDTO.FieldType.Journal);
            }
            if (KeyCheckbox.IsChecked.Value)
            {
                stageFields.Add(StageDTO.FieldType.Key);
            }
            if (MonthCheckbox.IsChecked.Value)
            {
                stageFields.Add(StageDTO.FieldType.Month);
            }
            if (NoteCheckbox.IsChecked.Value)
            {
                stageFields.Add(StageDTO.FieldType.Note);
            }
            if (NumberCheckbox.IsChecked.Value)
            {
                stageFields.Add(StageDTO.FieldType.Number);
            }
            if (OrganizationCheckbox.IsChecked.Value)
            {
                stageFields.Add(StageDTO.FieldType.Organization);
            }
            if (PagesCheckbox.IsChecked.Value)
            {
                stageFields.Add(StageDTO.FieldType.Pages);
            }
            if (PublisherCheckbox.IsChecked.Value)
            {
                stageFields.Add(StageDTO.FieldType.Publisher);
            }
            if (SchoolCheckbox.IsChecked.Value)
            {
                stageFields.Add(StageDTO.FieldType.School);
            }
            if (SeriesCheckbox.IsChecked.Value)
            {
                stageFields.Add(StageDTO.FieldType.Series);
            }
            if (TitleCheckbox.IsChecked.Value)
            {
                stageFields.Add(StageDTO.FieldType.Title);
            }
            if (TypeCheckbox.IsChecked.Value)
            {
                stageFields.Add(StageDTO.FieldType.Type);
            }
            if (VolumeCheckbox.IsChecked.Value)
            {
                stageFields.Add(StageDTO.FieldType.Volume);
            }
            if (YearCheckbox.IsChecked.Value)
            {
                stageFields.Add(StageDTO.FieldType.Year);
            }
            if (URLCheckbox.IsChecked.Value)
            {
                stageFields.Add(StageDTO.FieldType.URL);
            }
            if (ISBNCheckbox.IsChecked.Value)
            {
                stageFields.Add(StageDTO.FieldType.ISBN);
            }
            if (ISSNCheckbox.IsChecked.Value)
            {
                stageFields.Add(StageDTO.FieldType.ISSN);
            }
            if (LCCNCheckbox.IsChecked.Value)
            {
                stageFields.Add(StageDTO.FieldType.LCCN);
            }
            if (AbstractCheckbox.IsChecked.Value)
            {
                stageFields.Add(StageDTO.FieldType.Abstract);
            }
            if (KeywordsCheckbox.IsChecked.Value)
            {
                stageFields.Add(StageDTO.FieldType.Keywords);
            }
            if (PriceCheckbox.IsChecked.Value)
            {
                stageFields.Add(StageDTO.FieldType.Price);
            }
            if (CopyrightCheckbox.IsChecked.Value)
            {
                stageFields.Add(StageDTO.FieldType.Copyright);
            }
            if (LanguageCheckbox.IsChecked.Value)
            {
                stageFields.Add(StageDTO.FieldType.Language);
            }
            if (ContentsCheckbox.IsChecked.Value)
            {
                stageFields.Add(StageDTO.FieldType.Contents);
            }
            if (DoiCheckbox.IsChecked.Value)
            {
                stageFields.Add(StageDTO.FieldType.Doi);
            }
            _stageToWorkOn.VisibleFields = stageFields.ToArray();
        }

        /// <summary>
        /// Save the information to the page and save it to the logic object.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveAndReturn_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _stageToWorkOn.Name = nameInput.Text;
                SaveCriteria();
                AddReviewersAndValidators();
                AddDistribution();
                AddVisibleFields();
                _logic._StudyToWorkOn.Stages =
                    (_logic._StudyToWorkOn.Stages ?? Enumerable.Empty<StageDTO>()).Concat(new[] {_stageToWorkOn})
                        .ToArray(); //adds the stage to the array
                Frame.Navigate(typeof (ManageStudyPage), _logic);
            }
            catch (NullReferenceException)
            {
                ErrorBox.Text = " ERROR : Make sure that all fields are filled out.";
            }
        }
        /// <summary>
        /// Discards everything and just navigates back.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelAndReturn_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof (ManageStudyPage), _logic);
        }
    }
}
using ProgPoe_ClassLibrary;
using System;
using System.Collections.Generic;
using System.Media;
using System.Text.RegularExpressions;
using System.Windows;

namespace ProgPoe_WPF
{
    public partial class SelfStudyWindow : Window
    {  
        /// <summary>
        /// Store ModuleClass Obj
        /// </summary>
        private ModuleClass Module = new ModuleClass();

        /// <summary>
        /// Store StudyWeeksClass List
        /// </summary>
        private List<StudyWeeksClass> StudyWeeksList;

        ///--------------------------------------------------------------------------///
        /// <summary>
        /// Default Constructor
        /// Checks Module List for code to display Module self study in label
        /// </summary>
        public SelfStudyWindow()
        {
            InitializeComponent();

            this.Module = new DatabaseManagerClass().GetOneModule();
            this.StudyWeeksList = new DatabaseManagerClass().GetSelfStudyList();

            DisplayStudyWeeks();

            lblHoursRequired.Content = Module.StudyHoursPerWeek;
            lblModuleCode.Content = Module.ModuleCode;
        }

        ///--------------------------------------------------------------------------///
        /// <summary>
        /// Method Adds Study Sessions Inputs Date and hours into SQL Database
        /// TextBox and DatePicker are checked to see if they contain values, it returns error message in case inputs have no value
        /// If study record sucessfully is saved it will display a message to the user to inform him
        /// Error messages come with sounds
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddSelfStudy_Click(object sender, RoutedEventArgs e)
        {
            var HoursSpent = txtNumberOfSpentWorking.Text;

            if (!string.IsNullOrEmpty(HoursSpent) && dtDateWorked.SelectedDate.HasValue)
            {    
                var intHours = int.Parse(HoursSpent);
                var selectedDate = dtDateWorked.SelectedDate.Value;

                AddStudySessionToDb(intHours, selectedDate);
            }
            else
            {
                SystemSounds.Hand.Play();
                MessageBox.Show("Please add amount of hours spent working and the date", "Error");
            }
                txtNumberOfSpentWorking.Text = string.Empty;
                dtDateWorked.Text = null;
            
                DisplayStudyWeeks();    
        }

        ///--------------------------------------------------------------------------///
        /// <summary>
        /// Method adds Study Session to Database
        /// If date is behind start of semester it will display error message
        /// If study hours for week is complted display message
        /// If user overstudied display message
        /// </summary>
        public void AddStudySessionToDb(int intHours, DateTime selectedDate)
        {
            StudyWeeksClass closest = new DatabaseManagerClass().GetClosestDate(selectedDate);

            if (closest.StudyWeeksId == Guid.Empty)
            {
                SystemSounds.Hand.Play();
                MessageBox.Show("Please Select a valid date!", "Error");
                return;
            }
            else
            {
                var response = new DatabaseManagerClass().CreateStudySession(intHours, selectedDate);

                if (!response.Equals(string.Empty))
                {
                    SystemSounds.Hand.Play();
                    MessageBox.Show(response, "Error");
                    return;
                }
            }

            var selected = new CalculationsClass().GetDateForStudyHours(selectedDate, StudyWeeksList);
            if (selected.HoursLeft == 0)
            {
                MessageBox.Show("The self study hours are fulfilled for this week", "Note");
                return;

            }
            if (selected.HoursLeft < intHours)
            {
                MessageBox.Show("you overstudied", "Note");
                selected.HoursLeft = 0;
            }
            else
            {
                selected.HoursLeft -= intHours;
            }
        
            UpdateListToDb(selected);
        }

        ///--------------------------------------------------------------------------///
        /// <summary>
        /// Update study hours in DB
        /// </summary>
        public void UpdateListToDb(StudyWeeksClass selected)
        {
            // Update list and post to database
            string result = new DatabaseManagerClass().UpdateStudyHours(selected);
            if (!result.Equals(string.Empty))
            {
                SystemSounds.Hand.Play();
                MessageBox.Show(result, "Error");
                return;
            }
                MessageBox.Show("Study session saved.", "Saved");
        }
      
        ///--------------------------------------------------------------------------///
        /// <summary>
        /// Allow user to input only numbers
        /// Blocks Characters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNumberOfSpentWorking_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        ///--------------------------------------------------------------------------///
        /// <summary>
        /// Method takes user to previous window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGoBack_Click(object sender, RoutedEventArgs e)
        {
            ModulesWindow moduleWindow = new ModulesWindow();
            moduleWindow.Show();
            Hide();
        }
   
        ///--------------------------------------------------------------------------///
        /// <summary>
        /// Display Study Weeks in ListBox
        /// </summary>
        public void DisplayStudyWeeks()
        {
            //here is a reminder
            lstWeeks.Items.Clear();
            var studySessionsList = new DatabaseManagerClass().GetSelfStudyList();
            int i = 0;

            foreach ( var entry in studySessionsList )
            {
                i++;
                string Output = $"Week {i}: {entry.StartDate.ToShortDateString()} Time: {entry.HoursLeft}h";
                lstWeeks.Items.Add(Output);
            }
        }
    }
}
///-----------------------------------------------------------< END >---------------------------------------------------------------------///
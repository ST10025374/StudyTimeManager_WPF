using ProgPoe_ClassLibrary;
using System;
using System.Collections.Generic;
using System.Media;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace ProgPoe_WPF
{
    public partial class SelfStudyWindow : Window
    {
        /// <summary>
        /// Instance of CalculationClass
        /// </summary>
        private CalculationsClass Calculate = new CalculationsClass();
   
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
        /// Constructor
        /// Checks Module List for code to display Module self study in label
        /// </summary>
        public SelfStudyWindow()
        {
            InitializeComponent();

            this.Module = new DatabaseManagerClass().GetOneModule();
            UpdateCurrentWeeksLeft();
            lblHoursRequired.Content = Module.StudyHoursPerWeek;
            lblCurrentWeek.Content = Module.CurrentWeek;
         
            DisplayStudyWeeks();

            this.StudyWeeksList = new DatabaseManagerClass().GetSelfStudyList();
        }

        ///--------------------------------------------------------------------------///
        /// <summary>
        /// Method Adds Study Sessions Inputs Date and hours into Dictionary in ModuleClass
        /// TextBox and DatePicker are checked to see if they contain values, it returns error message in case inputs have no value
        /// Used LINQ to find the specific module by ModuleCode
        /// If the module is found and no record for the selected date exists
        /// If study record sucessfully is saved it will display a message to the user to inform him
        /// Error messages come with sounds
        /// If study time is completed a message will be displayed for user and selfStudy time will be restarted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddSelfStudy_Click(object sender, RoutedEventArgs e)
        {
            var HoursSpent = txtNumberOfSpentWorking.Text;
            if (!string.IsNullOrEmpty(HoursSpent) && dtDateWorked.SelectedDate.HasValue)
            {
                // Create study session from database
                var intHours = int.Parse(HoursSpent);
                var selectedDate = dtDateWorked.SelectedDate.Value;

                AddStudySessionToDb(intHours, HoursSpent, selectedDate);

                // reloads the current week
                UpdateCurrentWeeksLeft();
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
        /// 
        /// </summary>
        public void AddStudySessionToDb(int intHours, string HoursSpent, DateTime selectedDate)
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
                MessageBox.Show("The self study hours are fulfilled");
                return;

            }
            if (selected.HoursLeft < intHours)
            {
                MessageBox.Show("you overstudied");
                selected.HoursLeft = 0;
            }
            else
            {
                selected.HoursLeft -= intHours;
            }

            int NewHoursLeft = Calculate.CalcMinus(closest.HoursLeft, int.Parse(HoursSpent));

            UpdateListToDb(selected, NewHoursLeft);
        }

        ///--------------------------------------------------------------------------///
        /// <summary>
        /// 
        /// </summary>
        public void UpdateListToDb(StudyWeeksClass selected, int NewHoursLeft)
        {
            // Update list and post to database
            string result = new DatabaseManagerClass().UpdateStudyHours(selected);
            if (!result.Equals(string.Empty))
            {
                SystemSounds.Hand.Play();
                MessageBox.Show(result, "Error");
                return;
            }

            if (NewHoursLeft.Equals(0))
            {
                MessageBox.Show("Study time for this week was completed. " +
                        "Study time will restart for next week", "Hours completed");
            }
            else
            {
                MessageBox.Show("Study session saved.", "Saved");
            }
        }

        ///--------------------------------------------------------------------------///
        /// <summary>
        /// 
        /// </summary>
        private void UpdateCurrentWeeksLeft()
        {
            var currentStudyWeek = new DatabaseManagerClass().GetClosestDate(DateTime.Now);
            lblCurrentWeek.Content = currentStudyWeek.HoursLeft.ToString();
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
        /// 
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
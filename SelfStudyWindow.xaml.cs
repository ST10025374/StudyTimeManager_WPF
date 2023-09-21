using ProgPoe_ClassLibrary;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace ProgPoe_WPF
{
    public partial class SelfStudyWindow : Window
    {
        /// <summary>
        /// Instance of SemesterClass
        /// </summary>
        private SemesterClass _Semester;

        /// <summary>
        /// Instance of CalculationClass
        /// </summary>
        CalculationsClass Calculate = new CalculationsClass();

        /// <summary>
        /// Store Module Code selected from ModulesWindow
        /// </summary>
        private string ModuleCode;
        ///--------------------------------------------------------------------------///
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SelfStudyWindow()
        {
            InitializeComponent();           
        }

        ///--------------------------------------------------------------------------///
        /// <summary>
        /// Constructor
        /// Checks Module List for code to display Module self study in label
        /// </summary>
        public SelfStudyWindow(SemesterClass semester, string moduleCode)
        {
            InitializeComponent();
            _Semester = semester;
            ModuleCode = moduleCode;
            lblModuleCode.Content = ModuleCode;
            
            foreach (var Module in _Semester.ModulesList)
            {
                if (Module.ModuleCode == ModuleCode)
                {
                    lblCurrentWeek.Content = Module.CurrentWeek;
                    lblHoursRequired.Content = Module.ModuleHoursPerWeek.ToString();
                    lblHoursLeft.Content =  Module.SelfStudyHoursPerWeek.ToString();
                }
            }

            DisplayStudySessions();
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
              
                var targetModule = _Semester.ModulesList
                    .FirstOrDefault(module => module.ModuleCode == ModuleCode);

                if (targetModule != null &&
                    !targetModule.StudySessionsRecords.ContainsKey(dtDateWorked.SelectedDate.Value))
                {
                    double NewHoursLeft = Calculate.CalcMinus(targetModule.SelfStudyHoursPerWeek, double.Parse(HoursSpent));

                    if (NewHoursLeft.Equals(0))
                    {
                        MessageBox.Show("Study time for this week was completed. " +
                            "Study time will restart for next week", "Hours completed");
                        targetModule.SelfStudyHoursPerWeek = targetModule.ModuleHoursPerWeek;
                        lblHoursLeft.Content = targetModule.SelfStudyHoursPerWeek;
                        targetModule.StudySessionsRecords.Add(dtDateWorked.SelectedDate.Value, int.Parse(txtNumberOfSpentWorking.Text));

                        targetModule.CurrentWeek = targetModule.CurrentWeek + 1;
                        lblCurrentWeek.Content = targetModule.CurrentWeek;
                    }
                    else
                    {
                        targetModule.SelfStudyHoursPerWeek = NewHoursLeft;
                        lblHoursLeft.Content = targetModule.SelfStudyHoursPerWeek;
                        targetModule.StudySessionsRecords.Add(dtDateWorked.SelectedDate.Value, int.Parse(txtNumberOfSpentWorking.Text));
                        MessageBox.Show("Study session saved.", "Saved");
                    }                       
                }
                else if (targetModule != null)
                {
                    SystemSounds.Hand.Play();
                    MessageBox.Show("A study record for this date already exists.", "Error");
                }
            }
            else
            {
                SystemSounds.Hand.Play();
                MessageBox.Show("Please add amount of hours spent working and the date", "Error");
            }

            txtNumberOfSpentWorking.Text = string.Empty;

            DisplayStudySessions();

            /*             -------------- Old code without using LINQ----------------
            var HoursSpent = txtNumberOfSpentWorking.Text;
           
            if (!string.IsNullOrEmpty(HoursSpent) && dtDateWorked.SelectedDate.HasValue)
            {
                foreach (var Module in _Semester.ModulesList)
                {
                    if (Module.ModuleCode == ModuleCode)
                    {
                        if (Module.StudySessionsRecords.ContainsKey(dtDateWorked.SelectedDate.Value))
                        {
                            SystemSounds.Hand.Play();
                            MessageBox.Show("A study record for this date already exists.", "Error");
                            return;
                        }

                        double NewHoursLeft = Module.SelfStudyHoursPerWeek - double.Parse(HoursSpent);

                        Module.SelfStudyHoursPerWeek = NewHoursLeft;

                        lblHoursLeft.Content = Module.SelfStudyHoursPerWeek;

                        Module.StudySessionsRecords.Add(dtDateWorked.SelectedDate.Value, int.Parse(txtNumberOfSpentWorking.Text));
                        MessageBox.Show("Study record saved.", "Saved");
                    }
                }
            }
            else
            {
                SystemSounds.Hand.Play();
                MessageBox.Show("Please add amount of hours spent working and the date", "Error");
            }
            */
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
            ModulesWindow moduleWindow = new ModulesWindow(_Semester);
            moduleWindow.Show();
            Hide();
        }

        ///--------------------------------------------------------------------------///
        /// <summary>
        /// Method use to Display study sessions in ListBox
        /// Method gets desired moduleCode 
        /// It uses a for each to display each key in dictionary
        /// Combines Date and hours into a single string
        /// </summary>
        public void DisplayStudySessions()
        {
            lstDisplaySessions.Items.Clear();

            var TargetModule = _Semester.ModulesList.FirstOrDefault(module => module.ModuleCode == ModuleCode);

            if (TargetModule != null)
            {
                foreach (var entry in TargetModule.StudySessionsRecords)
                {
                    string Output = $"Date: {entry.Key.ToShortDateString()} Time: {entry.Value}h";
                    lstDisplaySessions.Items.Add(Output);
                }
            }
        }
    }
}
///-----------------------------------------------------------< END >---------------------------------------------------------------------///
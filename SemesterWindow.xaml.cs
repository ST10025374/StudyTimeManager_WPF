using ProgPoe_ClassLibrary;
using ProgPoePart1New;
using System;
using System.Media;
using System.Text.RegularExpressions;
using System.Windows;

namespace ProgPoe_WPF
{
    public partial class SemesterWindow : Window
    {
        ///--------------------------------------------------------------------------///
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SemesterWindow()
        {
            InitializeComponent();
        }

        ///--------------------------------------------------------------------------///
        /// <summary>
        /// Store Number of weeks and start date of semester
        /// It stores inputs in SemesterClass
        /// The method checks if the TextBox and DatePicker have values otherwise 
        /// It returns error message
        /// Opens ModulesWindow
        /// It passes _Semester object to ModulesWindow
        /// When error message is displayed it comes with a sound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddSemester_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNumberOfWeeks.Text) && dtStartDate.SelectedDate.HasValue)
            {
                // Create Semester from database
                var newSemester = new SemesterClass(dtStartDate.SelectedDate.Value, int.Parse(txtNumberOfWeeks.Text));
                var response = new DatabaseManagerClass().CreateSemester(newSemester);

                if (!response.Equals(string.Empty))
                {
                    SystemSounds.Hand.Play();
                    MessageBox.Show(response, "Error");
                    return;
                }

                StoredIDs.SemesterId = newSemester.SemesterId;  

                ModulesWindow moduleWindow = new ModulesWindow();
                moduleWindow.Show();
                Hide();
            }
            else
            {
                SystemSounds.Hand.Play();
                MessageBox.Show("Please select the start date and give the number of weeks for the semester.", "Error");               
            }
        }

        ///--------------------------------------------------------------------------///
        /// <summary>
        /// Allow user to input only numbers
        /// Blocks Characters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNumberOfWeeks_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
///-----------------------------------------------------------< END >---------------------------------------------------------------------///
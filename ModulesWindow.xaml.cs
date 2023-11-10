using ProgPoe_ClassLibrary;
using ProgPoePart1New;
using System.Collections.Generic;
using System.Dynamic;
using System.Media;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace ProgPoe_WPF
{
    public partial class ModulesWindow : Window
    {
        /// <summary>
        /// Store ModuleClass Object List
        /// </summary>
        private List<ModuleClass> ModuleList;

        ///--------------------------------------------------------------------------///
        /// <summary>
        /// Default Constructor
        /// Populate ListView with Module Details
        /// </summary>
        public ModulesWindow()
        {
            InitializeComponent();      
            PopulateListView();
        }

        ///--------------------------------------------------------------------------///
        /// <summary>
        /// Method to populate ListView with Module Details
        /// Clear ListView before populating
        /// Gets module details from database
        /// </summary>
        private void PopulateListView()
        {
            this.ModuleList = new DatabaseManagerClass().GetModuleList();
            
            lstDisplayModuleData.Items.Clear();

            foreach (var module in ModuleList)
            {
                lstDisplayModuleData.Items.Add(module.StringOutput());
            }

        }

        ///--------------------------------------------------------------------------///
        /// <summary>
        /// Get Modules details
        /// Store them in ModuleClass Object
        /// Module Code is Displayed in ListView for user to view
        /// If statement is used as input validation in case any input is empty it will display message  
        /// When button is clicked TextBoxes are cleared if info is accepted
        /// When error message is displayed it comes with a sound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddModule_Click(object sender, RoutedEventArgs e)
        {
            var moduleCode = txtModuleCode.Text;
            var moduleName = txtModuleName.Text;
            int numberOfCredits = 0;
            int classHoursPerWeek = 0;          
            var semester = new DatabaseManagerClass().ReadSemester();

            try
            {
                numberOfCredits = int.Parse(txtNumberOfCredits.Text);
                classHoursPerWeek = int.Parse(txtClassHoursPerWeek.Text);
            }
            catch
            {
                SystemSounds.Hand.Play();
                MessageBox.Show("Please make sure you added all module information", "Error");
                return;
            }

            if (!string.IsNullOrEmpty(moduleCode) && !string.IsNullOrEmpty(moduleName))
            {              
                CreateModule(moduleCode, moduleName, numberOfCredits, classHoursPerWeek, semester);
              
                txtModuleCode.Text = string.Empty;
                txtModuleName.Text = string.Empty;
                txtNumberOfCredits.Text = string.Empty;
                txtClassHoursPerWeek.Text = string.Empty;

                PopulateListView();
            }
            else
            {
                SystemSounds.Hand.Play();
                MessageBox.Show("Please make sure you added all module information", "Error");
            }
        }

        ///--------------------------------------------------------------------------///
        /// <summary>
        /// Method That Creates ModuleClass Object
        /// Returns ModuleClass Object
        /// </summary>
        /// <returns></returns>
        public void CreateModule(string moduleCode, string moduleName, int numberOfCredits,int classHoursPerWeek, SemesterClass semester)
        {
            var module = new ModuleClass(moduleCode, moduleName, numberOfCredits, classHoursPerWeek, semester.NumberOfWeeks);

            if (module.StudyHoursPerWeek == 0)
            {
                SystemSounds.Hand.Play();
                MessageBox.Show("Calculation Invalid, please enter a valid class hours and number of credit combination", "Error");           
            }
            else
            {
                CreateModuleInDB(module, semester);
            }          
        }

        ///--------------------------------------------------------------------------///
        /// <summary>
        /// Method to create module in database
        /// Displayes error message if any error occurs
        /// </summary>
        /// <param name="module"></param>
        public void CreateModuleInDB(ModuleClass module, SemesterClass semester)
        {
            var response = new DatabaseManagerClass().CreateModule(module);

            if (!response.Equals(string.Empty))
            {
                SystemSounds.Hand.Play();
                MessageBox.Show(response, "Error");
                return;
            }
            else
            {
                PopulateStudyWeeksInDB(semester, module);
            }

        }

        ///--------------------------------------------------------------------------///
        /// <summary>
        /// Method to populate database table with self study hours
        /// Displayes error message if any error occurs
        /// </summary>
        public void PopulateStudyWeeksInDB(SemesterClass semester, ModuleClass module)
        {
            var error = new DatabaseManagerClass().AddSelfStudy(semester.StartDate, semester.NumberOfWeeks, module.StudyHoursPerWeek, module.ModuleId);
            if (!error.Equals(string.Empty))
            {
                SystemSounds.Hand.Play();
                MessageBox.Show(error, "Error");
                return;
            }
        }

        ///--------------------------------------------------------------------------///
        /// <summary>
        /// Allow user to input only numbers
        /// Blocks Characters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNumberOfCredits_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        ///--------------------------------------------------------------------------///
        /// <summary>
        /// Allow user to input only numbers
        /// Blocks Characters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtClassHoursPerWeek_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        ///--------------------------------------------------------------------------///
        /// <summary>
        /// Take user to Menu Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGoBack_Click(object sender, RoutedEventArgs e)
        {
            MenuWindow menuWindow = new MenuWindow();
            menuWindow.Show();
            Hide();
        }

        ///--------------------------------------------------------------------------///
        /// <summary>
        /// Method to take user to Self Study Window
        /// Ensures the selected index is valid
        /// Stores ModuleId
        /// Opens Self Study Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbModules_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = lstDisplayModuleData.SelectedIndex;

            if (selectedIndex >= 0)
            {
                if (selectedIndex < this.ModuleList.Count)
                {
                    ModuleClass selectedModule = ModuleList[selectedIndex];
                    string ModuleCodeSelected = selectedModule.ModuleCode;

                    foreach (var Module in ModuleList)
                    {
                        if (Module.ModuleCode.Equals(ModuleCodeSelected))
                        {                      
                            StoredIDs.ModuleId = new DatabaseManagerClass().GetModuleID(ModuleCodeSelected);

                            SelfStudyWindow selfStudyWindow = new SelfStudyWindow();
                            selfStudyWindow.Show();
                            Hide();
                            break;
                        }
                    }
                }
            }
        }
    }
}
///-----------------------------------------------------------< END >---------------------------------------------------------------------///
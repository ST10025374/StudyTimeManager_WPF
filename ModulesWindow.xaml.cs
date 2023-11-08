﻿using ProgPoe_ClassLibrary;
using ProgPoePart1New;
using System.Collections.Generic;
using System.Media;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace ProgPoe_WPF
{
    public partial class ModulesWindow : Window
    {
        /// <summary>
        /// Instance of databaseManagerClass
        /// </summary>
        private DatabaseManagerClass _dbManager;

        /// <summary>
        /// Store ModuleClass Object List
        /// </summary>
        private List<ModuleClass> ModuleList;

        ///--------------------------------------------------------------------------///
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ModulesWindow()
        {
            InitializeComponent();
         
            PopulateListView();
        }

        ///--------------------------------------------------------------------------///
        /// <summary>
        /// 
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
        /// Store Modules in _Semester ModulesList
        /// Module Code is Displayed in ListView for user to view
        /// If statement is used as input validation in case any input is empty it will display message
        /// Calculate self hours study when button pressed
        /// Calculations class method called to get values and return self hours study
        /// returned value stored in module 
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
            var semester = new DatabaseManagerClass().ReadSemesterReturnNumOfWeeks();

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
                var Module = new ModuleClass(moduleCode, moduleName, numberOfCredits, classHoursPerWeek, semester.NumberOfWeeks);

                if (Module.StudyHoursPerWeek == 0)
                {
                    SystemSounds.Hand.Play();
                    MessageBox.Show("Calculation Invalid, please enter a valid class hours and number of credit combination", "Error");
                    return;            
                }

                // Create Module from database               
                var response = new DatabaseManagerClass().CreateModule(Module);

                if (!response.Equals(string.Empty))
                {
                    SystemSounds.Hand.Play();
                    MessageBox.Show(response, "Error");
                    return;
                }

                // Populate database Table
                var error = new DatabaseManagerClass().AddSelfStudy(semester.StartDate, semester.NumberOfWeeks, Module.StudyHoursPerWeek, Module.ModuleId);
                if (!error.Equals(string.Empty))
                {
                    SystemSounds.Hand.Play();
                    MessageBox.Show(error, "Error");
                    return;
                }

                StoredIDs.ModuleId = Module.ModuleId;

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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbModules_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = lstDisplayModuleData.SelectedIndex;

            if (selectedIndex >= 0)
            {
                // Ensure the selected index is valid
                if (selectedIndex < this.ModuleList.Count)
                {
                    ModuleClass selectedModule = ModuleList[selectedIndex];
                    string ModuleCodeSelected = selectedModule.ModuleCode;

                    foreach (var Module in ModuleList)
                    {
                        if (Module.ModuleCode.Equals(ModuleCodeSelected))
                        {
                            //Store ModuleId
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
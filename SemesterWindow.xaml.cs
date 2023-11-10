﻿using ProgPoe_ClassLibrary;
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
        /// It returns error message if the user did not input any value
        /// Creates a new semester and stores it in the database
        /// Records the last semester added by the user
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

                var LogInSemester = new DatabaseManagerClass().CreateLogIn(newSemester);

                MessageBox.Show("Semester information saved.", "Saved");

                txtNumberOfWeeks.Text = string.Empty;
                dtStartDate.Text = null; 
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

        ///--------------------------------------------------------------------------///
        /// <summary>
        /// Takie user to Menu Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGoBack_Click(object sender, RoutedEventArgs e)
        {
            MenuWindow menuWindow = new MenuWindow();
            menuWindow.Show();
            Hide();
        }
    }
}
///-----------------------------------------------------------< END >---------------------------------------------------------------------///
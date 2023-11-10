using ProgPoe_WPF;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ProgPoe_ClassLibrary;
using System;
using System.Media;

namespace ProgPoePart1New
{
    /// <summary>
    /// Interaction logic for MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {       
        ///--------------------------------------------------------------------------///
        /// <summary>
        /// Default Constructor
        /// Display a message if the user has not registered a semester
        /// Get last semester from SQL Database for this user and store it in StoredIDs
        /// </summary>
        public MenuWindow()
        {
            InitializeComponent();

            var response = new DatabaseManagerClass().GetSemesterFromUser();
            if (response.Equals(Guid.Empty))
            {
                MessageBox.Show("Please Register a Semester", "Note");
            }
            else
            {
                StoredIDs.SemesterId = response;
            }    
        }

        ///--------------------------------------------------------------------------///
        /// <summary>
        /// Take user to Semester Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuSemester_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            SemesterWindow semesterWindow = new SemesterWindow();
            semesterWindow.Show();
            Hide();
        }

        ///--------------------------------------------------------------------------///
        /// <summary>
        /// Take user to Modules Window
        /// In case the user has not registered a semester, display a message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuModule_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (StoredIDs.SemesterId.Equals(Guid.Empty))
            {
                MessageBox.Show("Please Register a Semester", "Note");
                return;
            }
            else
            {
                ModulesWindow modulesWindow = new ModulesWindow();
                modulesWindow.Show();
                Hide();
            }
        }

        ///--------------------------------------------------------------------------///
        /// <summary>
        /// Take user to Log Out
        /// Reset StoredIDs to empty
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuLogout_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            StoredIDs.SemesterId = Guid.Empty;
            StoredIDs.ModuleId = Guid.Empty;
            StoredIDs.UserId = Guid.Empty;

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Hide();
        }
    }
}
///-----------------------------------------------------------< END >---------------------------------------------------------------------///
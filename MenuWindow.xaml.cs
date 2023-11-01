using ProgPoe_WPF;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProgPoePart1New
{
    /// <summary>
    /// Interaction logic for MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public MenuWindow()
        {
            InitializeComponent();
        }

        ///--------------------------------------------------------------------------///
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (menuListView.SelectedItem != null)
            {
                string selectedItemText = ((ListViewItem)menuListView.SelectedItem).Content.ToString();

                if (selectedItemText.Equals("Semester Window"))
                {                  
                    SemesterWindow semesterWindow = new SemesterWindow();
                    semesterWindow.Show();
                }

                if (selectedItemText.Equals("Module Window"))
                {                 
                    ModulesWindow modulesWindow = new ModulesWindow();
                    modulesWindow.Show();
                }

                if (selectedItemText.Equals("Log Out"))
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuSemester_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            SemesterWindow semesterWindow = new SemesterWindow();
            semesterWindow.Show();
            Hide();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuModule_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ModulesWindow modulesWindow = new ModulesWindow();
            modulesWindow.Show();
            Hide();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuLogout_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Hide();
        }
    }
}
///-----------------------------------------------------------< END >---------------------------------------------------------------------///
using ProgPoe_WPF;
using System.Windows;

namespace ProgPoePart1New
{
    public partial class MainWindow : Window
    {
        ///--------------------------------------------------------------------------///
        /// <summary>
        /// Default Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        ///--------------------------------------------------------------------------///
        /// <summary>
        /// Button to Login into semester Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            SemesterWindow semesterWindow = new SemesterWindow();
            semesterWindow.Show();
            Hide();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
///-----------------------------------------------------------< END >---------------------------------------------------------------------///
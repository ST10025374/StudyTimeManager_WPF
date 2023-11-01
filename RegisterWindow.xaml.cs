using ProgPoe_ClassLibrary;
using System.Media;
using System.Text.RegularExpressions;
using System.Windows;

namespace ProgPoePart1New
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public RegisterWindow()
        {
            InitializeComponent();
        }

        ///--------------------------------------------------------------------------///
        /// <summary>
        /// Button to Register User
        /// If either the username or password is empty, display an error message and play a sound\
        /// Check if username and password are empty
        /// Create a new user with the provided username and password
        /// Attempt to create the user in the database
        /// Check if there was an error during user creation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            var username = txtUsername.Text;
            var password = txtPassword.Text;
           
            if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(password))
            {
                SystemSounds.Hand.Play();
                MessageBox.Show("Please make sure you added all user information", "Error");
                return;
            }

            // Password policy
            if (!Regex.IsMatch(password, @"\d"))
            {
                SystemSounds.Hand.Play();
                MessageBox.Show("Password must contain a number", "Error");
                return;
            }
            if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                SystemSounds.Hand.Play();
                MessageBox.Show("Password must contain a capital letter", "Error");
                return;
            }
            if (password.Length < 8)
            {
                SystemSounds.Hand.Play();
                MessageBox.Show("Password must be 8 or longer characters", "Error");
                return;
            }
            if (!password.Equals(txtConfirmPassword.Text))
            {
                SystemSounds.Hand.Play();
                MessageBox.Show("Passwords must match!", "Error");
                return;
            }

            // Create user from database
            var newUser = new UserClass(txtUsername.Text, txtPassword.Text);

            var response = new DatabaseManagerClass().CreateUser(newUser);
            if (!response.Equals(string.Empty))
            {
                SystemSounds.Hand.Play();
                MessageBox.Show(response, "Error");
                return;
            }

            // login to system
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Hide();
        }

        ///--------------------------------------------------------------------------///
        /// <summary>
        /// Button takes user back to main menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGoBackToMain_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Hide();
        }
    }
}
///-----------------------------------------------------------< END >---------------------------------------------------------------------///
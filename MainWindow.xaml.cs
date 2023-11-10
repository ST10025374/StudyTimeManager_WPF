using ProgPoe_ClassLibrary;
using ProgPoe_WPF;
using System;
using System.Media;
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
        /// Method retrieves User ID from Database and stores it in StoredIDs Class
        /// In case of error, a message box will be displayed
        /// If everything is correct, MenuWindow will be displayed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var response = new DatabaseManagerClass().ReadUser(txtUsername.Text, txtPassword.Text);
            Guid userId = Guid.NewGuid();
            try
            {
                userId = Guid.Parse(response);
            }
            catch
            {
                SystemSounds.Hand.Play();
                MessageBox.Show(response, "Error");
                return;
            }

            StoredIDs.UserId = userId;
            MenuWindow menuWindow = new MenuWindow();
            menuWindow.Show();
            Hide();
        }

        ///--------------------------------------------------------------------------///
        /// <summary>
        /// Method to Register User
        /// It opens a new RegisterWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.Show();
            Hide();
        }
    }
}
///-----------------------------------------------------------< END >---------------------------------------------------------------------///
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProgPoePart1New
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            var username = txtUsername.Text;
            var password = txtPassword.Text;
            //var confirmPassword = txtConfirmPassword.Text;
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
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
            if (password.Length > 8)
            {
                SystemSounds.Hand.Play();
                MessageBox.Show("Password must be 8 or longer characters", "Error");
                return;
            }


        }
    }
}

using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Registration
{
    public partial class MainWindow : Window
    {
        private DatabaseHelper dbHelper;

        public MainWindow()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
        }
        public bool Auth2(string username, string email, string password, string checkpassword)
        {
            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            if (username.Length > 50 || password.Length > 50)
            {
                return false;
            }
            

            if (!IsValidEmail(email))
            {
                return false;
            }
            if (password != checkpassword)
            {
                return false;
            }
            return true;

        }
    
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = NameBox.Text;
            string email = EmailBox.Text;
            string password = PasswordBox.Password;
            string checkpassword = CheckPasswordBox.Password;
            Auth2(username, email, password, checkpassword);

            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password)||
                string.IsNullOrWhiteSpace(checkpassword))
            {
                MessageBox.Show("Все поля должны быть заполнены!");
                return;
            }

            if (username.Length > 50 || password.Length > 50 || checkpassword.Length>50)
            {
                MessageBox.Show("Превышена максимальная длина данных");
                return;
            }

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Пожалуйста, введите корректный email адрес");
                return;
            }
            if (password != checkpassword)
            {
                MessageBox.Show("Пароли не совпадают");
                return;
            }
            bool isRegistered = dbHelper.RegisterUser(username, email, password);

            if (isRegistered)
            {
                MessageBox.Show("Регистрация успешна!");
            }
            
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}

public class DatabaseHelper
{
    private string connectionString = "Server=localhost;Database=FoodFest;Trusted_Connection=True;TrustServerCertificate=True;";

    public bool RegisterUser(string username, string email, string password)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string checkQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username OR Email = @Email";
                using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@Username", username);
                    checkCommand.Parameters.AddWithValue("@Email", email);

                    int existingUsers = (int)checkCommand.ExecuteScalar();
                    if (existingUsers > 0)
                    {
                        MessageBox.Show("Пользователь с таким именем или email уже существует");
                        return false;
                    }
                }

                string insertQuery = "INSERT INTO Users (Username, UPassword, Email, RoleID) VALUES (@Username, @Password, @Email, @RoleID)";
                using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                {
                    insertCommand.Parameters.AddWithValue("@Username", username);
                    insertCommand.Parameters.AddWithValue("@Password", password);
                    insertCommand.Parameters.AddWithValue("@Email", email);
                    insertCommand.Parameters.AddWithValue("@RoleID", 3);

                    int rowsAffected = insertCommand.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при регистрации: {ex.Message}");
            return false;
        }
    }
}
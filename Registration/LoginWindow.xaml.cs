using System;
using System.Data.SqlClient;
using System.Net.Http.Headers;
using System.Windows;

namespace Registration
{
    public partial class LoginWindow : Window
    {
        private string connectionString = "Server=localhost;Database=FoodFest;Trusted_Connection=True;TrustServerCertificate=True;";

        public LoginWindow()
        {
            InitializeComponent();
        }
        public bool Auth(string user, string password)
        {
            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                    SELECT COUNT(*) 
                    FROM Users 
                    WHERE Username LIKE @username + '%' 
                    AND UPassword LIKE @password + '%'";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@username", user);
                    command.Parameters.AddWithValue("@password", password);

                        int userCount = (int)command.ExecuteScalar();

                        if (userCount > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    
                }
            }
            catch (SqlException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    
        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = NameBox.Text;
            string password = PasswordBox.Password;
            Auth(username, password);
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите имя пользователя и пароль");
                return;
            }
            if (username.Length > 50 || password.Length > 50)
            {
                MessageBox.Show("Превышена максимальная длина данных");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                    SELECT COUNT(*) 
                    FROM Users 
                    WHERE Username LIKE @username + '%' 
                    AND UPassword LIKE @password + '%'";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);
                    int userCount = (int)command.ExecuteScalar();

                        if (userCount > 0)
                        {
                            MessageBox.Show("Успешный вход! Добро пожаловать, " + username,
                                          "Успех",
                                          MessageBoxButton.OK,
                                          MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Неверное имя пользователя или пароль",
                                          "Ошибка входа",
                                          MessageBoxButton.OK,
                                          MessageBoxImage.Warning);
                        }
                    }
                }
            
            catch (SqlException ex)
            {
                MessageBox.Show($"Ошибка базы данных: {ex.Message}",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неизвестная ошибка: {ex.Message}",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }
    }
}
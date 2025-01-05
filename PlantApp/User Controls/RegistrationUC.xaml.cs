using Npgsql;
using System.Windows;
using Brushes = System.Windows.Media.Brushes;
using UserControl = System.Windows.Controls.UserControl;


namespace PlantApp
{

	public partial class RegistrationUC : UserControl
	{
		DatabaseHelper db;
		public RegistrationUC()
		{
			InitializeComponent();
			db = new DatabaseHelper();
		}

		private void btnRegister_Click(object sender, RoutedEventArgs e)
		{
			var username = txtUsername.Text;
			var password = txtPassword.Password;
			var email = txtEmail.Text;

			if (username == string.Empty || password == string.Empty || email == string.Empty)
			{
				lblLogin.Content = "Fill the form";
				lblLogin.Visibility = Visibility.Visible;
				lblLogin.Foreground = Brushes.Red;
				return;
			}
			try
			{
				var sql = "INSERT INTO users VALUES (@Email, @Username, @Password);";
				var result = db.ExecuteNonQuery(sql, new { Email = email, Username = username, Password = password });

				if (result == 1)
				{
					lblLogin.Content = "Successful registration";
					lblLogin.Visibility = Visibility.Visible;
					lblLogin.Foreground = Brushes.Green;
				}
				else
				{
					lblLogin.Content = "Something went wrong";
					lblLogin.Visibility = Visibility.Visible;
					lblLogin.Foreground = Brushes.Red;
				}
			}
			catch (PostgresException ex)
			{
				HandlePostgresError(ex);
			}
			catch (Exception ex)
			{
				lblLogin.Content = "Something went wrong";
				lblLogin.Visibility = Visibility.Visible;
				lblLogin.Foreground = Brushes.Red;
			}

		}
		private void HandlePostgresError(PostgresException ex)
		{
			lblLogin.Visibility = Visibility.Visible;
			lblLogin.Foreground = Brushes.Red;
			switch (ex.SqlState)
			{
				case "23505":
					lblLogin.Content = "Email or username already registered";
					break;
				case "P0001":
					lblLogin.Content = "Wrong email format";
					break;
				default:
					lblLogin.Content = "Something went wrong";
					break;
			}
		}
	}
}

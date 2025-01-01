using PlantApp.Models;
using System.Windows.Controls;

namespace PlantApp
{

	public partial class LoginUC : UserControl
	{

		DatabaseHelper db;
		public LoginUC()
		{
			InitializeComponent();
			db = new DatabaseHelper();
		}

		private void btnLogin_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			var username = txtUsername.Text;
			var password = txtPassword.Password;

			if (username == string.Empty || password == string.Empty)
			{
				lblLogin.Content = "Fill the form";
				lblLogin.Visibility = System.Windows.Visibility.Visible;
				return;
			}
			try
			{


				var sql = "SELECT * FROM users WHERE username = @Username AND password = @Password";
				var users = db.ExecuteQuery<User>(sql, new { Username = username, Password = password }).ToList();

				if (users.Count == 1)
				{
					lblLogin.Content = "Uspješna prijava";
					lblLogin.Visibility = System.Windows.Visibility.Visible;
				}
				else
				{
					lblLogin.Content = "Wrong credentials";
					lblLogin.Visibility = System.Windows.Visibility.Visible;
				}
			}
			catch (Exception ex)
			{
				lblLogin.Content = "Something went wrong";
			}
		}
	}
}

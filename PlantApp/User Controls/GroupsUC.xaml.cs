using PlantApp.Models;
using System.Windows;
using UserControl = System.Windows.Controls.UserControl;

namespace PlantApp.User_Controls
{

	public partial class GroupsUC : UserControl
	{
		DatabaseHelper db;
		public GroupsUC()
		{
			db = new DatabaseHelper();
			InitializeComponent();
			LoadData();
		}

		private void LoadData()
		{
			var sql = "SELECT * FROM plant_groups WHERE owner = @Email;";
			var plantGroups = db.ExecuteQuery<PlantGroup>(sql, new { Authenticator.LoggedInUser.Email }).ToList();
			dtgGroups.ItemsSource = plantGroups;
		}

		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			var selectedGroup = dtgGroups.SelectedItem as PlantGroup;
			if (selectedGroup != null)
			{
				try
				{
					var sql = "DELETE FROM plant_groups WHERE id = @Id;";
					db.ExecuteNonQuery(sql, new { Id = selectedGroup.Id });
					LoadData();
				}
				catch (Exception ex)
				{
					lblMessage.Content = "Something went wrong";
					lblMessage.Visibility = System.Windows.Visibility.Visible;
				}
			}
		}

		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{
			String name = txtName.Text;
			String description = txtDescription.Text;

			if (name == string.Empty || description == string.Empty)
			{
				lblMessage.Content = "Fill the form";
				lblMessage.Visibility = System.Windows.Visibility.Visible;
				return;
			}
			try
			{
				var sql = "INSERT INTO plant_groups VALUES (Default, @Owner, @Name, @Description);";
				var result = db.ExecuteNonQuery(sql, new { Owner = Authenticator.LoggedInUser.Email, Name = name, Description = description });

				if (result == 1)
				{
					lblMessage.Visibility = System.Windows.Visibility.Collapsed;
					LoadData();
					RefreshForm();
				}
				else
				{
					lblMessage.Content = "Something went wrong";
					lblMessage.Visibility = System.Windows.Visibility.Visible;
				}
			}
			catch (Exception ex)
			{
				lblMessage.Content = "Something went wrong";
				lblMessage.Visibility = System.Windows.Visibility.Visible;
			}
		}

		private void RefreshForm()
		{
			txtName.Text = "";
			txtDescription.Text = "";
		}
	}
}

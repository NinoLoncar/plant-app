using Npgsql;
using PlantApp.Models;
using UserControl = System.Windows.Controls.UserControl;

namespace PlantApp.User_Controls
{
	public partial class PlantGroupMembersUC : UserControl
	{
		DatabaseHelper db;
		public PlantGroupMembersUC()
		{
			db = new DatabaseHelper();
			InitializeComponent();
			LoadData();
			LoadPlantData();
			LoadGroupData();
		}

		private void LoadData()
		{
			var sql = "SELECT plant_id Plant, plant_name PlantName, plant_group_id PlantGroup, group_name GroupName FROM get_plant_group_members_for_user(@Email);";
			var plantGroups = db.ExecuteQuery<PlantGroupMember>(sql, new { Authenticator.LoggedInUser.Email }).ToList();
			dtgGroupMembers.ItemsSource = plantGroups;
		}

		private void LoadGroupData()
		{
			var sql = "SELECT id, name, description FROM plant_groups WHERE owner = @Owner;";
			var groups = db.ExecuteQuery<PlantGroup>(sql, new { Owner = Authenticator.LoggedInUser.Email }).ToList();
			cbxGroups.ItemsSource = groups;
			if (groups.Count > 0)
			{
				cbxGroups.SelectedIndex = 0;
			}
		}

		private void LoadPlantData()
		{
			var sql = "SELECT id, species, name, date_added DateAdded FROM plants WHERE owner = @Owner;";
			var plants = db.ExecuteQuery<Plant>(sql, new { Owner = Authenticator.LoggedInUser.Email }).ToList();
			cbxPlants.ItemsSource = plants;
			if (plants.Count > 0)
			{
				cbxPlants.SelectedIndex = 0;
			}
		}

		private void btnDelete_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			var selectedMember = dtgGroupMembers.SelectedItem as PlantGroupMember;
			if (selectedMember != null)
			{
				try
				{
					var sql = "DELETE FROM plant_group_members WHERE plant = @Plant AND plant_group=@PlantGroup;";
					db.ExecuteNonQuery(sql, new { Plant = selectedMember.Plant, PlantGroup = selectedMember.PlantGroup });
					LoadData();
				}
				catch (Exception ex)
				{
					lblMessage.Content = "Something went wrong";
					lblMessage.Visibility = System.Windows.Visibility.Visible;
				}
			}
		}

		private void btnAdd_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			Plant? plant = cbxPlants.SelectedItem as Plant;
			PlantGroup? plantGroup = cbxGroups.SelectedItem as PlantGroup;
			if (plant == null || plantGroup == null)
			{
				lblMessage.Content = "Fill the form";
				lblMessage.Visibility = System.Windows.Visibility.Visible;
				return;
			}
			try
			{
				var sql = "INSERT INTO plant_group_members VALUES (@Plant, @Group);";
				var result = db.ExecuteNonQuery(sql, new { Plant = plant.Id, Group = plantGroup.Id });

				if (result == 1)
				{
					lblMessage.Visibility = System.Windows.Visibility.Collapsed;
					LoadData();
				}
				else
				{
					lblMessage.Content = "Something went wrong";
					lblMessage.Visibility = System.Windows.Visibility.Visible;
				}
			}
			catch (PostgresException ex)
			{
				switch (ex.SqlState)
				{
					case "23505":
						lblMessage.Content = "Member already exists";
						lblMessage.Visibility = System.Windows.Visibility.Visible;
						break;
					default:
						lblMessage.Content = "Something went wrong";
						lblMessage.Visibility = System.Windows.Visibility.Visible;
						break;
				}
			}
			catch (Exception ex)
			{
				lblMessage.Content = "Something went wrong";
				lblMessage.Visibility = System.Windows.Visibility.Visible;
			}
		}
	}

}

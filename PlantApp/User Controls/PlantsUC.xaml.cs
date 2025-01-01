using PlantApp.Models;
using System.Windows;
using UserControl = System.Windows.Controls.UserControl;

namespace PlantApp.User_Controls
{

	public partial class PlantsUC : UserControl
	{
		DatabaseHelper db;
		public PlantsUC()
		{
			db = new DatabaseHelper();
			InitializeComponent();
			LoadData();
			LoadSpeciesData();
		}

		private void LoadData()
		{
			var sql = "SELECT id, species, name, date_added DateAdded FROM plants WHERE owner = @Owner;";
			var plantSpecies = db.ExecuteQuery<Plant>(sql, new { Owner = Authenticator.LoggedInUser.Email }).ToList();
			dtgPlants.ItemsSource = plantSpecies;
		}
		private void LoadSpeciesData()
		{
			var sql = "SELECT id, name, watering_interval wateringInterval, required_water requiredWater FROM plant_species;";
			var plantSpecies = db.ExecuteQuery<PlantSpecies>(sql).ToList();
			cbxSpecies.ItemsSource = plantSpecies;
			if (plantSpecies.Count > 0)
			{
				cbxSpecies.SelectedIndex = 0;
			}
		}
		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			var selectedPlant = dtgPlants.SelectedItem as Plant;
			if (selectedPlant != null)
			{
				try
				{
					var sql = "DELETE FROM plants WHERE id = @Id;";
					db.ExecuteNonQuery(sql, new { Id = selectedPlant.Id });
					LoadData();
				}
				catch (Exception ex)
				{
					lblMessage.Content = "Something went wrong";
					lblMessage.Visibility = Visibility.Visible;
				}
			}
		}


		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{
			string name = txtName.Text;
			PlantSpecies? species = cbxSpecies.SelectedItem as PlantSpecies;
			if (name == string.Empty || species == null)
			{
				lblMessage.Content = "Fill the form";
				lblMessage.Visibility = Visibility.Visible;
				return;
			}
			try
			{
				var sql = "INSERT INTO plants VALUES (Default, @Owner, @Species, @Name, Default);";
				var result = db.ExecuteNonQuery(sql, new { Owner = Authenticator.LoggedInUser.Email, Species = species.Id, Name = name });

				if (result == 1)
				{
					lblMessage.Visibility = Visibility.Hidden;
					LoadData();
					RefreshForm();
				}
				else
				{
					lblMessage.Content = "Something went wrong";
					lblMessage.Visibility = Visibility.Visible;
				}
			}
			catch (Exception ex)
			{
				lblMessage.Content = "Something went wrong";
				lblMessage.Visibility = Visibility.Visible;
			}
		}

		private void RefreshForm()
		{
			txtName.Text = "";
		}
	}
}

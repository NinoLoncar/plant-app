using PlantApp.Models;
using System.Windows;
using System.Windows.Controls;

namespace PlantApp.User_Controls
{

	public partial class PlantSpeciesUC : UserControl
	{
		DatabaseHelper db;
		public PlantSpeciesUC()
		{
			db = new DatabaseHelper();
			InitializeComponent();
			LoadData();

		}

		private void LoadData()
		{
			var sql = "SELECT id, name, watering_interval wateringInterval, required_water requiredWater FROM plant_species;";
			var plantSpecies = db.ExecuteQuery<PlantSpecies>(sql).ToList();
			dtgPlantSpecies.ItemsSource = plantSpecies;
		}

		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{
			string name = txtName.Text;
			if (name == string.Empty)
			{
				lblMessage.Content = "Fill the form";
				lblMessage.Visibility = Visibility.Visible;
				return;
			}
			TimeSpan? wateringInterval = tmsWateringInterval.Value;
			Decimal? requiredWater = decRequiredWater.Value;

			try
			{
				var sql = "INSERT INTO plant_species VALUES (Default, @Name, @WateringInterval, @RequiredWater);";
				var result = db.ExecuteNonQuery(sql, new { Name = name, WateringInterval = wateringInterval, RequiredWater = requiredWater });

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
			decRequiredWater.Value = (decimal?)0.1;
		}

		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			var selectedSpecies = dtgPlantSpecies.SelectedItem as PlantSpecies;
			if (selectedSpecies != null)
			{
				try
				{
					var sql = "DELETE FROM plant_species WHERE id = @Id;";
					db.ExecuteNonQuery(sql, new { Id = selectedSpecies.Id });
					LoadData();
				}
				catch (Exception ex)
				{
					lblMessage.Content = "Something went wrong";
					lblMessage.Visibility = Visibility.Visible;
				}
			}

		}

	}
}


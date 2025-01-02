using Npgsql;
using PlantApp.Models;
using System.Windows;
using UserControl = System.Windows.Controls.UserControl;

namespace PlantApp.User_Controls
{

	public partial class PlantDieseaseCasesUC : UserControl
	{
		DatabaseHelper db;
		public PlantDieseaseCasesUC()
		{
			db = new DatabaseHelper();
			InitializeComponent();
			dtpInfectedAt.Maximum = DateTime.Now;
			dtpInfectedAt.Value = DateTime.Now;
			LoadData();
			LoadPlantData();
			LoadDiseases();
		}

		private void LoadData()
		{
			var sql = "SELECT plant Plant, plant_name PlantName, disease Disease, disease_name DiseaseName, infected_at InfectedAt, recovered Recovered, recovered_at RecoveredAt  FROM get_plant_disease_cases_for_user(@Email);";
			var diseaseCases = db.ExecuteQuery<PlantDiseaseCases>(sql, new { Authenticator.LoggedInUser.Email }).ToList();
			dtgDiseaseCases.ItemsSource = diseaseCases;
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

		private void LoadDiseases()
		{
			var sql = "SELECT * FROM plant_diseases;";
			var diseases = db.ExecuteQuery<PlantDisease>(sql, new { Owner = Authenticator.LoggedInUser.Email }).ToList();
			cbxDiseases.ItemsSource = diseases;
			if (diseases.Count > 0)
			{
				cbxDiseases.SelectedIndex = 0;
			}
		}
		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			var selectedCase = dtgDiseaseCases.SelectedItem as PlantDiseaseCases;
			if (selectedCase != null)
			{
				try
				{
					var sql = "DELETE FROM plant_disease_cases WHERE plant = @Plant AND plant_disease=@Disease;";
					db.ExecuteNonQuery(sql, new { Plant = selectedCase.Plant, Disease = selectedCase.Disease });
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
			Plant? plant = cbxPlants.SelectedItem as Plant;
			PlantDisease? plantDisease = cbxDiseases.SelectedItem as PlantDisease;
			DateTime? infectedAt = dtpInfectedAt.Value;
			if (plant == null || plantDisease == null || infectedAt == null)
			{
				lblMessage.Content = "Fill the form";
				lblMessage.Visibility = System.Windows.Visibility.Visible;
				return;
			}
			try
			{
				var sql = "INSERT INTO plant_disease_cases VALUES (@Plant, @Disease, @InfectedAt, DEFAULT);";
				var result = db.ExecuteNonQuery(sql, new { Plant = plant.Id, Disease = plantDisease.Id, InfectedAt = infectedAt });

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

		private void btnRecover_Click(object sender, RoutedEventArgs e)
		{
			var selectedCase = dtgDiseaseCases.SelectedItem as PlantDiseaseCases;
			if (selectedCase == null)
			{
				lblMessage.Content = "Select disease case";
				lblMessage.Visibility = System.Windows.Visibility.Visible;
				return;
			}
			if (selectedCase.Recovered)
			{
				lblMessage.Content = "Plant already recovered";
				lblMessage.Visibility = System.Windows.Visibility.Visible;
				return;
			}
			try
			{
				var sql = "UPDATE plant_disease_cases SET recovered = TRUE, recovered_at = CURRENT_TIMESTAMP WHERE plant = @Plant AND plant_disease = @Disease;";
				db.ExecuteNonQuery(sql, new { Plant = selectedCase.Plant, Disease = selectedCase.Disease });
				LoadData();
			}
			catch (Exception ex)
			{
				lblMessage.Content = "Something went wrong";
				lblMessage.Visibility = System.Windows.Visibility.Visible;
			}

		}
	}

}

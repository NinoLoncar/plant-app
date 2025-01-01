using PlantApp.Models;
using System.Windows;
using UserControl = System.Windows.Controls.UserControl;

namespace PlantApp.User_Controls
{
	public partial class WateringReportUC : UserControl
	{
		DatabaseHelper db;
		public WateringReportUC()
		{
			db = new DatabaseHelper();
			InitializeComponent();
			dtpWateredAt.Maximum = DateTime.Now;
			dtpWateredAt.Value = DateTime.Now;
			LoadData();
			LoadPlantData();
		}

		private void LoadData()
		{
			var sql = "SELECT id, watered_plant WateredPlant, watered_at WateredAt, water_amount WaterAmount FROM get_watering_reports_for_user(@Email)";
			var wateringReports = db.ExecuteQuery<WateringReport>(sql, new { Email = Authenticator.LoggedInUser.Email }).ToList();
			dtgWateringReports.ItemsSource = wateringReports;
		}
		private void LoadPlantData()
		{
			var sql = "SELECT id, species, name, date_added DateAdded FROM plants WHERE owner = @Owner;";
			var plantSpecies = db.ExecuteQuery<Plant>(sql, new { Owner = Authenticator.LoggedInUser.Email }).ToList();
			cbxPlants.ItemsSource = plantSpecies;
			if (plantSpecies.Count > 0)
			{
				cbxPlants.SelectedIndex = 0;
			}
		}
		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			var selectedWateringReport = dtgWateringReports.SelectedItem as WateringReport;
			if (selectedWateringReport != null)
			{
				try
				{
					var sql = "DELETE FROM watering_reports WHERE id = @Id;";
					db.ExecuteNonQuery(sql, new { Id = selectedWateringReport.Id });
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
			DateTime? wateredAt = dtpWateredAt.Value;
			Plant? wateredPlant = cbxPlants.SelectedItem as Plant;
			Decimal? waterAmount = decWater.Value;

			if (wateredAt == null || wateredPlant == null || waterAmount == null)
			{
				lblMessage.Content = "Fill the form";
				lblMessage.Visibility = Visibility.Visible;
				return;
			}
			try
			{
				var sql = "INSERT INTO watering_reports VALUES (Default, @Plant, @WateredAt, @WaterAmount);";
				var result = db.ExecuteNonQuery(sql, new { Plant = wateredPlant.Id, WateredAt = wateredAt, WaterAmount = waterAmount });

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
			cbxPlants.SelectedIndex = 0;
			dtpWateredAt.Value = dtpWateredAt.Maximum;
			decWater.Value = (decimal)0.1;
		}
	}
}

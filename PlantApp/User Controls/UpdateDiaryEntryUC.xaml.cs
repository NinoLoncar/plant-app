using PlantApp.Models;
using System.Windows;
using UserControl = System.Windows.Controls.UserControl;

namespace PlantApp.User_Controls
{
	public partial class UpdateDiaryEntryUC : UserControl
	{
		DatabaseHelper db;
		DiaryEntry diaryEntry;
		public UpdateDiaryEntryUC(DiaryEntry diaryEntry)
		{
			db = new DatabaseHelper();
			this.diaryEntry = diaryEntry;
			InitializeComponent();
			txtContent.Text = diaryEntry.Content;
			txtTitle.Text = diaryEntry.Title;
			LoadPlantData();

		}

		private void LoadPlantData()
		{
			var sql = "SELECT id, species, name, date_added DateAdded FROM plants WHERE owner = @Owner;";
			var plantSpecies = db.ExecuteQuery<Plant>(sql, new { Owner = Authenticator.LoggedInUser.Email }).ToList();
			cbxPlants.ItemsSource = plantSpecies;
			if (plantSpecies.Count > 0)
			{
				var selectedPlant = plantSpecies.FirstOrDefault(p => p.Id == diaryEntry.Plant);
				if (selectedPlant != null)
				{
					cbxPlants.SelectedItem = selectedPlant;
				}
				else
				{
					cbxPlants.SelectedIndex = 0;
				}
			}
		}

		private void btnUpdate_Click(object sender, RoutedEventArgs e)
		{
			Plant? plant = cbxPlants.SelectedItem as Plant;
			String title = txtTitle.Text;
			String content = txtContent.Text;

			if (plant == null || title == string.Empty || content == string.Empty)
			{
				lblMessage.Content = "Fill the form";
				lblMessage.Visibility = Visibility.Visible;
				return;
			}
			try
			{
				var sql = "UPDATE diary_entries SET title=@Title, content=@Content, plant=@Plant WHERE id = @Id;";
				var result = db.ExecuteNonQuery(sql, new { Plant = plant.Id, Title = title, Content = content, Id = diaryEntry.Id });

				if (result == 1)
				{
					var parentWindow = Window.GetWindow(this) as MainWindow;
					if (parentWindow != null)
					{
						parentWindow.contentControl.Content = new DiaryEntriesUC();
					}
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

		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			var parentWindow = Window.GetWindow(this) as MainWindow;
			if (parentWindow != null)
			{
				parentWindow.contentControl.Content = new DiaryEntriesUC();
			}
		}
	}
}

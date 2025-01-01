using PlantApp.Models;
using System.Windows;
using System.Windows.Controls;
namespace PlantApp.User_Controls
{
	/// <summary>
	/// Interaction logic for DiaryEntriesUC.xaml
	/// </summary>
	public partial class DiaryEntriesUC : UserControl
	{
		DatabaseHelper db;
		public DiaryEntriesUC()
		{
			db = new DatabaseHelper();
			InitializeComponent();
			LoadData();
			LoadPlantData();
		}

		private void LoadData()
		{
			var sql = "SELECT id, plant, title, content, added_at AddedAt, last_edited_at LastEditedAt  FROM get_diary_entries_for_user(@Email);";
			var diaryEntries = db.ExecuteQuery<DiaryEntry>(sql, new { Email = Authenticator.LoggedInUser.Email }).ToList();
			dtgDiaryEntries.ItemsSource = diaryEntries;
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
			var selectedDiaryEntry = dtgDiaryEntries.SelectedItem as DiaryEntry;
			if (selectedDiaryEntry != null)
			{
				try
				{
					var sql = "DELETE FROM diary_entries WHERE id = @Id;";
					db.ExecuteNonQuery(sql, new { Id = selectedDiaryEntry.Id });
					LoadData();
				}
				catch (Exception ex)
				{
					lblMessage.Content = "Something went wrong";
					lblMessage.Visibility = Visibility.Visible;
				}
			}
		}

		private void btnAdd_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			Plant? wateredPlant = cbxPlants.SelectedItem as Plant;
			String title = txtTitle.Text;
			String content = txtContent.Text;

			if (wateredPlant == null || title == string.Empty || content == string.Empty)
			{
				lblMessage.Content = "Fill the form";
				lblMessage.Visibility = Visibility.Visible;
				return;
			}
			try
			{
				var sql = "INSERT INTO diary_entries VALUES (Default, @Plant, @Title, @Content, DEFAULT, DEFAULT);";
				var result = db.ExecuteNonQuery(sql, new { Plant = wateredPlant.Id, Title = title, Content = content });

				if (result == 1)
				{
					lblMessage.Visibility = Visibility.Collapsed;
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
			txtTitle.Text = "";
			txtContent.Text = "";
		}
	}
}

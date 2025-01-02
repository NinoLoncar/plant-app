using Npgsql;
using PlantApp.Models;
using System.Diagnostics;
using System.IO;
using System.Windows;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using UserControl = System.Windows.Controls.UserControl;
namespace PlantApp.User_Controls
{
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
			var sql = "SELECT id, plant, title, content, added_at AddedAt, last_edited_at LastEditedAt, image_count ImageCount  FROM get_diary_entries_for_user(@Email);";
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

		private void btnAddImage_Click(object sender, RoutedEventArgs e)
		{
			var selectedDiaryEntry = dtgDiaryEntries.SelectedItem as DiaryEntry;
			if (selectedDiaryEntry == null)
			{
				lblMessage.Content = "Select diary entry";
				lblMessage.Visibility = Visibility.Visible;
				return;
			}
			string? imagePath = OpenImageFileExplorer();
			if (!string.IsNullOrEmpty(imagePath))
			{
				try
				{
					string imageExtension = Path.GetExtension(imagePath)?.TrimStart('.').ToLower();
					lblMessage.Visibility = Visibility.Collapsed;
					var sql = "INSERT INTO images VALUES(DEFAULT, @DiaryEntry, DEFAULT, lo_import(@ImagePath), @Type);";
					var result = db.ExecuteNonQuery(sql, new { DiaryEntry = selectedDiaryEntry.Id, ImagePath = imagePath, Type = imageExtension });
					if (result != 1)
					{
						lblMessage.Content = "Something went wrong";
						lblMessage.Visibility = Visibility.Visible;
					}
					LoadData();
				}
				catch (PostgresException ex)
				{
					switch (ex.SqlState)
					{
						case "42501":
							lblMessage.Content = "Permission denied (try desktop)";
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
					{
						lblMessage.Content = "Something went wrong";
						lblMessage.Visibility = Visibility.Visible;
					}
				}

			}
		}
		private string? OpenImageFileExplorer()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;";

			if (openFileDialog.ShowDialog() == true)
			{
				return openFileDialog.FileName;
			}

			return null;
		}

		private void btnExportImages_Click(object sender, RoutedEventArgs e)
		{
			var selectedDiaryEntry = dtgDiaryEntries.SelectedItem as DiaryEntry;
			if (selectedDiaryEntry == null)
			{
				lblMessage.Content = "Select diary entry";
				lblMessage.Visibility = Visibility.Visible;
				return;
			}
			string folderPath = OpenFolderExplorer();
			if (!string.IsNullOrEmpty(folderPath))
			{
				try
				{
					var sql = "SELECT export_images_for_diary_entry(@DiaryEntry, @FolderPath);";
					db.ExecuteNonQuery(sql, new { DiaryEntry = selectedDiaryEntry.Id, FolderPath = folderPath });
					Process.Start("explorer.exe", folderPath);

				}
				catch (PostgresException ex)
				{
					switch (ex.SqlState)
					{
						case "42501":
							lblMessage.Content = "Permission denied (try desktop)";
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
					lblMessage.Visibility = Visibility.Visible;
				}
			}
		}

		private string? OpenFolderExplorer()
		{

			var folderDialog = new FolderBrowserDialog();

			if (folderDialog.ShowDialog() == DialogResult.OK)
			{
				return folderDialog.SelectedPath;
			}

			return null;
		}

		private void btnUpdate_Click(object sender, RoutedEventArgs e)
		{
			var selectedDiaryEntry = dtgDiaryEntries.SelectedItem as DiaryEntry;
			if (selectedDiaryEntry == null)
			{
				lblMessage.Content = "Select diary entry";
				lblMessage.Visibility = Visibility.Visible;
				return;
			}
			var parentWindow = Window.GetWindow(this) as MainWindow;
			if (parentWindow != null)
			{
				parentWindow.contentControl.Content = new UpdateDiaryEntryUC(selectedDiaryEntry);
			}
		}
	}
}

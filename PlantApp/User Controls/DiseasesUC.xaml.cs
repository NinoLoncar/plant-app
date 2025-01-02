using PlantApp.Models;
using UserControl = System.Windows.Controls.UserControl;

namespace PlantApp.User_Controls
{

	public partial class DiseasesUC : UserControl
	{
		DatabaseHelper db;
		public DiseasesUC()
		{
			db = new DatabaseHelper();
			InitializeComponent();
			LoadData();
		}

		private void LoadData()
		{
			var sql = "SELECT * FROM plant_diseases;";
			var diseases = db.ExecuteQuery<PlantDiesease>(sql).ToList();
			dtgDiseases.ItemsSource = diseases;
		}

		private void btnDelete_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			var selectedDisease = dtgDiseases.SelectedItem as PlantDiesease;
			if (selectedDisease != null)
			{
				try
				{
					var sql = "DELETE FROM plant_diseases WHERE id = @Id;";
					db.ExecuteNonQuery(sql, new { Id = selectedDisease.Id });
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
				var sql = "INSERT INTO plant_diseases VALUES (Default, @Name, @Description);";
				var result = db.ExecuteNonQuery(sql, new { Name = name, Description = description });

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

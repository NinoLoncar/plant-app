using PlantApp.User_Controls;
using System.Windows;

namespace PlantApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			contentControl.Content = new NotificationsUC();
		}

		private void btnNotifications_Click(object sender, RoutedEventArgs e)
		{
			contentControl.Content = new NotificationsUC();
		}

		private void btnPlantSpecies_Click(object sender, RoutedEventArgs e)
		{
			contentControl.Content = new PlantSpeciesUC();
		}

		private void btnPlants_Click(object sender, RoutedEventArgs e)
		{
			contentControl.Content = new PlantsUC();
		}

		private void btnWateringReports_Click(object sender, RoutedEventArgs e)
		{
			contentControl.Content = new WateringReportUC();
		}
	}
}

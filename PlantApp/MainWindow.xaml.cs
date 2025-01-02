using PlantApp.User_Controls;
using System.Windows;

namespace PlantApp
{
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

		private void btnDiaryEntries_Click(object sender, RoutedEventArgs e)
		{
			contentControl.Content = new DiaryEntriesUC();
		}

		private void btnDiseases_Click(object sender, RoutedEventArgs e)
		{
			contentControl.Content = new DiseasesUC();
		}

		private void btnGroups_Click(object sender, RoutedEventArgs e)
		{
			contentControl.Content = new GroupsUC();
		}

		private void btnGroupMembers_Click(object sender, RoutedEventArgs e)
		{
			contentControl.Content = new PlantGroupMembersUC();
		}

		private void btnDiseaseCases_Click(object sender, RoutedEventArgs e)
		{
			contentControl.Content = new PlantDieseaseCasesUC();
		}
	}
}

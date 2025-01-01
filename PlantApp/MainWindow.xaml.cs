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
	}
}

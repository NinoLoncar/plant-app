using PlantApp.Models;
using System.Windows.Controls;

namespace PlantApp.User_Controls
{
	public partial class NotificationsUC : UserControl
	{
		DatabaseHelper db;
		public NotificationsUC()
		{
			InitializeComponent();
			db = new DatabaseHelper();
			LoadData();
		}

		private void LoadData()
		{
			var sql = "SELECT message, created_at createdAt FROM notifications WHERE receiver = @Email;";
			var notifications = db.ExecuteQuery<Notification>(sql, new { Email = Authenticator.LoggedInUser.Email }).ToList();
			dtgNotifications.ItemsSource = notifications;
		}
	}
}

﻿using PlantApp.Models;
using UserControl = System.Windows.Controls.UserControl;

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
			var sql = "SELECT message, created_at createdAt FROM notifications WHERE receiver = @Email ORDER BY created_at DESC;";
			var notifications = db.ExecuteQuery<Notification>(sql, new { Email = Authenticator.LoggedInUser.Email }).ToList();
			dtgNotifications.ItemsSource = notifications;
		}
	}
}

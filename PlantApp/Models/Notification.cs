using System.ComponentModel.DataAnnotations.Schema;

namespace PlantApp.Models
{
	class Notification
	{
		public string Message { get; set; }

		[Column("created_at")]
		public DateTime? CreatedAt { get; set; }
	}
}

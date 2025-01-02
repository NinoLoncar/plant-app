namespace PlantApp.Models
{
	public class DiaryEntry
	{
		public int Id { get; set; }

		public int Plant { get; set; }

		public string Title { get; set; }

		public string Content { get; set; }

		public DateTime AddedAt { get; set; }

		public DateTime LastEditedAt { get; set; }

		public int ImageCount { get; set; }
	}
}

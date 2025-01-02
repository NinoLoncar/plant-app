namespace PlantApp.Models
{
	internal class PlantDiseaseCases
	{
		public int Plant { get; set; }
		public string PlantName { get; set; }
		public int Disease { get; set; }
		public string DiseaseName { get; set; }
		public DateTime InfectedAt { get; set; }
		public bool Recovered { get; set; }
		public DateTime? RecoveredAt { get; set; }
	}
}

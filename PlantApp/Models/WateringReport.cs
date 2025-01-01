namespace PlantApp.Models
{
	public class WateringReport
	{
		public int Id { get; set; }
		public int WateredPlant { get; set; }
		public DateTime WateredAt { get; set; }
		public decimal WaterAmount { get; set; }
	}

}

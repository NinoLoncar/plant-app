namespace PlantApp.Models
{
	class PlantSpecies
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public TimeSpan WateringInterval { get; set; }

		public decimal RequiredWater { get; set; }

		public override string ToString()
		{
			return Name;
		}
	}
}

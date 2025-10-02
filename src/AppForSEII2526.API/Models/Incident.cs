namespace AppForSEII2526.API.Models
{

	[PrimaryKey(nameof(Id))]
	public class Incident
	{
		public string Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime DateOfldentification { get; set; }
		public string Exercise { get; set; }

	}
}

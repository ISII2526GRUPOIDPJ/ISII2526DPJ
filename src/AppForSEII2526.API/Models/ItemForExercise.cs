namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(Id))]
    public class ItemForExercise
	{
		public string Id { get; set; }
		public string Location { get; set; }
		public Item Item { get; set; }
        public IList<IncidentItem> IncidentItems { get; set; }
    }
}
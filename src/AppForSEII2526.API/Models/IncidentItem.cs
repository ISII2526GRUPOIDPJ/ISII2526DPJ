namespace AppForSEII2526.API.Models
{ 

[PrimaryKey(nameof(IncidentId), nameof(ItemId))]
public class IncidentItem
{
	public int IncidentId { get; set; }
	public int ItemId { get; set; }
	public Incident Incident { get; set; }
} }

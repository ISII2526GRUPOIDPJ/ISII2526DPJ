namespace AppForSEII2526.API.Models
{ 

[PrimaryKey(nameof(IncidentId), nameof(ItemForExerciseId))]
public class IncidentItem
{
	public int IncidentId { get; set; }
	public int ItemForExerciseId { get; set; }
	public Incident Incident { get; set; }

	public ItemForExercise ItemForExercise { get; set; }
        
	public IncidentPriority IncidentPriority { get; set; }
    } }

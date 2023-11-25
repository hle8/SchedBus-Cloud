namespace SchedBus.Models;

/* The Plan class represents each plan.
 * Property:
 * - ID               | uniquely identify each plan
 * - Destination      | destination stored in a plan
 * - Label            | a plan's name
 * - MaxRequestedTrip | the maximum number of requested trip
 * - Notification     | notification switch
 * - Vibration        | vibration switch
 * - TimeSet          | set time for a plan
 */
public class Plan
{
    public Guid ID { get; set; }
    public Destination? Destination { get; set; }
    public string? Label { get; set; }
    public uint MaxRequestedTrip { get; set; }
    public bool Notification { get; set; }
    public bool Vibration { get; set; }
    public List<TimeSet>? TimeSet { get; set; }
}

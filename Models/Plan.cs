namespace SchedBus.Models;

/* The Plan class represents each plan.
 * Property:
 * - Destination      | location's destination of a plan
 * - Label            | a plan's name
 * - MaxRequestedTrip | the maximum number of requested trip
 * - Notification     | notification switch
 * - Vibration        | vibration switch
 * - TimeSet          | set time for a plan
 */
public class Plan
{
    public string Destination { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public int MaxRequestedTrip { get; set; }
    public bool Notification {  get; set; }
    public bool Vibration { get; set; }
    public TimeSet TimeSet { get; set; } = new TimeSet();
}

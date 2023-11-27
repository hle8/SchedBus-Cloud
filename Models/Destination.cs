namespace SchedBus.Models;

/* The Destination class represents the plan destination.
 * Property:
 * - ID         | uniquely identify each destination
 * - Name       | The location that users want to go or save
 * - Longtitude	| The location logntitude 
 * - Latitude	| The location latitude
 */
public class Destination
{
	public uint ID { get; set; }
	public string? Name { get; set; }
	public double Longitude { get; set; }
	public double Latitude { get; set; }
}

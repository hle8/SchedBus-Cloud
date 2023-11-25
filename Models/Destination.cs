namespace SchedBus.Models;

/* The Destination class represent the plan destination.
	*	Property:
	*	- Name			| The location that users want to go or save
	*	- Longtitude	| The location logntitude 
	*	- Latitude		| The location latitude

*/

public class Destination
{
	public string Name{ get; set; }
	
	public double Longitude { get; set; }

	public double Latitude { get; set; }
}

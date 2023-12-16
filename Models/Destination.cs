using SQLite;

namespace SchedBus.Models;

/* The Destination class represents the plan destination.
 * Property:
 * - Id         | uniquely identify each destination
 * - Name		| The location that users want to go or save
 * - Longtitude	| The location logntitude 
 * - Latitude	  | The location latitude
 */

public class Destination
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string? Name { get; set; }
    public string? Address { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }

    [Ignore]
    public Location? Location { get; set; }
}

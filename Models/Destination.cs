using SQLite;

namespace SchedBus.Models;

/* The Destination class represent the plan destination.
 * Property:
 * - Id         | uniquely identify each destination
 * - Name		| The location that users want to go or save
 * - Longtitude	| The location logntitude 
 * - Latitude	| The location latitude
 */

// [Table("destination")]
public class Destination
{
    // [PrimaryKey, AutoIncrement]
    // public uint Id { get; set; }
    public string Name { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
}

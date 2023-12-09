using SQLite;

namespace SchedBus.Models;

/* The Plan class represents each plan.
 * Property:
 * - Id                | uniquely identify each plan
 * - Label             | a plan's name
 * - MaxNumberOfRoutes | the maximum number of requested routes
 * - Notification      | notification switch
 * - Vibration         | vibration switch
 */

[Table("Plans")]
public class Plan
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [MaxLength(250), Unique]
    public string Label { get; set; }

    [Indexed]
    public int DestinationId { get; set; }

    public int MaxNumberOfRoutes { get; set; }
    public bool Notification { get; set; }
    public bool Vibration { get; set; }
}
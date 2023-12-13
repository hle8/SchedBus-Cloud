using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.ObjectModel;

namespace SchedBus.Models;

/* The Plan class represents each plan.
 * Property:
 * - Id                | uniquely identify each plan
 * - Label             | a plan's name
 * - MaxNumberOfRoutes | the maximum number of requested routes
 * - Notification      | notification switch
 * - Vibration         | vibration switch
 */

public class Plan
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string? Label { get; set; }
    public int MaxNumberOfRoutes { get; set; }
    public bool Notification { get; set; }
    public bool Vibration { get; set; }

    [ForeignKey(typeof(Destination))]
    public int DestinationId { get; set; }

    [OneToOne(CascadeOperations = CascadeOperation.CascadeRead | CascadeOperation.CascadeInsert | CascadeOperation.CascadeDelete)]
    public Destination Destination { get; set; }

    [OneToMany(CascadeOperations = CascadeOperation.CascadeRead | CascadeOperation.CascadeInsert | CascadeOperation.CascadeDelete)]
    public ObservableCollection<TimeSet> TimeSets { get; set; }
}
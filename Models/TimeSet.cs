namespace SchedBus.Models;

/* The TimeSet class represents each timeset for each plan
   * Property:
   * - ID                | uniquely identify each timeset
   * - Hour              | user's input hour
   * - Minute            | user's input minute
   * - RepeatOnMonday    | is the set time repeated on Monday
   * - RepeatOnTuesday   | is the set time repeated on Tuesday
   * - RepeatOnWednesday | is the set time repeated on Wednesday
   * - RepeatOnThursday  | is the set time repeated on Thursday
   * - RepeatOnFriday    | is the set time repeated on Friday
   * - RepeatOnSaturday  | is the set time repeated on Saturday
   * - RepeatOnSunday    | is the set time repeated on Sunday
   */
public class TimeSet
{
    public Guid ID { get; set; }
    public uint Hour { get; set; }
    public uint Minute { get; set; }
    public bool IsAM { get; set; }
    public bool RepeatOnMonday { get; set; }
    public bool RepeatOnTuesday { get; set; }
    public bool RepeatOnWednesday { get; set; }
    public bool RepeatOnThursday { get; set; }
    public bool RepeatOnFriday { get; set; }
    public bool RepeatOnSaturday { get; set; }
    public bool RepeatOnSunday { get; set; }
}

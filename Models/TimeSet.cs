namespace SchedBus.Models;

/* The TimeSet class represents each timeset for each plan
   * Property:
   * - SetTime             | user's input time
   * - Enabled             | is the set time enabled
   * - RepeatedOnMonday    | is the set time repeated on Monday
   * - RepeatedOnTuesday   | is the set time repeated on Tuesday
   * - RepeatedOnWednesday | is the set time repeated on Wednesday
   * - RepeatedOnThursday  | is the set time repeated on Thursday
   * - RepeatedOnFriday    | is the set time repeated on Friday
   * - RepeatedOnSaturday  | is the set time repeated on Saturday
   * - RepeatedOnSunday    | is the set time repeated on Sunday
   */
public class TimeSet
{
    public TimeSpan SetTime { get; set; }
    public bool IsEnabled { get; set; }
    public bool RepeatedOnMonday { get; set; }
    public bool RepeatedOnTuesday { get; set; }
    public bool RepeatedOnWednesday { get; set; }
    public bool RepeatedOnThursday { get; set; }
    public bool RepeatedOnFriday { get; set; }
    public bool RepeatedOnSaturday { get; set; }
    public bool RepeatedOnSunday { get; set; }
}

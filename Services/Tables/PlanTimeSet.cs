using SQLite;

namespace SchedBus.Services.Tables;

[Table("PlanTimeSet")]
public class PlanTimeSet
{
    [PrimaryKey, AutoIncrement]
    [Column("id")]
    public int Id { get; set; }

    [Indexed]
    [Column("plan_id")]
    public int PlanId { get; set; }

    [Indexed]
    [Column("timeset_id")]
    public int TimeSetId { get; set; }
}

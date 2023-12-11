using SchedBus.Models;
using SchedBus.Services.Tables;
using SQLite;
using System.Collections.ObjectModel;

namespace SchedBus.Services;

public class PlanSQLiteService
{
    static PlanSQLiteService _instance;
    public static PlanSQLiteService Instance => _instance ??= new PlanSQLiteService();

    SQLiteAsyncConnection _database;

    const string DatabaseFilename = "SchedBus.db";

    const SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLiteOpenFlags.SharedCache;

    static string DatabasePath => Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);

    private PlanSQLiteService()
    {
    }

    async Task Init()
    {
        if (_database == null)
        {
            _database = new SQLiteAsyncConnection(DatabasePath, Flags);

            await _database.CreateTableAsync<Plan>();
            await _database.CreateTableAsync<Destination>();
            await _database.CreateTableAsync<TimeSet>();
            await _database.CreateTableAsync<PlanTimeSet>();

            SampleData();
        }
    }

    async void SampleData()
    {
        // Clear all tables
        await _database.DeleteAllAsync<Plan>();
        await _database.DeleteAllAsync<Destination>();
        await _database.DeleteAllAsync<TimeSet>();
        await _database.DeleteAllAsync<PlanTimeSet>();

        var random = new Random();
        var start = TimeSpan.FromHours(0);
        var end = TimeSpan.FromHours(23);
        var maxMinutes = (int)((end - start).TotalMinutes);

        for (int i = 1; i <= 5; i++)
        {
            // Insert Destination
            // await _database.InsertAsync(new Destination() { Name = $"Destination {i}" });
            // var destId = await _database.Table<Destination>().Where(i => i.Name == $"Destination {i}").FirstOrDefaultAsync();

            // Insert Plan
            var plan = new Plan() { Label = $"Plan {i}", DestinationId = i, MaxNumberOfRoutes = i, Notification = i % 2 == 0, Vibration = i % 2 == 0 };
            await _database.InsertAsync(plan);

            // Insert TimeSet
            var minute = random.Next(maxMinutes);
            var timeset = new TimeSet() { IsEnabled = i % 2 == 0, Time = start.Add(TimeSpan.FromMinutes(minute)) };
            await _database.InsertAsync(timeset);
        }

        var planList = await _database.Table<Plan>().ToListAsync();
        var timesetList = await _database.Table<TimeSet>().ToListAsync();

        // Insert relationship
        foreach (var i in planList)
        {
            foreach (var j in timesetList)
            {
                await _database.InsertAsync(new PlanTimeSet() { PlanId = i.Id, TimeSetId = j.Id });
            }
        }
    }

    // Get records
    public async Task<ObservableCollection<T>> GetDbAsync<T>() where T : new()
    {
        await Init();
        var result = await _database.Table<T>().ToListAsync();
        return new ObservableCollection<T>(result);
    }

    public async Task<T> GetDbAsync<T>(int id) where T : IEntity, new()
    {
        await Init();
        return await _database.Table<T>().Where(i => i.Id == id).FirstOrDefaultAsync();
    }

    // Delete records
    public async Task<int> DeleteDbAsync<T>(T item) where T : new()
    {
        await Init();
        return await _database.DeleteAsync(item);
    }

    // Save a plan
    public async Task<int> SavePlanAsync(Plan plan)
    {
        await Init();

        if (plan.Id != 0)
        {
            return await _database.UpdateAsync(plan);
        }
        else
        {
            return await _database.InsertAsync(plan);
        }
    }

    // Get timeset
    public async Task<ObservableCollection<TimeSet>> GetTimeSetOfPlanAsync(int planId)
    {
        await Init();

        var list = new ObservableCollection<TimeSet>();

        var rec = await _database.Table<PlanTimeSet>().Where(i => i.PlanId == planId).ToListAsync();

        if (rec.Count > 0)
        {
            foreach (var t in rec)
            {
                list.Add(await _database.Table<TimeSet>().Where(i => i.Id == t.TimeSetId).FirstOrDefaultAsync());
            }

            return list;
        }
        else { return list; }
    }

    // Save a timeset
    public async Task<int> SaveTimeSetAsync(TimeSet timeset, int planId)
    {
        await Init();

        // Get existing record
        var rec = await _database.Table<TimeSet>().Where(i => (i.IsEnabled == timeset.IsEnabled) && (i.Time == timeset.Time) && (i.RepeatedOnMonday == timeset.RepeatedOnMonday) && (i.RepeatedOnTuesday == timeset.RepeatedOnTuesday) && (i.RepeatedOnWednesday == timeset.RepeatedOnWednesday) && (i.RepeatedOnThursday == timeset.RepeatedOnThursday) && (i.RepeatedOnFriday == timeset.RepeatedOnFriday) && (i.RepeatedOnSaturday == timeset.RepeatedOnSunday) && (i.RepeatedOnSunday == timeset.RepeatedOnSunday)).FirstOrDefaultAsync();

        // If editing TimeSet
        if (timeset.Id != 0)
        {
            // If has duplicated 
            if (rec != null)
            {
                // If current plan using duplicated
                if ((await _database.Table<PlanTimeSet>().Where(i => (i.PlanId == planId) && (i.TimeSetId == rec.Id)).CountAsync()) > 0)
                {
                    // return using duplicated signal
                    return -1;
                }
                // If not
                else
                {
                    // If other plans using current timeset
                    if ((await _database.Table<PlanTimeSet>().Where(i => (i.PlanId != planId) && (i.TimeSetId == timeset.Id)).CountAsync()) > 0)
                    {
                        // Insert new timeset and relationship
                        timeset.Id = 0;
                        return await _database.InsertAsync(new PlanTimeSet() { PlanId = planId, TimeSetId = await _database.InsertAsync(timeset) });
                    }
                    // If not
                    else
                    {
                        // Update current timeset
                        return await _database.UpdateAsync(timeset);
                    }
                }
            }
            // If no duplicated
            else
            {
                // If other plans using current timeset
                if ((await _database.Table<PlanTimeSet>().Where(i => (i.PlanId != planId) && (i.TimeSetId == timeset.Id)).CountAsync()) > 0)
                {
                    // Insert new timeset and relationship
                    timeset.Id = 0;
                    return await _database.InsertAsync(new PlanTimeSet() { PlanId = planId, TimeSetId = await _database.InsertAsync(timeset) });
                }
                // If not
                else
                {
                    // Update current timeset
                    return await _database.UpdateAsync(timeset);
                }
            }
        }
        // If new timeset
        else
        {
            // If has duplicated 
            if (rec != null)
            {
                // Insert new relationship
                return await _database.InsertAsync(new PlanTimeSet() { PlanId = planId, TimeSetId = rec.Id });
            }
            // If not
            else
            {
                // Insert new timeset and relationship
                return await _database.InsertAsync(new PlanTimeSet() { PlanId = planId, TimeSetId = await _database.InsertAsync(timeset) });
            }
        }
    }

    public async Task RemoveNotUsedDestinationAsync(int DestinationId, int planId)
    {
        await Init();

        // If other plans not using current destination
        if ((await _database.Table<Plan>().Where(i => (i.Id != planId) && (i.DestinationId == DestinationId)).CountAsync()) == 0)
        {
            // Delete current destination
            await _database.DeleteAsync<Destination>(DestinationId);
        }
    }

    public async Task RemoveNotUsedTimeSetAsync(int timeSetId, int planId)
    {
        await Init();

        // If other plans not using current timeset
        if ((await _database.Table<PlanTimeSet>().Where(i => (i.PlanId != planId) && (i.TimeSetId == timeSetId)).CountAsync()) == 0)
        {
            // Delete current timeset
            await _database.DeleteAsync<TimeSet>(timeSetId);
        }

        await _database.Table<PlanTimeSet>().Where(i => (i.PlanId == planId) && (i.TimeSetId == timeSetId)).DeleteAsync();
    }
}
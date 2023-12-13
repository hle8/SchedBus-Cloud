using SchedBus.Models;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;
using System.Collections.ObjectModel;

namespace SchedBus.Services;

public class PlanSQLiteService
{
    static PlanSQLiteService? _instance;
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

            SampleData();
        }
    }

    async void SampleData()
    {
        var random = new Random();
        var start = TimeSpan.FromHours(0);
        var end = TimeSpan.FromHours(23);
        var maxMinutes = (int)((end - start).TotalMinutes);

        for (int i = 1; i <= 5; i++)
        {
            var samplePlan = new Plan
            {
                Label = $"Plan {i}",
                MaxNumberOfRoutes = i,
                Notification = i % 2 == 0,
                Vibration = i % 2 == 0,
                Destination = new Destination
                {
                    Name = $"Destination {i}",
                    Address = $"Address {i}"
                },
                TimeSets = new ObservableCollection<TimeSet>()
            };
            for (int j = 1; j <= 5; j++)
            {
                var minute = random.Next(maxMinutes);
                samplePlan.TimeSets.Add(new TimeSet { IsEnabled = j % 2 == 0, Time = start.Add(TimeSpan.FromMinutes(minute)) });
            }
            await _database.InsertWithChildrenAsync(samplePlan, recursive: true);
        }
    }

    // Get all plan
    public async Task<ObservableCollection<Plan>> GetPlansAsync()
    {
        await Init();

        var result = await _database.GetAllWithChildrenAsync<Plan>(recursive: true);
        return new ObservableCollection<Plan>(result);
    }

    // Get a plan
    public async Task<Plan> GetPlanAsync(int id)
    {
        await Init();

        return await _database.GetWithChildrenAsync<Plan>(id, recursive: true);
    }

    // Save a plan
    public async Task SavePlanAsync(Plan plan)
    {
        await Init();

        await _database.InsertOrReplaceWithChildrenAsync(plan, recursive: true);
    }

    // Delete a plan
    public async Task DeletePlanAsync(Plan plan)
    {
        await Init();

        await _database.DeleteAsync(plan, recursive: true);
    }
}
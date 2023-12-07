using SchedBus.Models;
using SQLite;

namespace SchedBus.Services;

class PlanDataService : IDataStore
{
    SQLiteAsyncConnection Database;

    const string DatabaseFilename = "SchedBus.db";

    const SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLiteOpenFlags.SharedCache;

    static string DatabasePath => Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
    async Task Init()
    {
        if (Database != null) return;

        Database = new SQLiteAsyncConnection(DatabasePath, Flags);

        await Database.CreateTableAsync<Plan>();

        await InsertSampleData();
    }

    async Task InsertSampleData()
    {
        // for (int i = 1; i <= 5; i++)
        // {
        //     await SavePlanAsync(new Plan() { Label = $"Plan {i}", MaxNumberOfRoutes = i, Vibration = (i % 2 == 0), Notification = (i % 2 == 0) });
        // }
        await SavePlanAsync(new Plan() { Label = $"Plan sample", MaxNumberOfRoutes = 2, Vibration = (2 % 2 == 0), Notification = (2 % 2 == 0) });
    }

    // Get all plans
    public async Task<List<Plan>> GetPlansAsync()
    {
        await Init();

        return await Database.Table<Plan>().ToListAsync();
    }

    // Get a plans
    public async Task<Plan> GetPlanAsync(int id)
    {
        await Init();

        return await Database.Table<Plan>().Where(i => i.Id == id).FirstOrDefaultAsync();
    }

    // Save a plan
    public async Task<int> SavePlanAsync(Plan plan)
    {
        await Init();

        if (plan.Id != 0) return await Database.UpdateAsync(plan);
        else return await Database.InsertAsync(plan);

        ;
    }

    // Remove a plan
    public async Task<int> RemovePlanAsync(int id)
    {
        await Init();

        return await Database.DeleteAsync<Plan>(id);
    }
}
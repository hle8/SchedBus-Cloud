using SchedBus.Models;

namespace SchedBus.Services;

interface IDataStore
{
    Task<List<Plan>> GetPlansAsync();
    Task<Plan> GetPlanAsync(int id);
    Task<int> SavePlanAsync(Plan plan);
    Task<int> RemovePlanAsync(int id);
}

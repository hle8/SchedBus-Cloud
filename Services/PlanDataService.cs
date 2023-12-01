using CommunityToolkit.Mvvm.ComponentModel;
using SchedBus.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace SchedBus.Services;

public class PlanDataService : ObservableObject
{
    private static PlanDataService _instance;
    private uint _lastId = 0;

    public static PlanDataService Instance => _instance ??= new PlanDataService();

    public ObservableCollection<Plan> Plans { get; private set; }

    private PlanDataService ()
    {
        Plans = new ObservableCollection<Plan> ();
        Plans.CollectionChanged += Plans_CollectionChanged;

        LoadSamplePlans();
    }

    private void LoadSamplePlans()
    {
        for (var i = 1; i <= 5; i++)
        {
            Plans.Add(new Plan { Label = $"Plan {i}" });
        }
    }

    private void Plans_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if(e.Action == NotifyCollectionChangedAction.Add)
        {
            foreach (Plan plan in e.NewItems) 
            {
                plan.ID = ++_lastId;
            }
        }
    }

    public void AddPlan(Plan plan)
    {
        Plans.Add(plan);
    }

    public void UpdatePlan(Plan plan)
    {
        var existingPlan = Plans.FirstOrDefault(a => a.ID == plan.ID);
        if (existingPlan != null)
        {
            existingPlan = plan;
        }
    }

    public void RemovePlan(Plan plan)
    {
        Plans.Remove(plan);
    }
}

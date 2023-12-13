﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SchedBus.Models;
using SchedBus.Services;
using System.Collections.ObjectModel;

namespace SchedBus.ViewModels;

public partial class PlansViewModel : ObservableObject
{
    protected static PlanSQLiteService Database => PlanSQLiteService.Instance;

    [ObservableProperty]
    public ObservableCollection<Plan> plans;

    public PlansViewModel()
    {
        Plans = new ObservableCollection<Plan>();
    }

    [RelayCommand]
    public async Task GetPlan()
    {
        Plans = await Database.GetPlansAsync();
    }

    [RelayCommand]
    async Task AddPlan()
    {
        await Shell.Current.GoToAsync($"{nameof(Pages.PlanEditPage)}");
    }

    [RelayCommand]
    async Task EditPlan(Plan plan)
    {
        var navigationParameter = new Dictionary<string, object> { { "selectedPlan", plan } };
        await Shell.Current.GoToAsync($"{nameof(Pages.PlanEditPage)}", navigationParameter);
    }

    [RelayCommand]
    async Task DeletePlan(Plan plan)
    {
        await Database.DeletePlanAsync(plan);
        Plans = await Database.GetPlansAsync();
    }
}
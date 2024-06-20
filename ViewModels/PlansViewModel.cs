using Auth0.OidcClient;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SchedBus.Extensions;
using SchedBus.Models.FirestoreDocuments;
using SchedBus.Services;
using System.Collections.ObjectModel;
using System.Security.Claims;

namespace SchedBus.ViewModels;

public partial class PlansViewModel : ObservableObject
{
    protected static PlanSQLiteService Database => PlanSQLiteService.Instance;

    private readonly FirestoreService _firestoreService;
    private readonly Auth0Client _auth0Client;

    public ClaimsPrincipal? User;

    [ObservableProperty]
    private bool navBarIsVisible = false;

    [ObservableProperty]
    private bool loginViewIsVisible = true;

    [ObservableProperty]
    private bool plansViewIsVisible = false;

    [ObservableProperty]
    private DateTime lastUpdate;

    [ObservableProperty]
    private ObservableCollection<PlanDocument> plans = [];

    [ObservableProperty]
    private bool isRefreshingPlans;

    public PlansViewModel(FirestoreService firestoreService, Auth0Client auth0Client)
    {
        _firestoreService = firestoreService;
        _auth0Client = auth0Client;
    }

    [RelayCommand]
    private async Task Login()
    {
        var loginResult = await _auth0Client.LoginAsync();

        if (!loginResult.IsError)
        {
            LoginViewIsVisible = false;
            NavBarIsVisible = true;
            PlansViewIsVisible = true;

            User = loginResult.User;

            RefreshPlans();
        }
        else
        {
            await Shell.Current.DisplayAlert("Error", loginResult.Error, "OK");
        }
    }

    [RelayCommand]
    private async Task Logout()
    {
        await _auth0Client.LogoutAsync();

        LoginViewIsVisible = true;
        NavBarIsVisible = false;
        PlansViewIsVisible = false;

        User = null;
    }

    public void RefreshPlans()
    {
        IsRefreshingPlans = true;
    }

    [RelayCommand]
    private async Task GetPlan()
    {
        var getPlans = await _firestoreService.GetAllPlans(User.GetUserEmail());

        if (getPlans != null)
        {
            Plans = new ObservableCollection<PlanDocument>(getPlans);
        }

        LastUpdate = DateTime.Now;

        IsRefreshingPlans = false;
    }

    [RelayCommand]
    private async Task AddPlan()
    {
        await Shell.Current.GoToAsync($"{nameof(Pages.PlanEditPage)}");
    }

    [RelayCommand]
    private async Task EditPlan(PlanDocument plan)
    {
        var navigationParameter = new Dictionary<string, object> { { "selectedPlan", plan } };

        await Shell.Current.GoToAsync($"{nameof(Pages.PlanEditPage)}", navigationParameter);
    }

    [RelayCommand]
    private async Task DeletePlan(PlanDocument plan)
    {
        await _firestoreService.DeletePlan(plan.Id, User.GetUserEmail());

        RefreshPlans();
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using SchedBus.Models;
using SchedBus.ViewModels;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using System.Collections.ObjectModel;

namespace SchedBus.Views
{
    public partial class TimeSetPage : ContentPage
    {
        private TimeSetViewModel? _viewModel;

        public TimeSetPage()
        {
            InitializeComponent();

            BindingContext = new TimeSetViewModel();
        }

        public ObservableCollection<TimeSet> TimeSets { get; set; }
        private void InitializeComponent()
        {
            throw new NotImplementedException();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Shell.SetBackButtonBehavior(this, new BackButtonBehavior
            {
                Command = new AsyncRelayCommand(async () =>
                {
                    await Shell.Current.GoToAsync($"..");
                })
            });
        }
    }
}
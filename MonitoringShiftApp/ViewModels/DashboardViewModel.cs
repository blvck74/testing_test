using System;
using CommunityToolkit.Mvvm.ComponentModel;
using MonitoringShiftApp.Services;

namespace MonitoringShiftApp.ViewModels;

public partial class DashboardViewModel : ViewModelBase
{
    private readonly IAuthenticationService _authService;

    [ObservableProperty]
    private int _totalWorks = 0;

    [ObservableProperty]
    private int _totalIncidents = 0;

    [ObservableProperty]
    private int _newTickets = 0;

    [ObservableProperty]
    private string _lastUpdateTime = string.Empty;

    public DashboardViewModel(IAuthenticationService authService)
    {
        _authService = authService;
        LoadDashboardData();
    }

    private void LoadDashboardData()
    {
        // Здесь будет загрузка данных из базы данных
        // Пока используем тестовые данные
        TotalWorks = 15;
        TotalIncidents = 3;
        NewTickets = 7;
        LastUpdateTime = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
    }
}
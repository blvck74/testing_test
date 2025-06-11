using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MonitoringShiftApp.Services;
using MonitoringShiftApp.Data;

namespace MonitoringShiftApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IAuthenticationService _authService;
    private readonly IThemeService _themeService;
    private readonly AppDbContext _context;

    [ObservableProperty]
    private ViewModelBase _currentViewModel;

    [ObservableProperty]
    private int _selectedTabIndex = 0;

    [ObservableProperty]
    private bool _isAuthenticated;

    [ObservableProperty]
    private string _currentUserName = string.Empty;

    [ObservableProperty]
    private bool _isDarkTheme = true;

    // ViewModels for tabs
    public LoginViewModel LoginViewModel { get; }
    public DashboardViewModel DashboardViewModel { get; }
    public NotesViewModel NotesViewModel { get; }
    public DepartmentGViewModel DepartmentGViewModel { get; }

    public MainWindowViewModel(
        IAuthenticationService authService, 
        IThemeService themeService,
        AppDbContext context)
    {
        _authService = authService;
        _themeService = themeService;
        _context = context;

        // Initialize ViewModels
        LoginViewModel = new LoginViewModel(_authService);
        DashboardViewModel = new DashboardViewModel(_authService);
        NotesViewModel = new NotesViewModel(_context, _authService);
        DepartmentGViewModel = new DepartmentGViewModel(_context);

        // Set initial view
        _currentViewModel = LoginViewModel;
        
        // Subscribe to authentication changes
        _authService.AuthenticationChanged += OnAuthenticationChanged;
        _themeService.ThemeChanged += OnThemeChanged;

        // Set initial state
        IsAuthenticated = _authService.IsAuthenticated;
        IsDarkTheme = _themeService.IsDarkTheme;
        UpdateCurrentUserName();
    }

    private void OnAuthenticationChanged(object? sender, Models.User? user)
    {
        IsAuthenticated = user != null;
        UpdateCurrentUserName();
        
        if (IsAuthenticated)
        {
            SelectedTabIndex = 0; // Dashboard
            CurrentViewModel = DashboardViewModel;
        }
        else
        {
            CurrentViewModel = LoginViewModel;
        }
    }

    private void OnThemeChanged(object? sender, bool isDark)
    {
        IsDarkTheme = isDark;
    }

    private void UpdateCurrentUserName()
    {
        var user = _authService.CurrentUser;
        if (user != null)
        {
            CurrentUserName = !string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.LastName)
                ? $"{user.FirstName} {user.LastName}"
                : user.Username;
        }
        else
        {
            CurrentUserName = string.Empty;
        }
    }

    [RelayCommand]
    private void SelectTab(string tabName)
    {
        if (!IsAuthenticated) return;

        CurrentViewModel = tabName switch
        {
            "Dashboard" => DashboardViewModel,
            "Works" => DashboardViewModel, // Placeholder for now
            "Incidents" => DashboardViewModel, // Placeholder for now
            "ShiftTransfer" => DashboardViewModel, // Placeholder for now
            "DepartmentG" => DepartmentGViewModel,
            "MZ" => DashboardViewModel, // Placeholder for now
            "Notes" => NotesViewModel,
            "Settings" => DashboardViewModel, // Placeholder for now
            _ => DashboardViewModel
        };
    }

    [RelayCommand]
    private void ToggleTheme()
    {
        _themeService.ToggleTheme();
    }

    [RelayCommand]
    private async Task Logout()
    {
        await _authService.LogoutAsync();
    }
}

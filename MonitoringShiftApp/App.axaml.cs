using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using MonitoringShiftApp.ViewModels;
using MonitoringShiftApp.Views;
using MonitoringShiftApp.Services;
using MonitoringShiftApp.Data;

namespace MonitoringShiftApp;

public partial class App : Application
{
    private ServiceProvider? _serviceProvider;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Configure services
        var services = new ServiceCollection();
        ConfigureServices(services);
        _serviceProvider = services.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            
            // Initialize database
            InitializeDatabase();
            
            desktop.MainWindow = new MainWindow
            {
                DataContext = _serviceProvider.GetRequiredService<MainWindowViewModel>(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void ConfigureServices(ServiceCollection services)
    {
        // Database
        services.AddDbContext<AppDbContext>(options =>
        {
            // For now, use SQLite for development. Will be changed to PostgreSQL later
            options.UseSqlite("Data Source=monitoring.db");
        });

        // Services
        services.AddSingleton<IAuthenticationService, AuthenticationService>();
        services.AddSingleton<IThemeService, ThemeService>();

        // ViewModels
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<LoginViewModel>();
        services.AddTransient<DashboardViewModel>();
        services.AddTransient<NotesViewModel>();
        services.AddTransient<DepartmentGViewModel>();
    }

    private void InitializeDatabase()
    {
        using var scope = _serviceProvider!.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        // Ensure database is created
        context.Database.EnsureCreated();
        
        // Seed default user if no users exist
        if (!context.Users.Any())
        {
            var defaultUser = new Models.User
            {
                Username = "admin",
                PasswordHash = Convert.ToBase64String(System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes("admin"))),
                FirstName = "Администратор",
                LastName = "Системы",
                Email = "admin@monitoring.local",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            
            context.Users.Add(defaultUser);
            context.SaveChanges();
        }
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}
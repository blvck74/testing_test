using System;
using System.Threading.Tasks;
using MonitoringShiftApp.Models;

namespace MonitoringShiftApp.Services;

public interface IAuthenticationService
{
    Task<User?> LoginAsync(string username, string password);
    Task<bool> RegisterAsync(string username, string password, string? firstName = null, string? lastName = null, string? email = null);
    Task LogoutAsync();
    User? CurrentUser { get; }
    bool IsAuthenticated { get; }
    event EventHandler<User?>? AuthenticationChanged;
}
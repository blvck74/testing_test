using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MonitoringShiftApp.Data;
using MonitoringShiftApp.Models;

namespace MonitoringShiftApp.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly AppDbContext _context;
    private User? _currentUser;

    public AuthenticationService(AppDbContext context)
    {
        _context = context;
    }

    public User? CurrentUser => _currentUser;
    public bool IsAuthenticated => _currentUser != null;

    public event EventHandler<User?>? AuthenticationChanged;

    public async Task<User?> LoginAsync(string username, string password)
    {
        var passwordHash = HashPassword(password);
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username && u.PasswordHash == passwordHash && u.IsActive);

        if (user != null)
        {
            user.LastLoginAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            
            _currentUser = user;
            AuthenticationChanged?.Invoke(this, _currentUser);
        }

        return user;
    }

    public async Task<bool> RegisterAsync(string username, string password, string? firstName = null, string? lastName = null, string? email = null)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (existingUser != null)
            return false;

        var user = new User
        {
            Username = username,
            PasswordHash = HashPassword(password),
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public Task LogoutAsync()
    {
        _currentUser = null;
        AuthenticationChanged?.Invoke(this, null);
        return Task.CompletedTask;
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
}
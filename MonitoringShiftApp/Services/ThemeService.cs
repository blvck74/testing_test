using System;
using Avalonia;
using Avalonia.Styling;

namespace MonitoringShiftApp.Services;

public class ThemeService : IThemeService
{
    private bool _isDarkTheme = true;

    public bool IsDarkTheme => _isDarkTheme;

    public event EventHandler<bool>? ThemeChanged;

    public void ToggleTheme()
    {
        _isDarkTheme = !_isDarkTheme;
        
        if (Application.Current != null)
        {
            Application.Current.RequestedThemeVariant = _isDarkTheme 
                ? ThemeVariant.Dark 
                : ThemeVariant.Light;
        }
        
        ThemeChanged?.Invoke(this, _isDarkTheme);
    }
}
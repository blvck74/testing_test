using System;
using System.ComponentModel.DataAnnotations;

namespace MonitoringShiftApp.Models;

public class AppSettings
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Key { get; set; } = string.Empty;
    
    public string Value { get; set; } = string.Empty;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
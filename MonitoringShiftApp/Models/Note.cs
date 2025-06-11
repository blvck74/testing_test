using System;
using System.ComponentModel.DataAnnotations;

namespace MonitoringShiftApp.Models;

public class Note
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    
    public string Content { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
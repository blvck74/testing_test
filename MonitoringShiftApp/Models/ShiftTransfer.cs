using System;
using System.ComponentModel.DataAnnotations;

namespace MonitoringShiftApp.Models;

public class ShiftTransfer
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    
    public string MainInformation { get; set; } = string.Empty;
    
    public string OperationalInformation { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime ShiftDate { get; set; }
    
    public int CreatedByUserId { get; set; }
    public User CreatedByUser { get; set; } = null!;
}
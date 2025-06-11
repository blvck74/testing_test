using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MonitoringShiftApp.Models;

public class DepartmentGTopic
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    public int Order { get; set; }
    
    // Navigation properties
    public ICollection<DepartmentGSubtopic> Subtopics { get; set; } = new List<DepartmentGSubtopic>();
}

public class DepartmentGSubtopic
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    public int Order { get; set; }
    
    public int TopicId { get; set; }
    public DepartmentGTopic Topic { get; set; } = null!;
    
    // Navigation properties
    public ICollection<DepartmentGInformation> Information { get; set; } = new List<DepartmentGInformation>();
}

public class DepartmentGInformation
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    
    public string Content { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    public int Order { get; set; }
    
    public int SubtopicId { get; set; }
    public DepartmentGSubtopic Subtopic { get; set; } = null!;
}
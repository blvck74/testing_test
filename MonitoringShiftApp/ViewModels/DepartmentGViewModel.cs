using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using MonitoringShiftApp.Data;
using MonitoringShiftApp.Models;

namespace MonitoringShiftApp.ViewModels;

public partial class DepartmentGViewModel : ViewModelBase
{
    private readonly AppDbContext _context;

    [ObservableProperty]
    private ObservableCollection<DepartmentGTopic> _topics = new();

    [ObservableProperty]
    private DepartmentGTopic? _selectedTopic;

    [ObservableProperty]
    private DepartmentGSubtopic? _selectedSubtopic;

    [ObservableProperty]
    private DepartmentGInformation? _selectedInformation;

    [ObservableProperty]
    private string _newTopicName = string.Empty;

    [ObservableProperty]
    private string _newSubtopicName = string.Empty;

    [ObservableProperty]
    private string _newInformationTitle = string.Empty;

    [ObservableProperty]
    private string _newInformationContent = string.Empty;

    [ObservableProperty]
    private bool _isEditingInformation;

    public DepartmentGViewModel(AppDbContext context)
    {
        _context = context;
        _ = LoadTopics();
    }

    [RelayCommand]
    private async Task LoadTopics()
    {
        var topics = await _context.DepartmentGTopics
            .Include(t => t.Subtopics)
                .ThenInclude(s => s.Information)
            .OrderBy(t => t.Order)
            .ToListAsync();

        Topics.Clear();
        foreach (var topic in topics)
        {
            Topics.Add(topic);
        }
    }

    [RelayCommand]
    private async Task AddTopic()
    {
        if (string.IsNullOrWhiteSpace(NewTopicName)) return;

        var maxOrder = Topics.Any() ? Topics.Max(t => t.Order) : 0;
        var topic = new DepartmentGTopic
        {
            Name = NewTopicName,
            Order = maxOrder + 1,
            CreatedAt = DateTime.UtcNow
        };

        _context.DepartmentGTopics.Add(topic);
        await _context.SaveChangesAsync();

        Topics.Add(topic);
        NewTopicName = string.Empty;
    }

    [RelayCommand]
    private async Task AddSubtopic()
    {
        if (SelectedTopic == null || string.IsNullOrWhiteSpace(NewSubtopicName)) return;

        var maxOrder = SelectedTopic.Subtopics.Any() ? SelectedTopic.Subtopics.Max(s => s.Order) : 0;
        var subtopic = new DepartmentGSubtopic
        {
            Name = NewSubtopicName,
            TopicId = SelectedTopic.Id,
            Order = maxOrder + 1,
            CreatedAt = DateTime.UtcNow
        };

        _context.DepartmentGSubtopics.Add(subtopic);
        await _context.SaveChangesAsync();

        SelectedTopic.Subtopics.Add(subtopic);
        NewSubtopicName = string.Empty;
    }

    [RelayCommand]
    private async Task AddInformation()
    {
        if (SelectedSubtopic == null || string.IsNullOrWhiteSpace(NewInformationTitle)) return;

        var maxOrder = SelectedSubtopic.Information.Any() ? SelectedSubtopic.Information.Max(i => i.Order) : 0;
        var information = new DepartmentGInformation
        {
            Title = NewInformationTitle,
            Content = NewInformationContent,
            SubtopicId = SelectedSubtopic.Id,
            Order = maxOrder + 1,
            CreatedAt = DateTime.UtcNow
        };

        _context.DepartmentGInformation.Add(information);
        await _context.SaveChangesAsync();

        SelectedSubtopic.Information.Add(information);
        NewInformationTitle = string.Empty;
        NewInformationContent = string.Empty;
    }

    [RelayCommand]
    private void EditInformation(DepartmentGInformation? information)
    {
        if (information == null) return;

        SelectedInformation = information;
        NewInformationTitle = information.Title;
        NewInformationContent = information.Content;
        IsEditingInformation = true;
    }

    [RelayCommand]
    private async Task UpdateInformation()
    {
        if (SelectedInformation == null) return;

        SelectedInformation.Title = NewInformationTitle;
        SelectedInformation.Content = NewInformationContent;
        SelectedInformation.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        
        IsEditingInformation = false;
        SelectedInformation = null;
        NewInformationTitle = string.Empty;
        NewInformationContent = string.Empty;
    }

    [RelayCommand]
    private void CancelEdit()
    {
        IsEditingInformation = false;
        SelectedInformation = null;
        NewInformationTitle = string.Empty;
        NewInformationContent = string.Empty;
    }

    [RelayCommand]
    private void SelectTopic(DepartmentGTopic? topic)
    {
        SelectedTopic = topic;
        SelectedSubtopic = null;
        SelectedInformation = null;
    }

    [RelayCommand]
    private void SelectSubtopic(DepartmentGSubtopic? subtopic)
    {
        SelectedSubtopic = subtopic;
        SelectedInformation = null;
    }
}
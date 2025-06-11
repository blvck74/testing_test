using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using MonitoringShiftApp.Data;
using MonitoringShiftApp.Models;
using MonitoringShiftApp.Services;

namespace MonitoringShiftApp.ViewModels;

public partial class NotesViewModel : ViewModelBase
{
    private readonly AppDbContext _context;
    private readonly IAuthenticationService _authService;

    [ObservableProperty]
    private ObservableCollection<Note> _notes = new();

    [ObservableProperty]
    private Note? _selectedNote;

    [ObservableProperty]
    private string _newNoteTitle = string.Empty;

    [ObservableProperty]
    private string _newNoteContent = string.Empty;

    [ObservableProperty]
    private bool _isEditing;

    public NotesViewModel(AppDbContext context, IAuthenticationService authService)
    {
        _context = context;
        _authService = authService;
        _ = LoadNotes();
    }

    [RelayCommand]
    private async Task LoadNotes()
    {
        if (_authService.CurrentUser == null) return;

        var notes = await _context.Notes
            .Where(n => n.UserId == _authService.CurrentUser.Id)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();

        Notes.Clear();
        foreach (var note in notes)
        {
            Notes.Add(note);
        }
    }

    [RelayCommand]
    private async Task AddNote()
    {
        if (_authService.CurrentUser == null || string.IsNullOrWhiteSpace(NewNoteTitle))
            return;

        var note = new Note
        {
            Title = NewNoteTitle,
            Content = NewNoteContent,
            UserId = _authService.CurrentUser.Id,
            CreatedAt = DateTime.UtcNow
        };

        _context.Notes.Add(note);
        await _context.SaveChangesAsync();

        Notes.Insert(0, note);
        
        NewNoteTitle = string.Empty;
        NewNoteContent = string.Empty;
    }

    [RelayCommand]
    private async Task UpdateNote()
    {
        if (SelectedNote == null) return;

        SelectedNote.Content = NewNoteContent;
        SelectedNote.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        IsEditing = false;
    }

    [RelayCommand]
    private async Task DeleteNote(Note? note)
    {
        if (note == null) return;

        _context.Notes.Remove(note);
        await _context.SaveChangesAsync();
        Notes.Remove(note);

        if (SelectedNote == note)
        {
            SelectedNote = null;
            IsEditing = false;
        }
    }

    [RelayCommand]
    private void EditNote(Note? note)
    {
        if (note == null) return;

        SelectedNote = note;
        NewNoteContent = note.Content;
        IsEditing = true;
    }

    [RelayCommand]
    private void CancelEdit()
    {
        IsEditing = false;
        SelectedNote = null;
        NewNoteContent = string.Empty;
    }
}
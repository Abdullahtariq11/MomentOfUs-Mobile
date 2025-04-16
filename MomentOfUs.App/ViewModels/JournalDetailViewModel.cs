using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Graphics;
using MomentOfUs.App.Dtos;
using MomentOfUs.App.Models;
using MomentOfUs.App.Services;

namespace MomentOfUs.App.ViewModels
{
    [QueryProperty(nameof(Id), "id")]
    public partial class JournalDetailViewModel : ObservableObject
    {
        private readonly JournalService _journalService;

        [ObservableProperty]
        private string id;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private ObservableCollection<string> availableColors = new()
        {
            "Default", "White", "LightPink", "Lavender", "LightBlue", "LightYellow"
        };

        [ObservableProperty]
        private string selectedColor = "Default";

        [ObservableProperty]
        private Color pageBackgroundColor = Color.FromArgb("#FEEBC8");

        [ObservableProperty]
        private JournalEntryUpdateDto currentEntry = new();

        [ObservableProperty]
        private string editingEntryId;

        public ObservableCollection<JournalEntryModel> JournalEntries { get; } = new();

        public IRelayCommand AddOrUpdateEntryCommand { get; }
        public IRelayCommand<JournalEntryModel> DeleteEntryCommand { get; }
        public IRelayCommand<JournalEntryModel> EditEntryCommand { get; }
        public ObservableCollection<string> MoodEmojis { get; } = new()
{
     "📔", // 0
    "😊", // 1
    "❤️", // 2
    "🌟", // 3
     "🎉"  // 4
};


        public JournalDetailViewModel(JournalService journalService)
        {
            _journalService = journalService;
            AddOrUpdateEntryCommand = new RelayCommand(async () => await AddOrUpdateEntryAsync());
            DeleteEntryCommand = new RelayCommand<JournalEntryModel>(async (entry) => await DeleteJournalEntryAsync(entry));
            EditEntryCommand = new RelayCommand<JournalEntryModel>((entry) => BeginEditEntry(entry));
        }

        public async Task OnNavigatedAsync()
        {
            if (!string.IsNullOrEmpty(Id))
            {
                await LoadJournalEntriesAsync(Id);
            }
        }

        public async Task LoadJournalEntriesAsync(string journalId)
        {
            if (IsLoading) return;

            try
            {
                IsLoading = true;
                JournalEntries.Clear();

                var result = await _journalService.GetJournalEntriesAsync(journalId);
                foreach (var entry in result)
                {
                    Debug.WriteLine($"📘 Entry: {entry.Id} | {entry.Content}");
                    JournalEntries.Add(entry);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading journal entries: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task AddOrUpdateEntryAsync()
        {
            if (string.IsNullOrWhiteSpace(CurrentEntry.Content)) return;

            if (!string.IsNullOrEmpty(EditingEntryId))
            {
                var result = await _journalService.UpdateJournalEntryAsync(Id, EditingEntryId, CurrentEntry);
                if (result == "Success")
                {
                    EditingEntryId = null;
                    CurrentEntry = new JournalEntryUpdateDto();
                    await LoadJournalEntriesAsync(Id);
                }
                else
                {
                    Debug.WriteLine("❌ Failed to update journal entry.");
                }
            }
            else
            {
                var result = await _journalService.CreateJournalEntryAsync(Id, CurrentEntry);
                if (result == "Success")
                {
                    CurrentEntry = new JournalEntryUpdateDto();
                    await LoadJournalEntriesAsync(Id);
                }
                else
                {
                    Debug.WriteLine($"⚠️ Failed to add journal entry: {result}");
                }
            }
        }

        private async Task DeleteJournalEntryAsync(JournalEntryModel entry)
        {
            var success = await _journalService.DeleteJournalEntry(Id, entry.Id);
            if (success)
            {
                JournalEntries.Remove(entry);
            }
            else
            {
                Debug.WriteLine("❌ Failed to delete journal entry.");
            }
        }

        private void BeginEditEntry(JournalEntryModel entry)
        {
            if (entry == null) return;

            EditingEntryId = entry.Id;
            CurrentEntry.Content = entry.Content;
            CurrentEntry.Mood = entry.Mood;
        }

        partial void OnIdChanged(string value)
        {
            _ = OnNavigatedAsync();
        }

        partial void OnSelectedColorChanged(string value)
        {
            var colorMap = new Dictionary<string, Color>
            {
                ["Default"] = Color.FromArgb("#FEEBC8"),
                ["White"] = Colors.White,
                ["LightPink"] = Color.FromArgb("#FFB6C1"),
                ["Lavender"] = Color.FromArgb("#E6E6FA"),
                ["LightBlue"] = Color.FromArgb("#ADD8E6"),
                ["LightYellow"] = Color.FromArgb("#FFFFE0")
            };

            if (!string.IsNullOrWhiteSpace(value) && colorMap.TryGetValue(value, out var color))
            {
                PageBackgroundColor = color;
            }
        }
    }
}
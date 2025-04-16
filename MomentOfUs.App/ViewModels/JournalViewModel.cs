using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MomentOfUs.App.Models;
using MomentOfUs.App.Services;
using MomentOfUs.App.Views;

namespace MomentOfUs.App.ViewModels
{
    public partial class JournalViewModel : ObservableObject
    {
        private readonly JournalService _journalService;

        public IRelayCommand<string> JournalTappedCommand { get; }

        public ObservableCollection<JournalModel> Journals { get; } = new ObservableCollection<JournalModel>();
        public ObservableCollection<JournalSharedModel> SharedJournals { get; } = new ObservableCollection<JournalSharedModel>();

        [ObservableProperty] private bool isLoading;
        public JournalViewModel(JournalService journalService)
        {
            _journalService = journalService;
            JournalTappedCommand = new RelayCommand<string>(OnJournalTapped);
        }

        private async void OnJournalTapped(string journalId)
        {
            await Shell.Current.GoToAsync($"{nameof(JournalDetailPage)}?id={journalId}");

        }



        public async Task LoadJournalsAsync()
        {
            if (isLoading) return;
            try
            {
                isLoading = true;
                Journals.Clear();

                var result = await _journalService.GetJournalsAsync();

                foreach (var journal in result)
                {
                    Journals.Add(journal);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading journals: {ex.Message}");
            }
            finally
            {
                isLoading = false;
            }
        }

        public async Task LoadSharedJournalsAsync()
        {
            if (isLoading) return;
            try
            {
                isLoading = true;
                SharedJournals.Clear();
                var result = await _journalService.GetSharedJournalsAsync();
                foreach (var journal in result)
                {
                    SharedJournals.Add(journal);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading shared journals: {ex.Message}");
            }
            finally
            {
                isLoading = false;
            }
        }

        public async void LoadJournals()
        {
            await LoadJournalsAsync();
            await LoadSharedJournalsAsync();
        }


    }
}

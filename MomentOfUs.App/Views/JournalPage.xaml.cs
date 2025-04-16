using System.Diagnostics;
using MomentOfUs.App.Models;
using MomentOfUs.App.Services;
using MomentOfUs.App.ViewModels;

namespace MomentOfUs.App.Views;

public partial class JournalPage : ContentPage
{
    public JournalPage(JournalViewModel journalViewModel )
    {
        InitializeComponent();
        BindingContext = journalViewModel;
        journalViewModel.LoadJournals(); // fire and forget
        
     
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Navigate manually just to test
       // await Shell.Current.GoToAsync($"{nameof(JournalDetailPage)}?id=24");
    }



}

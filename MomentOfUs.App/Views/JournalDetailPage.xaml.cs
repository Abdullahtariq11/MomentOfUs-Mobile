using MomentOfUs.App.ViewModels;

namespace MomentOfUs.App.Views;

public partial class JournalDetailPage : ContentPage
{
    private readonly JournalDetailViewModel _viewModel;

    public JournalDetailPage(JournalDetailViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.OnNavigatedAsync(); // Ensures journal entries load using ID passed via route
    }
}

using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MomentOfUs.App.Services;
using Microsoft.Maui.Storage;
using MomentOfUs.App.Views;

namespace MomentOfUs.App.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly AuthService _authService;

        [ObservableProperty] private string email;
        [ObservableProperty] private string password;
        [ObservableProperty] private bool rememberMe=true;
        [ObservableProperty] private string errorMessage;
        [ObservableProperty] private bool isLoading;

        public ICommand LoginCommand { get; }

        public LoginViewModel(AuthService authService)
        {
            _authService = authService;
            LoginCommand = new AsyncRelayCommand(LoginAsync);
        }

        private async Task LoginAsync()
        {
            ErrorMessage = string.Empty;
            IsLoading = true;

            var token = await _authService.LoginAsync(Email, Password, RememberMe);
            IsLoading = false;

            if (!string.IsNullOrEmpty(token))
            {
                // Store token securely
                Preferences.Set("AuthToken", token);

                // Navigate to Home Page
                await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
            }
            else
            {
                ErrorMessage = "Invalid username or password. Please try again.";
            }
        }
    }
}

using MomentOfUs.App.Services;
using MomentOfUs.App.Views;

namespace MomentOfUs.App;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new LoginPage(new ViewModels.LoginViewModel(new AuthService(new HttpClient())));
    }
}

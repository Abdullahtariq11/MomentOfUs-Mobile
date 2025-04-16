using System.Runtime.Versioning;
using MomentOfUs.App.Views;

namespace MomentOfUs.App;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Registering Route
        Routing.RegisterRoute(nameof(JournalDetailPage), typeof(JournalDetailPage));
    }

}

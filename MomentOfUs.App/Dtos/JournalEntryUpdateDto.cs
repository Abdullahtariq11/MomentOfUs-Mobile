using CommunityToolkit.Mvvm.ComponentModel;

namespace MomentOfUs.App.Dtos
{
    // To avoid ENC0014, do not modify the base class or interfaces of an existing class during a live edit.
    // Ensure the base class and interfaces are correct before running the application.
    public partial class JournalEntryUpdateDto : ObservableObject
    {
        [ObservableProperty]
        private string content;

        [ObservableProperty]
        private int mood;
    }
}

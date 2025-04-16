namespace MomentOfUs.App.Models
{
    public class JournalEntryModel
    {
        public string Id { get; set; } = string.Empty;
        public string JournalId { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Mood { get; set; }

        // Emoji Mood mapping
        public string MoodEmoji => Mood switch
        {
            0 => "📔",
            1 => "😊",
            2 => "❤️",
            3 => "🌟",
            4 => "🎉",
            _ => "📝"
        };

        // Display for Content line
        public string DisplayContent => $"{MoodEmoji} {Content}";

        // Display for Metadata (timestamp only, since no author)
        public string DisplayInfo => $"🕒 {CreatedAt:MMM dd, yyyy}";
    }
}

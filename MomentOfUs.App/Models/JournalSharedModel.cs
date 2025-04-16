using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomentOfUs.App.Models
{
    public class JournalSharedModel
    {
        public string SharedJournalId { get; set; }
        public string JournalId { get; set; }
        public string Title { get; set; }
        public string PhotoUrl { get; set; }
        public string OwnerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int PermissionLevel { get; set; }
        public string PermissionText => PermissionLevel == 0 ? "Read-only" : "Editable";
    }
}

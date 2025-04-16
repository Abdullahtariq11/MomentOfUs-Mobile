using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomentOfUs.App.Models
{
    public class JournalModel
    {
        public string Id { get; set; }
        public string OwnerID { get; set; }
        public string Title { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

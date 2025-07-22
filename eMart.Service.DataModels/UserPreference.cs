using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMart.Service.DataModels
{
    public class UserPreference
    {
        public string UserId { get; set; }
        public string Category { get; set; }

        // Navigation Property
        public User User { get; set; }
    }
}

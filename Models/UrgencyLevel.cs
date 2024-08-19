using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksManagementApp.Models
{
    public class UrgencyLevel
    {

        public int UrgencyLevelId { get; set; }
        public string UrgencyLevelName { get; set; } = null!;

        public override string ToString()
        {
            return UrgencyLevelName;
        }

    }
}

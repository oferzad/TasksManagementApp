using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TasksManagementApp.Models;

namespace TasksManagementApp.Models
{
    public class UserTask
    {
        public int TaskId { get; set; }

        public int? UserId { get; set; }

        public int? UrgencyLevelId { get; set; }

        public string TaskDescription { get; set; } = null!;

        public DateOnly TaskDueDate { get; set; }

        public DateOnly? TaskActualDate { get; set; }

        public virtual ICollection<TaskComment> TaskComments { get; set; } = new List<TaskComment>();

        public UserTask() { }

    }
}

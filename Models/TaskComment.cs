using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TasksManagementApp.Models
{
    public class TaskComment
    {
        public int CommentId { get; set; }

        public int? TaskId { get; set; }

        public string Comment { get; set; } = null!;

        public DateOnly CommentDate { get; set; }

        public TaskComment() { }
    }
}

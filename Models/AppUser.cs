using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TasksManagementApp.Models
{
    public class AppUser
    {
        public int Id { get; set; }

        public string UserName { get; set; } = null!;

        public string UserLastName { get; set; } = null!;

        public string UserEmail { get; set; } = null!;

        public string UserPassword { get; set; } = null!;

        public bool IsManager { get; set; }

        public string ProfileImagePath { get; set; } = "";

        public virtual ICollection<UserTask> UserTasks { get; set; } = new List<UserTask>();

        public AppUser() { }

    }
}

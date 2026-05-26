using System;

namespace SmartTaskManager.Models
{
    public enum Priority
    {
        High,
        Medium,
        Low
    }

    public class TaskItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = "";
        public Priority Priority { get; set; } = Priority.Medium;
        public DateTime DueDate { get; set; } = DateTime.Today.AddDays(1);
        public bool IsCompleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
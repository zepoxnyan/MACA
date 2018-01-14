using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MACAWeb.Models
{
    /// <summary>
    /// List of fixed statuses. Additional are being added manually.
    /// </summary>
    public enum TaskStatusFixedList
    {
        Pending,
        Started,
        Aborted,
        Completed,
        Queued
    }

    public class TaskStatus
    {
        [Key]
        public Guid TaskStatusId { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage="Enter the name!")]
        public string Name { get; set; }

        [Display(Name = "Message")]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        [Required(ErrorMessage = "Enter the message!")]
        public string Description { get; set; }

        [Display(Name = "Created")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Modified")]
        public DateTime DateModified { get; set; }

        [Display(Name = "Created by User")]
        public Guid UserCreatedId { get; set; }

        [Display(Name = "Modified by User")]
        public Guid UserModifiedId { get; set; }
    }

    public class TaskStatusViewModel
    {
        public Guid TaskStatusId { get; set; }

        [Required(ErrorMessage = "Enter the name!")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Enter the message!")]
        public string Message { get; set; }
    }

    public class TaskStatusDbContext : DbContext
    {
        public DbSet<TaskStatus> Statuses { get; set; }

        public TaskStatusDbContext() : base("MACA") { }
    }
}
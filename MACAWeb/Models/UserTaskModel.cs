using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MACAWeb.Models
{
    public class UserTask
    {
        [Key]
        public Guid TaskId { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Set a name!")]
        public string Name { get; set; }

        [Display(Name = "Remark")]
        [DataType(DataType.MultilineText)]
        public string Remark { get; set; }

        [Required(ErrorMessage = "Select an instance!")]
        [Display(Name = "Instance")]
        public Guid InstanceId { get; set; }
        public virtual Instance Instance { get; set; }

        [Required(ErrorMessage = "Select a function!")]
        [Display(Name = "Function")]
        public Guid FunctionId { get; set; }
        public virtual Function Function { get; set; }

        [Display(Name = "Task Status")]
        public Guid TaskStatusId { get; set; }
        public virtual TaskStatus TaskStatus { get; set; }

        [Display(Name = "Started")]
        public DateTime? TimeStarted { get; set; }

        [Display(Name = "Finished")]
        public DateTime? TimeFinished { get; set; }

        [Display(Name = "Created")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Modified")]
        public DateTime DateModified { get; set; }

        [Display(Name = "Created by User")]
        public Guid UserCreatedId { get; set; }

        [Display(Name = "Modified by User")]
        public Guid UserModifiedId { get; set; }
    }

    public class UserTaskViewModel
    {
        public Guid TaskId { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Remark")]
        public string Remark { get; set; }

        [Required(ErrorMessage = "Select your Instance")]
        public Guid InstanceId { get; set; }

        [Required(ErrorMessage = "Select your Function")]
        public Guid FunctionId { get; set; }
    }

    /*public class UserTaskViewModel
    {
        public Guid TaskId { get; set; }
        [DisplayName("Instance")]
        public string InstanceName { get; set; }
        [DisplayName("Function")]
        public string FunctionName { get; set; }
        [DisplayName("Status")]
        public string Status { get; set; }
        [DisplayName("Time Started")]
        public string TimeStarted { get; set; }
        [DisplayName("Time Finished")]
        public string TimeFinished { get; set; }
    }*/

    public class UserTaskDbContext : DbContext
    {
        public DbSet<UserTask> UserTasks { get; set; }
        public DbSet<Instance> Instances { get; set; }
        public DbSet<Function> Functions { get; set; }
        public DbSet<TaskStatus> TaskStatuses { get; set; }

        public UserTaskDbContext() : base("MACA") {}
    }
}
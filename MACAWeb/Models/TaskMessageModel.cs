using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MACAWeb.Models
{
    public class TaskMessage
    {
        [Key]
        public Guid TaskMessageId { get; set; }

        public Guid TaskId { get; set; }

        public Guid TaskStatusId { get; set; }

        public DateTime DateCreated { get; set; }

        public Guid UserId { get; set; }
    }

    public class TaskMessageDbContext : DbContext
    {
        public DbSet<TaskMessage> Messages { get; set; }

        public TaskMessageDbContext() : base("MACA") { }
    }
}
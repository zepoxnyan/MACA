using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MACAWeb.Models
{
    public class Instance
    {
        [Key]
        public Guid InstanceId { get; set; }

        public Guid UserId { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("File Name")]
        public string FileName { get; set; }

        [NotMapped]
        public HttpPostedFileBase UploadedFile { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayName("Description")]
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

    public class InstanceViewModel
    {
        public Guid InstanceId { get; set; }
        public Guid UserId { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Current File")]
        public string FileName { get; set; }

        [DisplayName("New File")]
        public HttpPostedFileBase UploadedFile { get; set; }       
    }

    public class InstanceDbContext : DbContext
    {
        public DbSet<Instance> Instances { get; set; }

        public InstanceDbContext() : base("MACA") {}

        /*protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<InstanceDbContext>(null);
            base.OnModelCreating(modelBuilder);
        }*/
    }
} 
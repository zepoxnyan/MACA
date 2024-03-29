﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MACAWeb.Models
{
    public class GrantStatus
    {
        [Key]        
        public Guid GrantStatusID { get; set; }

        [Display(Name = "Grant Status Name")]
        [Required(ErrorMessage = "The name must be specified!")]
        public string Name { get; set; }

        [Display(Name = "Description")]   
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Date Created")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Date Modified")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime DateModified { get; set; }
        
        public Guid UserCreatedID { get; set; }

        public Guid UserModifiedID { get; set; }
    }

    public class GrantStatusViewModel
    {
        public Guid GrantStatusID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class GrantStatusDbContext : DbContext
    {
        public DbSet<GrantStatus> GrantStatus { get; set; }

        public GrantStatusDbContext() : base("MACA") { }
    }
}
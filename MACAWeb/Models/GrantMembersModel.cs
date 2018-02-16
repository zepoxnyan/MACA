using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MACAWeb.Models
{
    public class GrantMember
    {
        [Key]        
        public Guid GrantMemberID { get; set; }

        public Guid GrantID { get; set; }

        [Display(Name = "Person")]
        [Required(ErrorMessage = "Person is required!")]
        public Guid PersonID { get; set; }
        public virtual Person Person { get; set; }

        [Display(Name = "Grant Member Type")]
        [Required(ErrorMessage = "Grant Member Type is required!")]
        public Guid GrantMemberTypeID { get; set; }
        public virtual GrantMemberType GrantMemberType { get; set; }

        [Display(Name = "Hours (per year)")]
        [DefaultValue(0.0)]
        public double Hours { get; set; }

        [Display(Name = "Year")]
        [Required(ErrorMessage = "Year is required!")]
        public int Year { get; set; }

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

    public class GrantMemberViewModel
    {
        public Guid GrantMemberID { get; set; }
        public Guid GrantID { get; set; }

        [Display(Name = "Person")]
        [Required(ErrorMessage = "Person is required!")]
        public Guid PersonID { get; set; }
        public virtual Person Person { get; set; }

        [Display(Name = "Grant Member Type")]
        [Required(ErrorMessage = "Grant Member Type is required!")]
        public Guid GrantMemberTypeID { get; set; }
        public virtual GrantMemberType GrantMemberType { get; set; }

        [Display(Name = "Hours (per year)")]
        [DefaultValue(0.0)]
        public double Hours { get; set; }

        [Display(Name = "Year")]
        [Required(ErrorMessage = "Year is required!")]
        public int Year { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }

    public class GrantMemberDbContext : DbContext
    {
        public DbSet<GrantMember> GrantMembers { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<GrantMemberType> GrantMemberTypes { get; set; }

        public GrantMemberDbContext() : base("MACA") { }
    }
}
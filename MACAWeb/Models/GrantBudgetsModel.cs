using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MACAWeb.Models
{
    public class GrantBudget
    {
        [Key]        
        public Guid GrantBudgetID { get; set; }

        public Guid GrantID { get; set; }
        public virtual Grant Grant { get; set; }

        [Display(Name = "Amount (in €)")]
        [DefaultValue(0.0)]
        public double Amount { get; set; }

        [Display(Name = "Year")]   
        public int Year { get; set; }

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

    public class GrantBudgetViewModel
    {
        public Guid GrantBudgetID { get; set; }
        public Guid GrantID { get; set; }
        public Grant Grant { get; set; }
        public double Amount { get; set; }
        public int Year { get; set; }
    }

    public class GrantBudgetDbContext : DbContext
    {
        public DbSet<GrantBudget> GrantBudgets { get; set; }

        public GrantBudgetDbContext() : base("MACA") { }
    }
}
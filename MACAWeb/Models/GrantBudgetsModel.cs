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

        [Display(Name = "Grant Budget Type")]
        [Required(ErrorMessage = "Grant Budget Type is required!")]
        public Guid GrantBudgetsTypeID { get; set; }
        public virtual GrantBudgetsType GrantBudgetsType { get; set; }

        [Display(Name = "Amount (in €)")]
        [DefaultValue(0.0)]
        public double Amount { get; set; }

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

    public class GrantBudgetViewModel
    {
        public Guid GrantBudgetID { get; set; }
        public Guid GrantID { get; set; }

        [Display(Name = "Grant Budget Type")]
        [Required(ErrorMessage = "Grant Budget Type is required!")]
        public Guid GrantBudgetsTypeID { get; set; }
        public virtual GrantBudgetsType GrantBudgetsType { get; set; }

        [Display(Name = "Amount (in €)")]
        public double Amount { get; set; }

        [Display(Name = "Year")]
        public int Year { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}
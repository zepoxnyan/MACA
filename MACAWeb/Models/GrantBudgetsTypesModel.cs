using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MACAWeb.Models
{
    public class GrantBudgetsType
    {
        [Key]
        public Guid GrantBudgetsTypeID { get; set; }

        [Display(Name = "Grant Budget Type Name")]
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

    public class GrantBudgetsTypeViewModel
    {
        public Guid GrantBudgetsTypeID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class GrantBudgetsTypeDbContext : DbContext
    {
        public DbSet<GrantBudgetsType> GrantBudgetsTypes { get; set; }

        public GrantBudgetsTypeDbContext() : base("MACA") { }
    }
}
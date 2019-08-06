using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MACAWeb.Models
{
    public class Activity
    {
        [Key]
        public Guid ActivityID { get; set; }

        [Display(Name = "Activity Type")]
        [Required(ErrorMessage = "The activity type must be specified!")]
        public Guid ActivityTypeID { get; set; }
        public virtual ActivityType ActivityType { get; set; }

        [Display(Name = "Person")]
        [Required(ErrorMessage = "The person be specified!")]
        public Guid PersonID { get; set; }
        public virtual Person Person { get; set; }

        [Display(Name = "Weight")]
        [DefaultValue(1.0)]
        public double Weight { get; set; }

        [Display(Name = "Remark")]
        [DataType(DataType.MultilineText)]
        public string Remark { get; set; }

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

    public class ActivityViewModel
    {
        public Guid ActivityID { get; set; }

        [Display(Name = "Activity Type")]
        [Required(ErrorMessage = "The activity type must be specified!")]
        public Guid ActivityTypeID { get; set; }
        public virtual ActivityType ActivityType { get; set; }

        [Display(Name = "Person")]
        [Required(ErrorMessage = "The person be specified!")]
        public Guid PersonID { get; set; }
        public virtual Person Person { get; set; }

        [Display(Name = "Weight")]
        [DefaultValue(1.0)]
        public double Weight { get; set; }

        [Display(Name = "Remark")]
        [DataType(DataType.MultilineText)]
        public string Remark { get; set; }
    }
}
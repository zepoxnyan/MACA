using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MACAWeb.Models
{
    public class Teaching
    {
        [Key]        
        public Guid TeachingID { get; set; }

        [Display(Name = "Teaching Type")]
        [Required(ErrorMessage = "The teaching type must be specified!")]
        public Guid TeachingTypeID { get; set; }
        public virtual TeachingType TeachingType { get; set; }

        [Display(Name = "Subject")]
        [Required(ErrorMessage = "The subject must be specified!")]
        public Guid SubjectID { get; set; }
        public virtual Subject Subject { get; set; }

        [Display(Name = "Person")]
        [Required(ErrorMessage = "The person be specified!")]
        public Guid PersonID { get; set; }
        public virtual Person Person { get; set; }

        [Display(Name = "Hours per week")]
        [Required(ErrorMessage = "Hours must be specified!")]
        public double Hours { get; set; }
                
        [Display(Name = "Remark")]   
        [DataType(DataType.MultilineText)]
        public string Remark { get; set; }

        [Display(Name = "Weight")]
        [DefaultValue(1.0)]
        public double Weight { get; set; }

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

    public class TeachingVievModel
    {
        public Guid TeachingID { get; set; }

        [Display(Name = "Teaching Type")]
        [Required(ErrorMessage = "The teaching type must be specified!")]
        public Guid TeachingTypeID { get; set; }
        public virtual TeachingType TeachingType { get; set; }

        [Display(Name = "Subject")]
        [Required(ErrorMessage = "The subject must be specified!")]
        public Guid SubjectID { get; set; }
        public virtual Subject Subject { get; set; }

        [Display(Name = "Person")]
        [Required(ErrorMessage = "The person be specified!")]
        public Guid PersonID { get; set; }
        public virtual Person Person { get; set; }

        [Display(Name = "Hours per week")]
        [Required(ErrorMessage = "Hours must be specified!")]
        public double Hours { get; set; }

        [Display(Name = "Remark")]
        [DataType(DataType.MultilineText)]
        public string Remark { get; set; }

        [Display(Name = "Weight")]
        [DefaultValue(1.0)]
        public double Weight { get; set; }
    }
}
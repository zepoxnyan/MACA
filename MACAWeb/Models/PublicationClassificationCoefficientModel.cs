using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MACAWeb.Models
{
    public class PublicationClassificationCoefficient
    {
        [Key]        
        public Guid PublicationClassificationCoefficientID { get; set; }

        [Display(Name = "Publication Classification")]
        [Required(ErrorMessage = "Publication Classification is required!")]
        public Guid PublicationClassificationID { get; set; }
        public virtual PublicationClassification PublicationClassification { get; set; }

        [Display(Name = "Publication Type Group")]
        [Required(ErrorMessage = "Publication type group is required!")]
        public Guid PublicationTypeGroupID { get; set; }
        public virtual PublicationTypeGroup PublicationTypeGroup { get; set; }

        [Display(Name = "Coefficient")]
        [DefaultValue(0.0)]
        public double Coefficient { get; set; }

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

    public class PublicationClassificationCoefficientViewModel
    {
        public Guid PublicationClassificationCoefficientID { get; set; }

        [Display(Name = "Publication Classification")]
        [Required(ErrorMessage = "Publication Classification is required!")]
        public Guid PublicationClassificationID { get; set; }
        public virtual PublicationClassification PublicationClassification { get; set; }

        [Display(Name = "Publication Type Group")]
        [Required(ErrorMessage = "Publication type group is required!")]
        public Guid PublicationTypeGroupID { get; set; }
        public virtual PublicationTypeGroup PublicationTypeGroup { get; set; }

        [Display(Name = "Coefficient")]
        [DefaultValue(0.0)]
        public double Coefficient { get; set; }

        [Display(Name = "Year")]
        [Required(ErrorMessage = "Year is required!")]
        public int Year { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}
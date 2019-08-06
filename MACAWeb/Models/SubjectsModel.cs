using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MACAWeb.Models
{
    public class Subject
    {
        [Key]        
        public Guid SubjectID { get; set; }

        [Display(Name = "Subject Name")]
        [Required(ErrorMessage = "The name must be specified!")]
        public string Name { get; set; }

        /*[Display(Name = "Study Level")]
        [Required(ErrorMessage = "The study level must be specified!")]
        public Guid StudyLevelID { get; set; }
        public virtual StudyLevel StudyLevel { get; set; }*/

        [Display(Name = "Description")]   
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Academic Year")]
        [Required(ErrorMessage = "The academic year must be specified!")]
        public string Year { get; set; }        

        [Display(Name = "Semester")]
        [Required(ErrorMessage = "The semester must be specified!")]
        public int Semester { get; set; }

        [Display(Name = "AISCode")]
        [Required(ErrorMessage = "The AIS code must be specified!")]
        public string AISCode { get; set; }

        [Display(Name = "Short Name")]
        [Required(ErrorMessage = "The short name must be specified!")]
        public string ShortName { get; set; }

        [Display(Name = "Department")]
        [Required(ErrorMessage = "The department must be specified!")]
        public string Department { get; set; }

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

    public class SubjectVievModel
    {
        public Guid SubjectID { get; set; }

        [Display(Name = "Subject Name")]
        [Required(ErrorMessage = "The name must be specified!")]
        public string Name { get; set; }

        /*[Display(Name = "Study Level")]
        [Required(ErrorMessage = "The study level must be specified!")]
        public Guid StudyLevelID { get; set; }
        public virtual StudyLevel StudyLevel { get; set; }*/

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Academic Year")]
        [Required(ErrorMessage = "The academic year must be specified!")]
        public string Year { get; set; }

        [Display(Name = "Semester")]
        [Required(ErrorMessage = "The semester must be specified!")]
        public int Semester { get; set; }

        [Display(Name = "AISCode")]
        [Required(ErrorMessage = "The AIS code must be specified!")]
        public string AISCode { get; set; }

        [Display(Name = "Short Name")]
        [Required(ErrorMessage = "The short name must be specified!")]
        public string ShortName { get; set; }

        [Display(Name = "Department")]
        [Required(ErrorMessage = "The department must be specified!")]
        public string Department { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MACAWeb.Models
{
    public class Mentorship
    {
        [Key]        
        public Guid MentorshipID { get; set; }

        [Display(Name = "Thesis Type")]
        [Required(ErrorMessage = "The thesis type must be specified!")]
        public Guid ThesisTypeID { get; set; }
        public virtual ThesisType ThesisType { get; set; }

        [Display(Name = "Mentorship Type")]
        [Required(ErrorMessage = "The mentorship type must be specified!")]
        public Guid MentorshipTypeID { get; set; }
        public virtual MentorshipType MentorshipType { get; set; }

        [Display(Name = "Person")]
        [Required(ErrorMessage = "The person be specified!")]
        public Guid PersonID { get; set; }
        public virtual Person Person { get; set; }

        [Display(Name = "Student")]
        [Required(ErrorMessage = "Student must be specified!")]
        public string Student { get; set; }
                
        [Display(Name = "Thesis Title")]
        [Required(ErrorMessage = "Thesis title must be specified!")]
        public string ThesisTitle { get; set; }

        [Display(Name = "Remarks")]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        [Display(Name = "Started in Academic Year")]
        [Required(ErrorMessage = "Year must be specified!")]
        public string Year { get; set; }

        [Display(Name = "Semester")]
        [Required(ErrorMessage = "Semester must be specified!")]
        public int Semester { get; set; }

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

    public class MentorshipViewModel
    {
        public Guid MentorshipID { get; set; }

        [Display(Name = "Thesis Type")]
        [Required(ErrorMessage = "The thesis type must be specified!")]
        public Guid ThesisTypeID { get; set; }
        public virtual ThesisType ThesisType { get; set; }

        [Display(Name = "Mentorship Type")]
        [Required(ErrorMessage = "The mentorship type must be specified!")]
        public Guid MentorshipTypeID { get; set; }
        public virtual MentorshipType MentorshipType { get; set; }

        [Display(Name = "Person")]
        [Required(ErrorMessage = "The person be specified!")]
        public Guid PersonID { get; set; }
        public virtual Person Person { get; set; }

        [Display(Name = "Student")]
        [Required(ErrorMessage = "Student must be specified!")]
        public string Student { get; set; }

        [Display(Name = "Thesis Title")]
        [Required(ErrorMessage = "Thesis title must be specified!")]
        public string ThesisTitle { get; set; }

        [Display(Name = "Remarks")]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        [Display(Name = "Started in Academic Year")]
        [Required(ErrorMessage = "Year must be specified!")]
        public string Year { get; set; }

        [Display(Name = "Semester")]
        [Required(ErrorMessage = "Semester must be specified!")]
        public int Semester { get; set; }
    }
}
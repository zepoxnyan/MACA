using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MACAWeb.Models
{
    public class PublicationAuthor
    {
        [Key]        
        public Guid PublicationAuthorID { get; set; }

        [Display(Name = "Author")]
        [Required(ErrorMessage = "The author must be specified!")]
        public Guid AuthorID { get; set; }
        public virtual Author Author { get; set; }

        [Display(Name = "Publication")]
        [Required(ErrorMessage = "Publication must be specified!")]
        public Guid PublicationID { get; set; }
        public virtual Publication Publication { get; set; }

        [Display(Name = "Author Type")]
        [Required(ErrorMessage = "The author type must be specified!")]
        public Guid AuthorTypeID { get; set; }
        public virtual AuthorType AuthorType { get; set; }

        [Display(Name = "Percent")]
        [DefaultValue("1.0")]
        public double Percent { get; set; }

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

    public class PublicationAuthorVievModel
    {
        public Guid PublicationAuthorID { get; set; }

        [Display(Name = "Author")]
        [Required(ErrorMessage = "The author must be specified!")]
        public Guid AuthorID { get; set; }
        public virtual Author Author { get; set; }

        [Display(Name = "Publication")]
        [Required(ErrorMessage = "Publication must be specified!")]
        public Guid PublicationID { get; set; }
        public virtual Publication Publication { get; set; }

        [Display(Name = "Author Type")]
        [Required(ErrorMessage = "The author type must be specified!")]
        public Guid AuthorTypeID { get; set; }
        public virtual AuthorType AuthorType { get; set; }

        [Display(Name = "Percent")]
        [DefaultValue("1.0")]
        public double Percent { get; set; }
    }
}
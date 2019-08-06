using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MACAWeb.Models
{
    public class PublicationTypeLocal
    {
        [Key]        
        public Guid PublicationTypeLocalID { get; set; }

        [Display(Name = "Publication Type Group")]
        [Required(ErrorMessage = "Publication type group must be specified!")]
        public Guid PublicationTypeGroupID { get; set; }
        public virtual PublicationTypeGroup PublicationTypeGroup { get; set; }

        [Display(Name = "Local Publication Type Name")]
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

    public class PublicationTypeLocalViewModel
    {
        public Guid PublicationTypeLocalID { get; set; }

        [Display(Name = "Publication Type Group")]
        [Required(ErrorMessage = "Publication type group must be specified!")]
        public Guid PublicationTypeGroupID { get; set; }
        public virtual PublicationTypeGroup PublicationTypeGroup { get; set; }

        [Display(Name = "Local Publication Type Name")]
        [Required(ErrorMessage = "The name must be specified!")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}
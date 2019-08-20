using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MACAWeb.Models
{
    public class Interest
    {
        [Key]
        public Guid InterestID { get; set; }

        [Display(Name = "Person")]
        [Required(ErrorMessage = "The person be specified!")]
        public Guid PersonID { get; set; }
        public virtual Person Person { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "The title must be specified!")]
        public string Title { get; set; }

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
    }

    public class InterestView
    {
        public Guid InterestID { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "The title must be specified!")]
        public string Title { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}
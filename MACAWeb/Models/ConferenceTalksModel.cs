using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MACAWeb.Models
{
    public class ConferenceTalk
    {
        [Key]
        public Guid ConferenceTalkID { get; set; }

        [Display(Name = "Person")]
        [Required(ErrorMessage = "The person be specified!")]
        public Guid PersonID { get; set; }
        public virtual Person Person { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "The title must be specified!")]
        public string Title { get; set; }

        [Display(Name = "Conference name")]
        [Required(ErrorMessage = "Conference name must be specified!")]
        public string ConferenceName { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "City name must be specified!")]
        public string City { get; set; }

        [Display(Name = "Country")]
        [Required(ErrorMessage = "Country name must be specified!")]
        public string Country { get; set; }

        [Display(Name = "Year")]
        [Required(ErrorMessage = "Year name must be specified!")]
        public string Year { get; set; }

        [Display(Name = "Invited Talk")]
        public Boolean InvitedTalk { get; set; }

        [Display(Name = "PDF Link")]
        public string PdfLink { get; set; }

        [Display(Name = "Date Created")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Date Modified")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime DateModified { get; set; }
    }
    public class ConferenceTalkView
    {
        public Guid ConferenceTalkID { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "The title must be specified!")]
        public string Title { get; set; }

        [Display(Name = "Conference name")]
        [Required(ErrorMessage = "Conference name must be specified!")]
        public string ConferenceName { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "City name must be specified!")]
        public string City { get; set; }

        [Display(Name = "Country")]
        [Required(ErrorMessage = "Country name must be specified!")]
        public string Country { get; set; }

        [Display(Name = "Year")]
        [Required(ErrorMessage = "Year name must be specified!")]
        public string Year { get; set; }

        [Display(Name = "Invited Talk")]
        public Boolean InvitedTalk { get; set; }

        [Display(Name = "PDF Link")]
        public string PdfLink { get; set; }
    }
}
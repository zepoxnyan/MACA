using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MACAWeb.Models
{
    public class SocialLinkType
    {
        [Key]
        public Guid SocialLinkTypeID { get; set; }

        [Display(Name = "Social Link Name")]
        [Required(ErrorMessage = "The name must be specified!")]
        public string Name { get; set; }

        [Display(Name = "Social Link URL")]
        [Required(ErrorMessage = "The URL must be specified!")]
        public string UrlShortcut { get; set; }

        [Required(ErrorMessage = "Please fill in the description!")]
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

        [Display(Name = "Logo")]
        public byte[] Logo { get; set; }
    }

    public class SocialLinkTypeViewModel
    {
        public Guid SocialLinkTypeID { get; set; }

        [Display(Name = "Social Link Name")]
        [Required(ErrorMessage = "The name must be specified!")]
        public string Name { get; set; }

        [Display(Name = "Social Link URL")]
        [Required(ErrorMessage = "The URL must be specified!")]
        public string UrlShortcut { get; set; }

        [Required(ErrorMessage = "Please fill in the description!")]
        public string Description { get; set; }

        [Display(Name = "Logo")]
        public HttpPostedFileBase Logo { get; set; }
    }
}
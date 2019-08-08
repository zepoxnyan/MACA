using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MACAWeb.Models
{
    public class SocialLink
    {
        [Key]
        public Guid SocialLinkID { get; set; }


        public Guid PersonID { get; set; }
        public virtual Person Person { get; set; }

        [Required(ErrorMessage = "The link type must be specified!")]
        public Guid SocialLinkTypeID { get; set; }
        public virtual SocialLinkType SocialLinkType { get; set; }

        [Display(Name = "Profile identifier")]
        public string ProfileUrl { get; set; }

        public override string ToString()
        {
            return SocialLinkType.UrlShortcut+ProfileUrl;
        }
    }
    public class SocialLinkViewModel
    {
        public Guid SocialLinkID { get; set; }

        [Display(Name = "Profile identifier")]
        public string ProfileUrl { get; set; }


        [Display(Name = "Social Link Type")]
        [Required(ErrorMessage = "The link type must be specified!")]
        public Guid SocialLinkTypeID { get; set; }

        public virtual SocialLinkType SocialLinkType { get; set; }
    }
}
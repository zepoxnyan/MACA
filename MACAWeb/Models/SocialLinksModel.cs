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

        [Required(ErrorMessage = "The link type must be specified!")]
        public Guid SocialLinkTypeID { get; set; }

    }
}
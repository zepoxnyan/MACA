﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MACAWeb.Models
{
    public class MentorshipType
    {
        [Key]        
        public Guid MentorshipTypeID { get; set; }

        [Display(Name = "Mentorship Type Name")]
        [Required(ErrorMessage = "The name must be specified!")]
        public string Name { get; set; }

        [Display(Name = "AISCode")]
        [DefaultValue(0)]
        public int AISCode { get; set; }

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

    public class MentorshipTypeViewModel
    {
        public Guid MentorshipTypeID { get; set; }

        [Display(Name = "Mentorship Type Name")]
        public string Name { get; set; }

        [Display(Name = "AISCode")]
        [DefaultValue(0)]
        public int AISCode { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}
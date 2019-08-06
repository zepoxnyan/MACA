using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MACAWeb.Models
{
    public class FAQ
    {
        [Key]        
        public Guid FaqID { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "Enter a title!")]
        public string Title { get; set; }

        [Display(Name = "Question")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Enter a question!")]
        public string Question { get; set; }

        [Display(Name = "Answer")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Enter an answer!")]
        public string Answer { get; set; }

        [Display(Name = "Author")]
        public string Author { get; set; }

        [Display(Name = "Created")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Modified")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime DateModified { get; set; }
        
        public Guid UserCreatedID { get; set; }

        public Guid UserModifiedID { get; set; }
    }

    public class FAQViewModel
    {
        public Guid FaqID { get; set; }
        public string Title { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Author { get; set; }
    }
}
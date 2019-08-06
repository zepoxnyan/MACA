using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MACAWeb.Models
{
    public class News
    {
        [Key]
        public Guid NewsID { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "Set the title!")]
        public string Title { get; set; }

        [Display(Name = "Abstract")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Enter the abstract!")]
        public string Abstract { get; set; }

        [Display(Name = "Content")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Enter the content!")]
        public string Text { get; set; }

        [Display(Name = "Author")]
        [Required(ErrorMessage = "Enter the author!")]
        public string Author { get; set; }

        [Display(Name = "Source")]
        public string Source { get; set; }

        [Display(Name = "Source link")]
        public string SourceLink { get; set; }

        [Display(Name = "Publication date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d. M. yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Enter publication date!")]
        public DateTime DatePublished { get; set; }

        [Display(Name = "Image")]
        public byte[] Image { get; set; }

        [Display(Name = "Image author")]
        public string ImageAuthor { get; set; }

        [Display(Name = "Image description")]
        public string ImageDescription { get; set; }

        [Display(Name = "Created")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Modified")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime DateModified { get; set; }

        [DataType(DataType.Text)]
        [StringLength(128)]
        public string UserCreatedID { get; set; }

        [DataType(DataType.Text)]
        [StringLength(128)]
        public string UserModifiedID { get; set; }
    }

    public class NewsViewModel
    {
        public Guid NewsID { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "Set the title!")]
        public string Title { get; set; }

        [Display(Name = "Abstract")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Enter the abstract!")]
        public string Abstract { get; set; }

        [Display(Name = "Content")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Enter the content!")]
        public string Text { get; set; }

        [Display(Name = "Author")]
        [Required(ErrorMessage = "Enter the author!")]
        public string Author { get; set; }

        [Display(Name = "Source")]
        public string Source { get; set; }

        [Display(Name = "Source link")]
        public string SourceLink { get; set; }

        [Display(Name = "Publication date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d. M. yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Enter publication date!")]
        public DateTime DatePublished { get; set; }

        [Display(Name = "Image")]
        public HttpPostedFileBase Image { get; set; }

        [Display(Name = "Image author")]
        public string ImageAuthor { get; set; }

        [Display(Name = "Image description")]
        public string ImageDescription { get; set; }
    }
}
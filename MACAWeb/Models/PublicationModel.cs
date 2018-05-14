using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MACAWeb.Models
{
    public class Publication
    {
        [Key]
        public Guid PublicationID { get; set; }

        [Display(Name = "Publication Type")]
        [Required(ErrorMessage = "Choose publication type!")]
        public Guid PublicationTypeID { get; set; }
        public virtual PublicationType PublicationType { get; set; }

        [Display(Name = "Local Publication Type")]
        [Required(ErrorMessage = "Choose local publication type!")]
        public Guid PublicationTypeLocalID { get; set; }
        public virtual PublicationTypeLocal PublicationTypeLocal { get; set; }

        [Display(Name = "Publication Classification")]
        [Required(ErrorMessage = "Choose publication classification!")]
        public Guid PublicationClassificationID { get; set; }
        public virtual PublicationClassification PublicationClassification { get; set; }

        [Display(Name = "Publication Status")]
        [Required(ErrorMessage = "Choose publication status!")]
        public Guid PublicationStatusID { get; set; }
        public virtual PublicationStatus PublicationStatus { get; set; }

        [Display(Name = "Nazov")]
        [Required(ErrorMessage = "Enter the title!")]
        public string Title { get; set; }
        
        [Display(Name = "Journal")]
        public string Journal { get; set; }

        [Display(Name = "Year")]
        [Required(ErrorMessage = "Enter the year!")]
        public int Year { get; set; }

        [Display(Name = "Volume")]
        public int? Volume { get; set; }

        [Display(Name = "Issue")]
        public int? Issue { get; set; }

        [Display(Name = "Pages")]
        public string Pages { get; set; }

        [Display(Name = "DOI")]
        public string DOI { get; set; }

        [Display(Name = "Hyperlink")]
        public string Link { get; set; }

        [Display(Name = "Preprint link")]
        public string PreprintLink { get; set; }

        [Display(Name = "Note")]
        public string Note { get; set; }

        [Display(Name = "Editor(s)")]
        public string Editors { get; set; }

        [Display(Name = "Publisher")]
        public string Publisher { get; set; }

        [Display(Name = "Series")]
        public string Series { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Edition")]
        public string Edition { get; set; }

        [Display(Name = "Book Title")]
        public string BookTitle { get; set; }

        [Display(Name = "Organization")]
        public string Organization { get; set; }

        [Display(Name = "Chapter")]
        public string Chapter { get; set; }
        
        [Display(Name = "Keywords")]
        public string Keywords { get; set; }

        [Display(Name = "KeywordsEN")]
        public string KeywordsEN { get; set; }

        [Display(Name = "Anglicky Nazov")]
        public string TitleEN { get; set; }

        [Display(Name = "Abstract")]
        [DataType(DataType.MultilineText)]
        public string Abstract { get; set; }

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

    public class PublicationViewModel
    {
        public Guid PublicationID { get; set; }

        [Display(Name = "Publication Type")]
        [Required(ErrorMessage = "Choose publication type!")]
        public Guid PublicationTypeID { get; set; }
        public virtual PublicationType PublicationType { get; set; }

        [Display(Name = "Local Publication Type")]
        [Required(ErrorMessage = "Choose local publication type!")]
        public Guid PublicationTypeLocalID { get; set; }
        public virtual PublicationTypeLocal PublicationTypeLocal { get; set; }

        [Display(Name = "Publication Classification")]
        [Required(ErrorMessage = "Choose publication classification!")]
        public Guid PublicationClassificationID { get; set; }
        public virtual PublicationClassification PublicationClassification { get; set; }

        [Display(Name = "Publication Status")]
        [Required(ErrorMessage = "Choose publication status!")]
        public Guid PublicationStatusID { get; set; }
        public virtual PublicationStatus PublicationStatus { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "Enter the title!")]
        public string Title { get; set; }

        [Display(Name = "Journal")]
        public string Journal { get; set; }

        [Display(Name = "Year")]
        [Required(ErrorMessage = "Enter the year!")]
        public int Year { get; set; }

        [Display(Name = "Volume")]
        public int? Volume { get; set; }

        [Display(Name = "Issue")]
        public int? Issue { get; set; }

        [Display(Name = "Pages")]
        public string Pages { get; set; }

        [Display(Name = "DOI")]
        public string DOI { get; set; }

        [Display(Name = "Hyperlink")]
        public string Link { get; set; }

        [Display(Name = "Preprint link")]
        public string PreprintLink { get; set; }

        [Display(Name = "Note")]
        public string Note { get; set; }

        [Display(Name = "Editor(s)")]
        public string Editors { get; set; }

        [Display(Name = "Publisher")]
        public string Publisher { get; set; }

        [Display(Name = "Series")]
        public string Series { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Edition")]
        public string Edition { get; set; }

        [Display(Name = "Book Title")]
        public string BookTitle { get; set; }

        [Display(Name = "Organization")]
        public string Organization { get; set; }

        [Display(Name = "Chapter")]
        public string Chapter { get; set; }

        [Display(Name = "Keywords")]
        public string Keywords { get; set; }

        [Display(Name = "Abstract")]
        [DataType(DataType.MultilineText)]
        public string Abstract { get; set; }

        [Display(Name = "KeywordsEN")]
        public string KeywordsEN { get; set; }

        [Display(Name = "Anglicky Nazov")]
        public string TitleEN { get; set; }
    }

    public class PublicationsDbContext : DbContext
    {
        public DbSet<Publication> Publications { get; set; }
        public DbSet<PublicationType> PublicationTypes { get; set; }
        public DbSet<PublicationTypeLocal> PublicationTypesLocal { get; set; }
        public DbSet<PublicationClassification> PublicationClassifications { get; set; }
        public DbSet<PublicationStatus> PublicationStatus { get; set; }

        public PublicationsDbContext() : base("MACA") { }
    }
}
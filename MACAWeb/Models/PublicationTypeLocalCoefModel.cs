using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MACAWeb.Models
{
    public class PublicationTypeLocalCoef
    {
        [Key]        
        public Guid PublicationTypeLocalCoefID { get; set; }

        [Display(Name = "Local Publication Type")]
        [Required(ErrorMessage = "Local publication type must be specified!")]
        public Guid PublicationTypeLocalID { get; set; }
        public virtual PublicationTypeLocal PublicationTypeLocal { get; set; }

        [Display(Name = "Year")]
        [Required(ErrorMessage = "The year must be specified!")]
        public int Year { get; set; }

        [Display(Name = "Coefficient")]
        [Required(ErrorMessage = "The coefficient must be specified!")]
        public double Coefficient { get; set; }

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

    public class PublicationTypeLocalCoefViewModel
    {
        public Guid PublicationTypeLocalCoefID { get; set; }

        [Display(Name = "Local Publication Type")]
        [Required(ErrorMessage = "Local publication type must be specified!")]
        public Guid PublicationTypeLocalID { get; set; }
        public virtual PublicationTypeLocal PublicationTypeLocal { get; set; }

        [Display(Name = "Year")]
        [Required(ErrorMessage = "The year must be specified!")]
        public int Year { get; set; }

        [Display(Name = "Coefficient")]
        [Required(ErrorMessage = "The coefficient must be specified!")]
        public double Coefficient { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }

    public class PublicationTypeLocalCoefDbContext : DbContext
    {
        public DbSet<PublicationTypeLocalCoef> PublicationTypesLocalCoef { get; set; }
        public DbSet<PublicationTypeLocal> PublicationTypesLocal { get; set; }

        public PublicationTypeLocalCoefDbContext() : base("MACA") { }
    }
}
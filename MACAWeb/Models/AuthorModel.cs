using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MACAWeb.Models
{
    public class Author
    {
        [Key]        
        public Guid AuthorID { get; set; }
        
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last name must be specified!")]
        public string Surname { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First name must be specified!")]
        public string FirstName { get; set; }

        [Display(Name = "CREPC Code")]
        public string CREPCCode { get; set; }

        [Display(Name = "ORC ID")]
        public string ORCID { get; set; }

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

    public class AuthorVievModel
    {
        public Guid AuthorID { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last name must be specified!")]
        public string Surname { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First name must be specified!")]
        public string FirstName { get; set; }

        [Display(Name = "CREPC Code")]
        public string CREPCCode { get; set; }

        [Display(Name = "ORC ID")]
        public string ORCID { get; set; }
    }

    public class AuthorsDbContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<AuthorType> AuthorTypes { get; set; }
        
        public AuthorsDbContext() : base("MACA") { }
    }
}
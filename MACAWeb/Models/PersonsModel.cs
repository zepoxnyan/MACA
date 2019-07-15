using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MACAWeb.Models
{
    public class Person
    {
        [Key]
        public Guid PersonID { get; set; }

        [Display(Name = "Surname")]
        [Required(ErrorMessage = "Enter the surname!")]
        public string Surname { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Enter the name!")]
        public string Name { get; set; }

        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "Enter the full name with titles!")]
        public string FullName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "AuthorID")]
        public Guid? AuthorID { get; set; }

        [Display(Name = "AISID")]
        public string AISID { get; set; }

        [Display(Name = "Image")]
        public byte[] Image { get; set; }


        [Display(Name = "ImageThumb")]
        public byte[] ImageThumb { get; set; }

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

    public class PersonViewModel
    {
        public Guid PersonID { get; set; }

        [Display(Name = "Surname")]
        [Required(ErrorMessage = "Enter the surname!")]
        public string Surname { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Enter the name!")]
        public string Name { get; set; }

        [Display(Name = "FullName")]
        [Required(ErrorMessage = "Enter the fullname with titles!")]
        public string FullName { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Image")]
        public HttpPostedFileBase Image { get; set; }

        [Display(Name = "ImageThumb")]
        public HttpPostedFileBase ImageThumb { get; set; }

    }

    public class PersonsDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }        

        public PersonsDbContext() : base("MACA") { }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MACAWeb.Models
{
    public class TeamMember
    {
        [Key]
        public Guid TeamMemberID { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Enter first name!")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Enter last name!")]
        public string LastName { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Enter person's description!")]
        public string Description { get; set; }

        [Display(Name = "Affiliation")]
        public string Affiliation { get; set; }

        [Display(Name = "Homepage Link")]
        public string HomepageLink { get; set; }

        [Display(Name = "Image")]
        [Required(ErrorMessage = "Image is required!")]
        public byte[] Image { get; set; }

        public int PagePosition { get; set; }

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

    public class TeamMemberEditViewModel
    {
        public Guid TeamMemberID { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Enter first name!")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Enter last name!")]
        public string LastName { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Enter person's description!")]
        public string Description { get; set; }

        [Display(Name = "Affiliation")]
        public string Affiliation { get; set; }

        [Display(Name = "Homepage Link")]
        public string HomepageLink { get; set; }

        [Display(Name = "Image")]
        public HttpPostedFileBase Image { get; set; }
    }

    public class TeamMemberCreateViewModel
    {
        public Guid TeamMemberID { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Enter first name!")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Enter last name!")]
        public string LastName { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Enter person's description!")]
        public string Description { get; set; }

        [Display(Name = "Affiliation")]
        public string Affiliation { get; set; }

        [Display(Name = "Homepage Link")]
        public string HomepageLink { get; set; }

        [Display(Name = "Image")]
        [Required(ErrorMessage = "Image is required!")]
        public HttpPostedFileBase Image { get; set; }
    }

    public class TeamMemberDbContext : DbContext
    {
        public DbSet<TeamMember> TeamMembers { get; set; }

        public TeamMemberDbContext() : base("MACA") { }
    }
}
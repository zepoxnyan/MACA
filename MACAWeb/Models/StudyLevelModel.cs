using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MACAWeb.Models
{
    public class StudyLevel
    {
        [Key]        
        public Guid StudyLevelID { get; set; }

        [Display(Name = "Study Level Name")]
        [Required(ErrorMessage = "The study level name must be specified!")]
        public string Name { get; set; }

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

    public class StudyLevelViewModel
    {
        public Guid StudyLevelID { get; set; }

        [Display(Name = "Study Level Name")]
        [Required(ErrorMessage = "The study level name must be specified!")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }

    public class StudyLevelDbContext : DbContext
    {
        public DbSet<StudyLevel> StudyLevels { get; set; }

        public StudyLevelDbContext() : base("MACA") { }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MACAWeb.Models
{
    public class Position
    {
        [Key]        
        public Guid PositionID { get; set; }

        [Required]
        public Guid PersonID { get; set; }

        [Display(Name = "Position Type")]
        [Required(ErrorMessage = "The position type must be specified!")]
        public Guid PositionTypeID { get; set; }
        public virtual PositionType PositionType { get; set; }

        [Display(Name = "Description")]   
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        
        [Display(Name = "Year")]
        [Required(ErrorMessage = "The year must be specified!")]
        public int Year { get; set; }

        [Display(Name = "Semester")]
        [Required(ErrorMessage = "The semester must be specified!")]
        public int Semester { get; set; }

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

    public class PositionViewModel
    {
        public Guid PositionID { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Year")]
        [Required(ErrorMessage = "The year must be specified!")]
        public int Year { get; set; }

        [Display(Name = "Semester")]
        [Required(ErrorMessage = "The semester must be specified!")]
        public int Semester { get; set; }

        [Display(Name = "Position Type")]
        [Required(ErrorMessage = "The position type must be specified!")]
        public Guid PositionTypeID { get; set; }
        public virtual PositionType PositionType { get; set; }
    }

    public class PositionDbContext : DbContext
    {
        public DbSet<Position> Positions { get; set; }
        public DbSet<PositionType> PositionTypes { get; set; }

        public PositionDbContext() : base("MACA") { }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MACAWeb.Models
{
    public class EventType
    {
        [Key]        
        public Guid EventTypeID { get; set; }

        [Display(Name = "Koda")]
        [Required(ErrorMessage = "Koda dogodka mora biti določena!")]
        public string Code { get; set; }

        [Display(Name = "Opis")]   
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Datum vnosa")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Datum spremembe")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime DateModified { get; set; }
        
        public Guid UserCreatedID { get; set; }

        public Guid UserModifiedID { get; set; }
    }

    public class EventTypeDbContext : DbContext
    {
        public DbSet<EventType> EventTypes { get; set; }

        public EventTypeDbContext() : base("MACA") { }
    }
}
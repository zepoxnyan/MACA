using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MACAWeb.Models
{
    public class Event
    {
        [Key]        
        public Guid EventID { get; set; }

        [ForeignKey("EventTypes")]
        public Guid EventTypeID { get; set; }

        [Display(Name = "Opis")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Datum dogodka")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Uporabnik")]
        public Guid? UserCreatedID { get; set; }
    }

    public class EventDbContext : DbContext
    {
        public DbSet<Event> Events { get; set; }

        public EventDbContext() : base("MACA") { }
    }
}
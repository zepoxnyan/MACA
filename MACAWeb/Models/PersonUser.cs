﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MACAWeb.Models
{
    public class PersonUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid PersonUserID { get; set; }
                      
        [StringLength(128)]
        public String UserID { get; set; }

        public Guid PersonID { get; set; }               
    }

    public class PersonUserDbContext : DbContext
    {
        public DbSet<PersonUser> PersonUser { get; set; }

        public PersonUserDbContext() : base("MACA") { }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MACAWeb.Models
{
    public class Parameter
    {
        [Key]
        public Guid ParameterId { get; set; }

        public Guid FunctionId { get; set; }

        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public Guid UserCreatedId { get; set; }
        public Guid UserModifiedId { get; set; }
    }

    public class ParameterViewModel
    {
        public Guid ParameterId { get; set; }
        public Guid FunctionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class ParameterDbContext : DbContext
    {
        public DbSet<Parameter> Parameters { get; set; }
        public DbSet<Function> Functions { get; set; }

        public ParameterDbContext() : base("MACA") { }
    }
}
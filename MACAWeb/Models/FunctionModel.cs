using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MACAWeb.Models
{
    public class Function
    {
        [Key]
        public Guid FunctionId { get; set; }

        [Required(ErrorMessage = "Select the feature type")]
        public Guid FeatureTypeId { get; set; }

        public virtual FeatureType FeatureType { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Description of input")]
        public string InputDescription { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Description of output")]
        public string OutputDescription { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Created")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Modified")]
        public DateTime DateModified { get; set; }

        [Display(Name = "Created by User")]
        public Guid UserCreatedId { get; set; }

        [Display(Name = "Modified by User")]
        public Guid UserModifiedId { get; set; }
    }

    public class FunctionViewModel
    {
        public Guid FunctionId { get; set; }

        [Required(ErrorMessage = "Select the feature type")]
        public Guid FeatureTypeId { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayName("Description of input")]
        public string InputDescription { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayName("Description of output")]
        public string OutputDescription { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        public string Description { get; set; }
    }

    public class FunctionDbContext : DbContext
    {
        public DbSet<Function> Functions { get; set; }
        public DbSet<FeatureType> FeatureTypes { get; set; }

        public FunctionDbContext() : base("MACA") { }
    }
}
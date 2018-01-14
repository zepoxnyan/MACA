using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MACAWeb.Models
{
    public class DataFormat
    {
        [Key]
        public Guid DataFormatId { get; set; }

        [Required(ErrorMessage = "Select a feature type")]
        public Guid FeatureTypeId { get; set; }

        public virtual FeatureType FeatureType { get; set; }

        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
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

    public class DataFormatViewModel
    {
        public Guid DataFormatId { get; set; }

        [Required(ErrorMessage = "Select the feature type")]
        public Guid FeatureTypeId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class DataFormatDbContext : DbContext
    {
        public DbSet<DataFormat> DataFormats { get; set; }
        public DbSet<FeatureType> FeatureTypes { get; set; }

        public DataFormatDbContext() : base("MACA") { }
    }
}
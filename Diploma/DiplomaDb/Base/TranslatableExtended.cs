using Diploma.Models;
using Diploma.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Diploma.DiplomaDb
{
    public abstract class TranslatableExtended : Translatable
    {
        [Display(Name = "DescriptionEN", ResourceType = typeof(Resource))]
        public string DescriptionEN { get; set; }

        [Display(Name = "DescriptionRU", ResourceType = typeof(Resource))]
        public string DescriptionRU { get; set; }

        [Display(Name = "DescriptionUA", ResourceType = typeof(Resource))]
        public string DescriptionUA { get; set; }
    }
}
using Diploma.Manager;
using Diploma.Models;
using Diploma.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Diploma.DiplomaDb
{
    public abstract class Translatable
    {
        public int Id { get; set; }

        [Display(Name = "CaptionEN", ResourceType = typeof(Resource))]
        public string CaptionEN { get; set; }

        [Display(Name = "CaptionRU", ResourceType = typeof(Resource))]
        public string CaptionRU { get; set; }

        [Display(Name = "CaptionUA", ResourceType = typeof(Resource))]
        public string CaptionUA { get; set; }

        public bool IsValid()
        {
            if (CaptionEN == null) CaptionEN = "";
            if (CaptionRU == null) CaptionRU = "";
            if (CaptionUA == null) CaptionUA = "";

            CaptionEN.Trim();
            CaptionRU.Trim();
            CaptionUA.Trim();

            return (CaptionEN == "" && CaptionRU == "" && CaptionUA == "") ? false : true;
        }
    }
}
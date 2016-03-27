using Diploma.DiplomaDb;
using Diploma.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Diploma.Controllers;
using System.ComponentModel.DataAnnotations;
using Diploma.Resources;

namespace Diploma.Models
{
    public class ViewModel
    {
        public ViewModel()
        {
            this.ViewManager = BaseController.GetViewManager();
        }
        protected EntityViewManager ViewManager { get; private set; }
    }

    public class TranslatableViewModel : ViewModel
    {
        public TranslatableViewModel(Translatable record)
        {
            Id = record.Id;

            foreach (var language in ViewManager.Languages)
            {
                switch (language)
                {
                    case "en": Caption = record.CaptionEN; break;
                    case "ru": Caption = record.CaptionRU; break;
                    case "uk": Caption = record.CaptionUA; break;
                }
                if (Caption != "") { Language = language; break; }
            }
        }

        protected string Language { get; set; }
        public int Id { get; set; }

        [Display(Name = "Caption", ResourceType = typeof(Resource))]
        public string Caption { get; set; }
    }

    public class TranslatableExtendedViewModel : TranslatableViewModel
    {
        public TranslatableExtendedViewModel(TranslatableExtended record)
            : base(record)
        {
            switch (Language)
            {
                case "en": Description = record.DescriptionEN; break;
                case "ru": Description = record.DescriptionRU; break;
                case "uk": Description = record.DescriptionUA; break;
            }
        }

        [Display(Name = "Description", ResourceType = typeof(Resource))]
        public string Description { get; set; }
    }
}
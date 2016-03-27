using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Diploma.Models;
using Diploma.DiplomaDb;
using System.Threading;
using System.Globalization;
using System.Web.Services;

namespace Diploma.Manager
{
    public class EntityViewManager
    {
        private string[] _languagePriority = new string[] { "uk", "ru", "en" };

        public EntityViewManager()
        {
            string language = HttpContext.Current.Request.RequestContext.RouteData.Values["language"].ToString();

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(language);

            Languages = GetLanguagePriorityStartingFrom(language);
        }

        public List<string> Languages { get; private set; }
        private List<string> GetLanguagePriorityStartingFrom(string language)
        {
            var languages = _languagePriority.ToList();
            languages.Remove(language);
            languages.Insert(0, language);

            return languages;
        }
    }
}
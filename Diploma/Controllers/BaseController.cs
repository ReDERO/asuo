using Diploma.DiplomaDb;
using Diploma.Manager;
using Diploma.Security;
using System;
using System.Data;
using System.Data.Entity;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Diploma.Controllers
{
    public abstract class BaseController : System.Web.Mvc.Controller
    {
        private static EntityViewManager _viewManager;
        protected DataContext db = new DataContext();
        protected virtual new UserPrincipal User
        {
            get { return HttpContext.User as UserPrincipal; }
        }
        public static EntityViewManager GetViewManager()
        {
            return _viewManager;
        }
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            _viewManager = new EntityViewManager();
        }

        public async Task<bool> IsAllowed(string entityName, string actionName)
        {
            var user = CurrentUser;
            if (user != null)
            {
                var action = await db.Actions.FirstAsync(a => a.Entity.Name == entityName && a.Name == actionName);
                var result = action.Roles.Intersect(user.Roles);
                return result.Count() > 0;
            }
            return false;
        }

        public User CurrentUser
        {
            get
            {
                return (User == null) ? null : db.Users.Find(User.UserId);
            }
        }
    }
}
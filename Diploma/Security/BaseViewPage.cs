using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Diploma.Security
{
    public abstract class BaseViewPage : WebViewPage
    {
        public virtual new UserPrincipal User
        {
            get { return base.User as UserPrincipal; }
        }
    }
    public abstract class BaseViewPage<TModel> : WebViewPage<TModel>
    {
        public virtual new UserPrincipal User
        {
            get { return base.User as UserPrincipal; }
        }
    }
}
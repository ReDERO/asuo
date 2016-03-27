using Diploma.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Diploma.Helpers
{
    public static class DiplomaHelper
    {
        // As the text the: "<span class='glyphicon glyphicon-plus'></span>" can be entered
        public static MvcHtmlString NoEncodeActionLink(this HtmlHelper htmlHelper,
                                             string text, string title, string action,
                                             string controller,
                                             object routeValues = null,
                                             object htmlAttributes = null)
        {
            UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            TagBuilder builder = new TagBuilder("a");
            builder.InnerHtml = text;
            builder.Attributes["title"] = title;
            builder.Attributes["href"] = urlHelper.Action(action, controller, routeValues);
            builder.MergeAttributes(new RouteValueDictionary(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)));

            return MvcHtmlString.Create(builder.ToString());
        }

        public static MvcHtmlString Breadcrumbs(this HtmlHelper htmlHelper, BreadCrumbsModel bread)
        {
            UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            var last = bread.Count() - 1;
            TagBuilder breadcrumbs = new TagBuilder("ul");
            breadcrumbs.AddCssClass("breadcrumb");

            if (last == 0)
            {
                breadcrumbs.InnerHtml = "<li class='active'>" + bread[0].Caption + "</li>";
            }
            else if (last == 1)
            {
                breadcrumbs.InnerHtml = "<li><a href='" + bread[1].URL + "'>" + bread[1].Caption + "</a></li>" +
                                        "<li class='active'>" + bread[0].Caption + "</li>";
            }
            else if (last == 2)
            {
                breadcrumbs.InnerHtml = "<li><a href='" + bread[2].URL + "'>" + bread[2].Caption + "</a></li>" +
                                        "<li><a href='" + bread[1].URL + "'>" + bread[1].Caption + "</a></li>" +
                                        "<li class='active'>" + bread[0].Caption + "</li>";
            }
            else if (last > 2)
            {
                breadcrumbs.InnerHtml = "<li><a href='" + bread[last].URL + "'>" + bread[last].Caption + "</a></li>" +
                                        "<li>...</li>" +
                                        "<li><a href='" + bread[1].URL + "'>" + bread[1].Caption + "</a></li>" +
                                        "<li class='active'>" + bread[0].Caption + "</li>";
            }

            return MvcHtmlString.Create(breadcrumbs.ToString());
        }

        public static MvcHtmlString Toggle(this HtmlHelper htmlHelper,
                                             bool value, string id,
                                             string dataOn, string dataOff)
        {
            UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            TagBuilder builder = new TagBuilder("input");
            builder.Attributes["type"] = "checkbox";
            builder.Attributes["data-toggle"] = "toggle";
            builder.Attributes["data-on"] = dataOn;
            builder.Attributes["data-off"] = dataOff;
            builder.Attributes["id"] = id;
            builder.Attributes["name"] = id;

            if (value) builder.Attributes["checked"] = "checked";

            return MvcHtmlString.Create(builder.ToString());
        }
    }
}
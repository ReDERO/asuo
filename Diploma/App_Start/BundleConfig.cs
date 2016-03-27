using System.Web;
using System.Web.Optimization;

namespace Diploma
{
    public class BundleConfig
    {
        //Дополнительные сведения об объединении см. по адресу: http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/toggle").Include(
                        "~/Scripts/bootstrap-toggle.js"));

            bundles.Add(new ScriptBundle("~/bundles/jplist").Include(
                        "~/Scripts/jplist-core.min.js",
                        "~/Scripts/jplist.sort-bundle.min.js",
                        "~/Scripts/jplist.textbox-control.min.js",
                        "~/Scripts/jplist.pagination-bundle.min.js",
                        "~/Scripts/jplist.history-bundle.min.js",
                        "~/Scripts/jplist.filter-toggle-bundle.min.js",
                        "~/Scripts/jplist.views-control.min.js",
                        "~/Scripts/jplist.preloader-control.min.js",
                        "~/Scripts/jplist.filter-dropdown-bundle.min.js",
                        "~/Scripts/skillRequest.js"));

            bundles.Add(new StyleBundle("~/Content/jplist").Include(
                        "~/Content/jplist-core.min.css",
                        "~/Content/jplist-textbox-control.min.css",
                        "~/Content/jplist-pagination-bundle.min.css",
                        "~/Content/jplist-history-bundle.min.css",
                        "~/Content/jplist-filter-toggle-bundle.min.css",
                        "~/Content/jplist-views-control.min.css",
                        "~/Content/jplist-preloader-control.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Используйте версию Modernizr для разработчиков, чтобы учиться работать. Когда вы будете готовы перейти к работе,
            // используйте средство сборки на сайте http://modernizr.com, чтобы выбрать только нужные тесты.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/chosen.css",
                      "~/Content/site.css",
                      "~/Content/bootstrap-toggle.css"));
        }
    }
}

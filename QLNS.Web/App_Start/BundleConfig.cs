using System.Web;
using System.Web.Optimization;

namespace QLNS.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*")
                        .Include("~/Scripts/AdditionValidation.js")
                        //config custom validation scripts
                        );

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/assets/appcss")
                .Include("~/assets/css/bootstrap.min.css")
                .Include("~/assets/font-awesome/4.5.0/css/font-awesome.min.css")
                .Include("~/assets/css/fonts.googleapis.com.css")
                .Include("~/assets/css/ace-skins.min.css")
                .Include("~/assets/css/ace-rtl.min.css")
                .Include("~/Content/hinet-table.css")
                .Include("~/assets/css/jquery.gritter.min.css")
                .Include("~/assets/css/chosen.min.css")
                );

            bundles.Add(new ScriptBundle("~/assets/appjs")
                .Include("~/assets/js/jquery-2.1.4.min.js")
                .Include("~/assets/js/bootstrap.min.js")
                .Include("~/assets/js/jquery-ui.custom.min.js")
                .Include("~/Scripts/jquery.unobtrusive-ajax.js")
                .Include("~/assets/js/jquery.ui.touch-punch.min.js")
                .Include("~/assets/js/jquery.easypiechart.min.js")
                .Include("~/assets/js/jquery.sparkline.index.min.js")
                .Include("~/assets/js/jquery.flot.min.js")
                .Include("~/assets/js/jquery.flot.pie.min.js")
                .Include("~/assets/js/jquery.flot.resize.min.js")
                .Include("~/assets/js/ace-elements.min.js")
                .Include("~/assets/js/ace.min.js")
                .Include("~/assets/js/jquery.gritter.min.js")
                .Include("~/assets/js/bootbox.js")
                .Include("~/assets/js/chosen.jquery.min.js")
                .Include("~/Scripts/jquery.validate*")
                .Include("~/Scripts/jquery-hinet-table.js")
                .Include("~/Scripts/LayoutAce.js")
                .Include("~/Scripts/UploadTool.js")
                .Include("~/Scripts/Common.js")
                );
            bundles.Add(new ScriptBundle("~/bundles/datepicker").Include(
           "~/Scripts/bootstrap-datepicker.js",
           "~/Scripts/locales/bootstrap-datepicker.*"));

            bundles.Add(new StyleBundle("~/Content/datepicker").Include(
            "~/Content/bootstrap-datepicker.css"));

            //sắp xếp bundles theo thứ tự không theo bảng chữ cái (mặc định)
            bundles.FileSetOrderList.Clear();
        }
    }
}

using System.Web;
using System.Web.Optimization;

namespace FoodOrdering63131236
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Add Style to bundle
            bundles.Add(new StyleBundle("~/Admin/css").Include("~/Asset/Admin/plugins/fontawesome-free/css/all.min.css",
                                                               "~/Asset/Admin/plugins/tempusdominus-bootstrap-4/css/tempusdominus-bootstrap-4.min.css",
                                                               "~/Asset/Admin/plugins/icheck-bootstrap/icheck-bootstrap.min.css",
                                                               "~/Asset/Admin/plugins/jqvmap/jqvmap.min.css",
                                                               "~/Asset/Admin/dist/css/adminlte.min.css",
                                                               "~/Asset/Admin/plugins/overlayScrollbars/css/OverlayScrollbars.min.css",
                                                               "~/Asset/Admin/plugins/daterangepicker/daterangepicker.css"));
            // Add Script to bundle 
            bundles.Add(new ScriptBundle("~/Admin/js").Include("~/Asset/Admin/plugins/jquery/jquery.min.js",
                                                              "~/Asset/Admin/plugins/jquery-ui/jquery-ui.min.js",
                                                              "~/Asset/Admin/plugins/bootstrap/js/bootstrap.bundle.min.js",
                                                              "~/Asset/Admin/plugins/chart.js/Chart.min.js",
                                                              "~/Asset/Admin/plugins/flot/jquery.flot.js",
                                                              "~/Asset/Admin/plugins/flot/plugins/jquery.flot.resize.js",
                                                              "~/Asset/Admin/plugins/flot/plugins/jquery.flot.pie.js",
                                                              "~/Asset/Admin/plugins/jquery-knob/jquery.knob.min.js",
                                                              "~/Asset/Admin/plugins/moment/moment.min.js",
                                                              "~/Asset/Admin/plugins/daterangepicker/daterangepicker.js",
                                                              "~/Asset/Admin/plugins/tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4.min.js",
                                                              "~/Asset/Admin/plugins/summernote/summernote-bs4.min.js",
                                                              "~/Asset/Admin/plugins/overlayScrollbars/js/jquery.overlayScrollbars.min.js",
                                                              "~/Asset/Admin/dist/js/adminlte.min.js",
                                                              "~/Asset/Admin/dist/js/demo.js",
                                                              "~/Asset/Admin/dist/js/pages/dashboard.js"));

            bundles.Add(new StyleBundle("~/Pubilc/css").Include("~/Asset/css/bootstrap.min.css",
                                                                "~/Asset/css/prettyPhoto.css",
                                                                "~/Asset/css/price-range.css",
                                                                "~/Asset/css/animate.css",
                                                                "~/Asset/css/main.css",
                                                                "~/Asset/css/responsive.css",
                                                                "~/Asset/css/mystyle.css"));

            bundles.Add(new ScriptBundle("~/Public/js").Include("~/Asset/js/jquery.js",
                                                                "~/Asset/js/bootstrap.min.js",
                                                                "~/Asset/js/jquery.scrollup.min.js",
                                                                "~/Asset/js/price-range.js",
                                                                "~/Asset/js/jquery.prettyPhoto.js",
                                                                "~/Asset/jquery.scrollUp.min.js",
                                                                "~/Asset/js/main.js"));
        }
    }
}

﻿using System.Web;
using System.Web.Optimization;

namespace LeaveApplication
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                       "~/Scripts/jquery.mask.js", "~/Scripts/jquery-3.4.1.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/datetime").Include(
                      "~/material-kit-master/assets/js/plugins/moment.min.js", "~/material-kit-master/assets/js/plugins/bootstrap-datetimepicker.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                       "~/material-kit-master/assets/js/core/popper.min.js",
                       "~/material-kit-master/assets/js/core/bootstrap-material-design.min.js",
                       "~/material-kit-master/assets/js/plugins/moment.min.js",
                       "~/material-kit-master/assets/js/plugins/nouislider.min.js",
                       "~/material-kit-master/assets/js/material-kit.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/material-kit-master/assets/css/material-kit.css",
                      "~/Content/site.css"));
            BundleTable.EnableOptimizations = false;//to disbale optimization in Realenvironment
        }
    }
}

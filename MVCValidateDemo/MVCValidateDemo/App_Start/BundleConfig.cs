﻿using System.Web;
using System.Web.Optimization;

namespace MVCValidateDemo
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/assets/jquery/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/assets/jquery/jquery.validate*"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/assets/js/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/assets/bootstrap/bootstrap.js",
                      "~/assets/bootstrap/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/assets/bootstrap/bootstrap.css",
                      "~/assets/css/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/base").Include(
                        "~/assets/js/comm.js",
                        "~/assets/js/jquery.pager.js",
                        "~/assets/js/fileupload.js"));
        }
    }
}

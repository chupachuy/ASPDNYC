using System.Web;
using System.Web.Optimization;

namespace DNyC
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region General
            bundles.Add(new ScriptBundle("~/js/General").Include(
                        "~/Scripts/jquery.min.js",
                        "~/Scripts/bootstrap.min.js",
                        "~/Scripts/popper.min.js",
                        "~/Scripts/modernizr-2.6.2.js",
                        "~/Scripts/aes.js",
                        "~/Scripts/general.js",
                        "~/Scripts/DNyCAnalytics.js",
                        "~/Scripts/jquery.blockUI.js",
                        "~/Scripts/jquery.maskedinput.js",
                        "~/Scripts/jquery.maskedmoney.min.js"
                        ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/animate.css",
                      "~/Content/normalize.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/js/slick").Include(
                       "~/Scripts/slick/slick.js"));

            bundles.Add(new StyleBundle("~/Content/slick/csslick").Include(
                      "~/Content/slick/slick.css",
                      "~/Content/slick/slick-theme.css"));
            #endregion

            bundles.Add(new StyleBundle("~/Content/HomeLanding").Include(
                "~/Content/styleSitioHome.css"
            ));

            bundles.Add(new ScriptBundle("~/js/HomeLanding").Include(
                       "~/Areas/Sitio/Scripts/index.js"
                       ));

            bundles.Add(new ScriptBundle("~/js/HomeLandingContacto").Include(
                       "~/Areas/Sitio/Scripts/contacto.js",
                       "~/Scripts/jquery.validate.min.js",
                       "~/Scripts/additional-methods.min.js"
                       ));

            bundles.Add(new ScriptBundle("~/js/Descargar_Recetas").Include(
                       "~/Areas/Sitio/Scripts/descargar.js",
                       "~/Scripts/jquery.validate.min.js",
                       "~/Scripts/additional-methods.min.js"
                       ));


            // jquery_ui
            bundles.Add(new ScriptBundle("~/js/jquery_ui").Include(
                "~/Scripts/jquery-ui.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/jquery_ui").Include(
                "~/Content/jquery-ui.css"
            ));


            // chartjs
            bundles.Add(new StyleBundle("~/Content/chartjs").Include(
                      "~/Content/Chart.css"
                      ));
            bundles.Add(new ScriptBundle("~/js/chartjs").Include(
                       "~/Scripts/moment.js",
                       "~/Scripts/Chart.js"
                       ));

            // DataTable Bootstrap
            bundles.Add(new StyleBundle("~/Content/datatables").Include(
                      "~/Content/datatables.css"
                      ));
            bundles.Add(new ScriptBundle("~/js/datatables").Include(
                       "~/Scripts/datatables.js"
                       ));
            bundles.Add(new ScriptBundle("~/js/datatablesfixedcolumns").Include(
                       "~/Scripts/dataTables.fixedColumns.min.js"
                       ));

            // Deshabilitar boton atras
            bundles.Add(new ScriptBundle("~/js/DisableBack").Include(
                       "~/Scripts/disableBack.js"
                       ));

            // Deshabilitar Title en img
            bundles.Add(new ScriptBundle("~/js/DisableTitle").Include(
                       "~/Scripts/disableTitle.js"
                       ));


            bundles.Add(new ScriptBundle("~/js/ApiGoogle").Include(
                     "~/Scripts/apigoogle.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/BootstrapMin").Include(
                      "~/Content/bootstrap.min.css"
                      ));


            bundles.Add(new StyleBundle("~/Content/NoMovil").Include(
                      "~/Content/StyleNoMovil.css"
                      ));


            #region Pruebas
            bundles.Add(new ScriptBundle("~/js/FileUpload_user").Include(
                "~/Scripts/jQuery.FileUpload/vendor/jquery.ui.widget.js",
                "~/Scripts/jQuery.FileUpload/jquery.fileupload.js",
                "~/Scripts/jQuery.FileUpload/jquery.iframe-transport.js",
                "~/Areas/Prueba/Scripts/_FileUpload.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/FileUpload_user").Include(
                "~/Areas/Prueba/Scripts/_FileUpload.css",
                "~/Content/jQuery.FileUpload/css/jquery.fileupload.css"
            ));
            #endregion

#if DEBUG
            System.Web.Optimization.BundleTable.EnableOptimizations = false;
#else
                        System.Web.Optimization.BundleTable.EnableOptimizations = true;
#endif

        }
    }
}
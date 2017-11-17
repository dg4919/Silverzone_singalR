using System.Web.Optimization;

namespace Silverzone.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region  Required Css for Project 

            // for Site
            bundles.Add(new StyleBundle("~/Content/Site/css").Include(
                    "~/Content/font-awesome.css",
                    "~/Content/Site.css",
                    "~/Content/pnotify.css",
                    "~/Content/angular-block-ui.css",
                    "~/Content/isteven-multi-select.css",
                    "~/Content/jquery.mCustomScrollbar.css"
            ));

            // for Admin
            bundles.Add(new StyleBundle("~/Content/Admin/css").Include(
                  "~/Content/font-awesome.css",
                  "~/Content/ionicons.css",
                  "~/Content/AdminLTE.css",
                  "~/Content/_all-skins.css",
                  "~/Content/jquery.dataTables.css",
                  "~/Content/3stepStyle.css",
                  "~/Content/angular-ui-switch.css",
                  "~/Content/pnotify.css",
                  "~/Content/angular-block-ui.css",
                  "~/Content/isteven-multi-select.css",
                  "~/Content/bootstrap-datepicker.css"
          ));

            #endregion


            #region  Main bundle files > for Site > if use before <body> than js of particular page won't run

            bundles.Add(new ScriptBundle("~/bundles/Site/jqueryBundle").Include(
                 "~/Scripts/Lib/jquery/jquery-1.10.2.js",
                 "~/Scripts/Lib/jquery/jquery-validate.js",
                 //"~/Scripts/Lib/jquery/bootstrap.js",
                 //"~/Scripts/Lib/jquery/jquery-ui.js",
                 "~/Scripts/Lib/jquery/pnotify.js",

                 // Use in angular to format date time
                 "~/Scripts/Lib/jquery/moment.js",

                 // jquery ScrollBar
                 "~/Scripts/Lib/jquery/jquery.mCustomScrollbar.js",

                 // user control panel jss start from here
                 "~/Scripts/Lib/jquery/userTheme/bootstrap-datepicker.js",
                 "~/Scripts/Lib/jquery/userTheme/ace-elements.min.js",
                 "~/Scripts/Lib/jquery/userTheme/ace.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/Site/AngularBundle").Include(
                    "~/Scripts/Lib/angularjs/angular-1.5.0.js",
                    "~/Scripts/Lib/angularjs/ui-bootstrap-tpls-2.3.0.js",

                    "~/Scripts/Lib/angularjs/angular-ui-router.js",
                    "~/Scripts/Lib/angularjs/angular-pnotify.js",

                     // for Angular validation
                     "~/Scripts/Lib/angularjs/angular-validate.js",

                     "~/Scripts/Lib/angularjs/angular-block-ui.js",
                     "~/Scripts/Lib/angularjs/angular-local-storage.js",
                     "~/Scripts/Lib/angularjs/isteven-multi-select.js",

                     // Angularjs ScrollBar
                     "~/Scripts/Lib/angularjs/scrollbars.js"
                 ));

            #endregion

            #region  ************  Application JS created by Developer > for Site  ************

            bundles.Add(new ScriptBundle("~/bundles/Site/AppBundle").Include(

                    // Shared Service in a new modeule use as dependncy injection
                    "~/Scripts/App/Shared/silverzone_app.Common.js",
                    "~/Scripts/App/Shared/shared_customDirective.js",
                    "~/Scripts/App/Shared/Silverzone_sharedService.js",
                    "~/Scripts/App/Shared/user_Account_sharedService.js",

                    "~/Scripts/App/Global_app/Silverzone_app.js",
                    "~/Scripts/App/Services/httpInterceptorService.js",
                    "~/Scripts/App/Global_app/customDirective.js",
                    "~/Scripts/App/Global_app/Filters.js",

                    "~/Scripts/App/Controller/siteMasterController.js",
                    "~/Scripts/App/Services/siteMasterService.js",

                    "~/Scripts/App/Controller/cartController.js",
                    "~/Scripts/App/Services/cartService.js",

                    "~/Scripts/App/Controller/bookController.js",
                    "~/Scripts/App/Services/bookService.js",

                    "~/Scripts/App/Controller/user_Account_Controller.js",

                    "~/Scripts/App/Services/loginModal_service.js",

                     "~/Areas/User/Scripts/Controller/userProfileController.js",
                    "~/Areas/User/Scripts/Services/userProfileService.js",
                    "~/Areas/User/Scripts/Controller/userOrderController.js"
                 ));

            #endregion



            #region  Main bundle files > for Admin Section

            bundles.Add(new ScriptBundle("~/bundles/Admin/jqueryBundle").Include(
                    "~/Scripts/Lib/jquery/jquery-1.10.2.js",
                    "~/Scripts/Lib/jquery/jquery-validate.js",

                    "~/Scripts/Lib/jquery/jquery.dataTables.js",

                    //  use in angular to format date time
                    "~/Scripts/Lib/jquery/moment.js",

                    "~/Scripts/Lib/jquery/bootstrap.js",
                    "~/Scripts/Lib/jquery/jquery-ui.js",

                    "~/Scripts/Lib/jquery/jquery.slimscroll.js",
                    "~/Scripts/Lib/jquery/pnotify.js",

                   // user control panel jss start from here
                    "~/Scripts/Lib/jquery/userTheme/bootstrap-datepicker.js",
                    "~/Scripts/Lib/jquery/Admin_theme/app.js",
                    "~/Scripts/Lib/jquery/Admin_theme/demo.js"

             ));

            bundles.Add(new ScriptBundle("~/bundles/Admin/AngularBundle").Include(

                "~/Scripts/Lib/angularjs/angular-1.5.0.js",
                "~/Scripts/Lib/angularjs/ui-bootstrap-tpls-2.3.0.js",

                //  for Angular validation 
                "~/Scripts/Lib/angularjs/angular-validate.js",

                // for Angular Routing
                "~/Scripts/Lib/angularjs/angular-ui-router.js",

                // for Angular dataTable 
                "~/Scripts/Lib/angularjs/angular-datatables.js",

                // for Angular switchery 
                "~/Scripts/Lib/angularjs/angular-ui-switch.js",

                // Angular Pnotify to show notifiaction > when user submit data in ajax call sucess
                "~/Scripts/Lib/angularjs/angular-pnotify.js",

                // for multi select drop down
                "~/Scripts/Lib/angularjs/isteven-multi-select.js",

                // to Block/Show loader while calling Angular Services
                "~/Scripts/Lib/angularjs/angular-block-ui.js",
                "~/Scripts/Lib/angularjs/angular-local-storage.js"

            ));


            #endregion

            #region  ************  Application JS created by Developer > for Admin  ************

            bundles.Add(new ScriptBundle("~/bundles/Admin/AppBundle").Include(                
                // Shared Service in a new modeule use as dependncy injection
                "~/Scripts/App/Shared/silverzone_app.Common.js",
                "~/Scripts/App/Shared/shared_customDirective.js",
                "~/Scripts/App/Shared/Silverzone_sharedService.js",
                "~/Scripts/App/Shared/user_Account_sharedService.js",

                // *********  Global Main App  ***********
                "~/Areas/Admin/Scripts/Global_app/admin_silverzone_app.js",
                    "~/Scripts/App/Services/httpInterceptorService.js",
                "~/Areas/Admin/Scripts/Global_app/customFilters.js",
                "~/Areas/Admin/Scripts/Global_app/customdirective.js",

                "~/Areas/Admin/Scripts/Controller/admin_main_Controller.js",

                // common service to show MODAL POPUP
                "~/Areas/Admin/Scripts/Services/commonModal_service.js",

                // *********js for admin > book category > use these JS on layout instead of its difrent modules * **********

                "~/Areas/Admin/Scripts/Controller/admin_book_cateogry_Controller.js",
                "~/Areas/Admin/Scripts/Services/admin_book_cateogry_Service.js",

                // *********  js for admin > book category  **********

                "~/Areas/Admin/Scripts/Controller/admin_book_Controller.js",
                "~/Areas/Admin/Scripts/Services/admin_book_Service.js",

                // *********  js for admin > Dispatch order   **********

                "~/Areas/Admin/Scripts/Controller/admin_dispatchOrders_Controller.js",
                "~/Areas/Admin/Scripts/Services/admin_dispatchOrders_Service.js",

                // *********  js for admin > Coupons  **********
                "~/Areas/Admin/Scripts/Controller/admin_coupons.js",

                // *********  js for admin > Quiz **********
                "~/Areas/Admin/Scripts/Controller/admin_quiz_Controller.js",
                "~/Areas/Admin/Scripts/Services/admin_quiz_Service.js",

                // *********  js for admin > create users **********
                "~/Areas/Admin/Scripts/Controller/admin_user_controller.js",
                "~/Areas/Admin/Scripts/Services/admin_user_Service.js"
          ));

            #endregion

            #region NewBundle for Home

            // New Bundles for New Home Design > Header & footer
            bundles.Add(new StyleBundle("~/Content/Site/Home_css").Include(
                    "~/Content/bootstrap.css",
                    "~/Content/font-awesome.css",
                    "~/Content/CSS/carousel.css",
                    "~/Content/CSS/slick-theme.css",
                    "~/Content/CSS/slick.css",
                    "~/Content/CSS/style.css",
                    "~/Content/fonts.googleapis.com.css",

                    "~/Content/Site.css",
                    "~/Content/pnotify.css",
                    "~/Content/angular-block-ui.css",
                    "~/Content/isteven-multi-select.css",
                    "~/Content/jquery.mCustomScrollbar.css"
            ));

            bundles.Add(new ScriptBundle("~/bundles/Site/Home_jqueryBundle").Include(
                 "~/Scripts/JS/bootstrap.min.js",
                 "~/Scripts/JS/isotope.js",
                 "~/Scripts/JS/imagesloaded.min.js",
                 "~/Scripts/JS/nivo-lightbox.min.js",
                 "~/Scripts/JS/jquery.backstretch.min.js",
                 "~/Scripts/JS/wow.min.js",
                 "~/Scripts/JS/custom.js",
                 "~/Scripts/JS/home.js",
                 "~/Scripts/JS/slick.js"
            ));

            #endregion

        }
    }
}

using System.Web.Optimization;

namespace SilverzoneERP.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region  Required Css for Project 

            // for Site
            bundles.Add(new StyleBundle("~/Content/ERP/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/bootstrap-datepicker.css",
                "~/Content/bootstrap-datetimepicker.css",
                "~/Content/font-awesome.css",
                //"~/Content/icon-font.min.css",
                "~/Content/style.css",
                "~/Content/Site.css",
                "~/Content/jquery.dataTables.css",
                "~/Content/angular-responsive-tables.css",
                "~/Content/pnotify.css",
                "~/Content/isteven-multi-select.css",
                "~/Content/angular-ui-switch.css",
                "~/Content/jquery-confirm.css"
                ));

            #endregion


            #region  Main Library bundle files  > if use before <body> than js of particular page won't run

            bundles.Add(new ScriptBundle("~/bundles/ERP/Lib").Include(

                    // ---  Latest Bootstrap & Jquery js  ---

                    // jqyery Lib
                    "~/Scripts/Lib/jquery/jquery-3.1.1.js",
                    "~/Scripts/Lib/jquery/moment.js",
                    "~/Scripts/Lib/jquery/bootstrap-3.3.7.js",

                    //"~/Scripts/Lib/jquery/jquery.signalR-2.2.2.js",

                    "~/Scripts/Lib/jquery/bootstrap-datepicker.js",
                    "~/Scripts/Lib/jquery/bootstrap-datetimepicker.js",

                    "~/Scripts/Lib/jquery/jquery.nicescroll.js",
                    "~/Scripts/Lib/jquery/scripts.js",
                    "~/Scripts/Lib/jquery/jquery-validate.js",

                    "~/Scripts/Lib/jquery/underscore.js",
                    "~/Scripts/Lib/jquery/pnotify.js",
                    "~/Scripts/Lib/jquery/jquery-confirm.js",
                    "~/Scripts/Lib/jquery/jquery.dataTables.js",

                    // angular Lib
                    "~/Scripts/Lib/angularjs/angular-1.5.0.js",
                    "~/Scripts/Lib/angularjs/ui-bootstrap-tpls-2.3.1.js",

                    "~/Scripts/Lib/angularjs/angular-block-ui.js",
                    "~/Scripts/Lib/angularjs/angular-local-storage.js",
                    "~/Scripts/Lib/angularjs/angular-ui-router.js",

                    "~/Scripts/Lib/angularjs/angular-animate.js",
                    "~/Scripts/Lib/angularjs/angular-sanitize.js",

                    "~/Scripts/Lib/angularjs/angular-responsive-tables.js",
                    "~/Scripts/Lib/angularjs/angular-validate.js",
                    "~/Scripts/Lib/angularjs/angular-pnotify.js",
                    "~/Scripts/Lib/angularjs/isteven-multi-select.js",
                    "~/Scripts/Lib/angularjs/angular-ui-switch.js",

                    "~/Scripts/Lib/angularjs/angular-datatables.js"

            //  ---  End Lib  ---
            ));

            #endregion

            #region  ************  Application JS created by Developer  ************

            #region  ***********  Common/Module files   ****************

            bundles.Add(new ScriptBundle("~/bundles/ERP/Common").Include(

                    "~/Scripts/App/Shared/Silverzone_sharedService.js",

                    "~/Scripts/App/Inventory/inventory_App.js",
                    "~/Scripts/App/School/school_App.js",
                    "~/Scripts/App/Admin/admin_silverzone_app.js",

                    "~/Scripts/App/Global_app/SilverzoneERP_App.js",
                    "~/Scripts/App/Global_app/httpInterceptorService.js",
                    "~/Scripts/App/Global_app/Service.js",

                    "~/Scripts/App/School/accountController.js",
                    "~/Scripts/App/School/homeController.js",
                    "~/Scripts/App/Global_app/masterController.js",
                    "~/Scripts/App/Global_app/Filters.js",
                    "~/Scripts/App/Global_app/Directive.js"
             ));
            #endregion

            #region  ***********  School   ****************

            bundles.Add(new ScriptBundle("~/bundles/ERP/School").Include(
                 "~/Scripts/App/School/User/registrationController.js",
                 "~/Scripts/App/School/Role/roleController.js",
                 "~/Scripts/App/School/Role/roleService.js",
                 "~/Scripts/App/School/Role/userRoleController.js",
                 "~/Scripts/App/School/User/userPermissionController.js",
                 "~/Scripts/App/School/User/userPermissionService.js",
                 "~/Scripts/App/School/User/userSummaryController.js",
                 "~/Scripts/App/School/Location/locationController.js",
                 "~/Scripts/App/School/Location/locationService.js",
                 "~/Scripts/App/School/Location/locationService.js",
                 "~/Scripts/App/School/Designation/designationController.js",
                 "~/Scripts/App/School/Title/titleController.js",
                 "~/Scripts/App/School/Event/eventController.js",
                 "~/Scripts/App/School/Event/eventYearController.js",
                 "~/Scripts/App/School/SchoolManagement/schoolManagementController.js",
                 "~/Scripts/App/School/Category/categoryController.js",
                 "~/Scripts/App/School/SchoolGroup/schoolGroupController.js",
                 "~/Scripts/App/School/EventManagement/eventManagementController.js",
                 "~/Scripts/App/School/SearchSchool/SearchSchoolController.js",
                 "~/Scripts/App/School/SchoolManagement/SchoolService.js",
                 "~/Scripts/App/School/ExamDate/examDateController.js",
                 "~/Scripts/App/School/EventManagement/EnrollmentOrderController.js",
                 "~/Scripts/App/School/EventManagement/BookOrderController.js",
                 "~/Scripts/App/School/EventManagement/OrderService.js",
                 "~/Scripts/App/School/EventManagement/PaymentController.js",
                 "~/Scripts/App/School/StudentEntry/studentEntryController.js",
                  "~/Scripts/App/School/Courier/courierController.js"
             ));

            #endregion

            #region  ***********  Inventory   ****************

            bundles.Add(new ScriptBundle("~/bundles/ERP/Inventory").Include(

                   // **********  Components  ***********

                   "~/Scripts/App/Inventory/Component/shared_Component.js",
                   "~/Scripts/App/Inventory/Component/Inventory_Component.js",
                   "~/Scripts/App/Inventory/Component/Inventory_Source_Component.js",
                   "~/Scripts/App/Inventory/Component/purchaseOrder_Component.js",
                   "~/Scripts/App/Inventory/Component/dispatch_otherItem_master_Component.js",
                   "~/Scripts/App/Inventory/Component/RegionalCordinator_Component.js",

                   // **********  Services  ***********
                   "~/Scripts/App/Inventory/Services/inventoryService.js",
                   "~/Scripts/App/Inventory/Services/dispatchService.js",
                   "~/Scripts/App/Inventory/Services/Silverzone_modalService.js"
             ));
            #endregion

            #region  ***********  Admin   ****************

            bundles.Add(new ScriptBundle("~/bundles/ERP/Admin").Include(
                   "~/Scripts/App/Admin/customdirective.js",

                   // **********  Controller  **********
                   "~/Scripts/App/Admin/Controller/admin_book_cateogry_Controller.js",
                   "~/Scripts/App/Admin/Controller/admin_book_Controller.js",
                   "~/Scripts/App/Admin/Controller/admin_coupons.js",
                   "~/Scripts/App/Admin/Controller/admin_dispatchOrders_Controller.js",
                   //"~/Scripts/App/Admin/Controller/admin_main_Controller.js",
                   "~/Scripts/App/Admin/Controller/admin_quiz_Controller.js",
                   "~/Scripts/App/Admin/Controller/admin_user_controller.js",
                   "~/Scripts/App/Admin/Controller/admin_itemTitle_Controller.js",

                   //  **********  Service  **********
                   "~/Scripts/App/Admin/Services/admin_book_cateogry_Service.js",
                   "~/Scripts/App/Admin/Services/admin_book_Service.js",
                   "~/Scripts/App/Admin/Services/admin_dispatchOrders_Service.js",
                   "~/Scripts/App/Admin/Services/admin_quiz_Service.js",
                   "~/Scripts/App/Admin/Services/admin_user_Service.js",
                   "~/Scripts/App/Admin/Services/commonModal_service.js",
                   "~/Scripts/App/Admin/Services/hubService.js"

             ));
            #endregion

            #endregion


        }
    }
}

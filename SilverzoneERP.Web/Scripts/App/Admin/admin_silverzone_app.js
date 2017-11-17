/// <reference path="../../Lib/angular-1.5.8.js" />

(function () {       

    'use strict';
    
    var configFn = function ($stateProvider, $urlRouterProvider) {

        $stateProvider
            //.state("admin_login", {
            //     url: '/ERP/Admin/Login',
            //     templateUrl: 'Templates/Admin/views/Login.html',
            //     controller: 'user_login',
            // })
            .state("book_category_add", {
                url: '/ERP/Admin/bookCategory/Create',
                templateUrl: 'Templates/Admin/views/book_category/Create.html',
                controller: 'bookCategory_create'
            })
            .state("book_category_list", {
                url: '/ERP/Admin/bookCategory/List',
                templateUrl: 'Templates/Admin/views/book_category/List.html',
                controller: 'bookCategory_list'
            })
            .state("book_create", {
                 url: '/ERP/Admin/Book/Create',
                 templateUrl: 'Templates/Admin/views/books/Create.html',
                 controller: 'book_create'
             })
            .state("book_edit", {
                 url: '/ERP/Admin/Book/Edit/{bookId:int}',
                 templateUrl: 'Templates/Admin/views/books/Create.html',
                 controller: 'book_create'
             })
            .state("book_list", {
                url: '/ERP/Admin/Book/List',
                templateUrl: 'Templates/Admin/views/books/List.html',
                controller: 'book_list',
            })
            .state("dispatch_printLabel", {
                 url: '/ERP/dispatch/printlabel',
                 templateUrl: 'Templates/Admin/views/dispatch/printLabel.html',
                 controller: 'dispatch_printLabel',
                 Role: ['Dispatch']
             })
            .state("dispatch_addWheight", {
                url: '/ERP/dispatch/wheight',
                templateUrl: 'Templates/Admin/views/dispatch/wheight.html',
                controller: 'dispatch_addWheight',
                Role: ['Dispatch']
            })
            .state("dispatch_addConsignment", {
             url: '/ERP/dispatch/consignment',
             templateUrl: 'Templates/Admin/views/dispatch/consignment.html',
             controller: 'dispatch_addConsignment',
             Role: ['Dispatch']
            })
            .state("coupon_list", {
                url: '/ERP/Admin/Coupons',
                templateUrl: 'Templates/Admin/views/Coupons.html',
                controller: 'coupons_list',
            })
            .state("bundle_list", {
                url: '/ERP/Admin/bookBundle/List',
                templateUrl: 'Templates/Admin/views/boookBundle/List.html',
                controller: 'bundle_list',
            })
            .state("bundle_create", {
                url: '/ERP/Admin/bookBundle/Create',
                templateUrl: 'Templates/Admin/views/boookBundle/Create.html',
                controller: 'bundle_create',
                controllerAs: 'bundleInfo'
            })
            .state("bundle_edit", {
                url: '/ERP/Admin/bookBundle/Edit/{bundleId:int}',
                templateUrl: 'Templates/Admin/views/boookBundle/Create.html',
                controller: 'bundle_create',
                controllerAs: 'bundleInfo'
            })
            .state("quiz_create", {
                url: '/ERP/Admin/Quiz/Create',
                templateUrl: 'Templates/Admin/views/QuizQuestions/Create.html',
                controller: 'quiz_create'
            })
            .state("quiz_list", {
                url: '/ERP/Admin/Quiz/List',
                templateUrl: 'Templates/Admin/views/QuizQuestions/List.html',
                controller: 'quiz_list',
            })
            .state("quiz_edit", {
                url: '/ERP/Admin/Quiz/Edit/{quizId:int}',
                templateUrl: 'Templates/Admin/views/QuizQuestions/Create.html',
                controller: 'quiz_create'
            })
            .state("itemTitle_create", {
                url: '/ERP/Admin/ItemTitle/Create',
                templateUrl: 'Templates/Admin/views/itemTitle_master.html',
                controller: 'itemMater_create'
            })
            .state("create_user", {
                url: '/ERP/Admin/User/Create/',
                templateUrl: 'Templates/Admin/views/Admin_Create_User.html',
                controller: 'user_create',
                //resolve: {
                //    access: ["Access", function (Access) { return Access.hasRole("ADMIN"); }],
                //}
            })
            .state("track_order", {
                url: '/ERP/dispatch/Track',
                templateUrl: 'Templates/Admin/views/TrackOrder.html',
                controller: 'trackOrder',
                Role: ['Dispatch', 'Support']
            })
            .state("unAuthorize_request", {
             url: '/ERP/Admin/Unauthorize',
             templateUrl: 'Templates/Admin/views/Unauthorize.html',
            })
            .state("search_dispatch", {
                url: '/ERP/dispatch/Search',
                templateUrl: 'Templates/Admin/views/dispatch/searchDispatch.html',
                controller: 'searchDispatch',
                Role: ['Dispatch', 'Support']
            })
        ;

    }

    var moduleDependencies =
        [
            'datatables',
            'isteven-multi-select',      // https://github.com/isteven/angular-multi-select
        ];

    angular.module('Silverzone_admin_app', moduleDependencies)
    .config(['$stateProvider', '$urlRouterProvider', configFn])
    .constant('userRoles', {
        Admin: 'Admin',
        Support: 'Support',
        Dispatch: 'Dispatch'
    })

    ;
   
})();
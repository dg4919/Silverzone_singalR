/// <reference path="../../Lib/angular-1.5.8.js" />

(function () {       

    'use strict';
    
    var configFn = function ($stateProvider, $urlRouterProvider) {

        // Default Url, if any URL not match, redirect to it
        $urlRouterProvider.otherwise('/Admin/Login');

        $stateProvider
             .state("admin_login", {
                 url: '/Admin/Login',
                 templateUrl: 'Areas/Admin/templates/views/Login.html',
                 controller: 'user_login',
             })
            .state("book_category_add", {
                url: '/Admin/bookCategory/Create',
                templateUrl: 'Areas/Admin/templates/views/book_category/Create.html',
                controller: 'bookCategory_create'
            })
            .state("book_category_list", {
                url: '/Admin/bookCategory/List',
                templateUrl: 'Areas/Admin/templates/views/book_category/List.html',
                controller: 'bookCategory_list'
            })
            .state("book_create", {
                 url: '/Admin/book/Create',
                 templateUrl: 'Areas/Admin/templates/views/books/Create.html',
                 controller: 'book_create'
             })
            .state("book_edit", {
                 url: '/Admin/book/Edit/{bookId:int}',
                 templateUrl: 'Areas/Admin/templates/views/books/Create.html',
                 controller: 'book_create'
             })
            .state("book_list", {
                url: '/Admin/book/List',
                templateUrl: 'Areas/Admin/templates/views/books/List.html',
                controller: 'book_list',
            })
            .state("dispatch_printLabel", {
                 url: '/Admin/dispatch/printlabel',
                 templateUrl: 'Areas/Admin/templates/views/dispatch/printLabel.html',
                 controller: 'dispatch_printLabel',
                 Role: ['Dispatch']
             })
            .state("dispatch_addWheight", {
                url: '/Admin/dispatch/wheight',
                templateUrl: 'Areas/Admin/templates/views/dispatch/wheight.html',
                controller: 'dispatch_addWheight',
                Role: ['Dispatch']
            })
            .state("dispatch_addConsignment", {
             url: '/Admin/dispatch/consignment',
             templateUrl: 'Areas/Admin/templates/views/dispatch/consignment.html',
             controller: 'dispatch_addConsignment',
             Role: ['Dispatch']
            })
            .state("coupon_list", {
                url: '/Admin/Coupons',
                templateUrl: 'Areas/Admin/templates/views/Coupons.html',
                controller: 'coupons_list',
            })
            .state("bundle_list", {
                url: '/Admin/bookBundle/List',
                templateUrl: 'Areas/Admin/templates/views/boookBundle/List.html',
                controller: 'bundle_list',
            })
            .state("bundle_create", {
                url: '/Admin/bookBundle/Create',
                templateUrl: 'Areas/Admin/templates/views/boookBundle/Create.html',
                controller: 'bundle_create',
                controllerAs: 'bundleInfo'
            })
            .state("bundle_edit", {
                url: '/Admin/bookBundle/Edit/{bundleId:int}',
                templateUrl: 'Areas/Admin/templates/views/boookBundle/Create.html',
                controller: 'bundle_create',
                controllerAs: 'bundleInfo'
            })
            .state("quiz_create", {
                url: '/Admin/Quiz/Create',
                templateUrl: 'Areas/Admin/templates/views/QuizQuestions/Create.html',
                controller: 'quiz_create'
            })
            .state("quiz_list", {
                url: '/Admin/Quiz/List',
                templateUrl: 'Areas/Admin/templates/views/QuizQuestions/List.html',
                controller: 'quiz_list',
            })
            .state("quiz_edit", {
                url: '/Admin/Quiz/Edit/{quizId:int}',
                templateUrl: 'Areas/Admin/templates/views/QuizQuestions/Create.html',
                controller: 'quiz_create'
            })
            .state("create_user", {
                url: '/Admin/User/Create/',
                templateUrl: 'Areas/Admin/templates/views/Admin_Create_User.html',
                controller: 'user_create',
                //resolve: {
                //    access: ["Access", function (Access) { return Access.hasRole("ADMIN"); }],
                //}
            })
            .state("track_order", {
                url: '/Admin/Order/Track',
                templateUrl: 'Areas/Admin/templates/views/TrackOrder.html',
                controller: 'trackOrder',
                Role: ['Dispatch', 'Support']
            })
         .state("unAuthorize_request", {
             url: '/Admin/Unauthorize',
             templateUrl: 'Areas/Admin/templates/views/Unauthorize.html',
         })
        ;

    }

    // it contains global setting like > global variable used across diffrent ctrlers in this application
    var runFn = function ($rsc, $state, $injector, webUrl, userRoles) {
        webUrl.module = 'Admin';

        $rsc.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {
            //console.log(toState.Role.indexOf($rsc.user.currentUser.Role));      // return index of matched element from array

            // when user in not logged in & login is required !
            if (($rsc.user.currentUser === '' || !$rsc.validateUser($rsc.user.currentUser.Role)) && toState.name !== 'admin_login') {
                event.preventDefault();
                $injector.get('user_Account_Service').Logout();     // calling directly service method
                $state.go('admin_login');
            }
            else if ($rsc.user.currentUser !== '' &&        // if user is login & try to navigate on login page
                     $rsc.validateUser($rsc.user.currentUser.Role)
                    && toState.name === 'admin_login') {
                event.preventDefault();         // v.Imp for route state change
                $state.go('book_category_list');
            }
            else if ($rsc.user.currentUser !== '' &&                    // for unauthorize reqst
                     $rsc.validateUser($rsc.user.currentUser.Role) &&   // only few roles can access this Portal
                     toState.name !== "unAuthorize_request" &&
                     $rsc.user.currentUser.Role !== 'Admin')
            {
                if ((toState.Role !== undefined &&        // admin can access all pages so nedd to define Role attribute with .State
                     toState.Role.indexOf($rsc.user.currentUser.Role) === -1) ||
                     toState.Role === undefined)    // check role for current requested page is matched or not)
                {
                    event.preventDefault();
                    $state.go('unAuthorize_request');
                }
            }
        });

        // check login user role is valid or not
        $rsc.validateUser = function (user_role) {
            var userRole_isValid = false;

            angular.forEach(userRoles, function (role, key) {
                if (role === user_role)
                    userRole_isValid = true;
            });
            return userRole_isValid;
        }

    }

    var moduleDependencies =
        [
            'datatables',
            'uiSwitch',             // https://github.com/xpepermint/angular-ui-switch
            'Silverzone_app.Common'
        ];

    angular.module('Silverzone_app', moduleDependencies)
    .config(['$stateProvider', '$urlRouterProvider', configFn])
    .constant('userRoles', {
        Admin: 'Admin',
        Support: 'Support',
        Dispatch: 'Dispatch'
    })
    .run(['$rootScope', '$state', '$injector', 'webUrl', 'userRoles', runFn])

    ;

   
})();
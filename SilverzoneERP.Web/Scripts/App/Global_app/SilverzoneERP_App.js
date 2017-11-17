/// <reference path="../../Lib/angular-1.5.8.js" />

(function () {
    var configFn = function ($urlRouterProvider, $locationProvider, $validatorProvider, localStorageServiceProvider, $httpProvider) {

        $httpProvider.interceptors.push('httpInterceptor_Service');

        // for validation by angularjs
        $validatorProvider.setDefaults({
            errorElement: 'span',           // error msg shoe in a <span> Tag
            errorClass: 'has-error'        // nam of custom error class
        });

        // expiry = Number of days before cookies expire // 0 = Does not expire
        // default > 30 days
        localStorageServiceProvider
            .setStorageCookie(1);       // set cookie for 1 days in OWIN startup token file

        // set default msgs
        $validatorProvider.setDefaultMessages({
            maxlength: $validatorProvider.format("You can not enter more than {0} characters."),
        });

        // custom validator method to check input text is mobile/email in Login POPUP
        $validatorProvider.addMethod("validateEmailMobile", function (inputtxt, element) {

            var mailFormat = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
            var mobileFormat = /^\d{10}$/;

            if (mailFormat.test(inputtxt)) {
                return true;
            }
            else {
                if (mobileFormat.test(inputtxt) && inputtxt.length === 10)
                    return true;
                else
                    return false;
            }
        }, "Please enter valid Email ID/Mobile number");

        // Default Url, if any URL not match, redirect to it > for whole project
        $urlRouterProvider.otherwise('/ERP/Login');

        // to remove #/URL > from URL of browser
        //$locationProvider.html5Mode(true);
        $locationProvider.html5Mode({
            enabled: true
        });

    }

    var runFn = function ($rsc, $state, localStorageService, $injector, userRoles, notifySvc) {

        // check cookie on start of application > if page is refresh or reload
        var userCookie = localStorageService.get('UserInfo');
        if (userCookie !== null) {
            $rsc.UserInfo = userCookie;
        }
        else {
            $injector.get('Service').Logout();
        }

        //// to check > user is logout/ clear history OR logged In
        //$rsc.$watch(function () {               // watch chnages in localStorage value
        //    return (null != localStorageService.get("UserInfo"));      // // check user is logIn using localstorage value > when page is refresh
        //}, function (newvalue, oldvalue) {
        //    if (newvalue === false && oldvalue === true) {      // whenever new value is assigned it will call > false = No value/null OR true = value is exist
        //        //alert('user is logout/ clear history > log out from server');
        //        $rsc.UserInfo = '';

        //        $state.go('ERP_Login');
        //    }
        //    else if (newvalue === true && oldvalue === false) {
        //        //alert('user is logoged in');

        //        $rsc.UserInfo = localStorageService.get('UserInfo');
        //    }
        //});


        $rsc.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {
            //console.log(toState.RoleName.indexOf($rsc.UserInfo.RoleName));      // return index of matched element from array

            // when user in not logged in & login is required !
            if (($rsc.UserInfo === '' || !validateUser($rsc.UserInfo.RoleName)) && toState.name !== 'ERP_Login') {
                event.preventDefault();
                $injector.get('Service').Logout();     // calling directly service method
                $state.go('ERP_Login');
            }
            else if ($rsc.UserInfo !== '' &&        // if user is login & try to navigate on login page
                     validateUser($rsc.UserInfo.RoleName)
                    && toState.name === 'ERP_Login') {
                event.preventDefault();         // v.Imp for route state change
                $state.go('ERP_Home');
            }
            else if ($rsc.UserInfo !== '' &&                    // for unauthorize reqst
                     validateUser($rsc.UserInfo.RoleName) &&   // only few roles can access this Portal
                     toState.name !== "unAuthorize_request" &&
                     $rsc.UserInfo.RoleName !== 'Admin') {
                if ((toState.RoleName !== undefined &&        // admin can access all pages so nedd to define Role attribute with .State
                     toState.RoleName.indexOf($rsc.UserInfo.RoleName) === -1) ||
                     toState.RoleName === undefined)    // check role for current requested page is matched or not)
                {
                    event.preventDefault();
                    $state.go('unAuthorize_request');
                }
            }
        });

        // check login user role is valid or not
        function validateUser(user_role) {
            var userRole_isValid = false;

            angular.forEach(userRoles, function (role, key) {
                if (role === user_role)
                    userRole_isValid = true;
            });
            return userRole_isValid;
        }

        $rsc.notify_fx = function (Text, _type) {
            notifySvc.notify({
                title: 'SilverZone',
                title_escape: false,
                text: Text,
                text_escape: false,
                styling: "bootstrap3",
                type: _type,
                icon: true,
                delay: 1500,
            });
        }
    }

    var moduleDependencies =
       [
           'LocalStorageModule',
           'ui.bootstrap',
           'ui.router',
           'ngValidate',
           'wt.responsive',
           'jlareau.pnotify',
           'ngAnimate',
           'ngSanitize',
           'uiSwitch',                  // https://github.com/xpepermint/angular-ui-switch
           'Silverzone_invenotry_App',
           'Silverzone_school_App',
           'Silverzone_service.Shared',
           'Silverzone_admin_app'
       ];

    angular.module('SilverzoneERP_App', moduleDependencies)
    .config(['$urlRouterProvider', '$locationProvider', '$validatorProvider', 'localStorageServiceProvider', '$httpProvider', configFn])
    .run(['$rootScope', '$state', 'localStorageService', '$injector', 'userRoles', 'notificationService', runFn])
    .constant('globalConfig', {
        apiEndPoint: "http://localhost:55615/api/",
        version: {
            School: 'School',
            Inventory: 'Inventory',
            Site: 'Site',
            Admin: 'Admin'
        }
    })
    ;

})();
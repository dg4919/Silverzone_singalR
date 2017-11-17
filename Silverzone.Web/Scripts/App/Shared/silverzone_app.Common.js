
(function () {

    'use strict';

    // initialize first > use for global setting for app & initialize provider
    configFn.$inject = ['$urlMatcherFactoryProvider', '$locationProvider', '$validatorProvider', 'blockUIConfig', 'localStorageServiceProvider', '$httpProvider', 'myServiceProvider'];         // use 'Provider' as suffix to use our own provider
    function configFn($urlMatcherFactoryProvider, $locationProvider, $validatorProvider, blockUIConfig, localStorageServiceProvider, $httpProvider, myService) {

        // interceptor call before start & end for each ajax call > a centralised mechanism to handle reqst & response
        $httpProvider.interceptors.push('httpInterceptor_Service');

        // expiry = Number of days before cookies expire // 0 = Does not expire
        // default > 30 days
        localStorageServiceProvider
            .setStorageCookie(1);       // set cookie for 1 days in OWIN startup token file

        // self invoke fx to show loader while calling any services on the page
        (function () {
            var loading_Template = ' <div class="block-ui-overlay"></div>'
                                   + ' <div class="block-ui-message-container">'
                                   + ' <i class="fa fa-spinner fa-spin fa-3x" random:color style="margin-left: 30px;"></i>'
                                   + ' <h2> <small style="color: #1d0000;">Please Wait ...</small></h2>'
                                   + ' </div>'

            blockUIConfig.template = loading_Template;
        })();

        // for validation by angularjs
        $validatorProvider.setDefaults({
            errorElement: 'span',           // error msg shoe in a <span> Tag
            errorClass: 'has-error'        // nam of custom error class
        });

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


        // to remove #/URL > from URL of browser
        //$locationProvider.html5Mode(true);
        $locationProvider.html5Mode({
            enabled: true
        });

        // URL case sensitive = false
        $urlMatcherFactoryProvider.caseInsensitive(true);
    }

    // run fx execute once (singleton) > first time of page Load
    var runFn = function ($rsc, svc, localStorageService, $injector, $state, webUrl) {

        $rsc.user = {
            currentUser: '',
            is_login: false
        }

        // check cookie on start of application > if page is refresh or reload
        var userCookie = localStorageService.get('authorizeData');
        if (userCookie !== null) {
            $rsc.user.currentUser = userCookie.userInfo;
            $rsc.user.is_login = true;
        }
        else {
            $injector.get('user_Account_Service').Logout();
        }

        // to check > user is logout/ clear history OR logged In
        $rsc.$watch(function () {               // watch chnages in localStorage value
            return (null != localStorageService.get("authorizeData"));      // // check user is logIn using localstorage value > when page is refresh
        }, function (newvalue, oldvalue) {
            if (newvalue === false && oldvalue === true) {      // whenever new value is assigned it will call > false = No value/null OR true = value is exist
                //alert('user is logout/ clear history > log out from server');

                $rsc.user.currentUser = '';
                $rsc.user.is_login = false;

                if (webUrl.module === 'Site')     // webUrl is value service
                    $injector.get('user_Account_Service').Logout();
                else if (webUrl.module === 'Admin')
                    $state.go('admin_login');
            }
            else if (newvalue === true && oldvalue === false) {
                //alert('user is logoged in');

                $rsc.user.currentUser = localStorageService.get('authorizeData').userInfo;
                $rsc.user.is_login = true;
            }
        });



        $rsc.notify_fx = function (Text, _type) {
            svc.notify({
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


    // common dependency & configuration which will be apply to another module > by just injecting it
    var moduleDependencies =
        [
            'LocalStorageModule',        // https://github.com/grevory/angular-local-storage,
            'ui.bootstrap',              // https://angular-ui.github.io/bootstrap/
            'customDirective_module',
            'ui.router',
            'jlareau.pnotify',           // https://github.com/jacqueslareau/angular-pnotify    
            'ngValidate',                // https://github.com/jpkleemans/angular-validate
            'blockUI',                   // https://github.com/McNull/angular-block-ui/tree/master/dist
            'isteven-multi-select',      // https://github.com/isteven/angular-multi-select
            'Silverzone_service.Shared'
        ];

    angular.module('Silverzone_app.Common', moduleDependencies)
    .config(configFn)
    .constant('globalConfig', {
        //apiEndPoint: "http://silverzoneapi-dev.us-west-2.elasticbeanstalk.com/api/",
        apiEndPoint: "http://localhost:55615/api/",
        version: {
            Site: 'Site',
            User: 'User',
            Admin: 'Admin',
        }
    })      // constant or value is also provider/service > we can't modify constant value but can change value object
    .value('webUrl', { module: '' })
    .run(['$rootScope', 'notificationService', 'localStorageService', '$injector', '$state', 'webUrl', runFn])
    .provider('myService', function () {        // we can inherit angular built in services only
        this.$get = ['$log', '$http', 'globalConfig', function ($log, $http, globalConfig) {
            $log.log('provider is being called' + globalConfig);
            // we can't inject any custom (our own) service/provider in it.. but a factory/service can inject to each other
        }]
    })

    ;

    // auto/self invoke fx, can't call explicitly eg. > disable_logs();
    // to disable console & debug output in browser..
    (function disable_logs() {
        if (!window.console) window.console = {
        };
        var methods = ["log", "debug", "warn", "info"];
        for (var i = 0; i < methods.length; i++) {
            console[methods[i]] = function () {
            };
        }
    })();

})();
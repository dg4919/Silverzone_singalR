(function (app) {
    'use strict';

    injectorFn.$inject = ['$q', '$injector', '$location', 'localStorageService', '$log'];

    function injectorFn($q, $injector, $location, localStorageService, $log) {
       
        return {

            request: function (config) {
                config.headers = config.headers || {};
                var authData = localStorageService.get('authorizeData');
                //localStorageService.get('authorizeData').access_token
                
                if (authData) {
                    config.headers.Authorization = 'Bearer ' + authData.access_token;
                }
                return config;
            },          // contains what $http ajax request we r going to send over the web
            requestError: function (rejection) {            // if error occured while calling ajax, like parameter is not define/ ajax type > GET/post is mention wrong
                $log.error('Request error:', rejection);
                return $q.reject(rejection);
            },
            response: function (response) {          // when response is retrive successfully from server means > 200 sttus code
                return response;
            },
            responseError: function (rejection) {    // if there is 401(unauthorise) OR 500(internal server error) OR 404(not found) error coming from server

                if (rejection.status == 401) {       // injector use for suppose if we login & then clear browser history then authorise fx will give 401 error > unauthorise uer, then show login popUp & Logout

                    // server side clear > formauthetiation cookie
                    $injector.get('Service').Logout();     // calling directly service method

                    return $q.reject(rejection);    // contains promise object of unauthorise type
                }

                if (rejection.status == 500) {
                    alert('Oops! there is some internal error..')
                    return $q.reject(rejection);
                }

                $log.error('Response error:', rejection);
            }

        }

    }

   
   app.factory('httpInterceptor_Service', injectorFn);

})(angular.module('SilverzoneERP_App'));

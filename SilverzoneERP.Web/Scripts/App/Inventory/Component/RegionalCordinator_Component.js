//https://tests4geeks.com/build-angular-1-5-component-angularjs-tutorial/

(function () {
    'use strict';

    angular
        .module('SilverzoneERP_invenotry_component')  // create a new module Here
        .component('rcRegister', {
            templateUrl: '/Templates/inventory/RegionalCordinator/Register.html',
            controller: ['$scope', '$rootScope', 'dispatchService', function ($sc, $rsc, svc) {
                $sc.model = {};
            }]
        })

    ;

})();
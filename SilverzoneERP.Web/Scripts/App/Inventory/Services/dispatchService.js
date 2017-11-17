(function () {

    var fn = function ($http, $q, globalConfig, $filter) {

        var apiUrl = globalConfig.apiEndPoint + globalConfig.version.Inventory,
            fac = {};

        fac.saveItems = function (_model) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Dispatch/create_dispatch_otherItem',
                method: 'POST',
                data: _model
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        fac.get_schoolInfo = function (_schCode) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Dispatch/get_schoolBy_SchCode',
                method: 'GET',
                params:{
                    schCode: _schCode
                },
                cache: true
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }


        fac.get_allDispatchItem = function () {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Dispatch/get_otherDispatch_Items',
                method: 'GET',
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        return fac;
    }

    angular
      .module('SilverzoneERP_invenotry_service')
      .factory('dispatchService', ['$http', '$q', 'globalConfig', '$filter', fn]);


})();
(function (app) {

    var fn = function ($http, $q, globalConfig, $filter) {

        var apiUrl = globalConfig.apiEndPoint + globalConfig.version.School,
            fac = {};

        fac.get_countryJson = function () {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/location/GetCountry',
                method: 'GET',
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

        return fac;
    }

    angular
       .module('SilverzoneERP_invenotry_service', [])
       .factory('locationService', ['$http', '$q', 'globalConfig', fn]);

})();
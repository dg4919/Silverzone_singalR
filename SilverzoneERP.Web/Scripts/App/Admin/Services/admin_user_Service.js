
(function (app) {

    var fn = function ($http, $q, globalConfig) {

        var apiUrl = globalConfig.apiEndPoint + globalConfig.version.Admin,
             fac = {};

        fac.create_user = function (_model) {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({
                url: apiUrl + '/User/Register_user',
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


        return fac;
    }

    app.factory('admin_user_Service', ['$http', '$q', 'globalConfig', fn]);

})(angular.module('Silverzone_admin_app'));


(function () {

    var fn = function ($http, $q, globalConfig) {
        
        var apiUrl = globalConfig.apiEndPoint + globalConfig.version.Site,
            fac = {};

        //  ***********  Services Start from Here  ***********

        fac.getAll_class = function () {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Book/getAllClass',
                method: 'GET',
                cache: true      // CACHE WILL WORK > if we redirect 1 page to other using Angular client routing
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        fac.get_subject_byClassId = function (_classId) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Subject/Get_subjects_ByclassId',
                method: 'GET',
                params: { classId: _classId },
                cache: true      // CACHE WILL WORK > if we redirect 1 page to other using Angular client routing
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        fac.get_bookCategorys = function () {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Category/category_List',
                method: 'GET',
                cache: true      // CACHE WILL WORK > if we redirect 1 page to other using Angular client routing
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        fac.getAll_subject = function () {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Subject/Get_allSubjects',
                method: 'GET',
                cache: true      // CACHE WILL WORK > if we redirect 1 page to other using Angular client routing
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        fac.get_class_bySubjctId = function (_subjectId) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Subject/Get_class_BysubjectId',
                method: 'GET',
                params: { subjectId: _subjectId },
                cache: true      // CACHE WILL WORK > if we redirect 1 page to other using Angular client routing
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        //  ********  **************  **********

        return fac;
    }


    angular.module('Silverzone_service.Shared', [])
    .factory('sharedService', ['$http', '$q', 'globalConfig', fn]);

})();


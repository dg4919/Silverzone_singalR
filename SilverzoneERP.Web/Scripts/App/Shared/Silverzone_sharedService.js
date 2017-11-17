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
                async: false,
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
                async: false,
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

        fac.get_bookCategory_byClassId = function (_subjectId, _classId) {
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Category/get_category_byClass',
                method: 'GET',
                params: {
                    subjectId: _subjectId,
                    classId: _classId
                },
                async: false,
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

        fac.getBook_itemTitle = function () {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Category/get_existedBook_Title',
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


        //  ********  **************  **********

        fac.get_bookTitle = function () {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Category/get_bookTitle',
                method: 'GET',
                async: false,
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

        return fac;
    }


    angular.module('Silverzone_service.Shared', [])
    .factory('sharedService', ['$http', '$q', 'globalConfig', fn]);

})();


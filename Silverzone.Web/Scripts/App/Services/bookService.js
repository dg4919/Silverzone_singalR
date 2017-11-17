
(function (app) {

    var fn = function ($http, $q, globalConfig) {

        var apiUrl = globalConfig.apiEndPoint + globalConfig.version.Site,
            fac = {};

        // ********* Common functions for $http request to handle promise object success/fail *******

        function handleSuccess(response) {
            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            // direct using $q.defer() > instead of assinging it into a variable
            var defer = $q.defer();

            defer.resolve(response);
            //return $q.defer().promise;          // return promise contain data only
        }

        function handleError(error) {
            defer.reject('something went wrong..' + error);
            //return $q.defer().promise;       // return promise contain error only
        }

        //***********************  Function End   *****************************************

        // Service methods start from here 

        fac.get_bookDetail = function (book_Id) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Book/get_bookDetail_byId',
                method: 'GET',
                params: { bookId: book_Id },
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

        //*********************** Searcing of Books  *********************************
       
        fac.searchBooks = function (model) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Book/searchBooks',
                //dataType: 'json',
                //isArray: true,
                method: 'GET',
                params: {
                    classId: model.classId,
                    subjectId: model.subjectId,
                    book_categoriesId: model.cateogysId
                },
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

        fac.get_booksuggestion = function (book_id) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Book/getbook_suggestions',
                method: 'GET',
                params: { bookId: book_id },
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

        fac.getbook_recommends = function (book_id) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Book/getbook_recommends',
                method: 'GET',
                params: { bookId: book_id },
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

        //*********************** Books Bundle  *********************************

        fac.get_bookBundleDetail = function (bundleId) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Book/get_bookBundleDetail_byId',
                method: 'GET',
                params: { bundleId: bundleId },
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

        fac.getbook_bundles = function (class_id) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Book/getbook_bundles',
                method: 'GET',
                params: { classId: class_id },
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

    app.factory('bookService', ['$http', '$q', 'globalConfig', fn]);

})(angular.module('Silverzone_app'));


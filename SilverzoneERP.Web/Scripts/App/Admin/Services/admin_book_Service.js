(function (app) {

    var fn = function ($http, $q, globalConfig) {

        var apiUrl = globalConfig.apiEndPoint + globalConfig.version.Admin,
             fac = {};


        fac.upload_bookImage = function (files) {
            var form_Data = new FormData();

            for (var i = 0; i < files.length; i++) {
                form_Data.append("file", files[i]);
            }

            var defer = $q.defer();

            $http({
                method: 'POST',
                url: apiUrl + '/Book/upload_bookImage',
                data: form_Data,
                headers: {
                    'Content-Type': undefined
                }
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('file upload failed');
                });
            return defer.promise;
        }

        fac.createBook = function (_model) {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Book/create_book',
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

        fac.book_isExist = function (_titleId) {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Book/is_bookExist',
                method: 'GET',
                params: {
                    titleId: _titleId
                }
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        fac.updateBook = function (_model) {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Book/update_book',
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

        fac.get_books = function (_model) {
            debugger;

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Book/get_bookList',
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

        fac.get_bookInfo = function (bookId) {
            debugger;

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Book/get_book_byId',
                method: 'GET',
                params: { Id: bookId }
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        fac.deleteBook = function (bookId) {
            debugger;

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Book/delete_book',
                method: 'GET',
                params: { Id: bookId }
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        fac.setBook_outOfStock = function (bookId, _status) {
            debugger;

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Book/setBook_outOfstock',
                method: 'GET',
                params: {
                    Id: bookId,
                    status: _status
                }
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        // *****************  For Book Bundle  *****************

        fac.createBundle = function (_model) {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Book/create_bundle',
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

        fac.updateBundle = function (_model) {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Book/update_bundle',
                method: 'PUT',
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

        fac.get_Bundles = function () {
            debugger;

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Book/get_bundleList',
                method: 'GET'
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        fac.get_Bundle_byId = function (Id) {
            debugger;

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Book/get_bundlebyId',
                method: 'GET',
                params: { bundleId: Id }
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }


        fac.deleteBundle = function (entity) {
            debugger;

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Book/delete_bundle',
                method: 'DELETE',
                params: {
                    bundleId: entity.Id,
                    status: entity.Status
                }
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

    angular.module('Silverzone_admin_app')
        .factory('admin_book_Service', ['$http', '$q', 'globalConfig', fn]);

})();


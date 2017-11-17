
(function (app) {

    var fn = function ($http, $q, globalConfig) {

        var apiUrl = globalConfig.apiEndPoint + globalConfig.version.Admin,
             fac = {};

        fac.create_BooksCategory = function (_model) {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Category/Create',
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

        fac.get_BooksCategory = function () {
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Category/List',
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

        fac.update_BooksCategory = function (_model) {
            debugger;

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Category/Edit',
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

        fac.delete_BooksCategory = function (book_categoryId) {
            debugger;

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Category/Delete',
                method: 'GET',
                params: {
                    categoryId: book_categoryId
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

        fac.advanceFilter_BooksCategory = function (_model) {
            debugger;

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Category/Search',
                method: 'GET',
                params: {
                    name: _model.name,
                    status: _model.status
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

        //   ***************  For Coupons  **************

        fac.get_CouponName = function () {
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Category/coupon_names',
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

        fac.create_Coupon = function (_model) {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Category/coupon_Create',
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

        fac.get_Coupons = function () {
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Category/coupon_List',
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

        fac.update_coupon = function (_model) {
            debugger;

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Category/coupon_Edit',
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

        fac.delete_coupon = function (coupon) {
            debugger;

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Category/coupon_Delete',
                method: 'GET',
                params: {
                    couponId: coupon.Id,
                    status: coupon.Status
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

        fac.get_items = function () {
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Category/get_Item',
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

        fac.Create_ItemTitle = function (_model) {
            debugger;

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Category/Create_ItemTitle',
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

    app.factory('admin_bookCategory_Service', ['$http', '$q', 'globalConfig', fn]);

})(angular.module('Silverzone_admin_app'));


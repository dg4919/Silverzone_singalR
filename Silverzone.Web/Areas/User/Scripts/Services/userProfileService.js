(function () {

    var fn = function ($http, $q, globalConfig) {

        // define multiple variables at a time
        var apiUrl = globalConfig.apiEndPoint + globalConfig.version.User,
            fac = {};

        fac.uploadImage = function (files) {
            var form_Data = new FormData();

            for (var i = 0; i < files.length; i++) {
                form_Data.append("file", files[i]);
            }

            var defer = $q.defer();

            $http({
                method: 'POST',
                url: apiUrl + '/UserProfile/uploaduserImage',
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

        fac.update_userinfo = function (_model) {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({
                url: apiUrl + '/UserProfile/saveProfile',
                method: 'POST',
                data: _model    // if we have to pass one model then pass wthout write '{ model:  _model } in case of web API
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        fac.get_userOrders = function () {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/UserProfile/getAll_userOrders',
                method: 'GET',
                //cache: true       // order can be add > hence every time we get data from server :)
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        fac.get_OrderInfo_byOrderId = function (order_id) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/UserProfile/get_orerInfo',
                method: 'GET',
                params: { orderId: order_id },
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

        fac.change_userPassword = function (_model) {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({
                url: apiUrl + '/UserProfile/chaange_userPassword',
                method: 'POST',
                params: {
                    oldPassword: _model.newPassword,
                    newPassword: _model.oldPassword
                }     // use params to send parameter for POST reqst
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        fac.change_user_emailMobile = function (_model) {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({
                url: apiUrl + '/UserProfile/change_EmailMobile',
                method: 'POST',
                data: _model     // use params to send parameter for POST reqst
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        fac.verify_forgetLogin_OTP = function (_model) {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({
                url: apiUrl + '/UserProfile/validate_OTP',
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

        fac.update_user_EmailMobile = function (_model) {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({
                url: apiUrl + '/UserProfile/update_EmailMobile',
                method: 'POST',
                params: {
                    EmailID: _model.email,
                    MobileNumber: _model.mobile
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

        fac.get_userQuiz = function () {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/UserProfile/get_userToday_quiz',
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

        fac.get_user_megaQuiz = function () {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/UserProfile/get_userMega_quiz',
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

        fac.save_userQuiz = function (_model) {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({
                url: apiUrl + '/UserProfile/save_userQuiz',
                method: 'POST',
                params: {
                    quizId: _model.quizId,
                    answerId: _model.answerId
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

        fac.get_userQuiz_history = function () {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/UserProfile/get_userQuiz_history',
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

    angular.module('Silverzone_app')
        .factory('userProfile_service', ['$http', '$q', 'globalConfig', fn]);


})();
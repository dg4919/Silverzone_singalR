// contains all ajax related to user A/c > like regiter,login, forget/reset passowrd etc

(function () {

    var fn = function ($http, $q, globalConfig, localStorageService, $filter, $rsc) {

        // define multiple variables at a time
        var apiUrl = globalConfig.apiEndPoint + globalConfig.version.Site,
            fac = {};

        fac.sendSms_onPhone = function (_mobileNo) {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Account/send_sms_onPhone',
                method: 'GET',
                params: { mobileNo: _mobileNo }     // use params to send parameter for GET reqst
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }
      
        fac.verify_OTP = function (_model) {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Account/verify_OTP',
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

        fac.Register = function (_model) {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Account/Register_user',
                method: 'POST',
                data: _model     // use params to send parameter for GET reqst
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        fac.Login = function (_model) {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({                
                url: apiUrl + '/Account/Login',
                method: 'POST',
                headers: {
                    'Access-Control-Allow-Origin': true
                },
                data: _model
            })
                .success(function (d) {
                    if (d.result === 'ok') {
                       fac.saveUserInfo(d.user, d.token);
                    }

                    defer.resolve({
                        msg: d.result,
                        role: d.user !== undefined ? d.user.Role : ''   // role use in admin login section
                    });     // only will pass status of login
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        fac.Logout = function () {

            localStorageService.remove('authorizeData');

            $rsc.user.currentUser = '';
            $rsc.user.is_login = false;
        }

        fac.saveUserInfo = function(user, token) {

            if (user.DOB !== null)
                user.DOB = $filter('dateFormat')(user.DOB, 'MM/DD/YYYY');

            // to refresh image add a unique datetime string
            user.ProfilePic = user.ProfilePic !== null ? (user.ProfilePic + '?' + $filter('date')(new Date(), 'HHmmss')) : null;

            var entity = {
                userInfo: user,
                tokenInfo: token
            }

            // local storage store user info in browser & when ever user clear history then it will also removed
            localStorageService.set('authorizeData', entity);

            $rsc.user.currentUser = user;
            $rsc.user.is_login = true;
        }


        //*********  Forget Login ******************

        fac.forgetPassword = function (_model) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Account/forget_password',
                method: 'POST',
                dataType: 'json',
                //headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                data: JSON.stringify(_model)
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
                url: apiUrl + '/Account/verify_forgetLogin_OTP',
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

        fac.save_newPassword = function (_model) {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Account/saved_newforget_password',
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

    angular.module('Silverzone_service.Shared')
        .factory('user_Account_Service', ['$http', '$q', 'globalConfig', 'localStorageService', '$filter', '$rootScope', fn]);

})();


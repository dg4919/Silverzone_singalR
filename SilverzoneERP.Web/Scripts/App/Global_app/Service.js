(function () {
    'use strict';

    var fun = function ($http, $q, globalConfig, localStorageService, $rootScope, $location,$filter, $modal) {
        var apiUrl = globalConfig.apiEndPoint;
        var fac = {}

        fac.guid = function () {
            var d = new Date().getTime();
            if (window.performance && typeof window.performance.now === "function") {
                d += performance.now(); //use high-precision timer if available
            }
            var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
                var r = (d + Math.random() * 16) % 16 | 0;
                d = Math.floor(d / 16);
                return (c == 'x' ? r : (r & 0x3 | 0x8)).toString(16);
            });
            return uuid;
        }

        fac.Login = function (_model) {
            debugger;
            var defer = $q.defer();
            //  var authorizeData = localStorageService.getItem('authorizeData');
            // prompt('', JSON.stringify(authorizeData));
            $http({
                url: apiUrl + 'School/account/Login',
                method: 'POST',
                data: _model
            }).success(function (response) {
                //prompt('', JSON.stringify(response));
                localStorageService.set('authorizeData', response.token);

                defer.resolve(response);
            }).error(function (error) {
                defer.reject('something went wrong..');
            });

            return defer.promise;
        }

        fac.Logout = function () {
            debugger;
            localStorageService.remove('authorizeData');
            localStorageService.remove('UserInfo');
            localStorageService.remove('BackColor');
            localStorageService.remove('Event');
            localStorageService.remove('Permission');
            $rootScope.UserInfo = {};
            $location.path("/ERP/Login");
        }

        fac.Create_Update = function (_model,_url) {
            debugger;
            var defer = $q.defer();
         
            $http({
                url: apiUrl + _url,
                method: 'POST',
                data: _model                
            }).success(function (response) {                
                defer.resolve(response);
            }).error(function (error) {
                defer.reject('something went wrong..');
            });

            return defer.promise;
        }

        fac.Get = function (_url) {
            debugger;
            var defer = $q.defer();

            $http({
                url: apiUrl + _url,
                method: 'GET'               
            })
                .success(function (response) {
                    //  prompt('', JSON.stringify(response));
                    defer.resolve(response);
                })
                .error(function (error) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        fac.Delete = function (Id,_url) {
            var defer = $q.defer();

            $http({
                url: apiUrl + _url,
                method: 'DELETE',
                params: {
                    Id: Id
                }
            })
                .success(function (response) {
                    //  prompt('', JSON.stringify(response));
                    defer.resolve(response);
                })
                .error(function (error) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        var onLoad = function (reader, deferred, scope) {
            return function () {
                scope.$apply(function () {
                    deferred.resolve(reader.result);
                });
            };
        };

        var getReader = function (deferred, scope) {
            var reader = new FileReader();
            reader.onload = onLoad(reader, deferred, scope);
            // reader.onerror = onError(reader, deferred, scope);
            // reader.onprogress = onProgress(reader, scope);
            return reader;
        };

        fac.readAsDataURL = function (file, scope) {

            var deferred = $q.defer();

            var reader = getReader(deferred, scope);
            reader.readAsDataURL(file);

            return deferred.promise;
        };
        
        fac.Reset = function (scope) {
            scope.frm.$setPristine();
            scope.frm.$setValidity();
            scope.frm.$setUntouched();
        }
       
        fac.SelectedIndexChanged = function (selectedIndex, data) {
            debugger;
            var IsAcive;
            var JsonArray = angular.copy(data);

            if (selectedIndex == "1")
                IsAcive = true;
            else if (selectedIndex == "2")
                IsAcive = false;
            else
                return JsonArray;

            var filterData = $filter("filter")(JsonArray, { Status: IsAcive }, true);

            return filterData;
        }

        fac.IsActive=function(Id, IsActiveList, scope)
        {
            var data = $filter("filter")(IsActiveList, { Id: Id }, true);
            if (data.length == 0) {
                IsActiveList.push({ "Id": Id });
            }
            else {
                IsActiveList.splice(data[0], 1);
            }
            if (IsActiveList.length == 0)
                scope.Changed = false;
            else
                scope.Changed = true;
            return IsActiveList;
        }
      
        fac.Alert = function (rootScope,message) {
            $.alert({
                title: rootScope.Title,
                content: message,
            });
        }
        fac.Notification = function (rootScope, message) {
            rootScope.notify_fx(message, 'info');
        }
        return fac;
    }

    angular.module('SilverzoneERP_App')
    .factory('Service', ['$http', '$q', 'globalConfig', 'localStorageService', '$rootScope', '$location', '$filter', fun]);
})();

(function (app) {

    var user_fun = function ($scope, $state, $rootScope, Service, globalConfig, localStorageService, hubSvc) {
           
        $scope.Account = {};
        $scope.msg = '';
        $rootScope.UserInfo = {};
        $scope.Login = function () {
            debugger;
            // prompt('', JSON.stringify($scope.Account));

            Service.Login($scope.Account).then(function (response) {
                if (response.result == 'Success') {
                    debugger;

                    $rootScope.UserInfo = {
                        "UserId": response.user.Id,
                        "UserName": response.user.UserName,
                        "RoleName": response.user.Role.RoleName,
                        "ImgPrefix": globalConfig.apiEndPoint.replace("/api/", "/"),
                        "ProfilePic": globalConfig.apiEndPoint.replace("/api/", "/") + (response.user.ProfilePic == null ? 'ProfilePic/placeholderImage.png' : response.user.ProfilePic),
                        "menu": response.menu,
                        "ShowMenu": true
                    };

                    angular.extend($rootScope.UserInfo, { menuList: createMenuArray() });
                    $rootScope.EventInfo = response.Event;
                    localStorageService.set("EventInfo", $rootScope.EventInfo);
                    localStorageService.set("UserInfo", $rootScope.UserInfo);
                    $state.go("ERP_Home");
                }
                else
                    Service.Notification($rootScope, response.message);
            });
        }

        function createMenuArray() {
            var arr = [];

            angular.forEach($rootScope.UserInfo.menu, function (entity, key) {
                angular.forEach(entity.Forms, function (form, key) {
                    if (form.subForms.length === 0) {
                        angular.extend(form, { moduleName: entity.FormName });
                        arr.push(form);
                    }
                    else {
                        angular.forEach(form.subForms, function (subform, key) {
                            angular.extend(subform, { moduleName: entity.FormName });
                            arr.push(subform);
                        });
                    }
                });
            });
            return arr;
        }

        $scope.validationOptions = {
            rules: {
                EmailID: {
                    required: true
                },
                Password: {
                    required: true
                }
            }
        }

    }
   
    app.controller('accountController', ['$scope', '$state', '$rootScope', 'Service', 'globalConfig', 'localStorageService', user_fun]);

})(angular.module('SilverzoneERP_App'));
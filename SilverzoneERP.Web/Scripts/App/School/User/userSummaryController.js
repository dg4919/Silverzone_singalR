(function () {
    var user_Summary_fn = function ($scope, $rootScope, Service, userPermissionService, $q, globalConfig, $http, localStorageService, roleService, $modal) {
        $scope.Tabactive = 1;
        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;
        $scope.rangeSize = 5;
        $scope.startIndex = 0;
        

        $scope.UserSummary = {"Limit":5,"UserId":0,"RoleId":0};
        $scope.resetCopy = angular.copy($scope.UserSummary);                
        $scope.isEdit = false;
        $scope.selectedItem;
        
        $rootScope.UserInfo = localStorageService.get('UserInfo');

        function Reset() {
            $scope.UserSummary = angular.copy($scope.resetCopy);           
            $scope.selectedItem;
            $scope.isEdit = false;
        }
        $scope.Back = function () {
            Reset();
        }
        //Reset();
        function GetRole() {
            Service.Get('school/user/GetRole').then(function (response) {

                $scope.RoleList = response.result;               
            });
        }
        GetRole();

        function GetUserList() {
            Service.Get('school/account/GetAllUser').then(function (response) {
                $scope.UserList = response.result;
            });
        }

        GetUserList();

        $scope.Generate = function () {
            debugger;
            
            $scope.itemsPerPage = $scope.UserSummary.Limit;
            $scope.currentPage = 1;
            $scope.rangeSize = 5;
            $scope.startIndex = 0;

            Service.Get('school/user/GetUserSummary?UserId=' + $scope.UserSummary.UserId + '&RoleId=' + $scope.UserSummary.RoleId + '&StartIndex=0&Limit=' + $scope.UserSummary.Limit).then(function (response) {
                $scope.UserRoleList = response.result;             
            });
        }

      
        $scope.searchTextChange = function (UserName) {
            $scope.UserList = GetUserList(UserName, 2);
        }

        $scope.selectedItemChange = function (item) {            
            if (!angular.isUndefined(item)) {
                $scope.UserSummary.UserId = item.UserId;               
            }
            else {
                $scope.UserSummary.UserId = 0;                
            }           
        }

        //function GetUserList(query) {

        //    var defer = $q.defer();

        //    $http({
        //        url: globalConfig.apiEndPoint + 'school/user/SearchUserRole?UserName=' + query + '&limit=' + 2,
        //        method: 'GET'
        //    })
        //        .success(function (response) {
        //            defer.resolve(response.result);
        //        })
        //        .error(function (error) {
        //            defer.reject('something went wrong..');
        //        });
        //    return defer.promise;
        //}

        var Preserve_UserPermissionList;

        $scope.Details = function (ev, item, data) {
            debugger;
            $scope.UserSummary.UserId = data.UserId;
           
            userPermissionService.GetUserPermission(data.UserId, item.RoleId).then(function (response) {
                Preserve_UserPermissionList = response.Preserve_UserPermission;
                $scope.UserPermissionList = response.UserPermission;
                $scope.isDetail = true;
                $scope.selectedItem = { "RoleName": item.RoleName, "EmailID": data.EmailID, "ProfilePic": data.ProfilePic, "UserName": data.UserName };
                $scope.isEdit = true;
            });
        }

        $scope.Checked_Unchecked_GroupWise = function (data, index, isDirect) {
            roleService.Checked_Unchecked_GroupWise(data, index, isDirect);
        }

        $scope.Checked_Unchecked = function (data, index) {
            roleService.Checked_Unchecked(data, index);
        }

        $scope.All = function (data) {
            debugger;
            roleService.All(data);
        }

        $scope.Submit = function () {            
            debugger;
            if ($scope.UserSummary.UserId == 0)
                return false;

            userPermissionService.Submit($scope.UserSummary.UserId, $scope.UserPermissionList, Preserve_UserPermissionList)
            .then(function (response) {
                alert(response.message);
                Reset();
            });          
        }

        //Start pagination 

        $scope.$watch("currentPage", function (newValue, oldValue) {
            debugger;
            
            
            var startIndex = ((newValue - 1) * $scope.itemsPerPage);

            Service.Get('school/user/GetUserSummary?UserId=' + $scope.UserSummary.UserId + '&RoleId=' + $scope.UserSummary.RoleId + '&StartIndex=' + startIndex + '&Limit=' + $scope.UserSummary.Limit).then(function (response) {

                //GetUserPermissionList(((newValue - 1) * $scope.itemsPerPage) + 1, $scope.itemsPerPage).then(function (response) {
                //var json = JSON.parse(response.data);
                debugger;
                $scope.UserRoleList = response.result;
                $scope.total = response.count;

                $scope.range = function () {
                    var ret = [];
                    var start = ((Math.ceil($scope.currentPage / $scope.rangeSize) - 1) * $scope.rangeSize) + 1;
                    for (var i = start; i < (start + $scope.rangeSize) && i <= $scope.pageCount() ; i++)
                        ret.push(i);
                    return ret;
                };


                $scope.prevPage = function () {
                    if ($scope.currentPage > 1) {
                        $scope.currentPage--;
                    }
                };

                $scope.prevPageDisabled = function () {
                    return $scope.currentPage === 1 ? "disabled" : "";
                };

                $scope.firstPage = function () {
                    $scope.currentPage = 1;
                }
                $scope.firstPageDisabled = function () {
                    return $scope.currentPage === 1 ? "disabled" : "";
                };

                $scope.lastPage = function () {
                    $scope.currentPage = $scope.pageCount();
                }
                $scope.lastPageDisabled = function () {
                    return $scope.currentPage === $scope.pageCount() ? "disabled" : "";
                };

                $scope.nextPage = function () {
                    if ($scope.currentPage <= $scope.pageCount()) {
                        $scope.currentPage++;
                    }
                };

                $scope.nextPageDisabled = function () {
                    return $scope.currentPage === $scope.pageCount() ? "disabled" : "";
                };

                $scope.pageCount = function () {
                    return Math.ceil($scope.total / $scope.itemsPerPage);
                };

                $scope.setPage = function (n) {
                    if (n > 0 && n <= $scope.pageCount()) {
                        $scope.currentPage = n;
                    }
                };

            }, function () {

            })
        });

    }

    angular.module('SilverzoneERP_App')
   .controller('userSummaryController', ['$scope', '$rootScope', 'Service', 'userPermissionService', '$q', 'globalConfig', '$http', 'localStorageService', 'roleService', user_Summary_fn]);
   
})();
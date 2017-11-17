(function(app) {    

    var userPermission_fun = function ($scope, $rootScope, Service, userPermissionService, globalConfig, $filter, roleService, $q, $timeout, $http, $location, localStorageService, $modal) {
        
        $scope.UserPermission = { "UserId": 0, "Forms": [], "RoleId": 0 };
        $scope.resetCopy = angular.copy($scope.UserPermission);       
        var RoleList_All;
        $scope.Changed = false;        
        $scope.isEdit = false;
        $scope.UserPermissionList = [];

        $rootScope.UserInfo = localStorageService.get('UserInfo');

        function GetUserList() {
            Service.Get('school/account/GetAllUser').then(function (response) {
                $scope.UserList = response.result;
            });
        }

        GetUserList();

        function Reset() {            
            $scope.UserPermission = angular.copy($scope.resetCopy);

            $scope.UserPermission = { "UserId": 0, "Forms": [], "RoleId": 0 };
            $scope.resetCopy = angular.copy($scope.UserPermission);
            var RoleList_All;
            $scope.Changed = false;
            $scope.isEdit = false;
            $scope.UserPermissionList = [];
        }
        $scope.Back = function () {
            Reset()
        }
        
          

        $scope.newState = function (state) {
            alert("Sorry! You'll need to create a Constitution for " + state + " first!");
        }

      
        $scope.searchTextChange = function (UserName) {            
            $scope.UserList = GetUserList(UserName);      
        }

        $scope.selectedItemChange = function (UserId) {
            debugger;

            $scope.UserPermissionList = [];
            $scope.selectedItem = $filter("filter")($scope.UserList, { Id: UserId }, true);
            if ($scope.selectedItem.lenght != 0)
            {
                $scope.UserPermission.UserId = UserId;
                $scope.UserPermission.RoleId = $scope.selectedItem[0].RoleId;
                
                GetUserPermissionList($scope.UserPermission.UserId, $scope.UserPermission.RoleId);
                $scope.selectedItem = $scope.selectedItem[0];                
                
            }
            else
            {
                $scope.UserPermission.UserId = 0;
                $scope.UserPermission.RoleId = 0;
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

        function GetUserPermissionList(UserId, RoleId) {
            debugger;
            if (UserId == 0 && RoleId == 0)
            {
                Preserve_UserPermissionList = [];
                $scope.UserPermissionList = [];
            }
            else {
                userPermissionService.GetUserPermission(UserId, RoleId).then(function (response) {
                    debugger;
                    Preserve_UserPermissionList = response.Preserve_UserPermission;
                    $scope.UserPermissionList = response.UserPermission;
                    if ($scope.UserPermissionList.lenght != 0)
                        $scope.isEdit = true;
                    else
                        alert('Permission not assigned ');
                });
            }            
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
            if ($scope.UserPermission.UserId == 0)
                return false;

            userPermissionService.Submit($scope.UserPermission.UserId, $scope.UserPermissionList, Preserve_UserPermissionList)
            .then(function (response) {
                alert(response.message);
                Reset();
            });
            //var obj = {};
            //obj.UserId = $scope.UserPermission.UserId
            //obj.Forms = [];

            //angular.forEach($scope.FormList, function (item,Parentindex) {

            //    angular.forEach(item.Forms, function (frm,ChildIndex) {
            //        debugger;
            //        if ((frm.Permission.Add == true || frm.Permission.Edit == true || frm.Permission.Read == true || frm.Permission.Print == true || frm.Permission.Delete == true) || (frm.Permission.Add == false && frm.Permission.Edit == false && frm.Permission.Read == false && frm.Permission.Print == false && frm.Permission.Delete == false)) {
            //            if (formData[Parentindex].Forms[ChildIndex].Permission.Add != frm.Permission.Add || formData[Parentindex].Forms[ChildIndex].Permission.Edit != frm.Permission.Edit || formData[Parentindex].Forms[ChildIndex].Permission.Read != frm.Permission.Read || formData[Parentindex].Forms[ChildIndex].Permission.Print != frm.Permission.Print || formData[Parentindex].Forms[ChildIndex].Permission.Delete != frm.Permission.Delete) {
            //                var for_obj = {"Id": frm.Id, "Permission": JSON.stringify(frm.Permission) };
            //                obj.Forms.push(for_obj);
            //            }
            //        }                    
            //    });
            //});
           
            //Service.Create_Update(obj, 'user/userPermission').then(function (response) {
            //    if (response.result == 'Success') {
            //        alert(response.message);
            //        Reset();
            //    }
            //    else
            //        alert(response.message);
            //});
           
        }
                                
    }

    
    app.controller('userPermissionController', ['$scope', '$rootScope', 'Service', 'userPermissionService', 'globalConfig', '$filter', 'roleService', '$q', '$timeout', '$http', '$location', 'localStorageService', userPermission_fun]);
})(angular.module('SilverzoneERP_App'));

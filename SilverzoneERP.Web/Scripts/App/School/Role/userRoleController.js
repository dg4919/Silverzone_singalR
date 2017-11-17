(function (app) {

    var userRole_fun = function ($scope, Service, $filter, $modal) {

        $scope.UserRole = { "Users": [], "RoleId": 0 };
        $scope.resetCopy = angular.copy($scope.UserRole);
        var IsActiveList = [];
        var UserRoleList_All;
        $scope.Changed = false;
        $scope.selectedIndex = "1";
        $scope.isEdit = false;


        function Reset() {
            $scope.UserRole = angular.copy($scope.resetCopy);            
            $scope.isEdit = false;
            $scope.selectedIndex = "1";
            $scope.Changed = false;
            IsActiveList = [];
            UserRoleList_All = null;
            De_Select();        
        }
        function De_Select() {
            angular.forEach($scope.UserList, function (item, index) {
                item.ticked = false;
            });
        }
        $scope.Add = function () {
            debugger;
            $scope.isEdit = true;            
        }
        $scope.Back = function () {
            debugger;
            Reset();         
        }

        function GetRole() {
            Service.Get('school/role/Get').then(function (response) {
                $scope.RoleList = response.result;                               
            });
        }

        function GetUserList() {
            Service.Get('school/account/GetAllUser').then(function (response) {
                $scope.UserList = response.result;
            });
        }

        function GetUserRole() {
            Service.Get('school/user/GetUserRole').then(function (response) {
                $scope.UserRoleList = response.result;              
            });
        }

        GetRole();

        GetUserList();

//        GetUserRole();

        $scope.Submit = function () {
            if ($scope.UserRole.Users.length == 0 || $scope.UserRole.RoleId == 0)
                return false;
            Service.Create_Update($scope.UserRole, 'school/user/userRole').then(function (response) {
                if (response.result == 'Success') {
                    alert(response.message);
                    Reset();
                    GetRole();
                    GetUserRole();
                    De_Select();
                }
                else
                    alert(response.message);
            });
        }

        $scope.Edit = function (data) {
            debugger;

            De_Select();            
            $scope.UserRole.Users = [{ "Id": data.UserId, "ticked": true }];
            $scope.UserRole.RoleId = data.RoleId;
            
            var getUser = $filter("filter")($scope.UserList, { Id: data.UserId }, true);
            if(getUser.length!=0)
            {
                getUser[0].ticked = true;
            }
            $scope.isEdit = true;
        }                       
    }

    
    app.controller('userRoleController', ['$scope', 'Service', '$filter', userRole_fun]);

})(angular.module('SilverzoneERP_App'));
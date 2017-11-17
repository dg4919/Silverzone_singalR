(function (app) {
    var Category_fun = function ($scope, $location, $rootScope, Service, $state, $filter) {

        var IsActiveList = [];
        var SchoolGroupList_All;

        function Reset() {
            debugger;
            $scope.SchoolGroupObj = {};
            $scope.IsAdd = true;
            $scope.selectedIndex = "1";
            IsActiveList = [];
            SchoolGroupList_All = null;
            $scope.Changed = false;
                
            Service.Get('schoolGroup/Get').then(function (response) {
                SchoolGroupList_All = JSON.stringify(response.result);
                $scope.SelectedIndexChanged($scope.selectedIndex);
            });
        }
        Reset();
       
        $scope.Back = function () {
            debugger;
            $scope.IsAdd = true;
        }
        $scope.Add = function () {
            debugger;
            $scope.SchoolGroupObj = {};
            $scope.IsAdd = false;
        }
        $scope.Edit = function (data) {
            debugger;
            $scope.SchoolGroupObj = { "Id": data.Id, "SchoolGroupName": '' + data.SchoolGroupName, "RowVersion": data.RowVersion };

            $scope.IsAdd = false;
        }
        $scope.Submit = function () {
            debugger;
            Service.Create_Update($scope.SchoolGroupObj, 'schoolGroup/Create_Update').then(function (response) {
                Service.Alert(response.message);
                if (response.result == 'Success') {                  
                    Reset();
                }                
            });
        }
        $scope.SelectedIndexChanged = function (selectedIndex) {
            debugger;
            $scope.SchoolGroupList = Service.SelectedIndexChanged(selectedIndex, SchoolGroupList_All);
        }
       

        $scope.IsActive = function (Id) {
            debugger;
            IsActiveList = Service.IsActive(Id, IsActiveList, $scope);
        }

        $scope.Active_Deactive = function () {
            Service.Create_Update(IsActiveList, 'schoolGroup/Active_Deactive')
             .then(function (response) {
                 Service.Alert(response.message);
                 if (response.result == 'Success') {
                     Reset();
                 }
             });
        }

    }
    app.controller('schoolGroupController', ['$scope', '$location', '$rootScope', 'Service', '$state', '$filter', Category_fun]);

})(angular.module('SilverzoneERP_App'));

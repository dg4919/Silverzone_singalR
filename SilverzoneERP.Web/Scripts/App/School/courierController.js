(function (app) {
    var class_fun = function ($scope, $location, $rootScope, Service, $state, $filter) {

        var IsActiveList = [];
        var CourierList_All;

        function Reset() {
            debugger;
            $scope.CourierObj = {};
            $scope.IsAdd = true;
            $scope.selectedIndex = "1";
            IsActiveList = [];
            CourierList_All = null;
            $scope.Changed = false;

            Service.Get('courier/Get').then(function (response) {                
                CourierList_All = JSON.stringify(response.result);
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
            $scope.CourierObj = {};
            $scope.IsAdd = false;
        }
        $scope.Edit = function (data) {
            debugger;
            $scope.CourierObj = { "Id": data.Id, "CourierName": '' + data.ClassName, "RowVersion": data.RowVersion };

            $scope.IsAdd = false;
        }
        $scope.Submit = function () {
            debugger;
            Service.Create_Update($scope.CourierObj, 'courier/Create_Update').then(function (response) {
                Service.Alert(response.message);
                if (response.result == 'Success') {                  
                    Reset();
                }                
            });
        }

        $scope.SelectedIndexChanged = function (selectedIndex) {
            debugger;
            $scope.CourierList = Service.SelectedIndexChanged(selectedIndex, CourierList_All);
        }       

        $scope.IsActive = function (Id) {
            debugger;
            IsActiveList = Service.IsActive(Id, IsActiveList, $scope);
        }

        $scope.Active_Deactive = function () {
            Service.Create_Update(IsActiveList, 'courier/Active_Deactive')
             .then(function (response) {
                 Service.Alert(response.message);
                 if (response.result == 'Success') {
                     Reset();
                 }
             });
        }

    }
    app.controller('courierController', ['$scope', '$location', '$rootScope', 'Service', '$state', '$filter', class_fun]);

})(angular.module('SilverzoneERP_App'));
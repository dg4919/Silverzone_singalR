(function (app) {
    var Category_fun = function ($scope, $location, $rootScope, Service, $state, $filter) {

        var IsActiveList = [];
        var CategoryList_All;

        function Reset() {
            debugger;
            $scope.CategoryObj = {};
            $scope.IsAdd = true;
            $scope.selectedIndex = "1";
            IsActiveList = [];
            CategoryList_All = null;
            $scope.Changed = false;
                
            Service.Get('schCategory/Get').then(function (response) {
                CategoryList_All = JSON.stringify(response.result);
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
            $scope.CategoryObj = {};
            $scope.IsAdd = false;
        }
        $scope.Edit = function (data) {
            debugger;
            $scope.CategoryObj = { "Id": data.Id, "CategoryName": '' + data.CategoryName, "RowVersion": data.RowVersion };

            $scope.IsAdd = false;
        }
        $scope.Submit = function () {
            debugger;
            Service.Create_Update($scope.CategoryObj, 'schCategory/Create_Update').then(function (response) {
                Service.Alert(response.message);
                if (response.result == 'Success') {                  
                    Reset();
                }                
            });
        }
        $scope.SelectedIndexChanged = function (selectedIndex) {
            debugger;
            $scope.CategoryList = Service.SelectedIndexChanged(selectedIndex, CategoryList_All);
        }

     

        $scope.IsActive = function (Id) {
            debugger;
            IsActiveList = Service.IsActive(Id, IsActiveList, $scope);
        }

        $scope.Active_Deactive = function () {
            Service.Create_Update(IsActiveList, 'schCategory/Active_Deactive')
             .then(function (response) {
                 Service.Alert(response.message);
                 if (response.result == 'Success') {
                     Reset();
                 }
             });
        }

    }
    app.controller('categoryController', ['$scope', '$location', '$rootScope', 'Service', '$state', '$filter', Category_fun]);

})(angular.module('SilverzoneERP_App'));
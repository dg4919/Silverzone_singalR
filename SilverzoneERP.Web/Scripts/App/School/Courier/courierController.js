
(function (app) {

    var desg_fun = function ($scope,$rootScope, $state, Service, globalConfig, $filter) {

        //Start Declare Global Variable
        $scope.Courier = {};
        $scope.resetCopy = angular.copy($scope.Courier);
        var IsActiveList = [];
        var CourierList_All;
        $scope.Changed = false;
        $scope.selectedIndex = "1";
        $scope.isEdit = false;
        //End Declare Global Variable
              
        function GetCourier() {
            Service.Get('school/Courier/Get').then(function (response) {
                CourierList_All = response.result;
                $scope.SelectedIndexChanged($scope.selectedIndex);
            });
        }

        GetCourier();
      
        $scope.Submit = function (form) {
            debugger;
            if (form.validate() == false)
                return false;
            Service.Create_Update($scope.Courier, 'school/Courier/Create_Update')
            .then(function (response) {
                Service.Notification($rootScope, response.message);
                if (response.result == 'Success') {
                    $state.reload();
                }               
            });
        }

        $scope.validationOptions = {
            rules: {
                Courier_Name: {          
                    required: true,
                    maxlength: 50
                },
                Courier_Link: {                             
                    maxlength: 100
                }
            }
        }

        $scope.Add = function () {
            debugger;
            if ($scope.Changed) {
                var result = confirm('Do you want to save change ?');
                if (result == true) {
                    $scope.Active_Deactive();
                    $scope.Changed = false;
                } else {                    
                    $scope.SelectedIndexChanged($scope.selectedIndex);
                    $scope.Changed = false;
                }
            }
            $scope.isEdit = true;           
        }

        $scope.Back = function () {
            $scope.Courier = angular.copy($scope.resetCopy);
            $scope.isEdit = false;
            Service.Reset($scope);
        }
        $scope.Edit = function (data) {
            debugger;
            if ($scope.Changed) {
                var result = confirm('Do you want to save change ?');
                if (result == true) {
                    $scope.Active_Deactive();                    
                    $scope.Changed = false;
                    IsActiveList = [];
                } else {                    
                    $scope.Changed = false;
                    IsActiveList = [];
                    $scope.SelectedIndexChanged($scope.selectedIndex);             
                }
            }                  
            $scope.Courier = { "Id": data.Id, "Courier_Name": data.Courier_Name, "Courier_Link": data.Courier_Link, "RowVersion": data.RowVersion };
            $scope.isEdit = true;
        }
       
        
        
        $scope.IsActive = function (Id) {
            debugger;            
            IsActiveList = Service.IsActive(Id, IsActiveList, $scope);
        }
        
        $scope.SelectedIndexChanged = function (selectedIndex) {
            debugger;
            $scope.CourierList = Service.SelectedIndexChanged(selectedIndex, CourierList_All);
        }
      
        $scope.Active_Deactive = function () {
            Service.Create_Update(IsActiveList, 'school/Courier/Active_Deactive')
             .then(function (response) {
                Service.Notification ($rootScope, response.message);
                 if (response.result == 'Success') {
                     GetCourier();
                     IsActiveList = [];
                     $scope.Changed = false;
                 }
             });
        }
    }

    app.controller('courierController', ['$scope', '$rootScope', '$state', 'Service', 'globalConfig', '$filter', desg_fun]);

})(angular.module('SilverzoneERP_App'));
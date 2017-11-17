
(function (app) {

    var desg_fun = function ($scope,$rootScope, $state, Service, globalConfig, $filter) {

        //Start Declare Global Variable
        $scope.Category = {};
        $scope.resetCopy = angular.copy($scope.Category);
        var IsActiveList = [];
        var CategoryList_All;
        $scope.Changed = false;
        $scope.selectedIndex = "1";
        $scope.isEdit = false;
        //End Declare Global Variable
              
        function GetCategory() {
            Service.Get('school/schoolCategory/Get').then(function (response) {
                CategoryList_All = response.result;
                $scope.SelectedIndexChanged($scope.selectedIndex);
            });
        }

        GetCategory();
      
        $scope.Submit = function (form) {
            debugger;
            if (form.validate() == false)
                return false;
            Service.Create_Update($scope.Category, 'school/schoolCategory/Create_Update')
            .then(function (response) {
                Service.Notification($rootScope, response.message);
                if (response.result == 'Success') {
                    $state.reload();
                }               
            });
        }

        $scope.validationOptions = {
            rules: {
                CategoryName: {            // field will be come from component
                    required: true,
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
            $scope.Category = angular.copy($scope.resetCopy);
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
            $scope.Category = { "Id": data.Id, "CategoryName": data.CategoryName, "RowVersion": data.RowVersion };
            $scope.isEdit = true;
        }
       
        
        
        $scope.IsActive = function (Id) {
            debugger;            
            IsActiveList = Service.IsActive(Id, IsActiveList, $scope);
        }
        
        $scope.SelectedIndexChanged = function (selectedIndex) {
            debugger;
            $scope.CategoryList = Service.SelectedIndexChanged(selectedIndex, CategoryList_All);
        }
      
        $scope.Active_Deactive = function () {
            Service.Create_Update(IsActiveList, 'school/schoolCategory/Active_Deactive')
             .then(function (response) {
                Service.Notification ($rootScope, response.message);
                 if (response.result == 'Success') {
                     GetCategory();
                     IsActiveList = [];
                     $scope.Changed = false;
                 }
             });
        }
    }

    app.controller('categoryController', ['$scope', '$rootScope', '$state', 'Service', 'globalConfig', '$filter', desg_fun]);

})(angular.module('SilverzoneERP_App'));
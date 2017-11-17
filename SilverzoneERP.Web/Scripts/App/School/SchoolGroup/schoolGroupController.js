
(function (app) {

    var desg_fun = function ($scope, $rootScope, $state, Service, globalConfig, $filter) {

        //Start Declare Global Variable
        $scope.SchoolGroup = {};
        $scope.resetCopy = angular.copy($scope.SchoolGroup);
        var IsActiveList = [];
        var SchoolGroupList_All;
        $scope.Changed = false;
        $scope.selectedIndex = "1";
        $scope.isEdit = false;
        //End Declare Global Variable
              
        function GetSchoolGroup() {
            Service.Get('school/schoolGroup/Get').then(function (response) {
                SchoolGroupList_All = response.result;
                $scope.SelectedIndexChanged($scope.selectedIndex);
            });
        }

        GetSchoolGroup();
      
        $scope.Submit = function (form) {
            debugger;
            if (form.validate() == false)
                return false;
            Service.Create_Update($scope.SchoolGroup, 'school/schoolGroup/Create_Update')
            .then(function (response) {
                Service.Notification($rootScope, response.message);
                if (response.result == 'Success') {
                    $state.reload();
                }               
            });
        }

        $scope.validationOptions = {
            rules: {
                SchoolGroupName: {            // field will be come from component
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
            $scope.SchoolGroup = angular.copy($scope.resetCopy);
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
            $scope.SchoolGroup = { "Id": data.Id, "SchoolGroupName": data.SchoolGroupName, "RowVersion": data.RowVersion };
            $scope.isEdit = true;
        }
       
        
        
        $scope.IsActive = function (Id) {
            debugger;            
            IsActiveList = Service.IsActive(Id, IsActiveList, $scope);
        }
        
        $scope.SelectedIndexChanged = function (selectedIndex) {
            debugger;
            $scope.SchoolGroupList = Service.SelectedIndexChanged(selectedIndex, SchoolGroupList_All);
        }
      
        $scope.Active_Deactive = function () {
            Service.Create_Update(IsActiveList, 'school/schoolGroup/Active_Deactive')
             .then(function (response) {
                 Service.Notification($rootScope, response.message);
                 if (response.result == 'Success') {
                     GetSchoolGroup();
                     IsActiveList = [];
                     $scope.Changed = false;
                 }
             });
        }
    }

    app.controller('schoolGroupController', ['$scope','$rootScope', '$state', 'Service', 'globalConfig', '$filter', desg_fun]);

})(angular.module('SilverzoneERP_App'));
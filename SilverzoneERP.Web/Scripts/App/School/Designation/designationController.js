
(function (app) {

    var desg_fun = function ($scope, $rootScope, $state, Service, globalConfig, $filter) {

        $scope.Designation = {};
        $scope.resetCopy = angular.copy($scope.Designation);
        var IsActiveList = [];
        
        var DesignationList_All;
        $scope.Changed = false;
        $scope.selectedIndex = "1";
        $scope.isEdit = false;        

        function Reset() {
            $scope.Designation = angular.copy($scope.resetCopy);            
            $scope.isEdit = false;
            $scope.selectedIndex = "1";
            $scope.Changed = false;
            IsActiveList = [];
            DesignationList_All = null;                         
        }
        
        function GetDesignation() {
            Service.Get('school/designation/Get').then(function (response) {
                debugger;
                DesignationList_All = response.result;
                $scope.SelectedIndexChanged($scope.selectedIndex);
            });
        }
        GetDesignation();


        $scope.Submit = function () {           
            Service.Create_Update($scope.Designation, 'school/designation/Create_Update')
            .then(function (response) {
                Service.Notification($rootScope, response.message);
                if (response.result == 'Success') {
                        $state.reload();
                }               
            });
        }

        $scope.Add = function () {
            debugger;
            if ($scope.Changed)
            {
                //var confirm = $mdDialog.confirm()
                //          .title('Silverzone')
                //          .textContent('Do you want to save change ?')                                                  
                //          .ok('Yes')
                //          .cancel('No');

                //$mdDialog.show(confirm).then(function () {
                //    $scope.Active_Deactive(true);                  
                //}, function () {
                //    Reset(true);
                //    $scope.isEdit = true;
                //});
            } else
            {
                $scope.isEdit = true;
                $scope.Designation = {};
            }            
        }        

        $scope.Back = function () {
            Reset();
        }
        $scope.Edit = function (data) {
            debugger;
            $scope.Designation = { "Id": data.Id, "DesgName": data.DesgName, "DesgDescription": data.DesgDescription, "RowVersion": data.RowVersion };
            $scope.isEdit = true;
        }
                                      
        $scope.IsActive = function (Id) {
            debugger;
            IsActiveList = Service.IsActive(Id, IsActiveList, $scope);            
        }

        $scope.SelectedIndexChanged = function (selectedIndex) {
            debugger;
            $scope.DesignationList = Service.SelectedIndexChanged(selectedIndex, DesignationList_All);
        }

        $scope.Active_Deactive = function () {
            Service.Create_Update(IsActiveList, 'school/designation/Active_Deactive')
             .then(function (response) {
                 Service.Notification(response.message);
                 if (response.result == 'Success') {
                     $state.reload();
                 }
             });
        }
    }

    app.controller('designationController', ['$scope', '$rootScope', '$state', 'Service', 'globalConfig', '$filter', desg_fun]);

})(angular.module('SilverzoneERP_App'));
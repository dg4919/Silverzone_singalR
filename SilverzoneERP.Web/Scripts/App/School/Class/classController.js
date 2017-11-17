
(function (app) {

    var class_fun = function ($scope, $rootScope, $state, Service, globalConfig, $filter) {
    
        $scope.Class = {};
        $scope.resetCopy = angular.copy($scope.Class);
        var IsActiveList = [];

        var ClassList_All;
        $scope.Changed = false;
        $scope.selectedIndex = "1";
        $scope.isEdit = false;
       
        function GetClass() {
            Service.Get('school/Class/Get').then(function (response) {
                ClassList_All = response.result;
                $scope.SelectedIndexChanged($scope.selectedIndex);
            });
        }
        
        GetClass();

        $scope.Submit = function (form) {
            debugger;
            if (form.validate() == false)
                return false;

            Service.Create_Update($scope.Class, 'school/class/Create_Update')
            .then(function (response) {
                Service.Notification($rootScope, response.message);
                if (response.result == 'Success') {
                    $state.reload();                   
                }               
            });
        }

        $scope.validationOptions = {
            rules: {
                ClassName: {
                    required: true,
                    maxlength:50
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
            $scope.Title = angular.copy($scope.resetCopy);
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
                } else {
                    $scope.Changed = false;
                    $scope.SelectedIndexChanged($scope.selectedIndex);
                }
            }          
            $scope.Class = angular.copy(data);
            $scope.isEdit = true;
        }

        $scope.IsActive = function (Id) {
            debugger;
            IsActiveList = Service.IsActive(Id, IsActiveList, $scope);
        }
        
        $scope.SelectedIndexChanged = function (selectedIndex) {
            debugger;
            $scope.ClassList = Service.SelectedIndexChanged(selectedIndex, ClassList_All);
        }
      
        $scope.Active_Deactive = function () {
            Service.Create_Update(IsActiveList, 'school/class/Active_Deactive')
             .then(function (response) {
                 Service.Notification($rootScope, response.message);
                 if (response.result == 'Success') {
                     GetClass();
                     IsActiveList = [];
                     $scope.Changed = false;
                 }
             });
        }
    }

    app.controller('classController', ['$scope', '$rootScope', '$state', 'Service', 'globalConfig', '$filter', class_fun]);

})(angular.module('SilverzoneERP_App'));
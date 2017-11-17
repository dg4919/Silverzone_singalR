
(function (app) {

    var desg_fun = function ($scope, $rootScope, $state, Service, globalConfig, $filter) {

        $scope.Title = {};
        $scope.resetCopy = angular.copy($scope.Title);
        var IsActiveList = [];

        var TitleList_All;
        $scope.Changed = false;
        $scope.selectedIndex = "1";
        $scope.isEdit = false;
       
        function GetTitle() {
            Service.Get('school/title/Get').then(function (response) {
                TitleList_All = response.result;
                $scope.SelectedIndexChanged($scope.selectedIndex);
            });
        }
        
        GetTitle();

        $scope.Submit = function () {
            debugger;
            Service.Create_Update($scope.Title, 'school/title/Create_Update')
            .then(function (response) {
                Service.Notification($rootScope, response.message);
                if (response.result == 'Success') {
                        $state.reload();
                }               
            });
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
            $scope.Title = JSON.parse(JSON.stringify(data));// { "Id": $scope.TitleList[index].Id, "DesgName": $scope.TitleList[index].DesgName, "DesgDescription": $scope.TitleList[index].DesgDescription, "RowVersion": $scope.TitleList[index].RowVersion };
            $scope.isEdit = true;
        }

        $scope.IsActive = function (Id) {
            debugger;
            IsActiveList = Service.IsActive(Id, IsActiveList, $scope);
        }
        
        $scope.SelectedIndexChanged = function (selectedIndex) {
            debugger;
            $scope.TitleList = Service.SelectedIndexChanged(selectedIndex, TitleList_All);
        }
      
        $scope.Active_Deactive = function () {
            Service.Create_Update(IsActiveList, 'school/title/Active_Deactive')
             .then(function (response) {
                 Service.Notification($rootScope, response.message);
                 if (response.result == 'Success') {
                     GetTitle();
                 }
             });
        }
    }

    app.controller('titleController', ['$scope', '$rootScope', '$state', 'Service', 'globalConfig', '$filter', desg_fun]);

})(angular.module('SilverzoneERP_App'));
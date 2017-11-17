
(function (app) {

    var desg_fun = function ($scope, $rootScope, $state, Service, globalConfig, $filter) {

        //Start Declare Global Variable
        $scope.Event = {};
        $scope.resetCopy = angular.copy($scope.Event);
        var IsActiveList = [];
        var EventList_All;
        $scope.Changed = false;
        $scope.selectedIndex = "1";
        $scope.isEdit = false;
        //End Declare Global Variable
              
        function GetEvent() {
            Service.Get('school/event/Get?IsClass=false').then(function (response) {
                EventList_All = response.result.Event;
                $scope.SelectedIndexChanged($scope.selectedIndex);
            });
        }

        GetEvent();
      
        $scope.Submit = function () {
            debugger;
            Service.Create_Update($scope.Event, 'school/event/Create_Update')
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
            $scope.Event = angular.copy($scope.resetCopy);
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
            $scope.Event = { "Id": data.Id, "EventName": data.EventName, "SubjectName": data.SubjectName, "EventCode": data.EventCode, "RowVersion": data.RowVersion };
            $scope.isEdit = true;
        }
       
        
        
        $scope.IsActive = function (Id) {
            debugger;            
            IsActiveList = Service.IsActive(Id, IsActiveList, $scope);
        }
        
        $scope.SelectedIndexChanged = function (selectedIndex) {
            debugger;
            $scope.EventList = Service.SelectedIndexChanged(selectedIndex, EventList_All);
        }
      
        $scope.Active_Deactive = function () {
            Service.Create_Update(IsActiveList, 'school/event/Active_Deactive')
             .then(function (response) {
                 alert(response.message);
                 if (response.result == 'Success') {
                     GetEvent();
                     IsActiveList = [];
                     $scope.Changed = false;
                 }
             });
        }
    }

    app.controller('eventController', ['$scope', '$rootScope', '$state', 'Service', 'globalConfig', '$filter', desg_fun]);

})(angular.module('SilverzoneERP_App'));
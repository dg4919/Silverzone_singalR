(function (app) {
    var class_fun = function ($scope, $location, $rootScope, Service, $state, $filter) {

        var IsActiveList = [];
        var ExamDate_All;
                    

        function Reset() {
            debugger;
            $scope.minExamDate = new Date();

            $scope.maxExamDate = new Date();
            $scope.maxExamDate.setYear($scope.maxExamDate.getFullYear() + 1);

            
            $scope.ExamDateObj = { "ExamDate": new Date() };

            $scope.IsAdd = true;
            $scope.selectedIndex = "1";
            IsActiveList = [];
            ExamDate_All = null;
            $scope.Changed = false;

            Service.Get('examDate/Get').then(function (response) {
                ExamDate_All = JSON.stringify(response.result);
                $scope.SelectedIndexChanged($scope.selectedIndex);
            });
           // prompt('', JSON.stringify($scope.ExamDateObj));
        }
        Reset();
       
        $scope.Back = function () {
            debugger;
            $scope.IsAdd = true;
        }
        function SetDateFormat(datetime) {
            return $filter('dateFormat')(datetime, 'MM/DD/YYYY')
        }
        $scope.Add = function () {
            debugger;           
            //$scope.ExamDateObj = { "ExamDate": SetDateFormat(new Date()) };
            $scope.ExamDateObj = { "ExamDate": new Date() };
            $scope.IsAdd = false;
        }
        $scope.Edit = function (data) {
            debugger;
         
            var _date = new Date($filter('dateFormat')(data.ExamDate, 'MM/DD/YYYY'));

            $scope.ExamDateObj = { "Id": data.ExaminationDateId, "ExamDate": _date, "RowVersion": data.RowVersion };

            $scope.IsAdd = false;
        }
        $scope.Submit = function () {
            debugger;
            $scope.ExamDateObj.ExamDate = moment($scope.ExamDateObj.ExamDate).format();

            prompt('', JSON.stringify($scope.ExamDateObj))
//            return false;
            Service.Create_Update($scope.ExamDateObj, '/examDate/Create_Update').then(function (response) {
                Service.Alert(response.message);
                if (response.result == 'Success') {                  
                    Reset();
                }                
            });
        }
        $scope.SelectedIndexChanged = function (selectedIndex) {
            debugger;
            $scope.ExameDateList = Service.SelectedIndexChanged(selectedIndex, ExamDate_All);
        }      

        $scope.IsActive = function (Id) {
            debugger;
            IsActiveList = Service.IsActive(Id, IsActiveList, $scope);
        }

        $scope.Active_Deactive = function () {
            Service.Create_Update(IsActiveList, 'examDate/Active_Deactive')
             .then(function (response) {
                 Service.Alert(response.message);
                 if (response.result == 'Success') {
                     Reset();
                 }
             });
        }
        $scope.onlyWeekendsPredicate = function (date) {
            var day = date.getDay();
            return day === 0 || day === 6;
        };

    }
    app.controller('examDateController', ['$scope', '$location', '$rootScope', 'Service', '$state', '$filter', class_fun]);

})(angular.module('SilverzoneERP_App'));

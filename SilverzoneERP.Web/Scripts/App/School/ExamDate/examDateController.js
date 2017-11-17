(function (app) {
    var class_fun = function ($scope, $rootScope, $location, $rootScope, Service, $state, $filter) {

       
        $scope.ExamDateObj = { "ExamDate": SetDateFormat(new Date()) };
                
        $scope.resetCopy = angular.copy($scope.ExamDateObj);
        var IsActiveList = [];

        var ExamDate_All;
        $scope.Changed = false;
        $scope.selectedIndex = "1";
        $scope.isEdit = false;
       
        function GetExamDate() {
            Service.Get('school/examDate/Get').then(function (response) {
                ExamDate_All = response.result;
                $scope.SelectedIndexChanged($scope.selectedIndex);
            });
        }

        GetExamDate();
       
        $scope.Back = function () {
            debugger;
            $scope.ExamDateObj = angular.copy($scope.resetCopy);
            $scope.isEdit = false;
            Service.Reset($scope);
        }
        function SetDateFormat(datetime) {
            return $filter('dateFormat')(datetime, 'DD-MMM-YYYY')
        }
        $scope.Add = function () {
            debugger;
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
        $scope.Edit = function (data) {
            debugger;
         
           // var _date = new Date($filter('dateFormat')(data.ExamDate, 'MM/DD/YYYY'));

            $scope.ExamDateObj = { "Id": data.ExaminationDateId, "ExamDate": SetDateFormat(data.ExamDate), "RowVersion": data.RowVersion };

            $scope.isEdit = true;
        }
        $scope.Submit = function (form) {
            debugger;
            if (form.validate() == false)
                return false;
            //$scope.ExamDateObj.ExamDate = moment($scope.ExamDateObj.ExamDate).format();

          //  prompt('', JSON.stringify($scope.ExamDateObj))
          //return false;
            Service.Create_Update($scope.ExamDateObj, 'school/examDate/Create_Update').then(function (response) {
                Service.Notification($rootScope, response.message);
                if (response.result == 'Success') {                  
                    $state.reload();
                }                
            });
        }
        $scope.validationOptions = {
            rules: {
                ExamDate: {            // field will be come from component
                    required: true                    
                }
            }
        }
        $scope.SelectedIndexChanged = function (selectedIndex) {
            debugger;
            $scope.ExamDateList = Service.SelectedIndexChanged(selectedIndex, ExamDate_All);
        }      

        $scope.IsActive = function (Id) {
            debugger;
            IsActiveList = Service.IsActive(Id, IsActiveList, $scope);
        }

        $scope.Active_Deactive = function () {
            Service.Create_Update(IsActiveList, 'school/examDate/Active_Deactive')
             .then(function (response) {
                 Service.Notification($rootScope, response.message);
                 if (response.result == 'Success') {
                     GetExamDate();
                 }
             });
        }
        

    }
    app.controller('examDateController', ['$scope', '$rootScope', '$location', '$rootScope', 'Service', '$state', '$filter', class_fun]);

})(angular.module('SilverzoneERP_App'));

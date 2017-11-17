(function (app) {

    var EnrollmentOrder_fun = function ($scope, $rootScope, modalInstance, $filter, Service, ExamDateList, EnrollmentOrder, ExaminationDetails, SchMngt) {
        debugger;        
        $scope.ExamDateList = ExamDateList;
        $scope.ExaminationDetails = ExaminationDetails;

        if (EnrollmentOrder == null) {
            $scope.EnrollmentOrder = {
                "EnrollmentOrderDetail": [],
                "TotlaEnrollment": 0,
                "EventManagementId": SchMngt.EventManagement.Id
            };
            if (!angular.isUndefined($scope.ExaminationDetails) && !angular.isUndefined($scope.ExaminationDetails.ExaminationDateId)) {
                $scope.EnrollmentOrder.ExaminationDateId = "" + $scope.ExaminationDetails.ExaminationDateId;
                $scope.EnrollmentOrder.ChangeExamDate = $scope.ExaminationDetails.ChangeExamDate;
            }

        }
        else {
            $scope.EnrollmentOrder = EnrollmentOrder;
            $scope.EnrollmentOrder.ExaminationDateId = "" + $scope.EnrollmentOrder.ExaminationDateId;
        }

        $scope.ClassList = angular.copy($rootScope.SelectedEvent.Class);
        
        angular.forEach($scope.ClassList, function (item) {
            item.No_Of_Student = Get_No_Of_Student($scope.EnrollmentOrder.EnrollmentOrderDetail, item.ClassName);
        });
       
        $scope.Back = function () {
            debugger;
            modalInstance.dismiss();
        }

        function Get_No_Of_Student(data, Class) {
            debugger;
            if (data == null)
                return 0;
            var Class_Filter = $filter("filter")(data, { ClassName: Class }, true);
            if (Class_Filter.length != 0 && !angular.isUndefined(Class_Filter[0].No_Of_Student))
                return Class_Filter[0].No_Of_Student;
            return 0;
        }

        $scope.No_Of_Student_Change = function () {
            debugger;
            $scope.EnrollmentOrder.TotlaEnrollment = 0
            angular.forEach($scope.ClassList, function (item) {
                if (item.No_Of_Student != null && item.No_Of_Student != '')
                    $scope.EnrollmentOrder.TotlaEnrollment += parseInt(item.No_Of_Student);
            })
        }

        $scope.No_Of_Student_Change();

        $scope.Submit = function (form) {
            if ($scope.EnrollmentOrder.TotlaEnrollment == 0) {
                Service.Notification($rootScope, "Enrollment can't be 0(zero) !");               
                return false;
            }
            debugger;
            if (form.validate() == false)
                return false;

            $scope.EnrollmentOrder.EnrollmentOrderDetail = $scope.ClassList;
            

            Service.Create_Update($scope.EnrollmentOrder, 'school/EventManagement/EnrollmentOrder?EventId=' + $rootScope.SelectedEvent.EventId + '&SchoolId=' + SchMngt.SchId)
                 .then(function (response) {
                     Service.Notification($rootScope, response.message);

                     if (angular.lowercase(response.result) == 'success') {
                         debugger;
                         modalInstance.close();
                     }
                 });

           // console.log(obj);
            //prompt('', JSON.stringify(obj));
            //modalInstance.close($scope.EnrollmentOrder);
        }

        $scope.validationOptions = {
            rules: {
                ExaminationDateId: {
                    required: true,
                },
                ChangeExamDate: {
                    required: true,
                }
            }
        }

    }
  
    app.controller('EnrollmentOrderController', ['$scope', '$rootScope', '$uibModalInstance', '$filter', 'Service', 'ExamDateList', 'EnrollmentOrder', 'ExaminationDetails', 'SchMngt', EnrollmentOrder_fun]);

})(angular.module('SilverzoneERP_App'));
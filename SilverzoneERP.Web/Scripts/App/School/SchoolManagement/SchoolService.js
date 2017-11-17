(function (app) {
    var fun = function ($http, $q, globalConfig, $filter, $uibModal, Service, $rootScope) {
        var apiUrl = globalConfig.apiEndPoint,
            fac = {}

        fac.Add_Co_Ordinator = function (scope, rootScope,  CoOrdinatorObj) {
            debugger;
          
            var modalInstance = $uibModal.open({
                controller: 'Co_Ord_Controller',
                templateUrl: 'Templates/School/EventManagement/Partial/Dialog_Co_Ordinator.html',
                backdrop: 'static',
                resolve: {
                    Related_Object: scope.Related_Object,
                    CoOrd: CoOrdinatorObj,
                    SchMngt: scope.SchMngt
                }
            });

            modalInstance.result.then(function (response) {
                debugger;
                fac.SearchBySchoolCode(scope, scope.SchMngt.SchCode, rootScope,false);                       
                //on ok button press 
            }, function () {
                //on cancel button press
                console.log("Modal Closed");
            });
        }

        fac.Delete_Co_Ordinator = function (scope, rootScope, CoOrdinatorId) {
            $.confirm({
                title: $rootScope.Title,
                content: 'Are you want delete ?',
                buttons: {
                    YES: function () {
                        Delete_CoOrdinator_Event(scope, rootScope, CoOrdinatorId);
                    },
                    NO: function () {
                        
                    }
                }
            });            
        }
        
        function Create_Event(scope, response) {
            debugger;
            scope.SchMngt.EventManagement.EventId = response.EventId;
            scope.SchMngt.EventManagement.EventCode = Get_EventCode(scope, response.EventId);
            Add_CoOrdinator(scope, response);            
        }

        function Add_CoOrdinator(scope,response) {
            debugger;
            var data = {
                "CoOrdinatorGuid": Service.guid(),
                "TitleId": response.TitleId,
                "CoOrdTitle": Get_CoOrdTitle(scope, response.TitleId),
                "CoOrdName": response.CoOrdName,
                "CoOrdMobile": response.CoOrdMobile,
                "CoOrdAltMobile1": response.CoOrdAltMobile1,
                "CoOrdAltMobile2": response.CoOrdAltMobile2,
                "CoOrdEmail": response.CoOrdEmail,
                "CoOrdAltEmail1": response.CoOrdAltEmail1,
                "CoOrdAltEmail2": response.CoOrdAltEmail2
            }
            scope.SchMngt.EventManagement.CoOrdinator.push(data);
        }

        function Delete_CoOrdinator_Event(scope, rootScope, CoOrdinatorId) {
            debugger;
            Service.Delete('school/EventManagement/Delete_CoOrdinator?CoOrdinatorId=' + CoOrdinatorId)
                  .then(function (response) {
                      debugger;
                      Service.Notification($rootScope, response.message);
                      if (angular.lowercase(response.result) == 'success') {
                          fac.SearchBySchoolCode(scope, scope.SchMngt.SchCode, rootScope, false);
                      }
                  });
        }

        function Get_EventCode(scope,EventId) {
            var Event_Filter = $filter("filter")(scope.Related_Object.Event, { EventId: EventId }, true);
            if (Event_Filter.length != 0)
                return Event_Filter[0].EventCode;
            return null;
        }

        function Get_CoOrdTitle(scope, TitleId) {
            debugger;
            var Title_Filter = $filter("filter")(scope.Related_Object.Title, { Id: TitleId }, true);
            if (Title_Filter.length != 0)
                return Title_Filter[0].TitleName;
            return null;
        }

        fac.Search = function (scope, rootScope,IsLoad) {
            debugger;
            //if (rootScope.SelectedEvent == null) {
            //    Service.Notification($rootScope, 'Please select event !');                
            //    return false;
            //}
            var modalInstance = $uibModal.open({
                controller: 'SearchSchoolController',
                templateUrl: 'Templates/School/SearchSchool/Index.html',
                windowClass: 'model-large',
                backdrop: 'static'
            });

            modalInstance.result.then(function (response) {
                debugger;
                scope.SchoolDetail = response;

                fac.SearchBySchoolCode(scope,response.SchCode, rootScope,IsLoad);

                //on ok button press 
            }, function () {
                //on cancel button press
                console.log("Modal Closed");
            });
        }

        fac.SearchBySchoolCode = function (scope, SchCode, rootScope,IsReset) {
            debugger;
            var evenId=0;
            if (!angular.isUndefined(rootScope.SelectedEvent) && rootScope.SelectedEvent != null)
                evenId= rootScope.SelectedEvent.EventId;
                
            Service.Get('school/schManagement/GetSchool?SchCode=' + SchCode + '&EventId=' + evenId)
             .then(function (response) {
                 debugger;
                 if (response.result != null) {

                     scope.SchMngt = response.result;
                     scope.CourierList = response.CourierList;
                     scope.ExamDateList = response.ExamDateList;

                     if (scope.SchMngt.EventManagement == null) {
                         scope.SchMngt.EventManagement = {
                             CoOrdinator: [],
                             EnrollmentOrderSummary: [],
                             CoOrdinatingTeacher: [],
                             Enrollment: [],
                             EnrollmentOrder: []
                         };

                     }

                   //  fac.Set_EnrollmentoederSummary(scope, rootScope);
                    // scope.SchMngt.EventManagement.EnrollmentOrder = [];
                     if(IsReset)
                     {
                         scope.Reset(0, 10); 
                     }
                 }
                 else {
                     Service.Notification(rootScope, 'School Code does not exists !');                    
                 }

             });
        }

        fac.Set_EnrollmentoederSummary = function (scope, rootScope) {
            debugger;
            scope.No_Of_Participants = 0;
            scope.Total_Amt = 0;
            scope.ClassList = angular.copy(rootScope.SelectedEvent.Class);

            scope.ExaminationDetails = { IsUnknown: true };


            angular.forEach(scope.SchMngt.EventManagement.EnrollmentOrder, function (data) {
                angular.forEach(scope.ClassList, function (item) {

                    var Class_Filter = $filter("filter")(scope.ClassList, { ClassName: item.ClassName }, true);
                    if (Class_Filter.length != 0)
                    {
                        if (angular.isUndefined(Class_Filter[0].No_Of_Student))
                            Class_Filter[0].No_Of_Student = 0;
                        Class_Filter[0].No_Of_Student += Get_No_Of_Student(data, item.ClassName);
                    }
                        
                })              
                scope.No_Of_Participants += parseInt(data.TotlaEnrollment);
                if (data.ExaminationDateId != -2)
                    scope.ExaminationDetails = { IsUnknown: false };
            });

            scope.Total_Amt = (rootScope.SelectedEvent.EventFee - rootScope.SelectedEvent.RetainFee) * scope.No_Of_Participants;

            SetExamDate(scope);
        }

        function Get_No_Of_Student(data, Class) {
            debugger;
            if (data == null)
                return 0;
            var Class_Filter = $filter("filter")(data.EnrollmentOrderDetail, { ClassName: Class }, true);
            if (Class_Filter.length != 0 && !angular.isUndefined(Class_Filter[0].No_Of_Student))
                return parseInt(Class_Filter[0].No_Of_Student);
            return 0;
        }

        function SetExamDate(scope) {
            debugger;
            var ExamDate_Filter = get_unique_from_array_object(scope.SchMngt.EventManagement.EnrollmentOrder, 'ExamDate');

            if (ExamDate_Filter.length == 1) {
                scope.SchMngt.EventManagement.ExamDate = ExamDate_Filter[0].ExamDate;

                scope.ExaminationDetails = {
                    ExaminationDateId: ExamDate_Filter[0].ExaminationDateId,
                    ChangeExamDate: ExamDate_Filter[0].ChangeExamDate
                };
            }
            else
                scope.SchMngt.EventManagement.ExamDate = null;
        }

        function get_unique_from_array_object(array, property) {
            debugger;
            var unique = {};
            var distinct = [];
            for (var i in array) {
                if (typeof (unique[array[i][property]]) == "undefined") {
                    distinct.push(array[i]);
                }
                unique[array[i][property]] = 0;
            }
            return distinct;
        }

        return fac;
    }

    app.factory('SchoolService', ['$http', '$q', 'globalConfig', '$filter', '$uibModal', 'Service', '$rootScope', fun]);

})(angular.module('SilverzoneERP_App'));
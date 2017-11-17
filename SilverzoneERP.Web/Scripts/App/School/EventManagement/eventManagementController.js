(function (app) {
    'use strict';

    var SchMngt_fun = function ($scope, $rootScope, Service, $filter, $state, $uibModal, SchoolService, Order, $stateParams) {
        $rootScope.IsEventShow = true;

        function Get_Related_Object() {
            Service.Get('school/schManagement/Get_Related_Object').then(function (response) {
                $scope.Related_Object = response.result;
            });
        }
        
        function Get_BookOrder() {
            debugger;
            Order.Get_BookOrder($scope);

            //Service.Get('school/EventManagement/AllOrderBySchool?SchoolId=' + SchoolId).then(function (response) {
            //    $scope.BookOrderlist = response.result;
            //});
        }

        $scope.Reset = function () {

            $state.reload();
        }

        Get_Related_Object();
        
        $scope.SearchSchool_KeyPress = function (SchCode) {
            debugger;
            if (angular.isUndefined($rootScope.SelectedEvent) || $rootScope.SelectedEvent == null)
            {
                Service.Notification($rootScope, 'Please select event !');
                return false;
            }

            SchoolService.SearchBySchoolCode($scope, SchCode, $rootScope, false);

        }

        $scope.Edit_School = function () {
            var params = {
                SchCode: $scope.SchMngt.SchCode,
            };
            $state.go('SchoolManagement', params);
        }

        $scope.Search_School = function () {
            debugger;
            if (angular.isUndefined($rootScope.SelectedEvent) || $rootScope.SelectedEvent == null) {
                Service.Notification($rootScope, 'Please select event !');
                return false;
            }

            SchoolService.Search($scope, $rootScope,false);          
        }

        function SearchBySchoolCode() {
            if ($stateParams.SchCode!=null)
                SchoolService.SearchBySchoolCode($scope, $stateParams.SchCode, $rootScope, false);
        }

        SearchBySchoolCode();

        $scope.StudentEntry = function () {
            debugger;
            var params = {
                SchCode: $scope.SchMngt.SchCode,
            };

            $state.go('StudentEntry', params);
        }

        $scope.Add_Co_Ordinator = function (EventId, CoOrdinatorObj) {
            debugger;
            var data = angular.copy(CoOrdinatorObj);

            if (data == null)
            {
                data = {};
                data.EventManagementId = $scope.SchMngt.EventManagement.Id;
            }
                                        
            data.EventId = EventId;
               
            SchoolService.Add_Co_Ordinator($scope, $rootScope, data);
        };

        $scope.Delete_CoOrdinator = function (CoOrdinatorTeacherId) {
            debugger;           
            SchoolService.Delete_Co_Ordinator($scope, $rootScope,CoOrdinatorId);
        }

        $scope.Add_CoOrdinating_Teacher = function () {
            debugger;

            var modalInstance = $uibModal.open({
                controller: 'CoOrdinating_Teacher_Controller',
                templateUrl: 'Templates/School/EventManagement/Partial/Dialog_CoOrdinating_Teachers.html',
                windowClass: 'app-modal-window',
                resolve: {
                    Teacher_List: function () {
                        return angular.copy($scope.SchMngt.EventManagement.CoOrdinatingTeacher);
                    },
                    SchMngt: function () {
                        return angular.copy($scope.SchMngt);
                    }
                }
            });

            modalInstance.result.then(function (response) {
                debugger;
               
                //on ok button press 
            }, function () {
                //on cancel button press
                SchoolService.SearchBySchoolCode($scope, $scope.SchMngt.SchCode, $rootScope, false);
            });
        }

        $scope.Delete_CoOrdinating_Teacher = function (CoOrdinatorTeacherId) {
            debugger;
            $.confirm({
                title: $rootScope.Title,
                content: 'Are you want delete ?',
                buttons: {
                    YES: function () {
                        Service.Delete('school/EventManagement/Delete_CoOrdinatorTeacher?CoOrdinatorTeacherId=' + CoOrdinatorTeacherId + '&IsLoad=false')
                                .then(function (response) {
                                    debugger;
                                    Service.Notification($rootScope, response.message);
                                    if (angular.lowercase(response.result) == 'success') {
                                        SchoolService.SearchBySchoolCode($scope, $scope.SchMngt.SchCode, $rootScope, false);
                                    }
                                });
                    },
                    NO: function () {

                    }
                }
            });

        }

        $scope.Submit = function (form) {
            debugger;
            if (form.validate() == false)
                return false;
            var msg = "Are you want to participate in '<strong>" + $rootScope.SelectedEvent.EventCode + "'</strong> ?";
            $.confirm({
                title: $rootScope.Title,
                content: msg,
                buttons: {
                    YES: function () {
                        var obj = {
                            Id: $scope.SchMngt.EventManagement.Id,
                            SchId: $scope.SchMngt.SchId,
                            EventId: $rootScope.SelectedEvent.EventId,
                            PostalCommunication: $scope.SchMngt.PostalCommunication,
                            CourierId: $scope.SchMngt.CourierId,
                            OtherCourier: $scope.SchMngt.OtherCourier,
                            IsParticipate : true,
                    };
            //angular.copy($scope.SchMngt.EventManagement)
                                                                        
                        Service.Create_Update(obj, 'school/EventManagement/Create_Update')
                               .then(function (response) {
                                   Service.Notification($rootScope, response.message);

                                   if (angular.lowercase(response.result) == 'success') {
                                       $state.reload();
                                   }
                               });
                    },
                    NO: function () {

                    }
                }
            });
        }

        $scope.validationOptions = {
            rules: {
                //CommunicationId: {
                //    required: true
                //},
                //CourierId: {
                //    required: true
                //},
                //OtherCourier: {
                //    required: true,
                //    maxlength: 50
                //},
                ExaminationDateId: {
                    required: true,
                },
                ChangeExamDate: {
                    required: true,
                }
            }
        }
        
        $scope.Tab = function (_isOrder, _isBook) {
            debugger;
            $scope.isOrder = _isOrder;
            $scope.isBook = _isBook;
            if ($scope.isBook == true && angular.isUndefined($scope.BookOrderlist))
            {
                Get_BookOrder();
            }
        }

        $scope.Tab(true,false);

        $scope.Add_Order = function (data) {
            debugger;
            if ($scope.isBook == false) {
                Order.Enrollment($scope, $rootScope, data);
            }
            else {
                debugger;               
                var BookOrder = {
                    "EventManagementId": $scope.SchMngt.EventManagement.Id,
                    "SchoolName": $scope.SchMngt.SchName,
                    "SchId":$scope.SchMngt.SchId,
                    "ClassList": $scope.SchMngt.EventManagement.EnrollmentOrderSummary
                }
                if (data != null) {
                    BookOrder.PO_masterId = data.Id;
                    BookOrder.Order = data.Order;
                }
                Order.Book($scope,BookOrder);
            }            
        }

        $scope.Delete_Order = function (data) {
            debugger;                       
            Service.Create_Update(null, 'Inventory/Stock/delete_purchaseOrder_byId?Id='+data.Id)
                  .then(function (response) {                                            
                      if (angular.lowercase(response.result) == 'success') {
                          Service.Notification($rootScope, 'Successfully order canceled !');  
                          Get_BookOrder()
                      }                     
                  });
        }

        $scope.Pament = function () {
            if (angular.isUndefined($scope.SchMngt.SchId))
                return false;

            var modalInstance = $uibModal.open({
                controller: 'PaymentController',
                templateUrl: 'Templates/School/EventManagement/Partial/Dialog_Payment.html',
                windowClass: 'model-large',
                backdrop: 'static',                
                resolve: {
                    SchMngt: function () {
                        return angular.copy($scope.SchMngt);
                    }                    
                }
            });

            modalInstance.result.then(function (response) {
                debugger;

                //on ok button press 
            }, function () {
                //on cancel button press
                console.log("Modal Closed");
            });
        }

        $scope.History = function () {
            var modalInstance = $uibModal.open({
                controller: 'History_Controller',
                templateUrl: 'Templates/School/EventManagement/Partial/Dialog_History.html',
                windowClass: 'model-large',
                backdrop: 'static',
                resolve: {
                    SchMngt: function () {
                        return angular.copy($scope.SchMngt);
                    }                   
                }
            });

            modalInstance.result.then(function (response) {
                debugger;

                //on ok button press 
            }, function () {
                //on cancel button press
                console.log("Modal Closed");
            });
        }
    }
  
    var CoOrdinating_Teacher_Fun = function ($scope, $rootScope, modalInstance, Service, $filter, $timeout, Teacher_List, SchMngt) {
        $scope.TeacherList = Teacher_List;

        var ResetCopy = {
            EventManagementId: SchMngt.EventManagement.Id
        };
        
        $scope.Teacher = angular.copy(ResetCopy);

        //$scope.TeacherList = [{ "Title": { "TitleName": "Mr.", "CreationDate": "2017-02-09T06:18:54.323", "CreatedBy": 1, "UpdationDate": "2017-02-09T06:29:01.54", "UpdatedBy": 1, "RowVersion": "AAAAAAAAKwI=", "Id": 1, "Status": true }, "Name": "a", "Mobile": "7530981456", "Email": "a@gmail.com", "SelectedQues": "8", "TeacherGuid": "28b95f25-5f04-4ad2-90f7-043676282eb5" }, { "Title": { "TitleName": "Mr.", "CreationDate": "2017-02-09T06:18:54.323", "CreatedBy": 1, "UpdationDate": "2017-02-09T06:29:01.54", "UpdatedBy": 1, "RowVersion": "AAAAAAAAKwI=", "Id": 1, "Status": true }, "Name": "b", "Mobile": "7530981457", "Email": "a@gmail.com", "SelectedQues": "8", "TeacherGuid": "a7558fcd-5b6d-411f-8a37-232b69b23102" }];
        Service.Get('school/title/GetActive')
               .then(function (response) {
                   debugger;
                   $scope.TileList = response.result;
               });


        $scope.Back = function () {
            debugger;
            modalInstance.dismiss();
        }

        $scope.Submit = function (form) {
            if (form.validate() == false)
                return false;
            debugger;
            Service.Create_Update($scope.Teacher, 'school/EventManagement/CoOrdinatingTeacher?EventId=' + $rootScope.SelectedEvent.EventId + '&SchoolId=' + SchMngt.SchId)
                  .then(function (response) {
                      Service.Notification($rootScope, response.message);

                      if (angular.lowercase(response.result) == 'success') {
                          
                          debugger;
                          $scope.Teacher = angular.copy(ResetCopy);

                          $scope.TeacherList = response.data;
                      }
                  });            
        }
        
        $scope.Edit = function (data) {
            debugger;
            $scope.Teacher = angular.copy(data);
            $scope.Teacher.No_Of_Selected_Ques =  $scope.Teacher.No_Of_Selected_Ques;
        }

        $scope.Delete = function (CoOrdinatorTeacherId) {
            debugger;
            $.confirm({
                title: $rootScope.Title,
                content: 'Are you want delete ?',
                buttons: {
                    YES: function () {
                        Service.Delete('school/EventManagement/Delete_CoOrdinatorTeacher?CoOrdinatorTeacherId=' + CoOrdinatorTeacherId + '&IsLoad=true')
                                .then(function (response) {
                                    debugger;
                                    Service.Notification($rootScope, response.message);
                                    if (angular.lowercase(response.result) == 'success') {
                                        $scope.TeacherList = response.data;
                                    }
                                });
                    },
                    NO: function () {

                    }
                }
            });

        }

        $scope.Reset = function () {
            $scope.Teacher = {};
            Service.Reset($scope);
        }

        $scope.Finish = function () {
            debugger;
            modalInstance.close($scope.TeacherList);
        }

        $scope.validationOptions = {
            rules: {
                TitleId: {
                    required: true
                },
                Name: {
                    required: true,
                    maxlength: 100
                },
                MobileNo: {                  
                    maxlength: 11,
                    minlength: 10
                },
                AltMobileNo1: {
                    maxlength: 11,
                    minlength: 10
                },
                AltMobileNo2: {
                    maxlength: 11,
                    minlength: 10
                },
                EmailID: {                   
                    maxlength: 50
                },
                AltEmailID1: {
                    maxlength: 50
                },
                AltEmailID2: {
                    maxlength: 50
                }

            }
        }
    }

    var Co_Ordinator_Fun = function ($scope, $rootScope, modalInstance, $filter, Service, Related_Object, CoOrd, SchMngt) {

        debugger;
        $scope.Related_Object = Related_Object;

        $scope.Co_Ord = CoOrd;

        $scope.Back = function () {
            debugger;
            modalInstance.dismiss();
        }

        $scope.Submit = function (form) {
            debugger;
            if (form.validate() == false)
                return false;
            
            Service.Create_Update($scope.Co_Ord, 'school/EventManagement/CoOrdinator?EventId=' + $scope.Co_Ord.EventId + '&SchoolId=' + SchMngt.SchId)
                  .then(function (response) {
                      Service.Notification($rootScope, response.message);

                      if (angular.lowercase(response.result) == 'success') {
                          modalInstance.close();
                      }
                  });
        }
        $scope.validationOptions = {
            rules: {
                EventId: {
                    required: true
                },
                TitleId: {
                    required: true
                },
                CoOrdName: {
                    required: true,
                    maxlength: 100
                },
                CoOrdMobile: {                  
                    maxlength: 10,
                    minlength: 10
                },
                CoOrdAltMobile1: {
                    maxlength: 10,
                    minlength: 10
                },
                CoOrdAltMobile2: {
                    maxlength: 10,
                    minlength: 10
                },
                CoOrdEmail: {                    
                    maxlength: 50,
                },
                CoOrdAltEmail1: {
                    maxlength: 50
                },
                CoOrdAltEmail2: {
                    maxlength: 50
                }
            }
        }
        
    }
     
    var History_Fun = function ($scope, $rootScope, modalInstance, $filter, Service, SchMngt) {
        $scope.SchMngt = SchMngt;
        $scope.SelectedYear = "";
        $scope.Preserve_History;
        function Get_History() {
            Service.Get('school/feePayment/History?SchoolId=' + SchMngt.SchId).then(function (response) {
                $scope.Preserve_History = response.result;
                $scope.YearChange();
            });
        }

        Get_History();

        $scope.YearChange = function () {
            debugger;
            if ($scope.SelectedYear == null || $scope.SelectedYear == "")
                $scope.History = angular.copy($scope.Preserve_History);
            else
            {
                $scope.History = $filter('filter')($scope.Preserve_History, { EventManagementYear: $scope.SelectedYear }, true);
            }
            
        }
        
        $scope.Back = function () {
            debugger;
            modalInstance.dismiss();
        }        
    }

    app.controller('eventManagementController', ['$scope', '$rootScope', 'Service', '$filter', '$state', '$uibModal', 'SchoolService', 'OrderService', '$stateParams', SchMngt_fun])
    
    .controller('CoOrdinating_Teacher_Controller', ['$scope', '$rootScope', '$uibModalInstance', 'Service', '$filter', '$timeout', 'Teacher_List', 'SchMngt', CoOrdinating_Teacher_Fun])
    .controller('Co_Ord_Controller', ['$scope', '$rootScope', '$uibModalInstance', '$filter', 'Service', 'Related_Object', 'CoOrd', 'SchMngt', Co_Ordinator_Fun])
    .controller('History_Controller', ['$scope', '$rootScope', '$uibModalInstance', '$filter', 'Service', 'SchMngt', History_Fun]);

})(angular.module('SilverzoneERP_App'));

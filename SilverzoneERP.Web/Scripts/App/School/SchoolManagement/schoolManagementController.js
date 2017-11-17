(function (app) {


    var scm_fun = function ($scope, $rootScope, Service, $filter, $state, $uibModal, $stateParams, SchoolService, localStorageService) {
        $scope.ContactInfo = {};
        $rootScope.IsEventShow = true;
        $scope.SchMngt = { };
        $scope.SchMngt.EventManagement = []
        $scope.SchMngt.Contact_List = [];
        $rootScope.StatusInfo = 'Last Gen Code : ';
        // $scope.SchMngt.EventManagement = [{ "EventGuid": "1578a10e-e914-415b-9c5e-8a3f9570236b", "EventId": 1, "EventCode": "iio", "CoOrdinators": [{ "CoOrdinatorGuid": "90f08684-8806-45e0-bd02-0aa60f6add6c", "TitleId": 1, "CoOrdTitle": "Mr.", "CoOrdName": "as", "CoOrdMobile": "1111111111", "CoOrdAltMobile1": "55555555555", "CoOrdAltMobile2": "666666666", "CoOrdEmail": "as@gmail.com", "CoOrdAltEmail1": "as2@gmail.com" }] }];

        //$scope.SchMngt = { "Events": [{ "EventGuid": "1578a10e-e914-415b-9c5e-8a3f9570236b", "EventId": 1, "EventCode": "iio", "CoOrdinators": [{ "CoOrdinatorGuid": "90f08684-8806-45e0-bd02-0aa60f6add6c", "TitleId": 1, "CoOrdTitle": "Mr.", "CoOrdName": "as", "CoOrdMobile": "1111111111", "CoOrdAltMobile1": "55555555555", "CoOrdAltMobile2": "666666666", "CoOrdEmail": "as@gmail.com", "CoOrdAltEmail1": "as2@gmail.com" }] }], "Contact_List": [], "SchName": "DAV", "SchEmail": "dav@gmail.com", "SchAddress": "B2", "CityId": 1, "DistrictId": 1, "DistrictName": "East Champaran", "StateId": 1, "StateName": "Bihar", "ZoneId": 1, "CountryId": 1, "CountryName": "india", "SchPinCode": "845415", "SchGroupId": 1, "SchCategoryId": 1 };

        $scope.PinCode_Leave = function () {
            if ($scope.SchMngt.SchPinCode.length < 6)
                return false;
            Service.Get('school/schManagement/GetSchoolList?PinCode=' + $scope.SchMngt.SchPinCode).then(function (response) {
                ShowSchoolList(response.result);
            });
        }

        if ($stateParams.SchCode != null)
            localStorageService.set('SchoolParams', $stateParams.SchCode);

        $scope.SchCode_Parameter = localStorageService.get('SchoolParams');

        $scope.Back = function () {

            if ($scope.SchCode_Parameter != null) {
                localStorageService.remove('SchoolParams');

                var params = {
                    SchCode: $scope.SchCode_Parameter,
                };
                $state.go('EventManagement', params);
            }
        }

        $scope.SearchSchool_KeyPress = function (SchCode) {
            debugger;
            SchoolService.SearchBySchoolCode($scope, SchCode, $rootScope, false);
        }

        function Get_Related_Object() {
            Service.Get('school/schManagement/Get_Related_Object').then(function (response) {
                $scope.Related_Object = response.result;
            });
        }

        Get_Related_Object();

        function GetCity() {
            Service.Get('school/schManagement/GetCity').then(function (response) {
                $scope.City = response.result;
            });
        }

        GetCity();

        $scope.CityChange = function () {
            debugger;
            var City_Filter = $filter("filter")($scope.City, { CityId: $scope.SchMngt.CityId }, true);
            if (City_Filter.length != 0) {
                $scope.SchMngt.CityId = City_Filter[0].CityId;
                $scope.SchMngt.DistrictId = City_Filter[0].DistrictId;
                $scope.SchMngt.DistrictName = City_Filter[0].DistrictName;
                $scope.SchMngt.StateId = City_Filter[0].StateId;
                $scope.SchMngt.StateName = City_Filter[0].StateName;
                $scope.SchMngt.ZoneId = City_Filter[0].ZoneId;
                $scope.SchMngt.CountryId = City_Filter[0].CountryId;
                $scope.SchMngt.CountryName = City_Filter[0].CountryName;
            }
        }

        $scope.Search_School = function () {
            debugger;
            //if (angular.isUndefined($rootScope.SelectedEvent) || $rootScope.SelectedEvent == null) {
            //    Service.Notification($rootScope, 'Please select event !');
            //    return false;
            //}
            SchoolService.Search($scope, $rootScope, false);
        }

        //$scope.SearchBySchoolCode = function (SchCode) {
        //    debugger;
        //    if (SchCode == null)
        //        return false;
        //    Service.Get('school/schManagement/GetSchool?SchCode=' + SchCode)
        //   .then(function (response) {               
        //       $scope.SchMngt = response.result==null?{}:response.result;
        //   });
        //}

        $scope.Reset = function () {
            $state.reload();
        }

       // $scope.SearchBySchoolCode();

        $scope.Submit = function (form) {
            if (form.validate() == false)
                return false;

            Service.Create_Update($scope.SchMngt, 'school/schManagement/Create_Update')
                   .then(function (response) {

                       if (response.result == 'Success') {
                           Service.Notification($rootScope, response.message);
                           if ($scope.SchCode_Parameter != null)
                           {
                               var params = {
                                   SchCode: $scope.SchCode_Parameter,
                               };
                               $state.go('EventManagement', params);
                           }
                           else
                            $state.reload();
                       }
                       else if (response.result == 'list') {
                           ShowSchoolList(response.data);
                       }
                       else
                           Service.Notification($rootScope, response.message);
                   });

        }

        $scope.validationOptions = {
            rules: {
                SchName: {           
                    required: true,
                    maxlength: 100
                },
                SchEmail: {                    
                    maxlength: 50
                },
                SchAddress: {
                    required: true,
                    maxlength: 150
                },
                SchAltAddress: {
                    maxlength: 150
                }
                ,
                CityId: {
                    required: true
                },
                SchPinCode: {
                    required: true,
                    maxlength: 6,
                    minlength: 6
                },
                SchPhoneNo: {
                    maxlength: 10,
                    minlength: 10
                },
                SchFaxNo: {
                    maxlength: 10,
                    minlength: 10
                },
                SchWebSite: {
                    maxlength: 100
                },
                SchAffiliationNo: {
                    maxlength: 50
                },
                SchBoard: {
                    maxlength: 50
                },
                BlackListedRemarks: {
                    maxlength: 200
                }
            }
        }

        Service.Get('school/schManagement/GetLastGenSchoolCode').then(function (response) {
             $rootScope.StatusInfo += response;
         });
     
        $scope.ChangeAddressTo = function (index) {
            debugger;
            for (var i = 0; i < $scope.SchMngt.Contact_List.length; i++) {
                if (index != i) {
                    $scope.SchMngt.Contact_List[i].AddressTo = 'false';
                }
            }
        }

        $scope.Reset = function () {
            $state.reload();
        }

        $scope.Add_Co_Ordinator = function (EventObj, CoOrdinatorObj) {

            debugger;

            var data = Get_CoOrdinator(EventObj, CoOrdinatorObj);

            var modalInstance = $uibModal.open({
                controller: 'Co_Ord_Controller',
                templateUrl: 'Templates/School/SchoolManagement/Partial/Co_Ordinator.html',
                resolve: {
                    items: $scope.Related_Object,
                    CoOrd: data,
                    SchMngt: $scope.SchMngt
                }
            });

            modalInstance.result.then(function (response) {
                debugger;
                if (!angular.isUndefined(response.EventGuid)) {
                    var Events_filter = $filter("filter")($scope.SchMngt.EventManagement, { EventGuid: response.EventGuid }, true);
                    if (Events_filter.length != 0) {
                        if (response.EventId != Events_filter[0].EventId)//This Block execute when change Events
                        {
                            Create_Event(response, true);
                        }
                        else {
                            if (!angular.isUndefined(response.CoOrdinatorGuid)) {
                                var CoOrdinators_filter = $filter("filter")(Events_filter[0].CoOrdinators, { CoOrdinatorGuid: response.CoOrdinatorGuid }, true);
                                if (CoOrdinators_filter.length != 0) {
                                    CoOrdinators_filter[0].TitleId = response.TitleId;
                                    CoOrdinators_filter[0].CoOrdTitle = Get_CoOrdTitle(response.TitleId);
                                    CoOrdinators_filter[0].CoOrdName = response.CoOrdName;
                                    CoOrdinators_filter[0].CoOrdMobile = response.CoOrdMobile;
                                    CoOrdinators_filter[0].CoOrdAltMobile1 = response.CoOrdAltMobile1;
                                    CoOrdinators_filter[0].CoOrdAltMobile2 = response.CoOrdAltMobile2;
                                    CoOrdinators_filter[0].CoOrdEmail = response.CoOrdEmail;
                                    CoOrdinators_filter[0].CoOrdAltEmail1 = response.CoOrdAltEmail1;
                                    CoOrdinators_filter[0].CoOrdAltEmail2 = response.CoOrdAltEmail2;
                                }
                            }
                        }
                    }
                }
                else {
                    Create_Event(response, false);
                }
                //on ok button press 
            }, function () {
                //on cancel button press
                console.log("Modal Closed");
            });
        };

        function Get_EventCode(EventId) {
            var Event_Filter = $filter("filter")($scope.Related_Object.Event, { EventId: EventId }, true);
            if (Event_Filter.length != 0)
                return Event_Filter[0].EventCode;
            return null;
        }

        function Get_CoOrdTitle(TitleId) {
            var Title_Filter = $filter("filter")($scope.Related_Object.Title, { Id: TitleId }, true);
            if (Title_Filter.length != 0)
                return Title_Filter[0].TitleName;
            return null;
        }

        function Get_CoOrdinator(EventObj, CoOrdinatorObj) {
            var data;
            if (EventObj != null) {
                data = {
                    "EventGuid": EventObj.EventGuid,
                    "EventId": EventObj.EventId,
                    "CoOrdinatorGuid": CoOrdinatorObj.CoOrdinatorGuid,
                    "TitleId": CoOrdinatorObj.TitleId,
                    "CoOrdName": CoOrdinatorObj.CoOrdName,
                    "CoOrdMobile": CoOrdinatorObj.CoOrdMobile,
                    "CoOrdAltMobile1": CoOrdinatorObj.CoOrdAltMobile1,
                    "CoOrdAltMobile2": CoOrdinatorObj.CoOrdAltMobile2,
                    "CoOrdEmail": CoOrdinatorObj.CoOrdEmail,
                    "CoOrdAltEmail1": CoOrdinatorObj.CoOrdAltEmail1,
                    "CoOrdAltEmail2": CoOrdinatorObj.CoOrdAltEmail2
                };
            }
            else {
                data = {};
            }
            return data;
        }

        function Create_Event(response, IsDelete) {
            debugger;
            var Events_EventId_filter = $filter("filter")($scope.SchMngt.EventManagement, { EventId: response.EventId }, true);
            if (Events_EventId_filter.length != 0) {
                if (IsDelete)
                    Delete_CoOrdinator_Event(response.EventGuid, response.CoOrdinatorGuid);
                Events_EventId_filter[0] = Add_CoOrdinator(Events_EventId_filter[0], response);
            }
            else {
                if (IsDelete)
                    Delete_CoOrdinator_Event(response.EventGuid, response.CoOrdinatorGuid);
                var EventsObj = {
                    "EventGuid": Service.guid(),
                    "EventId": response.EventId,
                    "EventCode": Get_EventCode(response.EventId),
                    "CoOrdinators": [
                        {
                            "CoOrdinatorGuid": Service.guid(),
                            "TitleId": response.TitleId,
                            "CoOrdTitle": Get_CoOrdTitle(response.TitleId),
                            "CoOrdName": response.CoOrdName,
                            "CoOrdMobile": response.CoOrdMobile,
                            "CoOrdAltMobile1": response.CoOrdAltMobile1,
                            "CoOrdAltMobile2": response.CoOrdAltMobile2,
                            "CoOrdEmail": response.CoOrdEmail,
                            "CoOrdAltEmail1": response.CoOrdAltEmail1,
                            "CoOrdAltEmail2": response.CoOrdAltEmail2
                        }
                    ]
                };
                $scope.SchMngt.EventManagement.push(EventsObj);
            }
        }

        $scope.Delete_CoOrdinator = function (EventGuid, CoOrdinatorGuid) {
            debugger;
            var result = confirm('Are you want delete ?');
            if (result) {
                Delete_CoOrdinator_Event(EventGuid, CoOrdinatorGuid);
            }
        }

        function Delete_CoOrdinator_Event(EventGuid, CoOrdinatorGuid) {
            debugger;
            var Events_Filter = $filter("filter")($scope.SchMngt.EventManagement, { EventGuid: EventGuid }, true);
            if (Events_Filter.length != 0) {
                var CoOrdinators_Filter = $filter("filter")(Events_Filter[0].CoOrdinators, { CoOrdinatorGuid: CoOrdinatorGuid }, true);
                if (CoOrdinators_Filter.length != 0) {
                    Events_Filter[0].CoOrdinators.splice(CoOrdinators_Filter[0], 1);
                    if (Events_Filter[0].CoOrdinators.length == 0)
                        $scope.SchMngt.EventManagement.splice(Events_Filter[0], 1);
                }
            }
        }

        function Add_CoOrdinator(Events, response) {
            debugger;
            var data = {
                "CoOrdinatorGuid": Service.guid(),
                "TitleId": response.TitleId,
                "CoOrdTitle": Get_CoOrdTitle(response.TitleId),
                "CoOrdName": response.CoOrdName,
                "CoOrdMobile": response.CoOrdMobile,
                "CoOrdEmail": response.CoOrdEmail
            }
            return Events.CoOrdinators.push(data);
        }

        $scope.AddressTo = '';

        $scope.Add_Address_To = function (AddressObj, IsOtherContact) {

            debugger;
            var data;
            if (AddressObj == null)
                data = {};
            else
                data = angular.copy(AddressObj);

            data.IsOtherContact = IsOtherContact;


            var modalInstance = $uibModal.open({
                controller: 'Address_To_Controller',
                templateUrl: 'Templates/School/SchoolManagement/Partial/AddressTo.html',
                resolve: {
                    items: $scope.Related_Object,
                    AddressTo: data,
                    SchMngt: $scope.SchMngt
                }
            });

            modalInstance.result.then(function (response) {
                debugger;

                response.DesgName = Get_Designation(response.DesgId);
                response.Title = Get_CoOrdTitle(response.TitleId);

                if (response.IsOtherContact) {
                    $scope.SchMngt.OtherContact = response;
                    $scope.SchMngt.IsOtherContact = true;
                }
                else {
                    response.DesgName = Get_Designation(response.DesgId);
                    response.Title = Get_CoOrdTitle(response.TitleId);

                    if (!angular.isUndefined(response.AddressGuid)) {
                        var Address_filter = $filter("filter")($scope.SchMngt.Contact_List, { AddressGuid: response.AddressGuid }, true);
                        if (Address_filter.length != 0) {

                            Address_filter[0].DesgId = response.DesgId;
                            Address_filter[0].DesgName = response.DesgName;
                            Address_filter[0].TitleId = response.TitleId;
                            Address_filter[0].Title = response.Title;
                            Address_filter[0].ContactName = response.ContactName;
                            Address_filter[0].ContactMobile = response.ContactMobile;
                            Address_filter[0].ContactAltMobile1 = response.ContactAltMobile1;
                            Address_filter[0].ContactAltMobile2 = response.ContactAltMobile2;
                            Address_filter[0].ContactEmail = response.ContactEmail;
                            Address_filter[0].ContactAltEmail1 = response.ContactAltEmail1;
                            Address_filter[0].ContactAltEmail2 = response.ContactAltEmail2;
                            //Address_filter[0].AddressTo = response.AddressTo;
                        }
                    }
                    else {
                        response.AddressGuid = Service.guid();
                        //response.AddressTo = false;
                        $scope.SchMngt.Contact_List.push(response);
                    }
                }
                //on ok button press 
            }, function () {
                //on cancel button press
                console.log("Modal Closed");
            });
        };

        $scope.Delete_Address_To = function (AddressGuid) {
            var result = confirm('Are you want delete ?');
            if (result) {
                var Address_filter = $filter("filter")($scope.SchMngt.Contact_List, { AddressGuid: AddressGuid }, true);
                if (Address_filter.length != 0) {
                    $scope.SchMngt.Contact_List.splice(Address_filter[0], 1);
                }
            }
        }

        function Get_Designation(DesgId) {
            var Designation_Filter = $filter("filter")($scope.Related_Object.Designation, { Id: DesgId }, true);
            if (Designation_Filter.length != 0)
                return Designation_Filter[0].DesgName;
            return null;
        }

        $scope.Remove_OtherContact = function () {
            delete $scope.SchMngt["OtherContact"];
            $scope.SchMngt.IsOtherContact = false;
        }

        $scope.Add_City = function () {
            var modalInstance = $uibModal.open({
                controller: 'Add_City_Controller',
                templateUrl: 'Templates/School/SchoolManagement/Partial/City.html'
            });

            modalInstance.result.then(function (response) {
                debugger;
                $scope.SchMngt.NewCity = {
                    "CityId": 0,
                    "CountryId": response.CountryId,
                    "CountryName": response.CountryName,
                    "ZoneId": response.ZoneId,
                    "ZoneName": response.ZoneName,
                    "StateId": response.StateId,
                    "StateName": response.StateName,
                    "DistrictId": response.DistrictId,
                    "DistrictName": response.DistrictName,
                    "CityName": response.CityName
                };
                var City_Filter = $filter("filter")($scope.City, { CityId: 0 }, true);
                if (City_Filter.length != 0) {
                    City_Filter[0] = SetValue_After_CityAdd(City_Filter[0], response);
                }
                else {
                    $scope.City.push($scope.SchMngt.NewCity);
                }
                $scope.SchMngt = SetValue_After_CityAdd($scope.SchMngt, response);
                //on ok button press 
            }, function () {
                //on cancel button press
                console.log("Modal Closed");
            });
        }

        function SetValue_After_CityAdd(data, response) {
            data.CityId = 0;
            data.CityName = response.CityName;
            data.DistrictId = response.DistrictId;
            data.DistrictName = response.DistrictName;
            data.StateId = response.StateId;
            data.StateName = response.StateName;
            data.ZoneId = response.ZoneId;
            data.CountryId = response.CountryId;
            data.CountryName = response.CountryName;
            return data;
        }

        function ShowSchoolList(data) {
            debugger;

            var modalInstance = $uibModal.open({
                controller: 'SchoolList_Controller',
                templateUrl: 'Templates/School/SchoolManagement/Partial/SchoolList.html',
                resolve: {
                    School_List: function () {
                        return data;
                    }
                }
            });

            modalInstance.result.then(function (response) {
                debugger;
                $scope.SearchSchool_KeyPress(response);
                //$scope.SchMngt.SchCode = response;
                //$scope.SearchBySchoolCode();
                //on ok button press 
            }, function () {
                //on cancel button press
                console.log("Modal Closed");
            });
        }

        $scope.Log = function () {
            debugger;
            var modalInstance = $uibModal.open({
                controller: 'SchoolLog_Controller',
                templateUrl: 'Templates/School/SchoolManagement/Partial/Log.html',
                resolve: {
                    SchId: function () {
                        return $scope.SchMngt.SchId;
                    }
                }
            });

            modalInstance.result.then(function (response) {
                debugger;
                $scope.SchMngt.SchCode = response;
                $scope.SearchBySchoolCode();
                //on ok button press 
            }, function () {
                //on cancel button press
                console.log("Modal Closed");
            });           
        }        
    }

    var addressTo_Fun = function ($scope, $rootScope, $modalInstance, $filter, items, AddressTo, SchMngt, Service) {

        $scope.AddressObj = AddressTo;
        $scope.Related_Object = items;

        $scope.Back = function () {
            debugger;
            $modalInstance.dismiss();
        }

        $scope.Submit = function (form) {
            debugger;
            if (form.validate() == false)
                return false;
            Validate($scope.AddressObj);

        }
        $scope.validationOptions = {
            rules: {
                DesgId: {
                    required: true
                },
                TitleId: {
                    required: true
                },
                ContactName: {
                    required: true,
                    maxlength: 100
                },
                ContactMobile: {                   
                    maxlength: 10,
                    minlength: 10
                },
                ContactAltMobile1: {
                    maxlength: 10,
                    minlength: 10
                },
                ContactAltMobile2: {
                    maxlength: 10,
                    minlength: 10
                },
                ContactEmail: {                    
                    maxlength: 50,
                },
                ContactAltEmail1: {
                    maxlength: 50
                },
                ContactAltEmail1: {
                    maxlength: 50
                }
            }
        }

        function Validate(data) {
            debugger;
            var Address_Filter;
            if (!angular.isUndefined(data.AddressGuid))
            {
                Address_Filter = $filter("filter")(SchMngt.Contact_List,function(item){
                    if (item.AddressGuid != data.AddressGuid && item.DesgId == data.DesgId)
                        return item;
                });
            }                
            else
                Address_Filter = $filter("filter")(SchMngt.Contact_List, { DesgId: data.DesgId}, true);
            if (Address_Filter.length != 0) {
                Service.Notification($rootScope, 'Co-Ordinator already exixt with by designation !');
            }
            else
                $modalInstance.close($scope.AddressObj);
        }
    }

    var SchoolList_Fun = function ($scope, $modalInstance, School_List) {
        $scope.SchoolList = School_List;

        $scope.Back = function () {
            debugger;
            $modalInstance.dismiss();
        }
        $scope.Edit = function (SchoolCode) {
            $modalInstance.close(SchoolCode);
        }
    }

    var SchoolLog_Fun = function ($scope, $modalInstance, Service, SchId) {

        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;
        $scope.rangeSize = 5;

        $scope.startIndex = 0;
        $scope.Back = function () {
            debugger;
            $modalInstance.dismiss();
        }

        $scope.$watch("currentPage", function (newValue, oldValue) {
            debugger;


            var startIndex = ((newValue - 1) * $scope.itemsPerPage);

            Service.Get('school/schManagement/GetLog?SchId=' + SchId + '&StartIndex=' + startIndex + '&Limit=' + $scope.itemsPerPage).then(function (response) {

                //GetUserPermissionList(((newValue - 1) * $scope.itemsPerPage) + 1, $scope.itemsPerPage).then(function (response) {
                //var json = JSON.parse(response.data);
                debugger;
                $scope.SchoolLog = response.result.Log;
                $scope.total = response.result.Count;

                $scope.range = function () {
                    var ret = [];
                    var start = ((Math.ceil($scope.currentPage / $scope.rangeSize) - 1) * $scope.rangeSize) + 1;
                    for (var i = start; i < (start + $scope.rangeSize) && i <= $scope.pageCount() ; i++)
                        ret.push(i);
                    return ret;
                };


                $scope.prevPage = function () {
                    if ($scope.currentPage > 1) {
                        $scope.currentPage--;
                    }
                };

                $scope.prevPageDisabled = function () {
                    return $scope.currentPage === 1 ? "disabled" : "";
                };

                $scope.firstPage = function () {
                    $scope.currentPage = 1;
                }
                $scope.firstPageDisabled = function () {
                    return $scope.currentPage === 1 ? "disabled" : "";
                };

                $scope.lastPage = function () {
                    $scope.currentPage = $scope.pageCount();
                }
                $scope.lastPageDisabled = function () {
                    return $scope.currentPage === $scope.pageCount() ? "disabled" : "";
                };

                $scope.nextPage = function () {
                    if ($scope.currentPage <= $scope.pageCount()) {
                        $scope.currentPage++;
                    }
                };

                $scope.nextPageDisabled = function () {
                    return $scope.currentPage === $scope.pageCount() ? "disabled" : "";
                };

                $scope.pageCount = function () {
                    return Math.ceil($scope.total / $scope.itemsPerPage);
                };

                $scope.setPage = function (n) {
                    if (n > 0 && n <= $scope.pageCount()) {
                        $scope.currentPage = n;
                    }
                };

            }, function () {

            })
        });
    }

    app.controller('schoolManagementController', ['$scope', '$rootScope', 'Service', '$filter', '$state', '$uibModal', '$stateParams', 'SchoolService', 'localStorageService', scm_fun])
    .controller('Address_To_Controller', ['$scope', '$rootScope', '$uibModalInstance', '$filter', 'items', 'AddressTo', 'SchMngt', 'Service', addressTo_Fun])
    .controller('SchoolList_Controller', ['$scope', '$uibModalInstance', 'School_List', SchoolList_Fun])
    .controller('SchoolLog_Controller', ['$scope', '$uibModalInstance', 'Service', 'SchId', SchoolLog_Fun]);

})(angular.module('SilverzoneERP_App'));

(function (app) {

    var SearchSchool_Fun = function ($scope, $rootScope, modalInstance, Service, $filter, locationService) {
        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;
        $scope.rangeSize = 5;
        $scope.startIndex = 0;

        $scope.Search = {};

        function GetCountry() {
            Service.Get('school/location/GetCountry').then(function (response) {
                $scope.CountryList = response.result;
                $scope.CountryChange(null);
            });
        }

        GetCountry();

        $scope.CountryChange = function (CountryId) {
            debugger;
            $scope.ZoneList = [];
            if (CountryId == null) {
                angular.forEach($scope.CountryList, function (_country) {
                    angular.forEach(_country.Zones, function (_zone) {
                        $scope.ZoneList.push(_zone);
                    })
                })
            }
            else {
                var _country = locationService.Get_Filter_Country($scope.CountryList, CountryId);
                if (_country.length != 0) {
                    $scope.ZoneList = _country[0].Zones

                }
            }
            $scope.ZoneChange(null);
        }

        $scope.ZoneChange = function (ZoneId) {
            debugger;
            $scope.StateList = [];
            if (ZoneId == null) {
                angular.forEach($scope.ZoneList, function (_zone) {
                    angular.forEach(_zone.States, function (_state) {
                        $scope.StateList.push(_state);
                    })
                })
            }
            else {
                var _zone = locationService.Get_Filter_Zone($scope.ZoneList, ZoneId);
                if (_zone.length != 0) {
                    $scope.StateList = _zone[0].States
                }
            }
            $scope.StateChange(null);
        }

        $scope.StateChange = function (StateId) {
            debugger;
            $scope.DistrictList = [];
            if (StateId == null) {
                angular.forEach($scope.StateList, function (_state) {
                    angular.forEach(_state.Districts, function (_district) {
                        $scope.DistrictList.push(_district);
                    })
                })
            }
            else {
                var _state = locationService.Get_Filter_State($scope.StateList, StateId);
                if (_state.length != 0) {
                    $scope.DistrictList = _state[0].Districts
                }
            }
            $scope.DistrictChange(null);
        }

        $scope.DistrictChange = function (DistrictId) {
            debugger;
            $scope.CityList = [];
            if (DistrictId == null) {
                angular.forEach($scope.DistrictList, function (_district) {
                    angular.forEach(_district.Cities, function (_city) {
                        $scope.CityList.push(_city);
                    })
                })
            }
            else {
                var _district = locationService.Get_Filter_District($scope.DistrictList, DistrictId);
                if (_district.length != 0) {
                    $scope.CityList = _district[0].Cities;
                }
            }
        }

        $scope.validationOptions = {
            rules: {
                Anything: {
                    maxlength: 100
                },
                SchCode: {
                    minlength: 6,
                    maxlength: 6
                },
                SchName: {
                    maxlength: 100
                },
                PinCode: {
                    minlength: 6,
                    maxlength: 6
                }
            }
        }
        var URL;
        $scope.Submit = function (form) {
            debugger;
            if (form.validate() == false)
                return false;
             URL = 'School/schManagement/Search?Anything=';
            if (angular.isUndefined($scope.Search.Anything))
                URL += '';
            else
                URL += $scope.Search.Anything;
            URL += '&SchoolName=';
            if (angular.isUndefined($scope.Search.SchName))
                URL += '';
            else
                URL += $scope.Search.SchName;
            URL += '&SchoolCode=';
            if (angular.isUndefined($scope.Search.SchCode))
                URL += '';
            else
                URL += $scope.Search.SchCode;
            URL += '&CountryId=';
            if (angular.isUndefined($scope.Search.CountryId))
                URL += '';
            else
                URL += $scope.Search.CountryId;
            URL += '&ZoneId=';
            if (angular.isUndefined($scope.Search.ZoneId))
                URL += '';
            else
                URL += $scope.Search.ZoneId;
            URL += '&StateId=';
            if (angular.isUndefined($scope.Search.StateId))
                URL += '';
            else
                URL += $scope.Search.StateId;
            URL += '&DistrictId=';
            if (angular.isUndefined($scope.Search.DistrictId))
                URL += '';
            else
                URL += $scope.Search.DistrictId;
            URL += '&CityId=';
            if (angular.isUndefined($scope.Search.CityId))
                URL += '';
            else
                URL += $scope.Search.CityId;
            URL += '&PinCode=';
            if (angular.isUndefined($scope.Search.PinCode))
                URL += '';
            else
                URL += $scope.Search.PinCode;
            URL += '&BlackListed=';
            if (angular.isUndefined($scope.Search.BlackListed) || $scope.Search.BlackListed == '')
                URL += '';
            else
                URL += $scope.Search.BlackListed == "1" ? true : false;

            URL += '&EventType=';
            if (angular.isUndefined($scope.Search.EventType))
                URL += '';
            else
                URL += $scope.Search.EventType;

            URL += '&EventId=';
            if (angular.isUndefined($rootScope.SelectedEvent) || $rootScope.SelectedEvent==null)
                URL += '0';
            else
                URL += $rootScope.SelectedEvent.EventId;


            //Service.Get(URL).then(function (response) {
            //    $scope.SearchList = response.result;
            //});
            GetSchool(1, 1, URL);
        }

        $scope.Back = function () {
            debugger;
            modalInstance.dismiss();
        }

        $scope.SelectedSchool = function (data) {
            modalInstance.close(data);
        }

        
        $scope.$watch("currentPage", function (newValue, oldValue) {
            debugger;
            if (URL == null)
                return false;
            GetSchool(newValue, oldValue, URL);
        });

        function GetSchool(newValue, oldValue,URL) {
            var startIndex = ((newValue - 1) * $scope.itemsPerPage);

            Service.Get(URL+'&StartIndex=' + startIndex + '&Limit=' + $scope.itemsPerPage)
                .then(function (response) {

                    debugger;
                    $scope.SearchList = response.result;
                    $scope.total = response.Count;
                    
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
        }
    }

    app.controller('SearchSchoolController', ['$scope', '$rootScope', '$uibModalInstance', 'Service', '$filter', 'locationService', SearchSchool_Fun]);

})(angular.module('SilverzoneERP_App'));
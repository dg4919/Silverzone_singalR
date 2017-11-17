(function (app) {
    var zone_fun = function ($scope, $rootScope, $filter, Service, locationService, $state) {
        //Start City Variable Declaration
        var City_Filter_Country;

        $scope.City_SelectedZone_Arr;
        var City_Filter_Zone;

        $scope.City_SelectedState_Arr;
        var City_Filter_State;

        $scope.City_SelectedDistrict_Arr;
        //End City Variable Declaration

        //Start Stete Variable Declaration
        var State_Filter_Country;
        $scope.State_SelectedZone_Arr;
        //End Stete Variable Declaration

        //Start Stete Variable Declaration
        var District_Filter_Country;
        $scope.District_SelectedZone_Arr;

        var District_Filter_Zone;

        $scope.District_SelectedState_Arr;
        
        //End Stete Variable Declaration

        function ResetCountry(isLoad) {
            $scope.Country = {};
            $scope.isAdd_Country = true;
            if (isLoad)
                GetCountry();
        }

        function ResetZone(isLoad) {
            $scope.Zone = {};
            $scope.isAdd_Zone = true;
            if (isLoad)
                GetCountry();
        }

        function ResetState(isLoad) {
            $scope.State = {};
            $scope.isAdd_State = true;
            if (isLoad)
                GetCountry();
        }
        function ResetDistrict(isLoad) {
            $scope.District = {};
            $scope.isAdd_District = true;
            //if (isLoad)
            //    GetState();
        }
        function ResetCity(isLoad) {
            $scope.City = {};
            $scope.isAdd_City = true;
            //if (isLoad)
            //    GetState();
        }
    
        function GetCountry() {
            Service.Get('school/location/GetCountry').then(function (response) {
                $scope.CountryList = response.result;
            });
        }

        ResetCountry(true);
        ResetZone(false);
        ResetState(false);
        ResetDistrict(false);
        ResetCity(false);
       

        $scope.Add = function (objName) {
            debugger;
            if (objName == 'country')
                $scope.isAdd_Country = false;
            else if (objName == 'zone')
                $scope.isAdd_Zone = false;
            else if (objName == 'state')
                $scope.isAdd_State = false;
            else if (objName == 'district')
                $scope.isAdd_District = false;
            else if (objName == 'city')
                $scope.isAdd_City = false;
        }

        $scope.Back = function (objName) {
            debugger;
            if (objName == 'country')
                ResetCountry(false);
            if (objName == 'zone')
                ResetZone(false);
            if (objName == 'state')
                ResetState(false);
            if (objName == 'district')
                ResetDistrict(false);
            if (objName == 'city')
                ResetCity(false);
        }

        $scope.Submit = function (objName) {
            debugger;                     
            var data;

            var url = 'school/location/' + objName;
            if (objName == 'country')            
                data = $scope.Country;                           
            else if (objName == 'zone') 
                data = $scope.Zone;                          
            else if (objName == 'state') 
                data = $scope.State;                          
            else if (objName == 'district') 
                data = $scope.District;
            else if (objName == 'city')
                data = $scope.City;
           
            Service.Create_Update(data, url)
            .then(function (response) {
                if (response.result == 'Success') {
                    debugger;
                    if (objName == 'country')
                        ResetCountry(true);
                    else if (objName == 'zone')
                        ResetZone(true);
                    else if (objName == 'state')
                        ResetState(true);
                    else if (objName == 'district')
                        ResetDistrict(true);
                    //Service.Reset($scope);
                    $state.reload();
                }
                alert(response.message);
            });
        }
        
        $scope.Edit = function (CountryObj,ZoneObj,StateObj,DistrictObj,CityObj, objName) {
            debugger;
            if (objName == 'country')
                $scope.Country = { "Id": CountryObj.CountryId, "CountryName": CountryObj.CountryName, "RowVersion": CountryObj.RowVersion };
            else if (objName == 'zone')
                $scope.Zone = { "Id": ZoneObj.ZoneId, "ZoneName": ZoneObj.ZoneName, "CountryId": CountryObj.CountryId, "RowVersion": ZoneObj.RowVersion };
            else if (objName == 'state') {
                $scope.State = { "Id": StateObj.StateId, "StateName": StateObj.StateName, "StateCode": StateObj.StateCode, "CountryId": CountryObj.CountryId, "CountryName": CountryObj.CountryName, "RowVersion": StateObj.RowVersion };
                $scope.ChangeCountry(objName);
                $scope.State.ZoneId =  ZoneObj.ZoneId;
            }
            else if (objName == 'district') {
                $scope.District = {
                    "Id": DistrictObj.DistrictId,
                    "DistrictName": DistrictObj.DistrictName,
                    "RowVersion": DistrictObj.RowVersion,
                    "CountryId": CountryObj.CountryId                                      
                };
                $scope.ChangeCountry(objName);
                $scope.District.ZoneId = ZoneObj.ZoneId;
                $scope.ChangeZone(objName);
                $scope.District.StateId = StateObj.StateId;
            }
            $scope.Add(objName);
        }

        $scope.ChangeCountry=function(objName)
        {
            debugger;

            var CountryId;
            if (objName == 'state')
            {                
                State_Filter_Country = Get_Filter_Country($scope.State.CountryId);
                $scope.State_SelectedZone_Arr = Get_Zone_Arr(State_Filter_Country);
                $scope.State.ZoneId = 0;
            }                
            else if (objName == 'district')
            {
                District_Filter_Country = Get_Filter_Country($scope.District.CountryId);
                $scope.District_SelectedZone_Arr = Get_Zone_Arr(District_Filter_Country);
                $scope.District.ZoneId = 0;
                $scope.District.StateId = 0;
                $scope.District_SelectedState_Arr = [];
            }                         
           else if (objName == 'city')
           {               
               City_Filter_Country = Get_Filter_Country($scope.City.CountryId);
               $scope.City_SelectedZone_Arr = Get_Zone_Arr(City_Filter_Country);
               $scope.City_SelectedState_Arr = [];
               $scope.City.ZoneId = 0;
               $scope.City.StateId = 0;
               $scope.City.DistrictId = 0;
           }                          
        }                
        $scope.ChangeZone = function (objName) {
            debugger;

            if (objName == 'district') {
                District_Filter_Zone = Get_Filter_Zone($scope.District_SelectedZone_Arr, $scope.District.ZoneId);
                $scope.District_SelectedState_Arr = Get_State_Arr(District_Filter_Zone);
                $scope.District.StateId = 0;               
            }
            else if (objName == 'city') {
                City_Filter_Zone = Get_Filter_Zone($scope.City_SelectedZone_Arr, $scope.City.ZoneId);
                $scope.City_SelectedState_Arr = Get_State_Arr(City_Filter_Zone);

                $scope.City_SelectedDistrict_Arr = [];
                $scope.City.StateId = 0;
                $scope.City.DistrictId = 0;                
            }
        }

        $scope.ChangeState = function (objName) {
            debugger;
            if (objName == 'city')
            {
                City_Filter_State = Get_Filter_State($scope.City_SelectedState_Arr, $scope.City.StateId);
                $scope.City_SelectedDistrict_Arr = Get_District_Arr(City_Filter_State);
                $scope.City.DistrictId = 0;
            }
        }
        function Get_Filter_Country(CountryId) {
            return $filter("filter")($scope.CountryList, { CountryId: parseInt(CountryId) }, true);
        }

        function Get_Zone_Arr(SelectedCountry_Obj) {
            var selectedZone = []
            if (SelectedCountry_Obj.length != 0)
                selectedZone = SelectedCountry_Obj[0].Zones;
            return selectedZone;
        }

        function Get_Filter_Zone(SelectedZone_Arr, ZoneId) {
            return $filter("filter")(SelectedZone_Arr, { ZoneId: parseInt(ZoneId) }, true);
        }

        function Get_State_Arr(SelectedZone_Obj) {
            var selectedState = []
            if (SelectedZone_Obj.length != 0)
                selectedState = SelectedZone_Obj[0].States;
            return selectedState;
        }

        function Get_Filter_State(SelectedState_Arr, StateId) {
            return $filter("filter")(SelectedState_Arr, { StateId: parseInt(StateId) }, true);
        }

        function Get_District_Arr(SelectedState_Obj) {
            var selectedDistrict = []
            if (SelectedState_Obj.length != 0)
                selectedDistrict = SelectedState_Obj[0].Districts;
            return selectedDistrict;
        }
        //End Conuntry Section
    };

    app.controller('locationController', ['$scope', '$rootScope', '$filter', 'Service', 'locationService', '$state', zone_fun]);
})(angular.module('SilverzoneERP_App'));

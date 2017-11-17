(function (app) {

    var fun = function ($http, $q, globalConfig, $filter) {
        var apiUrl = globalConfig.apiEndPoint,
            fac = {}
        fac.Get_Filter_Country = function (CountryList, CountryId) {
            return $filter("filter")(CountryList, { CountryId: parseInt(CountryId) }, true);
        }

        fac.Get_Filter_Zone = function (ZoneList, ZoneId) {
            return $filter("filter")(ZoneList, { ZoneId: parseInt(ZoneId) }, true);
        }

        fac.Get_Filter_State = function (StateList, StateId) {
            return $filter("filter")(StateList, { StateId: parseInt(StateId) }, true);
        }

        fac.Get_Filter_District = function (DistrictList, DistrictId) {
            return $filter("filter")(DistrictList, { DistrictId: parseInt(DistrictId) }, true);
        }
        

        return fac;
    }
    app.factory('locationService', ['$http', '$q', 'globalConfig', '$filter', fun]);
})(angular.module('SilverzoneERP_App'));

(function () {
    'use strict';

    var inventory_sourceDetail = function ($sc, $rsc, svc, locationSvc, $filter) {

        debugger;

        $sc.inventory_sources = [];
        $sc.inventory_sourceDetail_List = [];
        $sc.sourceInfo = {
            delaerBookDiscounts: [],
            addressList: []
        };

        $sc.isEdit_record = false;

        var get_inventorySource = function () {
            svc.get_inventorySource().then(function (d) {
                $sc.inventory_sources = d.result;
            });
        }
        get_inventorySource();

        $sc.submit_data = function (form) {
            if (form.validate()) {

                if (parseInt($sc.sourceInfo.SourceId) === 2) {
                    $sc.sourceInfo.delaerBookDiscounts = $sc.sourceInfo.delaerBookDiscounts.map(function (v, k) {
                        return ({ categoryId: v.category.id, amount: v.amount });
                    });

                    $sc.sourceInfo.addressList = $sc.sourceInfo.addressList.map(function (v, k) {
                        return v.adresName;
                    });

                    if (!$sc.sourceInfo.Id)           // for create
                    {
                        svc.save_dealer_sourceInfo($sc.sourceInfo)
                        .then(common_responseFx);
                    }
                    else {
                        svc.update_dealer_sourceInfo($sc.sourceInfo)
                        .then(common_responseFx);
                    }

                    return;
                }

                if (!$sc.sourceInfo.Id) {           // for create
                    svc.save_sourceInfo($sc.sourceInfo)
                    .then(common_responseFx);
                }
                else {
                    svc.update_sourceInfo($sc.sourceInfo)
                    .then(common_responseFx);
                }
            }
        }

        var common_responseFx = function (d) {
            if (d.result === 'success') {
                if (!$sc.isEdit_record)
                    $rsc.notify_fx('Inventory source is created !', 'success');
                else
                    $rsc.notify_fx('Inventory source is updated !', 'success');
                
                var srcId = $sc.sourceInfo.SourceId;
                $sc.sourceInfo = {};
                $sc.sourceInfo.SourceId = srcId;

                $sc.get_inventorySources();
                $sc.isEdit_record = false;
            }
            else if (d.result === 'exist') {
                $rsc.notify_fx('Inventory source already exist, Try another !', 'warning');
            }
            else
                $rsc.notify_fx('Inventory not found :(, Try another !', 'error');
        }

        $sc.validationOptions = {
            rules: {
                source: {            // field will be come from component
                    required: true
                },
                name: {
                    required: true,
                },
                address: {
                    required: true,
                },
                contactPerson: {
                    required: true,
                },
                mobile: {
                    required: true,
                },
                city: {
                    required: true,
                },
                pincode: {
                    required: true,
                },
                email: {
                    required: true,
                    email: true
                },
                panNo: {
                    required: true,
                },
                tanNo: {
                    required: true,
                }
            }
        }

        $sc.get_inventorySources = function () {
            var _model = {
                Id: $sc.sourceInfo.SourceId,
                type: 1,
                Status: true
            }

            svc.get_sourceInfo(_model).then(function (d) {
                $sc.inventory_sourceDetail_List = d.result;
            });
        }

        $sc.changeCallback = function (inventorySource) {
            svc.delete_sourceInfo(inventorySource)
                .then(function (d) {
                    if (d.result === 'success') {
                        $rsc.notify_fx('Inventory source status is changed !', 'success');
                    }
                    else
                        $rsc.notify_fx('Inventory not found :(, Try another !', 'error');
                });
        }

        $sc.EditData = function (sourceDetails) {
            var srcId = $sc.sourceInfo.SourceId;

            $sc.sourceInfo = sourceDetails;

            get_inventorySource();
            $sc.sourceInfo.SourceId = srcId;
            $sc.isEdit_record = true;

            if (parseInt($sc.sourceInfo.SourceId) === 2) {
                $sc.sourceInfo.pincode = sourceDetails.PinCode;
                $sc.sourceInfo.delaerBookDiscounts = sourceDetails.delaerBookDiscounts;
                $sc.sourceInfo.addressList = sourceDetails.DealerSceondaryAddressess;

                $sc.locationModel.selected_country = sourceDetails.cityModel.Country_Id;
                $sc.get_state();

                $sc.locationModel.selected_state = sourceDetails.cityModel.State_Id;

                $sc.get_city();
                $sc.sourceInfo.cityId = sourceDetails.cityModel.City_Id;
            }
        }


        //********** Location  *******************
        $sc.locationModel = {};

        var jsonResult = '';
        locationSvc
        .Get('school/location/GetCountry')
        .then(function (d) {
            jsonResult = d.result;

            $sc.countrList = jsonResult.map(function (v, k) {
                return ({ CountryName: v.CountryName, CountryId: v.CountryId });
            });
        });

        var states; // data is in json aready at client side so it will render fast
        $sc.get_state = function () {

            states = $filter('filter')(jsonResult, { CountryId: $sc.locationModel.selected_country })[0]
                    .Zones
                    .map(function (v, k) {
                        return v.States;
                    });

            states = mergeData(states);

            $sc.stateList = states.map(function (v, k) {
                return ({ StateId: v.StateId, StateName: v.StateName });
            });
        }

        function mergeData(List) {
            var list = [];

            List.forEach(function (state, k) {
                state.forEach(function (v, k) {
                    list.push(v);
                });
            });
            return list;
        }

        $sc.get_city = function () {
            var cities = $filter('filter')(states, { StateId: $sc.locationModel.selected_state })[0]
                        .Districts
                        .map(function (v, k) {
                            return v.Cities;
                        });

            cities = mergeData(cities);

            $sc.cityList = cities.map(function (v, k) {
                return ({ CityId: v.CityId, CityName: v.CityName });
            });
        }

    }

    angular
        .module('SilverzoneERP_invenotry_component')
        .component('inventorySource', {
            templateUrl: '/Templates/Inventory/sourceDetail.html',
            controller: ['$scope', '$rootScope', 'inventoryService', 'Service', '$filter', inventory_sourceDetail]
        })
    ;

})();
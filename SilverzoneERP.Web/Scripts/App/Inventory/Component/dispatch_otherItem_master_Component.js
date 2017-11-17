//https://tests4geeks.com/build-angular-1-5-component-angularjs-tutorial/

(function () {
    'use strict';

    angular
        .module('SilverzoneERP_invenotry_component')  // create a new module Here
        .component('dipacthOtherItem', {
            templateUrl: '/Templates/inventory/dispatch_otherItem.html',
            controller: ['$scope', '$rootScope', 'dispatchService', function ($sc, $rsc, svc) {
                $sc.model = {};
                $sc.inventory_sources = [
                    { Id: 6, SourceName: 'School' },
                    { Id: 9, SourceName: 'Other' }
                ];
                $sc.classsName = 'col-sm-4';
                $sc.dispatch_itemList = [];
                $sc.cordinatorList = [];

                $sc.get_dispatchItems = function () {
                    svc.get_allDispatchItem()
                    .then(function (d) {
                        $sc.dispatch_itemList = d.result;
                    });
                }
                $sc.get_dispatchItems();


                $sc.searchSchool = function () {
                    if (!$sc.schCode) {
                        $rsc.notify_fx('Please Enter School Code !', 'info');
                        return;
                    }

                    svc.get_schoolInfo($sc.schCode)
                    .then(function (d) {
                        if (d.result === 'notfound')
                            $rsc.notify_fx('School Not found with given Code !', 'danger');
                        else {
                            // first assign country & state then city would be assign here below :)
                            $sc.$broadcast('on_getSchool_Location', { locationInfo: d.result.cityModel });

                            parseModel(d.result);
                            $sc.cordinatorList = _.flatten(d.result.Cordinators);
                        }
                    });
                }

                $sc.setText = function (val) {
                    if (val === 1 || val === 2) {
                        $sc.model.LabelValue = val === 1 ? ($sc.model.Principal || '') : ($sc.model.HM || '');
                        $sc.model.LabelCord = '';
                        $sc.model.LabelOth = '';
                    }
                    else if (val === 4) {
                        $sc.model.LabelValue = '';
                        $sc.model.LabelCord = '';
                        $sc.model.LabelOth = $sc.model.Other || '';
                    }
                    else if (val === 0) {
                        $sc.model.LabelValue = '';
                        $sc.model.LabelCord = '';
                        $sc.model.LabelOth = '';
                    }
                }


                function parseModel(vm) {
                    $sc.model.sourceId = vm.sourceId;
                    $sc.model.Name = vm.Name;
                    $sc.model.Address = vm.Address;
                    $sc.model.PhoneNo = vm.PhoneNo;
                    $sc.model.PinCode = vm.PinCode;
                    $sc.model.CityId = vm.cityModel.CityId;

                    $sc.model.HM = vm.HM;
                    $sc.model.Other = vm.Other;
                    $sc.model.Principal = vm.Principal;
                }

                $sc.submit_data = function (form, isBulk_insert) {
                    if (form.validate()) {
                        if ($sc.model.LabelType === 1 ||
                            $sc.model.LabelType === 2)
                            $sc.model.LabelName = $sc.model.LabelValue;
                        else if ($sc.model.LabelType === 0)
                            $sc.model.LabelName = '';
                        else if ($sc.model.LabelType === 3)
                            $sc.model.LabelName = $sc.model.LabelCord;
                        else if ($sc.model.LabelType === 4)
                            $sc.model.LabelName = $sc.model.LabelOth;

                        svc.saveItems($sc.model)
                           .then(function (d) {
                               if (d.result === 'ok') {
                                   $rsc.notify_fx('Data is saved :)', 'success');
                                   $sc.model = {};
                                   $sc.get_dispatchItems();
                               } else
                                   $rsc.notify_fx('Something went wrong, try again :(', 'danger');
                           });
                    }
                }

                $sc.validationOptions = {
                    rules: {
                        source: {
                            required: true,
                        },
                        type: {
                            required: true,
                        },
                        Items: {
                            required: true,
                        },
                        name: {
                            required: true,
                        },
                        address: {
                            required: true,
                        },
                        city: {
                            required: true,
                        },
                        Labeltype: {
                            required: true,
                        },
                        mobile: {
                            required: true,
                        },
                        pincode: {
                            required: true,
                        },
                        contactPerson: {
                            required: true,
                        }
                    }
                }


            }]
        })

    ;

})();
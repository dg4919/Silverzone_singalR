(function () {

    var printLabel_Controllerfn = function ($sc, svc, $filter, $modal) {
        $sc.order_list = [];
        $sc.searchModel = {             // set to default value
            printType: 0,
            orderDate: $filter('dateFormat')(new Date(), 'MM/DD/YYYY')
        };

        $sc.searchOrder = function () {
            svc.get_Orders($sc.searchModel).then(function (d) {
                $sc.order_list = d.result;
            });
        }

        $sc.searchOrder();

        $sc.printLabel = function ($index) {
            var _entity = $sc.order_list[$index];

            svc.createLabel(_entity.Id).then(function (d) {
                if (d.result.status === "success") {
                    _entity.packetNumber = d.result.packetInfo.Packet_Id;
                    svc.get_labelTemplate(d.result);
                }
                else
                    showModal(d.result);
            });
        }


        function showModal(_model) {
            var Template = ' <div class="modal-header">                                                                          '
                             + ' <h4 class="box-title">Give us Reason ? </h4>  </div>                                                '
                             + ' <form class="form-horizontal" name="printReason_form" ng-submit="ok(printReason_form, reason)" ng-validate="validationOptions">                                                                      '
                             + ' <div class="modal-body">                                                                            '
                             + ' <div class="form-group">                                                                            '
                             + ' <div class="col-sm-offset-1 col-sm-10">                                                             '
                             + ' <textarea rows="5" name="reason_description" class="form-control" placeholder="Reason of Print" ng-model="reason"></textarea> '
                             + ' </div> </div> </div>                                                                        '
                             + ' <div class="modal-footer">                                                                          '
                             + ' <button type="submit" class="btn btn-primary">Save</button>                                       '
                             + ' <button type="button" class="btn btn-warning" ng-click="cancel()">Cancel</button></div>                           '
                             + ' </form> '

            var modalInstance = $modal.open({
                template: Template,
                controller: 'lebelPrint_reason',            // No need to specify this controller on page explicitly    
                size: 'md',
                resolve: {
                    entityModel: function () {
                        return _model;               // send value from here to controller as dependency
                    }
                }
            });
        }

    }

    var printLabel_reason_Controllerfn = function ($sc, svc, $modalInstance, model) {   // model is pass by resolve() of > $modal.open() with parameter as DI

        $sc.ok = function (form, reason) {

            if (form.validate()) {
                var entity = {
                    Id: model.packetInfo.Id,
                    reason: reason
                }

                $modalInstance.close();
                svc.updateLabel(entity).then(function (d) {
                    svc.get_labelTemplate(model);
                });
            }
        }

        $sc.cancel = function () {
            $modalInstance.dismiss();
        }

        $sc.validationOptions = {
            rules: {
                reason_description: {            // use with name attribute in control
                    required: true
                }
            },
            messages: {
                reason_description: {            // use with name attribute in control
                    required: "Please enter reason of print !"
                }
            }
        }

    }

    var addWheight_Controllerfn = function ($sc, $rsc, $filter, $modal, svc) {
        $sc.packet_list = [];
        $sc.searchModel = {             // set to default value
            type: 0,
            orderDate: $filter('dateFormat')(new Date(), 'MM/DD/YYYY')
        };

        $sc.searchPacket = function () {
            svc.get_packets($sc.searchModel).then(function (d) {
                $sc.packet_list = d.result;
            });
        }
        $sc.searchPacket();

        $sc.addWheight = function (entity) {

            if (!entity.wheight) {
                $rsc.notify_fx('Please enter wheight of packet !', 'error');
                return;
            }

            var _entity = {
                packetNumber: entity.packetNumber,
                packetWheight: entity.wheight
            }
            svc.add_packetWheight(_entity).then(function (data) {
                if (data.result === "notfound")
                    $rsc.notify_fx('Packet number not found !', 'warning');
                else {
                    entity.hasAdd = true;
                    $rsc.notify_fx('Packet wheight has beed added !', 'info');
                }
            });
        }

        $sc.showModal = function () {
            var Template = ' <div class="modal-header">                                                                                                 '
                             + ' <h4 class="box-title">Give us Reason ? </h4>  </div>                                                                       '
                             + ' <form name="packetWheight_Form" class="form-horizontal" ng-validate="validationOptions">                                   '
                             + '<div class="box-body" style="margin-top: 25px;padding: 28px;">                                                              '
                             + '<div class="form-group"> <div class="col-sm-4">                                                                             '
                             + '<input type="text" class="form-control" placeholder="Packet Number" name="packetNumber" ng-model="packetInfo.packetNumber"> '
                             + '</div> <div class="col-sm-4">                                                                                               '
                             + '<span style="position: absolute;top: 8px;right: 25px;z-index: 10;">                                                         '
                             + '<strong>Kg</strong></span> <div class="input-group">                                                                        '
                             + '<input type="text" class="form-control" placeholder="Wheight" name="packetWheight" decimal:Only                             '
                             + 'ng-model="packetInfo.packetWheight" style="padding-right: 33px;"></div></div>                                               '
                             + '<div class="col-sm-2">                                                                                                      '
                             + '<button class="btn btn-info" type="button" ng-click="ok(packetWheight_Form)">                                               '
                             + 'Submit</button></div> <div class="col-sm-2">                                                                                '
                             + '<button class="btn btn-warning" type="button" ng-click="cancel()">                                                          '
                             + 'Cancel</button></div> </div></div>                                                                                          '
                             + '</form> '

            var modalInstance = $modal.open({
                template: Template,
                controller: 'addWheight_Modal',            // No need to specify this controller on page explicitly    
                size: 'md'
            });
        }

    }

    var addWheightModal_Controllerfn = function ($sc, $rsc, svc, $modalInstance) {   // model is pass by resolve() of > $modal.open() with parameter as DI
        $sc.packetInfo = {};

        $sc.ok = function (form, reason) {

            if (form.validate()) {

                svc.add_packetWheight($sc.packetInfo).then(function (data) {
                    if (data.result === "notfound")
                        $rsc.notify_fx('Packet number not found !', 'warning');
                    else {
                        $rsc.notify_fx('Packet wheight has beed added !', 'info');
                        $modalInstance.close();
                    }
                });
            }
        }

        $sc.cancel = function () {
            $modalInstance.dismiss();
        }

        $sc.validationOptions = {
            rules: {
                packetNumber: {            // use with name attribute in control
                    required: true
                },
                packetWheight: {
                    required: true
                }
            }
        }

    }

    var addConsignment_Controllerfn = function ($sc, $rsc, $filter, $modal, svc, myService) {
        $sc.packet_list = [];
        $sc.searchModel = {             // set to default value
            type: 0,
            orderDate: $filter('dateFormat')(new Date(), 'MM/DD/YYYY')
        };

        $sc.searchPacket = function () {
            svc.get_consignmentPackets($sc.searchModel).then(function (d) {
                $sc.packet_list = d.result;
            });
        }
        $sc.searchPacket();

        $sc.addConsignment = function (entity) {

            if (!entity.packetConsignmentNo) {
                $rsc.notify_fx('Please enter wheight of packet !', 'error');
                return;
            }

            getModal(entity).then(function (result) {
                entity.hasAdd = result.status;
            });
        }

        getModal = function (data) {
            var Template = '<div class="modal-header" style="padding: 10px !important;">                   \
                            <h4 class="box-title">Add Coupon</h4></div>                                    \
                            <form name="courierForm" ng-submit="ok(courierForm)" ng-validate="validationOptions" > \
                            <div class="modal-body"><div class="row">                           \
                            <div class="col-sm-6"> <select class="form-control" name="courier" ng-model="options.selected_courier" \
                            ng-options="::courier.Id as ::courier.Courier_name for courier in courierList"     \
                            ng-change="options.selected_courier && get_courierMode(options.selected_courier)"> \
                            <option value="">Select Courier Name</option> </select> </div>              \
                            <div class="col-sm-6" ng-hide="courierModeList.length === 0 || !options.selected_courier"> <select class="form-control" name="courierMode" ng-model="options.selected_courierMode" \
                            ng-options="::mode.Id as ::mode.Courier_Mode for mode in courierModeList"> \
                            <option value="">Select Courier Mode</option> </select> </div> </div> </div>      \
                            <div class="modal-footer">                                  \
                            <button type="submit" class="btn btn-primary">Save</button> \
                            <button type="button" class="btn btn-warning" ng-click="cancel()">Cancel</button></div> </form>'

            var modalInstance = $modal.open({
                template: Template,
                controller: 'courierModal',
                size: 'md',
                resolve: {
                    entity: function () {
                        return data;               // send value from here to controller as dependency
                    }
                }
            });

            return modalInstance.result;
        }

        //$sc.$watch('packet_list', function (newVal, oldVal) {
        //    $sc.$apply();
        //});

    }

    var courierModalFn = function ($sc, $rsc, svc, $modalInstance, entity) {
        $sc.options = {};
        $sc.courierModeList = [];

        svc.get_courierList().then(function (data) {
            $sc.courierList = data.result;
        });

        $sc.get_courierMode = function (courierId) {
            svc.get_courierModes(courierId).then(function (data) {
                $sc.courierModeList = data.result;
            });
        }

        $sc.ok = function (form) {

            if (form.validate()) {
                
                var _entity = {
                    dispatchId: entity.Id,
                    packetConsignment: entity.packetConsignmentNo,
                    courierMode: $sc.options.selected_courierMode
                }
                svc.add_packetConsignment(_entity).then(function (data) {
                    if (data.result === "exist")
                        $rsc.notify_fx('Consignment Number already Exist !', 'warning');
                    else if (data.result === "notfound")
                        $rsc.notify_fx('Packet number not found !', 'warning');
                    else if (data.result === "error")
                        $rsc.notify_fx('oops! error occured, try again', 'error');
                    else {
                        $rsc.notify_fx('Packet Consignment has beed added !', 'info');
                        $modalInstance.close({ status: true });
                    }
                });
            }
        }

        $sc.cancel = function () {
            $modalInstance.dismiss();
        }

        $sc.validationOptions = {
            rules: {
                courier: {            // use with name attribute in control
                    required: true
                },
                courierMode: {
                    required: true
                }
            }
        }
    }

    var trackOrder_Controllerfn = function ($sc, svc, $filter) {
        $sc._date = $filter('dateFormat')(new Date(), 'MM/DD/YYYY')

        $sc.searchPacket = function () {
            svc.get_trackingOrder($sc._date).then(function (d) {
                $sc.tracking_orderList = d.result;
            });
        }
        $sc.searchPacket();
    }



    angular.module('Silverzone_app')
      .controller('dispatch_printLabel', ['$scope', 'admin_orderDispatch_Service', '$filter', '$uibModal', printLabel_Controllerfn])
      .controller('lebelPrint_reason', ['$scope', 'admin_orderDispatch_Service', '$uibModalInstance', 'entityModel', printLabel_reason_Controllerfn])
      .controller('dispatch_addWheight', ['$scope', '$rootScope', '$filter', '$uibModal', 'admin_orderDispatch_Service', addWheight_Controllerfn])
      .controller('addWheight_Modal', ['$scope', '$rootScope', 'admin_orderDispatch_Service', '$uibModalInstance', addWheightModal_Controllerfn])
      .controller('dispatch_addConsignment', ['$scope', '$rootScope', '$filter', '$uibModal', 'admin_orderDispatch_Service', 'myService', addConsignment_Controllerfn])
      .controller('courierModal', ['$scope', '$rootScope', 'admin_orderDispatch_Service', '$uibModalInstance', 'entity', courierModalFn])
      .controller('trackOrder', ['$scope', 'admin_orderDispatch_Service', '$filter', trackOrder_Controllerfn])

    ;

})();


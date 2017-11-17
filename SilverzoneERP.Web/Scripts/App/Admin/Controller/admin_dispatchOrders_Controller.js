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
            var orderType = $sc.order_list[$index].orderType;

            var _entity = {
                Id: $sc.order_list[$index].Id,
                sourceType: orderType    // 3 for online order, 7 for silverzone orders (dealer, school, press etc), 8 for other orders    
            };

            svc.createLabel(_entity).then(function (d) {
                if (d.result.status === "success") {
                    $sc.order_list[$index].packetNumber = d.result.packetInfo.Packet_Id;

                    if (orderType === 3)    // for online orders
                        svc.get_labelTemplate(d.result);
                    else if (orderType === 9)          // for other orders
                        //alert('Order form need to create');
                        svc.get_otherItemlabelTemplate(d.result);
                }
                else {      // for dealer/press etc..
                    angular.extend(d.result, { orderType: orderType });
                    showModal(d.result);
                }
            });
        }


        function showModal(_model) {
            var Template = ' <div class="modal-header">                                                                          '
                             + ' <h4 class="box-title">Give us Reason ? </h4>  </div>                                                '
                             + ' <form class="form-horizontal" name="printReason_form" ng-submit="ok(printReason_form, reason)" ng-validate="validationOptions">                                                                      '
                             + ' <div class="modal-body">                                                                            '
                             + ' <div class="form-group">                                                                            '
                             + ' <div class="col-sm-offset-1 col-sm-10">                                                             '
                             + ' <textarea rows="5" name="reason_description" class="form-control1" placeholder="Reason of Print" ng-model="reason"></textarea> '
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

    var printLabel_reason_Controllerfn = function ($sc, svc, inventory_svc, $modalInstance, model) {   // model is pass by resolve() of > $modal.open() with parameter as DI

        $sc.ok = function (form, reason) {

            if (form.validate()) {
                var entity = {
                    Id: model.packetInfo.Id,
                    reason: reason
                }

                $modalInstance.close();
                svc.updateLabel(entity).then(function (d) {
                    if (model.orderType === 3)          // for online orders
                        svc.get_labelTemplate(model);
                    if (model.orderType === 9)          // for other orders
                        //alert('Order form need to create');
                        svc.get_otherItemlabelTemplate(model);
                    else                                // for Silverzone (press, dealer, school)
                        inventory_svc.print_challan(model);
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
        $sc.srchType = 0;

        $sc.searchPacket = function () {
            svc.get_packets($sc.searchModel).then(function (d) {
                $sc.srchType = parseInt($sc.searchModel.type);
                $sc.packet_list = d.result;
            });
        }

        $sc.searchPacket();

        $sc.addWheight = function (entity) {
            show_add_wheightModal(entity).then(function () {
                $sc.searchPacket();
            });
        }

        $sc.verifyOrder = function (packetId) {
            svc.verify_packetWheight(packetId)
            .then(function (d) {
                if (d.result === 'success') {
                    $rsc.notify_fx('Packet has been verified :)', 'info');
                    $sc.searchPacket();
                }
                else
                    $rsc.notify_fx('Packet not found :(', 'danger');
            });
        }

        $sc.printBundle = function (packet) {
            var _model = {
                Id: packet.order_challnId,
                sourceType: packet.order_isOnine ? 3 : 7    // 3 for online order & 7 for silverzone orders (dealer, school, press etc)
            }

            svc.createLabel(_model).then(function (d) {
                angular.extend(d.result, { bundleLength: packet.packet_bundleList.length });
                svc.printBundle(d.result);

            });
        }

        function show_add_wheightModal(entity) {
            var Template = ' <div class="modal-header">                                                                       \
                             <span class="box-title">Add Bundle </span>                                                       \
                             <input type="button" class="btn btn-sm btn-warning pull-right"                                   \
                                    value="Submit" style="margin-top: -7px;" ng-click="submitBundle()"> </div>                \
                             <form name="packetWheight_Form" class="form-horizontal" ng-validate="validationOptions">         \
                             <div class="box-body" style="margin-top: 25px;padding: 28px;">                                   \
                             <div class="form-group"> <div class="col-sm-6">                                                  \
                             <select class="form-control1" name="packageName" ng-model="packetInfo.packetInfo"                \
                             ng-options="::package.Name for package in packgeList">                                           \
                             <option value="">Select Packet Name</option> </select>                                           \
                             </div> <div class="col-sm-4">                                                                    \
                             <span style="position: absolute;top: 8px;right: 25px;z-index: 10;">                              \
                             <strong>Kg</strong></span> <div class="input-group">                                             \
                             <input type="text" class="form-control1" placeholder="Wheight" name="packetWheight" decimal:Only \
                             ng-model="packetInfo.packetWheight" style="padding-right: 33px;"></div></div>                    \
                             <div class="col-sm-2" style="    margin-top: 5px;">                                              \
                             <a href="#" class="text-warning" ng-click="addBundle()" style="font-size: larger;">              \
                             Add</a></div></div></div> </form>                                                                \
                             <table class="table table-striped responsive"                                                    \
                                    wt-responsive-table                                                                       \
                                    ng-if="packet_bundleList.length">                                                         \
                                 <thead>                                                                                      \
                                     <tr>                                                                                     \
                                         <th>S.No.</th>                                                                       \
                                         <th>Packet Name</th>                                                                 \
                                         <th class="text-center">Wheight</th>                                                 \
                                         <th>Delete</th>                                                                      \
                                     </tr>                                                                                    \
                             </thead>                                                                                         \
                             <tbody>                                                                                          \
                             <tr ng-repeat="entity in packet_bundleList track by $index">                                     \
                                <td>{{ $index+1 }}</td>                                                                       \
                                <td ng-bind="::entity.packetInfo.Name"></td>                                                  \
			                    <td> <div class="col-sm-offset-3 col-sm-7">                                                   \
                                  <input type="text" class="form-control1" ng-model="entity.packetWheight" decimal:only />    \
                             </div> </td>                                                                                     \
                                <td><a href="#" ng-click="deleteBundle($index)"> <i class="fa fa-trash"></i></a></td>         \
                             </tbody> </table>                                                                   '
            var modalInstance = $modal.open({
                template: Template,
                resolve: {
                    entity: function () {
                        return entity;               // send value from here to controller as dependency
                    }
                },
                controller: ['$scope', 'admin_orderDispatch_Service', '$uibModalInstance', 'entity', function ($sc, svc, $modalInstance, packet) {
                    $sc.packetInfo = {};
                    $sc.packet_bundleList = [];

                    svc.get_packages()
                        .then(function (d) {
                            $sc.packgeList = d.result;
                        });

                    $sc.addBundle = function () {
                        $sc.packet_bundleList.push($sc.packetInfo);
                        $sc.packetInfo = {};
                    }

                    $sc.submitBundle = function () {
                        $sc.packet_bundleList;
                        var _model = $sc.packet_bundleList.map(function (entity) {
                            return ({
                                PM_Id: entity.packetInfo.Id,
                                Netwheight: entity.packetWheight
                            })
                        });

                        var newModel = {
                            dispatch_mId: packet.Id,
                            wheightModel: _model
                        };

                        svc
                        .add_packetWheight(newModel)
                        .then(function (d) {
                            // $rsc is getting from base ctrler
                            $rsc.notify_fx('Wheight is added !', 'info');
                            $modalInstance.close();
                        });
                    }

                    $sc.deleteBundle = function (index) {
                        $sc.packet_bundleList.splice(index, 1);
                    }

                }],
                size: 'md'
            });
            return modalInstance.result;
        }

    }

    var addConsignment_Controllerfn = function ($sc, $rsc, $filter, $modal, svc) {
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
                $sc.searchPacket();
            });
        }

        getModal = function (data) {
            var Template = '<div class="modal-header" style="padding: 10px !important;">                   \
                            <h4 class="box-title">Add Courier</h4></div>                                    \
                            <form name="courierForm" ng-submit="ok(courierForm)" ng-validate="validationOptions" > \
                            <div class="modal-body"><div class="row">                           \
                            <div class="col-sm-6"> <select class="form-control1" name="courier" ng-model="options.selected_courier" \
                            ng-options="::courier.Id as ::courier.Courier_Name for courier in courierList"     \
                            ng-change="options.selected_courier && get_courierMode(options.selected_courier)"> \
                            <option value="">Select Courier Name</option> </select> </div>              \
                            <div class="col-sm-6" ng-hide="courierModeList.length === 0 || !options.selected_courier"> <select class="form-control1" name="courierMode" ng-model="options.selected_courierMode" \
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

    var trackOrder_Controllerfn = function ($sc, $rsc, svc, $filter) {
        $sc._date = $filter('dateFormat')(new Date(), 'MM/DD/YYYY')
        $sc.orderStatusList = [];

        $sc.searchPacket = function () {
            svc.get_trackingOrder($sc._date).then(function (d) {
                $sc.tracking_orderList = d.result;

                angular.forEach($sc.tracking_orderList, function (pkt, key) {
                    if (pkt.orderHistory.length) {
                        var str = '';
                        angular.forEach(pkt.orderHistory, function (histry, key) {
                            str += histry.Action + ' : ' + $filter('dateFormat')(histry.Action_Date, 'DD/MM/YYYY') + '  ';
                        });
                        pkt.orderHistory_Info = str;
                    }
                    else 
                        pkt.orderHistory_Info = 'No Data Found';
                });
            });
        }

        $sc.searchPacket();

        svc.get_orderStatusList()
          .then(function (d) {
              $sc.orderStatusList = d.result;

              if ($sc.orderStatusList)
                  getReasons();
          });

        $sc.change_orderStatus = function (order) {
            if (order.dispatch_Status === 11) {
                svc.Resend_Order(order.Id)
                   .then(function (d) {
                       if (d.result === 'ok')
                           $rsc.notify_fx('Packet has been Resend :)', 'info');
                       else
                           $rsc.notify_fx('Packet not found :(', 'warning');
                   });
            }
            else {
                svc.change_trackingStatus(order.dispatch_Status, order.Id, order.remarks)
                   .then(function (d) {
                       if (d.result === 'ok')
                           $rsc.notify_fx('Packet Status has changed :)', 'info');
                       else
                           $rsc.notify_fx('Packet not found :(', 'warning');
                   });
            }
        }

        $sc.onKeyUp = function (order) {
            order.showRemarks = true;
        }

        $sc.onClik = function (order, str) {
            order.remarks = str;
            order.showRemarks = false;
        }

        function getReasons() {
            svc.get_orderStatus_Reasons()
                .then(function (d) {
                    $sc.OrderStatus_Reasons = d.result;
                });
        }

    }

    var searchDispatch_Controllerfn = function ($sc, svc, inventorySvc, $filter) {
        $sc.model = {};
        $sc.inventory_sources = [];
        $sc.courierList = [];
        $sc.courierModeList = [];
        $sc.orderStatusList = [];
        $sc.packet_status = [
            { Id: 1, type: 'Without Wheight' },
            { Id: 2, type: 'With Wheight, without Verified' },
            { Id: 3, type: 'With Wheight, without consignment' },
            { Id: 4, type: 'With Consignment' },
        ];
        $sc.srchResult = [];

        inventorySvc.get_inventorySource().then(function (d) {
            $sc.inventory_sources = d.result;
        });

        svc.get_orderStatusList()
           .then(function (d) {
               $sc.orderStatusList = d.result;
           });

        svc.get_courierList().then(function (data) {
            $sc.courierList = data.result;
        });

        $sc.get_courierMode = function () {
            svc.get_courierModes($sc.model.courierId).then(function (data) {
                $sc.courierModeList = data.result;
            });
        }

        $sc.search = function () {
            svc.searchItem($sc.model)
               .then(function (d) {
                   $sc.srchResult = d.result;
               });
        }

    }


    angular.module('Silverzone_admin_app')
      .controller('dispatch_printLabel', ['$scope', 'admin_orderDispatch_Service', '$filter', '$uibModal', printLabel_Controllerfn])
      .controller('lebelPrint_reason', ['$scope', 'admin_orderDispatch_Service', 'inventoryService', '$uibModalInstance', 'entityModel', printLabel_reason_Controllerfn])
      .controller('dispatch_addWheight', ['$scope', '$rootScope', '$filter', '$uibModal', 'admin_orderDispatch_Service', addWheight_Controllerfn])
      .controller('dispatch_addConsignment', ['$scope', '$rootScope', '$filter', '$uibModal', 'admin_orderDispatch_Service', addConsignment_Controllerfn])
      .controller('courierModal', ['$scope', '$rootScope', 'admin_orderDispatch_Service', '$uibModalInstance', 'entity', courierModalFn])
      .controller('trackOrder', ['$scope', '$rootScope', 'admin_orderDispatch_Service', '$filter', trackOrder_Controllerfn])
      .controller('searchDispatch', ['$scope', 'admin_orderDispatch_Service', 'inventoryService', '$filter', searchDispatch_Controllerfn])

    ;

})();


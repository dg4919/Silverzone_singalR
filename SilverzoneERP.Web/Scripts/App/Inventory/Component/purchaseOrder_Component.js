
(function () {
    'use strict';

    purchaseOrder_createFn.$inject = ['$scope', '$rootScope', '$location', 'inventoryService', 'inventory_modalService'];
    function purchaseOrder_createFn($sc, $rsc, $location, svc, modalSvc) {
            $sc.model = {};
            $sc.className = 'col-md-4';
            $sc.isEdit_record = false;
            $sc.isDisable = false;
            $sc.created_PO_list = [];          

            var path = $location.path();
            if (path.indexOf('counterSale') > -1) {
                $sc.model.From = 10;
                $sc.model.srcFrom = 49;
                $sc.model.To = 7;
                $sc.model.srcTo = 37;
            }

            svc.get_inventorySource().then(function (d) {
                $sc.inventory_sources = d.result;
            });

            $sc.submit_data = function (form) {

                if (!$sc.model.From
                    || !$sc.model.To) {
                    $rsc.notify_fx('Please select inventory source !', 'warning');
                    return false;
                }

                if (form.validate()) {
                        svc.create_PO($sc.model)
                        .then(common_responseFx);
                }
            }

            var common_responseFx = function (data) {
                if (data.result === 'success') {
                    $sc.model.PO_masterId = data.PO_masterId;
                    parseModel();

                    get_purchaseOrder_byId();
                    $sc.isDisable = true;

                    if (!$sc.isEdit_record)
                        $rsc.notify_fx('Purchase order has been created !', 'success');
                    else
                        $rsc.notify_fx('Purchase order has been updated !', 'success');

                    $sc.isEdit_record = false;
                }
                else if (data.result === 'exist') {
                    modalSvc.get_editeModal().then(function (res) {
                        $sc.EditData(data.entity);
                    });
                }
                else
                    $rsc.notify_fx('Something went wrong, Try again :(', 'error');
            }

            var get_purchaseOrder_byId = function () {
                svc.get_purchaseOrders($sc.model.PO_masterId)
                .then(function (d) {
                    $sc.created_PO_list = d.result;
                })
            }

            $sc.validationOptions = {
                rules: {
                    source: {            // field will be come from component
                        required: true
                    },
                    from: {
                        required: true,
                    },
                    to: {
                        required: true,
                    },
                    Qty: {
                        required: true,
                        min: 1
                    },
                    rate: {
                        required: true,
                    },
                    Class: {
                        required: true,
                    },
                    Subject: {
                        required: true,
                    },
                    Categoory: {
                        required: true,
                    }
                }
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
                parseModel(true, sourceDetails);
                $sc.isEdit_record = true;
            }

            $sc.DeleteData = function (_id) {

                svc.delete_PO(_id)
                 .then(function (d) {
                     if (d.result === 'success') {
                         $rsc.notify_fx('Purchase order has been deleted succesfully !', 'info');
                         get_purchaseOrder_byId();
                     }
                     else
                         $rsc.notify_fx('Oops record is not found, Try Again :(', 'warning');
                 });
            }

            var parseModel = function (isEdit, _model) {
                $sc.model.POId = isEdit ? _model.Id : 0;
                $sc.model.Quantity = isEdit ? _model.Quantity : '';
                if ($sc.model.Rate !== undefined)
                    $sc.model.Rate = isEdit ? _model.Rate : '';

                $sc.model.BookISBN = '';

                if (isEdit)
                    $sc.$broadcast('on_getBookCategoy', _model.Book.bookId_bundle);
                else
                    $sc.$broadcast('onSubmitData', $sc.model.bookId);
            }

            $sc.$on('onEmit_getBookCategoy', function (event, args) {
                $sc.$broadcast('on_getBookCategoy', args);
            });
    }

    var search_POFn = {
        templateUrl: '/Templates/Inventory/purchase_order/pendingPO_search.html',
        controller: ['$scope', '$rootScope', 'inventoryService', 'inventory_modalService', function ($sc, $rsc, svc, modalSvc) {
            $sc.model = {
                from: moment(new Date()).subtract(1, 'months').format('MM/DD/YYYY'),       // moment JS for date
                to: moment(new Date()).format('MM/DD/YYYY')
            };
            $sc.bookSearchModel = {};
            $sc.classsName = 'col-sm-3';
            $sc.PO_searchResult = [];

            $sc.search = function (form) {
                $sc.model.subjectId = $sc.bookSearchModel.subjectId;
                $sc.model.classId = $sc.bookSearchModel.classId;
                $sc.model.CategoryId = $sc.bookSearchModel.category && $sc.bookSearchModel.category.CategoryId;

                $sc.model.sourceType = $sc.source_model && $sc.source_model.SourceInfo_Id;

                if (form.validate()) {
                    svc
                        .search_pendingPO($sc.model)
                        .then(function (d) {
                            $sc.PO_searchResult = d.result;
                        });
                }
            }

            $sc.get_pendingPO_details = function (srcId, bookId) {
                svc
                   .getPendingPO_info(srcId, bookId)
                   .then(function (d) {
                       modalSvc.get_PendingPOModal(d.result[0], 2);
                   });
            }

            $sc.validationOptions = {
                rules: {
                    formDate: {
                        required: true,
                    },
                    toDate: {
                        required: true,
                    }
                }
            }

            $sc.showInfo = function (entity, poType) {
                var result = entity.poQty - entity.stockQty;

                poType = parseInt(poType);
                if (poType === 1) {       // pending PO
                    if (result > 0)
                        return true;
                }
                else if (poType === 2) {  // completed PO
                    if (result <= 0)
                        return true;
                }
            }

        }]
    }

    angular
        .module('SilverzoneERP_invenotry_component')
        .component('purchaseOrderCreate', {
            templateUrl: '/Templates/Inventory/purchase_order/purchaseOrder.html',
            controller: purchaseOrder_createFn
        })
        .component('counterSaleCreate', {
            templateUrl: '/Templates/Inventory/purchase_order/counterSale.html',
            controller: purchaseOrder_createFn
        })
        .component('purchaseOrderEdit', {
            templateUrl: '/Templates/Inventory/purchase_order/purchaseOrder_Edit.html',
            controller: ['$scope', '$rootScope', 'inventoryService', 'inventory_modalService', function ($sc, $rsc, svc, modalSvc) {
                $sc.model = {};
                $sc.className = 'col-md-4';
                $sc.isEdit_record = false;
                $sc.created_PO_list = [];
                $sc.clikIndex = [];
                $sc.txtIndex = [];

                $sc.showBtn = function (index, status, isCancel) {
                    $sc.clikIndex[index] = status;

                    if (status)
                        $sc.txtIndex[index] = $sc.created_PO_list.PO_detail[index].Quantity;
                    else if (!status && !isCancel)
                        $sc.created_PO_list.PO_detail[index].Quantity = parseInt($sc.txtIndex[index]);
                }

                $sc.search_PO = function () {
                    var poNumber = $sc.model.PO_Number;
                    if (poNumber) {
                        svc.getPO_byPoNo(poNumber)
                        .then(function (d) {
                            if (d.result === 'stock_exist')
                                $rsc.notify_fx('Inventory is created for this PO', 'info');  // not allow to edit a PO if inventory is created :)
                            if (d.result === 'notfound')
                                $rsc.notify_fx('PO not found', 'warning');
                            else {
                                $sc.model.PO_masterId = d.result.po_mId;
                                $sc.created_PO_list = d.result;
                            }
                        })
                    }
                    else
                        $rsc.notify_fx('Enter PO number', 'error');
                }

                $sc.EditData = function (sourceDetails) {
                    parseModel(true, sourceDetails);
                    $sc.isEdit_record = true;
                }

                $sc.Delete = function (id) {
                    modalSvc.get_deleteModal()
                     .then(function (d) {

                         svc.delete_PO(id)
                         .then(function (d) {
                             if (d.result === 'success') {
                                 $rsc.notify_fx('Purchase order has been deleted succesfully !', 'info');
                                 get_purchaseOrder_byId();
                             }
                             else
                                 $rsc.notify_fx('Oops record is not found, Try Again :(', 'warning');
                         });
                     });

                }

                $sc.submit_data = function (entity, index, status) {
                    if (index !== undefined)
                        $sc.showBtn(index, status);

                    if (!entity.Id) {            // create New PO
                        if (!$sc.model.BookId) {
                            $rsc.notify_fx('Please select book category !', 'error');
                            return;
                        }

                        if (!$sc.model.Quantity) {
                            $rsc.notify_fx('Please enter book quantity !', 'error');
                            return;
                        }

                        $sc.model.Rate = $sc.model.Rate || 0;

                        svc.create_PO($sc.model)
                        .then(common_responseFx);
                    }
                    else {          // update PO
                        $sc.model.Id = entity.Id;
                        $sc.model.Quantity = entity.Quantity;
                        $sc.model.BookId = entity.Book.bookId_bundle.bookId;
                        $sc.model.Rate = entity.Rate;

                        $sc.isEdit_record = true;

                        svc.update_PO($sc.model)
                        .then(common_responseFx);
                    }

                }

                var common_responseFx = function (data) {
                    if (data.result === 'success') {
                        $sc.model.PO_masterId = data.PO_masterId;
                        parseModel();

                        get_purchaseOrder_byId();

                        if (!$sc.isEdit_record)
                            $rsc.notify_fx('Purchase order has been created !', 'success');
                        else
                            $rsc.notify_fx('Purchase order has been updated !', 'success');

                        $sc.isEdit_record = false;
                    }
                    else if (data.result === 'exist') {
                        $rsc.notify_fx('Book already exist for this PO', 'info');
                    }
                    else
                        $rsc.notify_fx('Something went wrong, Try again :(', 'error');
                }

                var get_purchaseOrder_byId = function () {
                    svc.get_purchaseOrders($sc.model.PO_masterId)
                    .then(function (d) {
                        $sc.created_PO_list = d.result;
                    })
                }

                var parseModel = function (isEdit, _model) {
                    $sc.model.Id = isEdit ? _model.Id : '';
                    $sc.model.Quantity = isEdit ? _model.Quantity : '';
                    if ($sc.model.Rate !== undefined)
                        $sc.model.Rate = isEdit ? _model.Rate : '';

                    $sc.model.BookISBN = '';

                    if (isEdit)
                        $sc.$broadcast('on_getBookCategoy', _model.Book.bookId_bundle);
                    else
                        $sc.$broadcast('onSubmitData', $sc.model.bookId);
                }

            }]
        })

        .component('searchPendingPo', search_POFn)
        .component('pendingPoDetailsModal', {
            templateUrl: '/Templates/Inventory/modal/search_pendingPO_modal.html',
            bindings: {  // 1 way bindings
                resultList: '<',
                modalInstance: '<'
            },
            controller: ['$scope', 'inventoryService', function ($sc, svc) {
                $sc.poList = this.resultList;

                $sc.adjustEntry = function (PO) {
                    var qty = PO.poQty - PO.stockQty;

                    $.confirm({
                        title: 'Adjustment Confirmation !',
                        content: '<h5> Do you really want to adjust ' + qty + ' pending Quantities </h5>'
                               + ' <input type="text" placeholder="Adjustment Reason" class="name form-control1" required />',
                        buttons: {
                            cancel: function () {
                                //$.alert('Canceled!');
                            },
                            Ok: {
                                text: 'OK',
                                btnClass: 'btn-blue',
                                keys: ['enter', 'shift'],
                                action: function () {
                                    var name = this.$content.find('.name').val();
                                    if (!name) {
                                        $.alert('Provide adjustment reason !');
                                        return false;
                                    }

                                    svc.adjust_pendingPO(PO.Id, name)
                                    .then(function (d) {
                                        if (d.result == 'success') {
                                            $.alert('Inventory is adjusted !');
                                            $sc.$ctrl.modalInstance.close();
                                        }
                                        else {
                                            $.alert('Inventory not found !');
                                        }
                                    });
                                }
                            }
                        }
                    });
                }

            }]
        })
    ;

})();
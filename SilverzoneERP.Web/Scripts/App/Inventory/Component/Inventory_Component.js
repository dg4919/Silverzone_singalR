//https://tests4geeks.com/build-angular-1-5-component-angularjs-tutorial/

(function () {
    'use strict';

    inventory_fn.$inject = ['$scope', '$rootScope', 'inventoryService', 'inventory_modalService', '$filter', '$location', '$state', '$stateParams'];
    function inventory_fn($sc, $rsc, svc, modalSvc, $filter, $location, $state, $stateParams) {
        $sc.model = {
            InventoryType: 1,
            chekQty: true,
        };
        $sc.editChallan = false;                   // to prevent bulk inserrtion for challan create

        var path = $location.path();
        if (path.indexOf('Press') > -1)            // $state will not work bcoz Url is creating dynamically using.. (a href)
            $sc.model.srcId = 1;
        else if (path.indexOf('Dealer') > -1)      // for Dealer inventory
            $sc.model.srcId = 2;
        else if (path.indexOf('School') > -1)      // for school inventory create
            $sc.model.srcId = 6;
        else if (path.indexOf('Challan') > -1)     // for challan Edit Screen
            $sc.editChallan = true;
        else if (path.indexOf('CounterSale') > -1) { // for Counter sale
            $sc.model.srcId = 10;
            $sc.model.InventoryType = 2;
            $sc.disableChallan = true;
            $sc.model.SourceInfo_Id = 49;
        }

        $sc.disableItem = false;
        $sc.disableChallan = false;
        $sc.poInfo = {};
        $sc.className = 'col-sm-4';

        // we can also use $sc.$watch || $sc.$watch >  both wrk same
        $sc.$watchCollection('[model.SourceInfo_Id, model.InventoryType]', function (newVal, oldVal) {
            // SourceInfo_Id != 49 >> for counter custmer
            if (newVal[0] && newVal[0] !== 49)      // 1 array hold new & old value of SourceInfo_Id
                get_POlist();

            if (newVal[1] === 1)
                $sc.disableChallan = false;
            else if (newVal[1] === 2)
                $sc.disableChallan = true;
        });

        var get_POlist = function () {
            var _model = {
                srcId: $sc.model.srcId,
                SourceInfo_Id: $sc.model.SourceInfo_Id,
                type: $sc.model.InventoryType
            }

            svc.getPendingPO(_model)
                 .then(function (d) {
                     if (d.result === 'notfound')
                         $rsc.notify_fx('Oops! no PO found :(, try again', 'info');
                     else if (d.result === 'exist')
                         $rsc.notify_fx('PO already exist in stock !', 'warning');
                     else
                         $sc.poInfo = d.result;
                 });
        }

        $sc.submit_data = function (form, isBulk_insert) {
            if (form.validate())
                $sc.call_service(isBulk_insert);
        }

        $sc.call_service = function (isBulk_insert) {
            if (isBulk_insert)
                svc.create_bulkStock($sc.model)
                .then(common_responeFn);
            else
                svc.save_stock($sc.model)
                .then(common_responeFn);
        }

        var common_responeFn = function (d) {
            if (d.result === 'success') {
                $sc.disableChallan = true;
                $sc.disableItem = true;

                $sc.model.stock_mId = d.stock_mId;

                if ($sc.model.InventoryType === 2)  // for outward
                {
                    $sc.model.ChallanNo = d.challanInfo.ChallanNo;
                    $sc.model.ChallanDate = $filter('dateFormat')(d.challanInfo.ChallanDate, 'MM/DD/YYYY hh:mm A');
                }

                getInventory_list();
                get_POlist();

                if ($sc.model.stockId)
                    $rsc.notify_fx('Inventory is updated !', 'success');
                else
                    $rsc.notify_fx('Inventory is created !', 'success');

                $sc.model.chekQty = true;
            }
            else if (d.result === 'maxQty') {
                modalSvc.get_qtyModal()
                        .then(function (d) {
                            $sc.model.chekQty = false;
                            $sc.call_service();
                        });
            }
            else if (d.result === 'notfound') {
                $rsc.notify_fx('Please enter a valid PO number !', 'warning');
            }
            else if (d.result === 'book_notfound') {
                $rsc.notify_fx('Book is not found against given PO number !', 'warning');
            }
            else if (d.result === 'exist') {
                $rsc.notify_fx('Inventory already exist !', 'warning');
            }
            else
                $rsc.notify_fx('Something went wrong :(, Try again !', 'error');
        }

        var getInventory_list = function () {
            svc.get_inventoryList($sc.model.stock_mId).then(function (d) {
                $sc.inventoryList = d.result;
            });
        }

        $sc.DeleteRecord = function (_id) {
            svc.delete_stock(_id)
                 .then(function (d) {
                     if (d.result === 'success') {
                         $rsc.notify_fx('Inventory has been deleted succesfully !', 'info');
                         getInventory_list();
                         get_POlist();
                     }
                     else
                         $rsc.notify_fx('Oops record is not found, Try Again :(', 'warning');
                 });
        }

        $sc.validationOptions = {
            rules: {
                source: {
                    required: true,
                },
                callanNo: {
                    required: true,
                },
                callanDt: {
                    required: true,
                }
            }
        }

        $sc.goto_viewChallan = function () {
            $state.go('EditInventory', { challanNo: $sc.model.ChallanNo });
        }


        // *********************  for Counter Sale  ************************

        $sc.searchPO = function () {
            svc.get_counterSalePO($sc.model)
               .then(function (d) {
                   if (d.result)
                       $sc.poInfo = d.result;
                   else
                       $rsc.notify_fx('Customer Po not found :(', 'warning');
               });
        }

        // *********************  for edit PO :)  ************************

        $sc.search_stock = function () {
            if (!$sc.model.ChallanNo) {
                $rsc.notify_fx('Enter challan number !', 'warning');
                return;
            }

            svc.get_inventory_byChallan($sc.model.ChallanNo)
            .then(function (d) {
                if (d.result === 'notfound')
                    $rsc.notify_fx('Challan does not exsit, try again :(', 'warning');
                else {
                    $sc.model.stock_mId = d.result.stock_mId;
                    $sc.model.InventoryType = d.result.InventoryType;
                    $sc.model.SourceInfo_Id = d.result.SourceInfo_Id;
                    $sc.model.srcId = d.result.srcId;
                    $sc.inventoryList = d.result;
                    $sc.dispatchInfo = d.dispatchInfo;
                    $sc.isChalan_disable = true;
                }
            })
        }

        if ($stateParams.challanNo) {         // contains only parmaeters
            $sc.model.ChallanNo = $stateParams.challanNo;
            $sc.search_stock();
        }

        $sc.div_isVisible = false;
        $sc.toggleDiv = function (event) {
            var target = angular.element(event.currentTarget).parents('.sz_card').find('#toggleDiv');

            $sc.div_isVisible = !$sc.div_isVisible;
            target.toggle(200);
        }

        $sc.create_Invoice = function () {
            if ($sc.inventoryList.srcId === 10)     // for coounter sale show POPup
            {
                svc.getCustomer_BooksPrice($sc.model.stock_mId)
                   .then(function (d) {
                       modalSvc.getCustomer_InfoModal(d.result)
                       .then(function (d) {
                           var _model = angular.merge({},
                                        { customerInfo: d },
                                        { source: $sc.inventoryList.srcFrom_info },
                                        { stockInfo: $sc.inventoryList.stockInfo });

                           svc
                          .create_CounterOrder(angular
                                              .extend(d, { StockId: $sc.model.stock_mId })
                                              ).then(function (d) {
                                                  if (d.result === 'ok') {
                                                      svc.CounterOrder_template(_model);
                                                      $rsc.notify_fx('Customer Order successfully placed :)', 'info');
                                                  } else
                                                      $rsc.notify_fx('Invoice already exist for this order !', 'warning');
                                              });
                       });
                   });
            }
            else {
                var _model = {
                    Id: $sc.model.stock_mId,
                    sourceType: $sc.inventoryList.srcId
                }

                svc
                .ceateOrder(_model)
                .then(function (d) {
                    if (d.result.status === 'success') {
                        $rsc.notify_fx('Invoice is generated :)', 'info');
                        $sc.dispatchInfo = d.result.dispatchInfo;

                        svc.print_challan(d.result, $sc.poInfo);
                    }

                });
            }
        }

    }


    angular
        .module('SilverzoneERP_invenotry_component')  // create a new module Here
        .component('pressInventory', {
            templateUrl: '/Templates/inventory/stock_inventory/press_InventoryCreate.html',
            controller: inventory_fn
        })
        .component('counterInventory', {
            templateUrl: '/Templates/inventory/stock_inventory/counterSale.html',
            controller: inventory_fn
        })

        .component('editInventory', {
            templateUrl: '/Templates/inventory/stock_inventory/Inventory_Edit.html',
            controller: inventory_fn
        })

        .component('inventoryDetail', {
            templateUrl: '/Templates/inventory/stock_inventory/inventoryDetail.html',
            controller: ['$scope', 'inventoryService', 'inventory_modalService', function ($sc, svc, modalSvc) {
                $sc.stockDetail = {};

                svc.searchStock()
                   .then(function (d) {
                       $sc.stockDetail = d.result;
                   });

                $sc.getDetail = function (bookId, inventoryType) {
                    svc.searchStock_byBookid(bookId, inventoryType)
                    .then(function (d) {
                        modalSvc
                        .get_stock_detailModal(d.result, inventoryType);
                    });
                }

            }]
        })

        .component('invoiceSearch', {
            templateUrl: '/Templates/inventory/stock_inventory/invoice_search.html',
            controller: ['$scope', 'inventoryService', 'inventory_modalService', function ($sc, svc, modalSvc) {
                $sc.model = {
                    from: moment(new Date()).subtract(1, 'months').format('MM/DD/YYYY'),       // moment JS for date
                    to: moment(new Date()).format('MM/DD/YYYY')
                };
                $sc.classsName = 'col-sm-3';

                $sc.search = function (form) {
                    svc.get_allInvoice($sc.model)
                       .then(function (d) {
                           $sc.invoiceDetails = d.result;
                       });
                }

                $sc.get_invoiceInfo = function (_dealerId) {
                    $sc.model.dealerId = _dealerId;

                    modalSvc
                    .get_invoice_detailModal($sc.model);
                }

            }]
        })

        .component('pendingPoModal', {
            templateUrl: '/Templates/Inventory/modal/inventory_pendingPO_modal.html',
            bindings: {  // 1 way bindings
                resultList: '<',
                modalInstance: '<'
            },
            controller: ['$scope', 'inventoryService', function ($sc, svc) {
                $sc.poList = this.resultList;

                $sc.EditData = function (entity, poNumber) {
                    var model = {
                        PO_Number: poNumber,
                        bookISBN: entity.bookISBN,
                        bookId_bundle: entity.bookId_bundle
                    }
                    $sc.$ctrl.modalInstance.close(model);
                }

                $sc.show_pendingPO = function (list) {
                    var result = false;

                    // in angularjs foreach > return does not work
                    list.forEach(function (v, k) {
                        if (v.pendingQty > 0) {
                            result = true;
                            return;
                        }
                    });
                    return result;
                }
            }]
        })

        .component('verifyInventory', {
            templateUrl: '/Templates/inventory/stock_inventory/Inventory_Verify.html',
            controller: ['$scope', '$rootScope', 'inventoryService', '$filter', function ($sc, $rsc, svc, $filter) {
                $sc.model = {};
                $sc.className = 'col-sm-4';
                $sc.get_bookModel = true;
                $sc.challanList = [];
                $sc.isDisableChallan = false;

                $sc.clikIndex = '';
                $sc.showSts = '';
                $sc.txtIndex = [];

                $sc.showBtn = function (index, status, isCancel) {
                    $sc.clikIndex = index;
                    $sc.showSts = status;

                    if (status)
                        $sc.txtIndex[index] = $sc.challanList[index].Quantity;
                    else if (!status && !isCancel)
                        if ($sc.txtIndex[index])
                            $sc.challanList[index].Quantity = parseInt($sc.txtIndex[index]);
                }

                $sc.AddChallan = function (form) {
                    if (form.validate()) {
                        var record_isExist = false;
                        angular.forEach($sc.challanList, function (entity) {
                            if (entity.PO_Number === $sc.model.PO_Number &&
                               entity.BookId === $sc.model.BookId)
                                record_isExist = true;
                        });

                        if (record_isExist) {
                            $rsc.notify_fx('Record is exist, try another :(', 'warning');
                            return;
                        }

                        var res = {
                            PO_Number: $sc.model.PO_Number,
                            BookId: $sc.model.BookId,
                            BookName: getBookName(),
                            Quantity: $sc.model.Quantity,
                        };

                        $sc.challanList.push(res);
                        $sc.model = {};
                    }
                }

                $sc.verifyChallan = function () {
                    var _model = {
                        ChallanNumber: $sc.ChallanNo,
                        ChallanList: $sc.challanList
                    };

                    svc.verifyChallan(_model)
                       .then(function (d) {
                           if (d.Status === 'notFound') {
                               $rsc.notify_fx('Challan Does not Exist, Try another :(', 'danger');
                               return;
                           }

                           if (d.Status === "verified") {
                               $rsc.notify_fx('Challan already verified :)', 'info');
                               $sc.isDisableChallan = true;
                               return;
                           }

                           if (d.Status) {
                               $rsc.notify_fx('Congrats !, Challan is verified :)', 'success');
                               $sc.isDisableChallan = true;
                           }
                           else if (!d.Status)
                               $rsc.notify_fx('Challan is not verified :(', 'warning');

                           $sc.challanList.map(function (entity, key) {
                               entity.Status = d.result[key].status;
                           });

                           // it will check existing challan & add new record if not found from DB :)
                           angular.forEach(d.challanInfo, function (entity, k) {

                               var is_entityMatched = false;

                               angular.forEach($sc.challanList, function (challan, k) {
                                   if (entity.BookId === challan.BookId) {
                                       is_entityMatched = true;
                                       challan.challanQty = entity.challanQty;
                                   }
                               });
                               if (!is_entityMatched) {
                                   entity.Status = 1;
                                   entity.Quantity = 0;
                                   $sc.challanList.push(entity);
                               }
                           });

                           $sc.challanList;
                       });
                }

                $sc.Delete = function (index) {
                    $sc.challanList.splice(index, 1);
                }

                $sc.subjects = {};
                $sc.$on('on_emit_getBookModel', function (e, args) {
                    $sc.subjects = args.bookModel;
                });

                function getBookName() {
                    var subject = $filter('filter')($sc.subjects, function (entity) {
                        return entity.subject.Id === $sc.model.subjectId;
                    }, true)[0].subject;

                    var Class = $filter('filter')(subject.Class, { Id: $sc.model.classId }, true)[0];   // true is must for exact value match
                    var category = $filter('filter')(Class.category, { BookId: $sc.model.BookId }, true)[0];

                    return subject.Name + ' : ' + Class.Name + ' - ' + category.Name;
                }

                $sc.validationOptions = {
                    rules: {
                        Categoory: {
                            required: true,
                        },
                        PO_Number: {
                            required: true,
                        },
                        Qty: {
                            required: true,
                        },
                        ChallanNo: {
                            required: true,
                        }
                    }
                }


            }]

        })

    ;

})();
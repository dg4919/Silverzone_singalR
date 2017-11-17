//https://tests4geeks.com/build-angular-1-5-component-angularjs-tutorial/

(function () {
    'use strict';

    var searchBook = {
        templateUrl: '/Templates/inventory/shared/bookSearch.html',
        bindings: {
            //clastype: '<'       // 1 way binding
            clastype: '@clas',    // @ is for string parameters
            model: '='            // 2 way binding  
        },      // $ctrl is default alias name of component
        controller: ['$scope', 'sharedService', '$timeout', 'inventoryService', '$filter', function ($sc, sharedSvc, $timeout, svc, $filter) {
            //alert(this.clastype);       // to get value from parent ctrl > access using $ctrl.className on the page

            var subjects = {},
                clasess = {};

            var get_allSubject = function () {
                sharedSvc.getBook_itemTitle().then(function (data) {
                    subjects = data.result;
                    $sc.subjectList = subjects;

                    if ($sc.$parent.get_bookModel)           // for verify inventory > we need name of selected book 
                        $sc.$emit('on_emit_getBookModel', { bookModel: data.result });

                });

                $sc.book_categoryList = {};
                $sc.classList = {};
            }


            get_allSubject();

            $sc.get_class = function () {
                $sc.book_categoryList = {};

                clasess = $filter('filter')(subjects, function (entity) {
                    return entity.subject.Id === $sc.$ctrl.model.subjectId;
                })[0].subject.Class;


                $sc.classList = clasess;
            }

            $sc.get_bookCategorys = function () {
                $sc.book_categoryList = $filter('filter')(clasess, { Id: $sc.$ctrl.model.classId })[0].category;
            }


            $sc.$watch('$ctrl.model.BookId', function (n, o) {
                if (n) {
                    svc.getBookISBN(n)
                    .then(function (d) {
                        $sc.$ctrl.model.BookISBN = d.result;
                    });
                }
            });

            //***********  On  Save Data reset fields  ************ 

            $sc.$on('onSubmitData', function ($event, bookId) {
                $sc.$ctrl.model.classId = '';
            });

            //***********  On Edit Model  ************ 
            $sc.$on('on_getBookCategoy', function ($event, model) {

                $sc.$ctrl.model.subjectId = model.subjectId;

                $sc.get_class(model.subjectId);

                $sc.$ctrl.model.classId = model.classId;
                $sc.get_bookCategorys();

                $sc.$ctrl.model.BookId = model.bookId;
            });

        }]
    }

    var _bindings = {
        model: '=',
        clasType: '<',
        disabl: '='
    };

    var _srcBindings = angular.merge({}, _bindings, { showLabel: '@' });
    var _srcInfoBindings = angular.merge({}, _bindings, { srcId: '<' });
    var _table_PO_Bindings = angular.merge({}, _bindings, { poInfo: '<', formName: '<' });
    var _table_Stock_Bindings = angular.merge({}, _bindings, { stockItem: '<', showToInfo: '@' });


    angular
        .module('SilverzoneERP_invenotry_component', [])  // create a new module Here
        .component('bookSearch', searchBook)              // we can't pass injection params here like >  component('bookSearch', ['$scope', 'sharedService', searchBook]);
        .component('refreshState', {
            template: '<button type="button"                    \
                         class="btn btn-info btn-xs pull-right"\
                         style="margin-top: -13px;"            \
                         ng-click="reload()">                  \
                            <i class="fa fa-plus"></i>         \
                        Add New                                \
                      </button>',
            controller: ['$scope', '$state', 'inventoryService', function ($sc, $state) {
                $sc.reload = function () {
                    $state.reload();
                }
            }]
        })
        .component('collapseDiv', {
            template: '<i class="fa myclass fa-minus-circle"                                 \
                          ng-class="show_Addbtn ? \'fa-minus-circle\' : \'fa-plus-circle\'"  \
                          ng-click="toggleDiv($event)"                                       \
                          style="position: absolute;right: 38px;margin-top: -8px;cursor: pointer;"> </i>',
            controller: ['$scope', function ($sc, $state) {
                $sc.show_Addbtn = true;

                $sc.toggleDiv = function (event) {
                    var target = angular.element(event.currentTarget).parent().next();

                    $sc.show_Addbtn = !$sc.show_Addbtn
                    target.toggle(200);
                }

            }]
        })

        .component('printList', {
            bindings: {
                list: '<',
                type: '@'
            },
            template: '<div class="panel">                                 \
                   <div class="row">                                       \
                        <div class="col-sm-offset-1 col-sm-6">             \
                            <button type="button"                          \
                                    class="btn btn-info"                   \
                                     ng-click="print_PO()">                \
                        <i class="fa fa-print"></i> Print</button></div>   \
                        <div class="col-sm-5">                             \
                            <button type="button"                          \
                                    class="btn btn-primary"                \
                                    ng-click="send_POemail()">             \
                        <i class="fa fa-mail-forward"></i> Email </button> \
                        </div> </div> </div>',
            controller: ['$scope', '$rootScope', 'inventoryService', function ($sc, $rsc, svc) {
                var type = parseInt($sc.$ctrl.type);

                $sc.print_PO = function () {
                    if (type === 1)             // 1 = PO, 2 = inventory
                        svc.send_PO_confirmation_Email($sc.$ctrl.list, false);
                    else if (type === 2)
                        svc.send_Stock_confirmation_Email($sc.$ctrl.list, false);
                }

                $sc.send_POemail = function () {
                    if (type === 1)
                        svc.send_PO_confirmation_Email($sc.$ctrl.list, true);
                    else if (type === 2)
                        svc.send_Stock_confirmation_Email($sc.$ctrl.list, true);

                    $rsc.notify_fx('Email has sent to the user !', 'info');
                }

            }]
        })

        .component('actionIcons', {
            bindings: {        // $ctrl is default name of component otherwise define controllerAs
                entity: '<',   // $ctrl use to access value of parent component scope obj
                onUpdate: '&',
                onDelete: '&',
            },
            template: '<a href="#" ng-click="Edit($ctrl.entity)" style="margin: 0 5px;">  \
                         <i class="fa fa-edit fa-lg"></i>                                     \
                        </a>                                                                  \
                       <a href="#" class="text-danger" ng-click="Delete($ctrl.entity.Id)" \
                          style="margin: 0 5px;">                                             \
                          <i class="fa fa-trash fa-lg"></i>                                   \
                       </a>',
            controller: ['$scope', 'inventory_modalService', function ($sc, modalSvc) {
                // var ctrl = this;   // OR  $sc.$ctrl to access binding objects

                $sc.Edit = function (entity) {
                    $sc.$ctrl.onUpdate(entity);
                }

                $sc.Delete = function (Id) {
                    modalSvc.get_deleteModal()
                      .then(function (d) {
                          $sc.$ctrl.onDelete({ _id: Id });
                      });
                }

            }]
        })
        .component('dealerBookDiscount', {
            templateUrl: '/Templates/inventory/shared/dealerBookDiscount.html',
            bindings: {
                modelList: '='            // 2 way binding  > contain discount List
            },      // $ctrl is default alias name of component
            controller: ['$scope', '$rootScope', 'sharedService', function ($sc, $rsc, sharedSvc) {
                $sc.discountModel = {};
                $sc.isEdit = false;
                var editRecord = '';

                sharedSvc.get_bookCategorys()
                .then(function (d) {
                    $sc.categoryList = d.result;
                });

                $sc.addDsicount = function () {
                    if ($sc.isEdit) {
                        var discount = _.find($sc.$ctrl.modelList, function (v) {
                            if (v.category.id === editRecord)
                                return v;
                        });
                        discount.amount = $sc.discountModel.amount;
                    }
                    else {
                        var record_isExist = _.find($sc.$ctrl.modelList, function (v) {
                            return v.category.id === $sc.discountModel.category.id;
                        });

                        if (record_isExist) {
                            $rsc.notify_fx('Book Discount already exist in list', 'warning');
                            $sc.model.dealerAdress = '';
                            return;
                        }

                        $sc.$ctrl.modelList.push($sc.discountModel);
                        $sc.discountModel = {};
                    }
                }


                $sc.EditData = function (entity) {
                    $sc.discountModel.category = entity.category;
                    $sc.discountModel.amount = entity.amount;
                    $sc.isEdit = true;
                    editRecord = entity.category.id;
                }

                $sc.DeleteData = function (entity) {
                    //console.log(entity);
                    var index = _.indexOf($sc.$ctrl.modelList, entity);

                    $sc.$ctrl.modelList.splice(index, 1);
                }


            }]
        })

        .component('dealerAddress', {
            template: '<div class="col-sm-5">                                  \
                       <div class="form-group col-md-9">                       \
                       <label class="control-label col-sm-12">Secondary Address </label> \
                       <div class="col-sm-12">                                 \
                           <textarea class="form-control1"                     \
                                     ng-model="model.dealerAdress"> </textarea>\
                       </div></div>                                            \
                       <div class="form-group col-md-3" style="margin-top: 27px;"> \
                           <a href="#" class="btn btn-primary btn-sm"          \
                              ng-click="model.dealerAdress && addAdress()"     \
                              ng-disabled="!model.dealerAdress"                \
                              ng-bind="isEdit ? \'UPDATE\' : \'ADD\'">         \
                           </a>                                                \
                       </div> </div>                                           \
                       <div class="col-sm-6">                                  \
                       <table ng-if="$ctrl.modelList.length"                   \
                              class="table table-striped responsive"           \
                              wt-responsive-table>                             \
                           <thead>                                             \
                               <tr>                                            \
                                   <th>S.No.</th>                              \
                                   <th>Address</th>                            \
                                   <th>Set default</th>                        \
                                   <th>Actions</th>                            \
                               </tr>                                           \
                           </thead>                                            \
                           <tbody>                                             \
                               <tr ng-repeat="adress in $ctrl.modelList">      \
                                   <td>{{ $index+1 }}</td>                     \
                                   <td ng-bind="adress.adresName"></td>        \
                                   <td ng-class="text-center"> <a href="#">    \
                                 <i class="fa fa-check"                        \
                                    ng-click="toggleSelect(true, $index)"      \
                                    ng-hide="Adres_radio === $index"> </i>     \
                                 <i class="fa fa-close"                        \
                                    ng-click="toggleSelect(false, $index)"     \
                                    ng-show="Adres_radio === $index"> </i>     \
                                    </a> </td>                                 \
                                   <td>                                        \
                               <action-icons entity="adress"                   \
                                             on_update="EditData(adress)"      \
                                             on-delete="DeleteData(adress)">   \
                               </action-icons>                                 \
                               </td></tr></tbody></table></div></div>          ',
            bindings: {
                modelList: '='            // 2 way binding  > contain discount List
            },      // $ctrl is default alias name of component
            controller: ['$scope', '$rootScope', 'sharedService', function ($sc, $rsc, sharedSvc) {
                $sc.model = {};
                $sc.isEdit = false;
                var editRecord = '';

                $sc.addAdress = function () {

                    if ($sc.isEdit) {
                        angular.forEach($sc.$ctrl.modelList, function (v, k) {
                            if (v.adresName === editRecord)
                                v.adresName = $sc.model.dealerAdress;
                        });
                    }
                    else {

                        var record_isExist = _.find($sc.$ctrl.modelList, function (v) {
                            return v.adresName === $sc.model.dealerAdress;
                        });

                        if (record_isExist) {
                            $rsc.notify_fx('Address already exist in list', 'warning');
                            $sc.model.dealerAdress = '';
                            return;
                        }

                        $sc.$ctrl.modelList.push({ adresName: $sc.model.dealerAdress });
                    }

                    $sc.model.dealerAdress = '';
                    $sc.isEdit = false;
                }

                $sc.toggleSelect = function (isSelect, index) {
                    if (isSelect)
                        $sc.$parent.sourceInfo.defaultAdres_radio = index;
                    else
                        $sc.$parent.sourceInfo.defaultAdres_radio = null;

                    // bcoz parent scope can't access in template of component directly so taking an new internal scope obj
                    $sc.Adres_radio = $sc.$parent.sourceInfo.defaultAdres_radio;
                }

                $sc.EditData = function (entity) {
                    $sc.isEdit = true;
                    $sc.model.dealerAdress = entity.adresName;
                    editRecord = entity.adresName;
                }

                $sc.DeleteData = function (entity) {
                    var index = _.indexOf($sc.$ctrl.modelList, entity);

                    $sc.$ctrl.modelList.splice(index, 1);
                }

            }]
        })

        .component('quantity', {
            bindings: _bindings,
            template: '<div class="form-group" ng-class="$ctrl.clasType">      \
                       <label class="control-label col-sm-12">Quantity</label> \
                       <div class="col-sm-12">                                 \
                       <input type="text" name="Qty" class="form-control1"     \
                        ng-model="$ctrl.model.Quantity" only-numbers>          \
                       </div> </div>',
        })
        .component('rate', {
            bindings: _bindings,
            template: '<div class="form-group" ng-class="$ctrl.clasType">   \
                       <label class="control-label col-sm-12">Rate</label>  \
                       <div class="col-sm-12">                              \
                       <input type="text" name="rate" class="form-control1" \
                        ng-model="$ctrl.model.Rate" decimal:only>           \
                       </div> </div>',
        })
        .component('remarks', {
            bindings: _bindings,
            template: '<div class="form-group" ng-class="$ctrl.clasType">     \
                       <label class="control-label col-sm-12">Remarks</label> \
                       <div class="col-sm-12">                                \
                       <textarea name="remarks" class="form-control1"         \
                        ng-model="$ctrl.model.Remarks"                        \
                        ng-disabled="$ctrl.disabl"> </textarea>               \
                       </div> </div>',
        })
        .component('bookIsbn', {
            bindings: _bindings,
            template: '<div class="form-group" ng-class="$ctrl.clasType">       \
                       <label class="control-label col-sm-12">Book ISBN</label> \
                       <div class="col-sm-12">                                  \
                       <input type="text" name="ISBN" class="form-control1"     \
                        ng-model="$ctrl.model.BookISBN"                         \
                        ng-blur="$ctrl.model.BookISBN && getBookCategory()">    \
                       </div> </div>',
            controller: ['$scope', 'inventoryService', function ($sc, svc) {

                $sc.getBookCategory = function () {
                    svc
                        .get_bookCategory($sc.$ctrl.model.BookISBN)
                        .then(function (d) {
                            if (d.result)
                                $sc.$emit('onEmit_getBookCategoy', d.result);       // pass value to parent then child 
                        });
                }

            }]

        })
        .component('poNumber', {
            bindings: _bindings,
            template: '<div class="form-group" ng-class="$ctrl.clasType">       \
                      <label class="control-label col-sm-12">PO Number</label>  \
                      <div class="col-sm-12">                                    \
                      <input type="text" class="form-control1" name="PO_Number" \
                             ng-model="$ctrl.model.PO_Number"                   \
                             style="padding-right: 87px;">                      \
                      <span ng-show="$ctrl.model.SourceInfo_Id && $ctrl.model.InventoryType === 1" \
                            style="position: absolute; top: 13px; right: 22px; z-index: 10; font-weight: 600; font-size: 13px;"> \
                      <a href="#" ng-click="get_PendingPO()"> Pending PO </a> \
                      </span> </div> </div>',
            controller: ['$scope', '$rootScope', 'inventoryService', 'inventory_modalService', function ($sc, $rsc, svc, modalSvc) {
                $sc.inventory_sourceDetail_List = [];

                $sc.get_PendingPO = function () {

                    svc
                    .getPendingPO($sc.$ctrl.model.SourceInfo_Id)
                    .then(function (d) {

                        modalSvc
                            .get_PendingPOModal(d.result, 1)
                            .then(function (result) {       // when model will be close give a result
                                $sc.$ctrl.model.PO_Number = result.PO_Number;
                                $sc.$ctrl.model.BookISBN = result.bookISBN;
                                $sc.$emit('onEmit_getBookCategoy', result.bookId_bundle);       // pass value to roote component then its child
                            });
                    });

                }

            }]
        })

        .component('sourc', {
            bindings: _srcBindings,
            template: '<div class="form-group" ng-class="$ctrl.clasType">                                          \
                      <label for="inputEmail3" class="control-label col-sm-12">{{ $ctrl.showLabel && "TO" || "From" }}</label>  \
                      <div class="col-sm-12">                                                                      \
                      <select class="form-control1"                                                                \
                              name="from"                                                                          \
                              ng-model="$ctrl.model"                                                               \
                              ng-options="::source.Id as ::source.SourceName for source in inventory_sources"      \
                              ng-disabled="$ctrl.disabl">                                                          \
                              <option value=""> --- Select Source --- </option>                                    \
                      </select> </div></div>',
            controller: ['$scope', 'inventoryService', function ($sc, svc) {
                $sc.inventory_sources = [];

                svc.get_inventorySource().then(function (d) {
                    $sc.inventory_sources = d.result;
                });
            }]
        })
        .component('sourceInfo', {
            bindings: _srcInfoBindings,
            template: '<div class="form-group" ng-class="$ctrl.clasType">                                          \
                      <label for="inputEmail3" class="control-label col-sm-12"> Inventory Source </label>                         \
                      <div class="col-sm-12">                                                                      \
                      <select class="form-control1"                                                                \
                              name="source"                                                                            \
                              ng-model="$ctrl.model"                                                            \
                              ng-options="::source.Id as ::source.SourceName for source in inventory_sourceDetail_List"      \
                              ng-disabled="$ctrl.disabl">                                                          \
                              <option value=""> --- Select Source --- </option>                                    \
                      </select> </div></div>',
            controller: ['$scope', 'inventoryService', function ($sc, svc) {

                // for PO
                $sc.$watch('$ctrl.srcId', function (newVal, oldVal) {     // use in inventory create
                    if (newVal)     // if newval have some value > !== undefined || ''
                    {
                        var _model = {
                            Id: newVal,
                            type: 1,
                            Status: false
                        }

                        svc.get_sourceInfo(_model).then(function (d) {
                            $sc.inventory_sourceDetail_List = d.result;
                        });
                    }
                });

                // for inventory
                $sc.$watch('$parent.model.InventoryType', function (newVal, oldVal) {     // use in inventory create
                    if (newVal)     // if newval have some value > !== undefined || ''
                    {
                        var _model = {
                            Id: $sc.$parent.model.srcId,
                            type: newVal,
                            Status: false
                        }

                        svc.get_sourceInfo(_model).then(function (d) {
                            $sc.inventory_sourceDetail_List = d.result;
                        });
                    }
                });

            }]
        })

        .component('challanInfo', {
            bindings: _bindings,
            template: '<div class="form-group col-sm-3">                       \
                       <label class="control-label">Challan No</label>         \
                       <input type="text" class="form-control1"                \
                              name="callanNo"                                  \
                              ng-disabled="$ctrl.disabl"                       \
                              ng-model="$ctrl.model.ChallanNo">  </div>        \
                       <div class="col-sm-offset-1 form-group col-sm-3 date">  \
                            <label class="control-label">Challan Date</label>  \
                       <div class="date datepicker" style="display: table;">   \
                            <input type="text" class="form-control1"           \
                                   show-picker                                 \
                                   ng-disabled="$ctrl.disabl"                  \
                                   name="callanDt"                             \
                                   ng-model="$ctrl.model.ChallanDate">         \
                            <span class="input-group-addon">                   \
                                  <span class="fa fa-calendar"></span>         \
                            </span> </div> </div>  ',
        })

        .component('inventoryType', {
            bindings: _bindings,
            template: '<div class="col-sm-4">                                   \
                <span class="radio-inline">                                     \
                <input id="radio1" type="radio" class="radio"                   \
                       ng-value="1"                                             \
                       name="sourceRadio"                                       \
                       ng-model="$ctrl.model.InventoryType"                     \
                       ng-change="onRadioChange()" />                           \
                <label for="radio1" class="control-label">Inward</label></span> \
                <span class="radio-inline">                                     \
                <input id="radio2" type="radio" class="radio"                   \
                       ng-value="2"                                             \
                       name="sourceRadio"                                       \
                       ng-model="$ctrl.model.InventoryType"                     \
                       ng-change="onRadioChange()" />                           \
                <label for="radio2" class="control-label">Outward</label> </span> </div>',
            controller: ['$scope', function ($sc) {
                $sc.onRadioChange = function () {
                    $sc.$ctrl.model.SourceInfo_Id = '';
                }
            }]
        })

        .component('fromDate', {
            bindings: _bindings,
            template: '<div class=form-group" ng-class="$ctrl.clasType">    \
                    <label class="control-label">From</label>               \
                    <div class="date datepicker" style="display: table;">   \
                        <input type="text" class="form-control1"            \
                               ng-model="$ctrl.model.from" name="formDate"> \
                        <span class="input-group-addon">                    \
                            <span class="fa fa-calendar"></span>            \
                        </span> </div> </div>',
            controller: ['$scope', function ($sc) {
                $sc.$ctrl.model.from = moment(new Date()).subtract(1, 'months').format('MM/DD/YYYY');
            }]
        })

        .component('toDate', {
            bindings: _bindings,
            template: '<div class=form-group"  ng-class="$ctrl.clasType">     \
                    <label class="control-label">To</label>               \
                    <div class="date datepicker" style="display: table;"> \
                        <input type="text" class="form-control1"          \
                               ng-model="$ctrl.model.to" name="toDate">   \
                        <span class="input-group-addon">                  \
                            <span class="fa fa-calendar"></span>          \
                        </span> </div> </div>',
            controller: ['$scope', function ($sc) {
                $sc.$ctrl.model.to = moment(new Date()).format('MM/DD/YYYY');
            }]
        })

        .component('invoiceNumber', {
            bindings: _bindings,
            template: '<div class="form-group" ng-class="$ctrl.clasType">            \
                       <label class="control-label col-sm-12">Invoice Number</label> \
                       <div class="col-sm-12">                   \
                       <input type="text" class="form-control1"  \
                        ng-model="$ctrl.model"                   \
                        ng-disabled="$ctrl.disabl">              \
                       </div> </div>',
        })

        .component('tableInward', {
            bindings: _table_PO_Bindings,
            template: '<table class="table table-striped responsive" \
                   wt-responsive-table                            \
                   ng-if="$ctrl.poInfo.PO_Masters.length">        \
                   <thead><tr>                                    \
                     <th>S.No.</th>                               \
                     <th>Book</th>                                \
                     <th>PO Number</th>                           \
                     <th>Order Quantity</th>                      \
                     <th>Recieved Quantity</th>                   \
                     <th>Actions</th>                             \
                   </tr> </thead>                                 \
                   <tbody ng-repeat="poMaster in $ctrl.poInfo.PO_Masters"> \
                   <tr ng-repeat="entity in poMaster.PO_detail">  \
                     <td>{{ $index+1 }}</td>                      \
                     <td ng-bind="::entity.Book.bookName"></td>   \
                     <td ng-bind="::poMaster.PO_Number"></td>     \
                     <td ng-bind="::entity.pendingQty"></td>      \
                     <td>                                         \
                    <input type="text" class="form-control1"      \
                           placeholder="Enter Qunatity"           \
                           ng-model="entity.newQty"/>             \
                    </td><td>                                     \
                    <button type="submit"                         \
                            ng-click="createChallan($ctrl.formName, entity, poMaster.PO_Number)"  \
                            ng-disabled="!entity.newQty"                                          \
                            style="margin: 0 5px;">                                               \
                        <i class="fa fa-plus text-success"></i></button>                          \
                    </td> </tr></tbody></table>',
            controller: ['$scope', function ($sc) {

                $sc.createChallan = function (form, entity, _poNumber) {

                    var _model = {
                        stockId: 0,        // for cretae item
                        PO_Number: _poNumber,
                        Quantity: entity.newQty,
                        BookId: entity.Book.bookId_bundle.bookId
                    };
                    angular.extend($sc.$ctrl.model, _model);

                    $sc.$parent.submit_data(form);
                }
            }]
        })

        .component('tableOutward', {
            bindings: _table_PO_Bindings,
            template: '<table class="table table-striped responsive"   \
                               wt-responsive-table>                     \
                        <thead>                                         \
                          <tr>                                          \
                              <th ng-show="showChallanBtn">             \
                                <p> Select All </p>                     \
                                <input type="checkbox"                  \
                                ng-click="checkAll()" />                \
                              </th>                                     \
                              <th>S.No.</th>                            \
                              <th>Book</th>                             \
                              <th>PO number</th>                        \
                              <th>Quantity</th>                         \
                              <th>Available Stock</th>                  \
                              <th>Actions</th>                          \
                          </tr>                                         \
                        </thead>                                        \
                        <tbody ng-repeat="inventory in $ctrl.poInfo">   \
                         <tr ng-repeat="entity in inventory.PO_detail"  \
                             ng-init="entity.initial_qty = entity.Quantity" \
                             ng-style="entity.Quantity > entity.AvailableStock && { \'border\': \'2px solid red\' }"> \
                          <td ng-show="showChallanBtn">      \
                              <input type="checkbox"         \
                               ng-model="entity.isChecked"   \
                               ng-checked="entity.isChecked" \
                               ng-disabled="entity.Quantity > entity.AvailableStock"/> </td> \
                          <td>{{ $index+1 }}</td>                   \
                          <td ng-bind="::entity.bookName"></td>     \
                          <td ng-bind="::inventory.PO_Number"></td> \
                          <td>                                      \
                              <span ng-bind="entity.Quantity"       \
                                    ng-hide="parentIndex === $parent.$index && childIndex === $index && showSts"></span>  \
                              <input type="text" class="form-control1" ng-model="txtIndex[$index]"                        \
                                     ng-show="parentIndex === $parent.$index && childIndex === $index && showSts" /></td> \
                          <td ng-bind="::entity.AvailableStock"></td>                                                     \
                          <td> <a href="#" ng-click="showBtn($index, $parent.$index, true)" style="margin: 0 5px;"        \
                             ng-hide="parentIndex === $parent.$index && childIndex === $index && showSts">                \
                              <i class="fa fa-edit fa-lg"></i> </a>                                                       \
                          <a href="#" ng-click="editChallan                                                               \
                                              ? addStock($ctrl.formName, entity, inventory.PO_Number, $index, $parent.$index, false) \
                                              : showBtn($index, $parent.$index, false, false)"                            \
                                      style="margin: 0 5px;"       \
                             ng-show="parentIndex === $parent.$index && childIndex === $index && showSts">           \
                             <i class="fa fa-plus fa-lg"></i></a>                                                    \
                          <a href="#" ng-click="showBtn($index, $parent.$index, false, true)" style="margin: 0 5px;" \
                             ng-show="parentIndex === $parent.$index && childIndex === $index && showSts">           \
                             <i class="fa fa-close fa-lg"></i> </a> </td></tr></tbody></table>                       \
                          <div class="row" style="padding: 0px 20px; margin: 20px 0px;">                             \
                           <input type="button" class="pull-right btn btn-warning" ng-show="showChallanBtn"          \
                                  ng-click=createChallan($ctrl.formName) value="Create Challan" /></div>',
            controller: ['$scope', '$rootScope', 'inventory_modalService', function ($sc, $rsc, modalSvc) {
                $sc.childIndex = '';
                $sc.parentIndex = '';
                $sc.showSts = '';
                $sc.txtIndex = [];
                $sc.showChallanBtn = true;
                $sc.editChallan = $sc.$parent.editChallan;

                if ($sc.editChallan)
                    $sc.showChallanBtn = false;

                var status = false;
                $sc.checkAll = function () {
                    status = !status;

                    angular.forEach($sc.$ctrl.poInfo, function (inventory) {
                        angular.forEach(inventory.PO_detail, function (entity) {
                            if (entity.Quantity < entity.AvailableStock)
                                entity.isChecked = status;
                        });
                    });
                }

                $sc.showBtn = function (index, parentIndex, status, isCancel) {
                    $sc.childIndex = index;
                    $sc.parentIndex = parentIndex
                    $sc.showSts = status;

                    if (status)
                        $sc.txtIndex[index] = $sc.$ctrl.poInfo[parentIndex].PO_detail[index].Quantity;
                    else if (!status && !isCancel)
                        if ($sc.txtIndex[index]) {
                            if ($sc.txtIndex[index] > $sc.$ctrl.poInfo[parentIndex].PO_detail[index].initial_qty) {
                                $rsc.notify_fx('You can not Enter Qunatity more than PO Quantity', 'danger');
                                return;
                            }

                            $sc.$ctrl.poInfo[parentIndex].PO_detail[index].Quantity = parseInt($sc.txtIndex[index]);
                        }
                }

                $sc.createChallan = function (form) {

                    var _model = [];
                    angular.forEach($sc.$ctrl.poInfo, function (inventory) {
                        angular.forEach(inventory.PO_detail, function (entity) {
                            if (entity.isChecked)
                                _model.push({
                                    PO_Number: inventory.PO_Number,
                                    Quantity: entity.Quantity,
                                    BookId: entity.bookId
                                });
                        })
                    });

                    if (!_model.length) {
                        $rsc.notify_fx('Select atleast one record !', 'danger');
                        return;
                    }

                    angular.extend($sc.$ctrl.model, { stockList: _model });
                    commonMethod(form, true);
                }

                $sc.addStock = function (form, entity, _poNumber, index, parentIndex, status) {
                    $sc.showBtn(index, parentIndex, status);

                    if (entity.Quantity > entity.AvailableStock) {   // check avilable stock while outward it 
                        $rsc.notify_fx('Book is not available, Try later :(', 'warning');
                        return;
                    }

                    var _model = {
                        stockId: 0,        // for cretae item
                        PO_Number: _poNumber,
                        Quantity: entity.Quantity,
                        BookId: entity.bookId
                    };

                    angular.extend($sc.$ctrl.model, _model);
                    commonMethod(form);
                }

                function commonMethod(form, status) {

                    if ($sc.$ctrl.model.stock_mId === undefined &&
                        $sc.$ctrl.model.InventoryType === 2 &&
                        $sc.$ctrl.model.srcId === 2) {             // for dealer outward inventory

                        modalSvc.get_dealer_adresseModal($sc.$ctrl.model.SourceInfo_Id)
                                .then(function (result) {
                                    $sc.$ctrl.model.dealer_adressId = result;
                                    $sc.showChallanBtn = false;
                                    $sc.$parent.submit_data(form, status);
                                });
                    }
                    else {
                        $sc.showChallanBtn = false;
                        $sc.$parent.submit_data(form, status);   // status = true > for bulk insert
                    }
                }

            }]
        })

        .component('stockDetail', {
            bindings: _table_Stock_Bindings,
            template: '<div class="col-md-12"                                                                 \
                             style="margin-top: 16px; border-bottom: 1px solid #ddd; margin-bottom: 18px;"> \
                        <div ng-class="$ctrl.clasType" ng-if="$ctrl.showToInfo !== \'false\'">    \
                            <label class="control-label">Source: </label>                         \
                            <h4 ng-bind="$ctrl.stockItem.srcTo"> </h4></div>                      \
                        <div ng-class="$ctrl.clasType" ng-if="$ctrl.showToInfo !== \'false\'">    \
                            <label class="control-label">Name: </label>                           \
                            <h4 ng-bind="$ctrl.stockItem.SourceType"> </h4></div>                 \
                        <div class="{{ $ctrl.showToInfo === \'false\' && \'col-md-offset-2\' }}"  \
                             ng-class="$ctrl.clasType">                                           \
                            <label class="control-label">Challan No: </label>                     \
                            <h4 ng-bind="$ctrl.stockItem.ChallanNo"> </h4></div>                  \
                        <div ng-class="$ctrl.clasType">                                           \
                            <label class="control-label">Challan Date: </label>                   \
                            <h4 ng-bind="$ctrl.stockItem.ChallanDate | dateFormat: \'DD/MM/YYYY hh:mm a\'"> \
                         </h4></div></div>                                          \
                        <table ng-if="$ctrl.stockItem.stockInfo.length"             \
                               class="table table-striped responsive"               \
                               wt-responsive-table>                                 \
                            <thead>                                                 \
                                <tr>                                                \
                                    <th>S.No.</th>                                  \
                                    <th>Book</th>                                   \
                                    <th>Quantity</th>                               \
                                    <th>PO No</th>                                  \
                                    <th ng-if="!dispatchInfo && !$ctrl.stockItem.stock_isVerified">Actions</th>                                \
                                </tr>                                               \
                            </thead>                                                \
                            <tbody>                                                 \
                                <tr ng-repeat="stock in $ctrl.stockItem.stockInfo"> \
                                    <td>{{ $index+1 }}</td>                         \
                                    <td ng-bind="stock.Book.bookName"></td>         \
                                    <td>                                            \
                                    <span class="col-sm-6">                         \
                                    <span ng-bind="stock.Quantity" ng-hide="clikIndex[$index]"></span>  \
                                    <input type="text" class="form-control1" ng-model="stock.newQty"    \
                                           ng-show="clikIndex[$index]" /> </span> </td>     \
                                    <td ng-bind="stock.PO_Number"></td>                     \
                                    <td ng-if="!dispatchInfo && !$ctrl.stockItem.stock_isVerified">                                      \
                                        <a href="#" ng-click="showBtn($index, true)" style="margin: 0 5px;" ng-hide="clikIndex[$index]"> \
                                          <i class="fa fa-edit fa-lg"></i></a>                                                           \
                                        <a href="#" ng-click="submit_data(stock, $index, false)" style="margin: 0 5px;"                  \
                                           ng-show="clikIndex[$index]">                                                                  \
                                            <i class="fa fa-plus fa-lg"></i></a>                                                         \
                                        <a href="#" ng-click="showBtn($index, false, true)" style="margin: 0 5px;" ng-show="clikIndex[$index]"> \
                                            <i class="fa fa-close fa-lg"></i></a>                                            \
                                        <a href="#" class="text-danger" ng-click="Delete(stock.Id)" style="margin: 0 5px;">  \
                                            <i class="fa fa-trash fa-lg"></i></a>  \                                         \
                                </td></tr></tbody></table>                         \
                        <print-list list="$ctrl.stockItem" type="2"> </print-list> ',
            controller: ['$scope', 'inventoryService', 'inventory_modalService', function ($sc, svc, modalSvc) {
                $sc.clikIndex = [];
                $sc.txtIndex = [];

                $sc.showBtn = function (index, status, isCancel) {
                    $sc.clikIndex[index] = status;
                    var stockInfo = $sc.$ctrl.stockItem.stockInfo[index];

                    if (status)
                        stockInfo.newQty = stockInfo.Quantity;
                    else if (!status && !isCancel)
                        if (stockInfo.newQty)
                            stockInfo.Quantity = parseInt(stockInfo.newQty);
                }

                $sc.submit_data = function (entity, index, status) {
                    $sc.showBtn(index, status);

                    // changed model of parent control with required props for update  :)
                    $sc.$parent.model.stockId = entity.Id;
                    $sc.$parent.model.BookId = entity.Book.bookId_bundle.bookId;
                    $sc.$parent.model.PO_Number = entity.PO_Number;
                    $sc.$parent.model.Quantity = entity.newQty;

                    $sc.$parent.call_service();
                }

                $sc.Delete = function (Id) {
                    modalSvc.get_deleteModal()
                      .then(function (d) {
                          $sc.$parent.DeleteRecord(Id);
                      });
                }

            }]
        })

        .component('selectLocation', {
            bindings: _bindings,
            template: ' <div class="form-group col-md-4">                                                                    \
                         <label for="inputEmail3" class="control-label col-sm-12">Country</label>                         \
                         <div class="col-sm-9">                                                                           \
                             <select class="form-control1"                                                                \
                                     name="country"                                                                       \
                                     ng-model="locationModel.selected_country"                                            \
                                     ng-options="::country.CountryId as ::country.CountryName for country in countrList"  \
                                     ng-change="locationModel.selected_country && get_state()">                           \
                                                                                                                          \
                                 <option value=""> --- Select Country --- </option>                                       \
                             </select></div></div>                                                                        \
                     <div class="form-group col-md-4">                                                                    \
                         <label for="inputEmail3" class="control-label col-sm-12">State</label>                           \
                         <div class="col-sm-9">                                                                           \
                             <select class="form-control1"                                                                \
                                     name="country"                                                                       \
                                     ng-model="locationModel.selected_state"                                              \
                                     ng-options="::state.StateId as ::state.StateName for state in stateList"             \
                                     ng-change="locationModel.selected_country && get_city()">                            \
                                                                                                                          \
                                 <option value=""> --- Select State --- </option>                                         \
                             </select></div></div>                                                                        \
                     <div class="form-group col-md-4">                                                                    \
                         <label for="inputEmail3" class="control-label col-sm-12">City</label>                            \
                         <div class="col-sm-9">                                                                           \
                             <select class="form-control1"                                                                \
                                     name="city"                                                                          \
                                     ng-model="$ctrl.model.CityId"                                                               \
                                     ng-options="::city.CityId as ::city.CityName for city in cityList">                  \
                                                                                                                          \
                                 <option value=""> --- Select City --- </option>                                          \
                             </select></div></div> ',
            controller: ['$scope', '$filter', 'Service', function ($sc, $filter, locationSvc) {
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

                $sc.$on('on_getSchool_Location', function (ev, args) {
                    $sc.locationModel.selected_country = args.locationInfo.CountryId;

                    $sc.get_state();
                    $sc.locationModel.selected_state = args.locationInfo.StateId;

                    $sc.get_city();
                });

            }]
        })


    ;

})();
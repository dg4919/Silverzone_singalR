
(function () {

    var fn = function ($modal) {

        var fac = {};

        fac.get_deleteModal = function () {

            var delete_template = ' <div class="modal-header">                                         '
                                + '  <h4 class="box-title">Delete Confirmation</h4>                    '
                                + ' </div>                                                             '
                                + ' <div class="modal-body">                                           '
                                + '   <p> <h3>Are You Sure Want To Delete This Record ? </h3></p>      '
                                + ' </div>                                                             '
                                + ' <div class="modal-footer">                                         '
                                + ' <button class="btn btn-primary" ng-click="ok()">Yes</button>       '
                                + ' <span class="col-sm-1">                                            '
                                + '    <button class="btn btn-warning" ng-click="cancel()">No</button> '
                                + ' </div>  </div>                                                     '

            var modalInstance = $modal.open({
                template: delete_template,
                controller: 'modalCtrl',            // No need to specify this controller on HTML page explicitly    
                size: 'md',
            });

            return modalInstance.result;
        }

        fac.get_editeModal = function () {

            var delete_template = ' <div class="modal-header">                                         '
                                + '  <h4 class="box-title">Edit Confirmation</h4>                    '
                                + ' </div>                                                             '
                                + ' <div class="modal-body">                                           '
                                + '   <p> <h3> Record already exist, Do Want To Edit It ? </h3></p>      '
                                + ' </div>                                                             '
                                + ' <div class="modal-footer">                                         '
                                + ' <button class="btn btn-primary" ng-click="ok()">Yes</button>       '
                                + ' <span class="col-sm-1">                                            '
                                + '    <button class="btn btn-warning" ng-click="cancel()">No</button> '
                                + ' </div>  </div>                                                     '

            var modalInstance = $modal.open({
                template: delete_template,
                controller: 'modalCtrl',            // No need to specify this controller on HTML page explicitly    
                size: 'md',
            });

            return modalInstance.result;
        }

        fac.get_qtyModal = function () {

            var template = ' <div class="modal-header">                                         '
                                + '  <h4 class="box-title">Quantity Exceed Confirmation</h4>                    '
                                + ' </div>                                                             '
                                + ' <div class="modal-body">                                           '
                                + '   <p> <h4>Stock qunaity is greater than PO quantity, are you sure want to continue ?</h4></p>      '
                                + ' </div>                                                             '
                                + ' <div class="modal-footer">                                         '
                                + ' <button class="btn btn-primary" ng-click="ok()">Yes</button>       '
                                + ' <span class="col-sm-1">                                            '
                                + '    <button class="btn btn-warning" ng-click="cancel()">No</button> '
                                + ' </div>  </div>                                                     '

            var modalInstance = $modal.open({
                template: template,
                controller: 'modalCtrl',            // No need to specify this controller on HTML page explicitly    
                size: 'md',
            });

            return modalInstance.result;
        }


        fac.get_PendingPOModal = function (_data, type) {
            var template = 'result-list="modalCtrl.List"               \
                            modal-instance="modalCtrl.modal_instance">'

            var modal_template = '';

            if (type === 1) {
                modal_template = '<pending_po:modal '
                                   + template + '</pending_po:modal>';
            }
            else if (type === 2) {
                modal_template = '<pending_po-details:modal '
                                   + template + '</pending_po-details:modal>';
            }

            var modalInstance = $modal.open({
                template: modal_template,
                size: 'lg',
                controller: 'POmodal_Ctrl',// ctroler of $modal & from here pass value to component
                controllerAs: 'modalCtrl',
                resolve: {
                    result: function () { //  inject result as DI into ctrler
                        return _data;     // send value from here to controller as dependency
                    }
                }
            });
            return modalInstance.result;
        }

        fac.get_dealer_adresseModal = function (srcId) {

            var delete_template = ' <div class="modal-header">                         \
                                    <h4 class="box-title">Select Dealer Adress </h4>   \
                                    <i class="close fa fa-close" ng-click="cancel()"   \
                                       style="margin-top: -26px;"></i>                 \
                                    </div>                                             \
                                    <div class="modal-body">                           \
                                    <div class="row" style="padding: 5px;">            \
                                    <div class="col-sm-3 divHover" ng-repeat="entity in dealer_adresList"   \
                                         ng-click="selectadress(entity.adressId)">                          \
                                      <i class="fa fa-check fa-lg" ng-if="entity.isDefault"                 \
                                           style="position: absolute;margin-top: -8px;right: -11px;color: #5bc0de;"></i> \
                                    <p ng-bind="entity.adress"></p></div></div>  \
                                    </div> </div>                                '

            var modalInstance = $modal.open({
                template: delete_template,
                size: 'md',
                resolve: {
                    sourceId: function () {
                        return srcId;
                    }
                },
                controller: ['$scope', '$uibModalInstance', 'inventoryService', 'sourceId', function ($sc, $modalInstance, svc, sourceId) {
                    svc.get_dealerAdress(sourceId)
                       .then(function (d) {
                           $sc.dealer_adresList = d.result;
                       });

                    $sc.selectadress = function (adressid) {
                        $modalInstance.close(adressid);
                    }

                    $sc.cancel = function () {
                        $modalInstance.dismiss();
                    }

                }],
            });

            return modalInstance.result;
        }

        fac.get_stock_detailModal = function (list, type) {

            var delete_template = ' <div class="modal-header">                       \
                                    <h4 class="box-title">Stock ' + type + ' Detail </h4> \
                                    <i class="close fa fa-close" ng-click="cancel()" \
                                       style="margin-top: -26px;"></i>               \
                                    </div>                                           \
                                    <div class="modal-body">                         \
                                    <div class="col-sm-offset-2 col-md-6">           \
                                    <label class="control-label">Source: </label>    \
                                    <h4 ng-bind="stockList.BookName"> </h4></div>    \
                                    <div class="row" style="padding: 5px;">          \
                                    <div ng-if="stockList.stockInfo.length">         \
                                    <table class="table table-striped responsive"    \
                                           wt-responsive-table>                      \
                                    <thead>                                          \
                                            <tr>                                     \
                                                <th>S.No.</th>                       \
                                                <th>PO Number</th>                   \
                                                <th>PO Date</th>                     \
                                                <th>Challan Number</th>              \
                                                <th>Challan Date</th>                \
                                                <th>Qunatity</th>                    \
                                            </tr>                                    \
                                    </thead>                                         \
                                    <tbody>                                          \
                                        <tr ng-repeat="entity in stockList.stockInfo">  \
                                          <td>{{ $index+1 }}</td>                       \
                                          <td ng-bind="::entity.PO_Number"></td>        \
                                          <td ng-bind="::entity.PO_Date | dateFormat: \'DD/MM/YYYY hh:mm a\'"></td>    \
                                         <td ng-bind="::entity.ChallanNo"></td>                                        \
                                         <td ng-bind="::entity.ChallanDate | dateFormat: \'DD/MM/YYYY hh:mm a\'"></td> \
                                         <td ng-bind="::entity.Quantity"></td> \
                                     </tr></tbody></table></div></div> </div>  '

            var modalInstance = $modal.open({
                template: delete_template,
                size: 'lg',
                resolve: {
                    stockList: function () {
                        return list;
                    }
                },
                controller: ['$scope', '$uibModalInstance', 'stockList', function ($sc, $modalInstance, stockList) {
                    $sc.stockList = stockList;

                    $sc.cancel = function () {
                        $modalInstance.dismiss();
                    }

                }],
            });

            return modalInstance.result;
        }

        fac.get_invoice_detailModal = function (_model) {

            var delete_template = ' <div class="modal-header">                       \
                                    <h4 class="box-title">Invoice Detail </h4>       \
                                    <i class="close fa fa-close" ng-click="cancel()" \
                                       style="margin-top: -26px;"></i>               \
                                    </div>                                           \
                                    <div class="modal-body">                         \
                                    <div class="col-sm-offset-2 col-md-6">           \
                                    <label class="control-label">Source: </label>    \
                                    <h4 ng-bind="stockList.SourceName"> </h4></div>  \
                                    <div class="row" style="padding: 5px;">          \
                                    <div ng-if="stockList.invoiceInfo.length">       \
                                    <table class="table table-striped responsive"    \
                                           wt-responsive-table>                      \
                                    <thead>                                          \
                                            <tr>                                     \
                                                <th>S.No.</th>                       \
                                                <th>Invoice Number</th>              \
                                                <th>Invoice Date</th>                \
                                                <th>Total Amount</th>                \
                                            </tr>                                    \
                                    </thead>                                         \
                                    <tbody>                                          \
                                        <tr ng-repeat="entity in stockList.invoiceInfo"> \
                                          <td>{{ $index+1 }}</td>                        \
                                          <td ng-bind="::entity.Invoice_No"></td>        \
                                          <td ng-bind="::entity.Invoice_Date | dateFormat: \'DD/MM/YYYY hh:mm a\'"></td>    \
                                          <td> <a href="#"                               \
                                                  ng-bind="::entity.totalPrice"          \
                                                  ng-click="printinvoice(entity.Invoice_Id)">       \
                                          </a></td>                                      \
                                     </tr></tbody></table></div></div> </div>  '

            var modalInstance = $modal.open({
                template: delete_template,
                size: 'md',
                resolve: {
                    model: function () {
                        return _model;
                    }
                },
                controller: ['$scope', '$uibModalInstance', 'inventoryService', 'model', function ($sc, $modalInstance, svc, model) {
                    $sc.stockList = {};

                    svc.get_invoiceDetail(model)
                       .then(function (d) {
                           $sc.stockList = d.result[0];
                       });

                    $sc.printinvoice = function (id) {
                        svc.get_invoicePrintInfo(id)
                        .then(function (d) {
                            svc.print_challan(d.result);
                        });
                    }

                    $sc.cancel = function () {
                        $modalInstance.dismiss();
                    }

                }],
            });

            return modalInstance.result;
        }

        fac.getCustomer_InfoModal = function (totalAmt) {

            var delete_template = ' <div class="modal-header">                                         '
                                + '  <h4 class="box-title">Customer Detail</h4> </div>                 '
                                + ' <div class="modal-body">                                           '
                                + ' <form name="create_inventorySourceForm" ng-submit="submit_data(create_inventorySourceForm)"  '
                                + ' ng-validate="validationOptions" novalidate="novalidate">           '
                                + ' <div class="row">                                                  '
                                + ' <div class="form-group col-md-3">                                  '
                                + ' <label class="control-label col-sm-12">Customer Name</label>       '
                                + ' <div class="col-sm-12">                                            '
                                + '     <input type="text" name="name" class="form-control1" ng-model="usrInfo.name">'
                                + ' </div> </div>                                                      '
                                + ' <div class="form-group col-md-3">                                  '
                                + ' <label class="control-label col-sm-12">Address</label>             '
                                + ' <div class="col-sm-12">                                            '
                                + '   <textarea name="address" class="form-control1" ng-model="usrInfo.address"> </textarea>'
                                + ' </div> </div>                                                      '
                                + ' <div class="form-group col-md-3">                                  '
                                + ' <label class="control-label col-sm-12">Mobile</label>              '
                                + ' <div class="col-sm-12">                                            '
                                + '     <input type="text" name="mobile" class="form-control1" ng-model="usrInfo.mobile">'
                                + ' </div> </div>                                                      '
                                + ' <div class="form-group col-md-3">                                  '
                                + ' <label class="control-label col-sm-12">Email Id</label>            '
                                + ' <div class="col-sm-12">                                            '
                                + '     <input type="text" name="email" class="form-control1" ng-model="usrInfo.emailId">'
                                + ' </div> </div> </div>                                               '
                                + ' <div class="row" style="margin-top: 35px;margin-bottom: 25px;">    '
                                + ' <div class="col-sm-4"> </div> <div class="col-sm-4"> </div>        '
                                + ' <div class="col-sm-4">                                             '
                                + ' <div class="pull-right">                                           '
                                + ' <div class="col-sm-12"> <h4>Pay Through</h4>                      '
                                + '  <label class="radio-inline"><input type="radio" name="payMode" ng-value="1" ng-model="usrInfo.PaymentMode" class="radio"> Cash</label> <br /> '
                                + '  <label class="radio-inline"><input type="radio" name="payMode" ng-value="2" ng-model="usrInfo.PaymentMode" class="radio"> Credit Card</label> '
                                + ' </div>                                                                      '
                                + ' <div class="col-sm-12" style="margin-top: 10px;">                           '
                                + ' <b>Total Payble Amount : </b> {{ totalAmt }}  '
                                + ' <div ng-show="usrInfo.PaymentMode === 1"> <div class="col-sm-12" style="padding: 0px !important;margin: 10px 0px;">   '
                                + ' <div class="col-sm-5" style="margin-top: 5px;"> Paid Amount : </div>        '
                                + ' <input type="text" class="form-control1" ng-model="paidAmt" style="width: 146px;position: absolute;margin-left: 10px;margin-top: 5px;" /> </div>       '
                                + ' <b>Balance : </b> {{ paidAmt - totalAmt }} </div>                  '
                                + ' </div> </div> </div> </div> </div> </form> </div>                  '
                                + ' <div class="modal-footer">                                         '
                                + ' <button class="btn btn-primary" ng-click="ok()">Yes</button>       '
                                + ' <span class="col-sm-1">                                            '
                                + '  <button class="btn btn-warning" ng-click="cancel()">No</button>   '
                                + ' </div> </div>                                                      '

            var modalInstance = $modal.open({
                template: delete_template,
                size: 'lg',
                resolve: {
                    total_paybleAmt: function () {
                        return totalAmt;
                    }
                },
                controller: ['$scope', '$uibModalInstance', 'inventoryService', 'total_paybleAmt', function ($sc, $modalInstance, svc, price) {
                    $sc.totalAmt = price;
                    $sc.usrInfo = {};

                    $sc.ok = function () {
                        modalInstance.close($sc.usrInfo);
                    }

                }]
            });

            return modalInstance.result;
        }


        return fac;
    }

    var modal_Fn = function ($sc, $modalInstance) {

        $sc.ok = function () {
            $modalInstance.close();
        }

        $sc.cancel = function () {
            $modalInstance.dismiss();
        }
    }

    var POmodal_Fn = function ($modalInstance, result) {

        this.modal_instance = $modalInstance;      // scope objects
        this.List = result;
    }

    angular.module('SilverzoneERP_invenotry_service')
    .factory('inventory_modalService', ['$uibModal', fn])
    .controller('modalCtrl', ['$scope', '$uibModalInstance', modal_Fn])
    .controller('POmodal_Ctrl', ['$uibModalInstance', 'result', POmodal_Fn])
    ;

})();


(function () {
    'use strict';

    coupons_list_ControllerFn.$inject = ['$scope', '$rootScope', '$filter', 'admin_bookCategory_Service', '$uibModal', 'modalService']
    function coupons_list_ControllerFn($sc, $rsc, $filter, svc, $modal, modalSvc) {
        $sc.couponsList = [];

        $rsc.getCoupons = function () {
            svc.get_Coupons().then(function (d) {     // d is a promise object
                $sc.couponsList = d.result;
            });
        }
        $rsc.getCoupons();

        $sc.show_create_copuonModal = function () {
            showModal();
        }

        $sc.Show_editModal = function (entity) {
            entity.Start_time = $filter('dateFormat')(entity.Start_time, 'MM/DD/YYYY');
            entity.End_time = $filter('dateFormat')(entity.End_time, 'MM/DD/YYYY');

            showModal(entity);
        }

        function showModal(_model) {
            var Template =' <div class="modal-header" style="padding: 10px !important;">                                                                                             '
                         + ' <h4 class="box-title">Add Coupon</h4></div>                                                                                                             '
                         + ' <div class="modal-body">                                                                                                                                '
                         + ' <form class="form-horizontal" name="couponForm" ng-submit="ok(couponForm)" ng-validate="validationOptions">                                             '
                         + ' <div class="form-group"> <div class="col-sm-6">                                                                                                         '
                         + ' <input type="text" class="form-control" placeholder="Coupon Name" ng-model="coupon.Coupon_name" name="Coupon_name">                                     '
                         + ' </div><div class="col-sm-6">                                                                                                                            '
                         + ' <input type="text" class="form-control" placeholder="Coupon Amount" ng-model="coupon.Coupon_amount" name="Coupon_amount" number-only>                   '
                         + ' </div> </div> <div class="form-group">                                                                                                                  '
                         + ' <div class="col-sm-6">                                                                                                                                  '
                         + ' <textarea rows="3" class="form-control ng-empty" placeholder="Coupon Description" ng-model="coupon.Description" name="Description"></textarea>          '
                         + ' </div><div class="col-sm-6" style="padding-top: 10px;">                                                                                                 '
                         + ' <span style="font-size: larger;padding: 7px;">                                                                                                          '
                         + ' <input type="radio" name="Coupon_type" ng-model="coupon.DiscountType" value="1"> Flat Discount</span>                                                   '
                         + ' <div style="font-size: larger;padding: 7px;"> <input type="radio" name="Coupon_type" ng-model="coupon.DiscountType" value="2"> Percentage Discount</div>'
                         + ' </div></div> <div class="form-group"><div class="col-md-6"><div class="input-group date" data-provide="datepicker">                                     '
                         + ' <input type="text" class="form-control" ng-model="coupon.Start_time" placeholder="Start Date" name="Start_time">                                        '
                         + ' <span class="input-group-addon"><span class="fa fa-calendar"></span></span></div></div>                                                                 '
                         + ' <div class="col-md-6"><div class="input-group date" data-provide="datepicker">                                                                          '
                         + ' <input type="text" class="form-control" ng-model="coupon.End_time" placeholder="End Date" name="End_time">                                              '
                         + ' <span class="input-group-addon"><span class="fa fa-calendar"></span></span></div></div></div>                                                           '
                         + ' <div class="modal-footer"><button type="submit" class="btn btn-primary">Save</button>                                                                   '
                         + ' <button type="button" class="btn btn-warning" ng-click="cancel()">Cancel</button></div> </form>                                                                       '

            var modalInstance = $modal.open({
                template: Template,
                controller: 'couponModal',            // No need to specify this controller on page explicitly    
                size: 'md',
                resolve: {
                    entityModel: function () {
                        return _model;               // send value from here to controller as dependency
                    }
                }
            });
        }

        $sc.changeCallback = function (coupon) {
            svc.delete_coupon(coupon).then(function () {
                $rsc.notify_fx('Coupon Status has changed !', 'info');
            });
        }

        $sc.Show_deleteModal = function (Id) {
            var couponId = Id;
            modalSvc.deleteModal('Coupon').then(function (data) {
                // TO DO > call delete coupon Svc
            });
        }

    }

    var couponModal_Controllerfn = function ($sc, $rsc, svc, $modalInstance, model) {   // model is pass by resolve() of > $modal.open() with parameter as DI
        $sc.coupon = model || {};

        if (!$sc.coupon.Id)     // for Add a Coupon > Id won't be there 
            $sc.coupon.DiscountType = 1;   // default value set > for coupon type

        $sc.ok = function (form) {
            if (form.validate()) {

                if ($sc.coupon.Id) {           // update coupon
                    svc.update_coupon($sc.coupon).then(function (d) {     // d is a promise object
                        if (d.result === 'Success') {
                            $rsc.notify_fx('Coupon has been updated !', 'info');
                            $rsc.getCoupons();
                            $modalInstance.close();
                        }
                        else
                            $rsc.notify_fx('Coupon already exist !', 'error');
                    });
                }
                else {           // create coupon
                    svc.create_Coupon($sc.coupon).then(function (d) {     // d is a promise object
                        if (d.result === 'Success') {
                            $rsc.notify_fx('Coupon has been created !', 'success');
                            $rsc.getCoupons();
                            $modalInstance.close();
                        }
                        else
                            $rsc.notify_fx('Coupon already exist !', 'error');
                    });
                }
            }
        }

        $sc.cancel = function () {
            $modalInstance.dismiss();
        }

        $sc.validationOptions = {
            rules: {
                Coupon_name: {  // use with name attribute in control
                    required: true
                },
                Description: {
                    required: true
                },
                Coupon_amount: {
                    required: true
                },
                Start_time: {
                    required: true
                },
                End_time: {
                    required: true
                }
            },
            messages: {
                Coupon_name: {            // use with name attribute in control
                    required: "Please enter coupon code !"
                },
                Description: {
                    required: "Please enter desccription !"
                },
                Coupon_amount: {
                    required: "Please enter coupon amount !"
                },
                Start_time: {
                    required: "Please enter coupon start time !"
                },
                End_time: {
                    required: "Please enter coupon end time !"
                }
            }
        }

    }

    angular.module('Silverzone_app')
    .controller('coupons_list', coupons_list_ControllerFn)
    .controller('couponModal', ['$scope', '$rootScope', 'admin_bookCategory_Service', '$uibModalInstance', 'entityModel', couponModal_Controllerfn]);

    ;

})();
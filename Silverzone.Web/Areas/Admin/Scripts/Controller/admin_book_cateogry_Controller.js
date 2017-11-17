/// <reference path="C:\tfs\Silverzone\Silverzone\Silverzone.Web\Scripts/Lib/jquery/jquery-1.10.2.js" />

// use app variable as local scope, i.e can access inside this JS only
(function (app) {
    'use strict';

    // common method to show notificaction on succes of CRUD operations
    /* @svc is injectable service to show notification */
    /* @text use to show what type of CRUD operation is done */
    /* @_type use to show different type of notification according to CRUD operation */
    function notify_fx(svc, Text, _type) {
        svc.notify({
            title: 'Book Category',
            title_escape: false,
            text: Text,
            text_escape: false,
            styling: "bootstrap3",
            type: _type,
            icon: true,
        });
    }

    // 1st way > use fx and pass parameter using $inject
    bookCategory_list_Controllerfn.$inject = ['$scope', '$rootScope', '$uibModal', '$compile', 'admin_bookCategory_Service'];

    function bookCategory_list_Controllerfn($sc, $rsc, $modal, $compile, bookSvc) {
        //  suppose, if we dont use it & Pass blank value from UI, then these property does not made
        $sc.book_category_advanceSerach = { name: '', status: false };

        // $scope object contains object/properties/methods of this/current ctler
        // $tootscope object contains object/properties/methods which can access any where in other ctrler

        $rsc.get_all_bookCategory = function () {
            bookSvc.get_BooksCategory().then(function (d) {     // d is a promise object
                $rsc.book_category_list = d.result;

                if (d.result.length > 0) {
                    bind_advanceSearch_HTML();
                }

                //console.log('in sucess   ' + d.result);
                //console.log(moment(d.result[1].CreationDate).format('DD/MM/YYYY hh:mm a'));
            },
            function () {
                console.log('in error');
            });
        }

        $rsc.get_all_bookCategory();

        function bind_advanceSearch_HTML() {
            // clear data while advance search
            $sc.book_category_advanceSerach = { name: '', status: true };

            // bind HTML after 1 second
            setTimeout(function () {
                var $el = '<div id="advance_search_Box" class="col-md-6 pull-right" style="padding: 3px;margin: 3px;padding-left: 79px; display: none;">'
                          + ' <div class="col-md-6"><input type="search" class="form-control" placeholder="Book Category Name" ng-model="book_category_advanceSerach.name"> </div>'
                          + ' <div class="col-md-2"><span> <switch class="green" ng-model="book_category_advanceSerach.status"> </switch> </span> </div>'
                          + ' <div class="col-md-4"> <input type="button" value="Search" class="btn btn-info btn-sm" ng-click="search()" />'
                          + ' <span style="margin-left: 5px;"> <input id="srch_btnClose" type="button" value="Close" class="btn btn-danger btn-sm"></span></div> </div>';

                // $compile enable use to use angular component like ng-click, ng-model
                $('.dataTables_filter').after($compile($el)($sc));
            }, 1000);       // set Time out finish aftr 1 secnd

            // jquery code
            $('.table').on('click', '#advance_search_btn', function () {
                $('.dataTables_filter').hide();
                $('#advance_search_Box').fadeIn();
            });

            $('.table').on('click', '#srch_btnClose', function () {
                $('.dataTables_filter').fadeIn();
                $('#advance_search_Box').hide();
            });

        }

        $sc.search = function () {
            debugger;

                bookSvc.advanceFilter_BooksCategory($sc.book_category_advanceSerach).then(function (d) {     // d is a promise object
                    $rsc.book_category_list = d.result;
                    bind_advanceSearch_HTML();
                },
                function () {
                    console.log('in error');
                });
        }

        // use to show modal on click of edit btn of Book category list
        $sc.Show_editModal = function (model) {
            debugger;

            // if we direct pass model parameter then it changes to in it, which is wrong
            var _model = angular.copy(model);

            var modalInstance = $modal.open({
                templateUrl: 'Areas/Admin/templates/modal/book_categoryEdit.html',
                controller: 'bookCategory_edit',            // No need to specify this controller on page explicitly    
                size: 'md',
                resolve: {
                    book_categoryModel: function () {
                        return _model;               // send value from here to controller as dependency
                    }
                }
            });
            modalInstance.result.then(function () {
                //on ok button press 
            }, function () {
                //on cancel button press
                console.log("Modal Closed");
            });
        }

        $sc.Show_deleteModal = function (RowIndex) {

            var delete_template = '  <div class="modal-header">                                                           '
                                + '   <h4 class="box-title">Delete Books Category</h4>                                    '
                                + '  </div>                                                                               '
                                + '  <div class="modal-body">                                                             '
                                + '  <p> </p><h3>Are You Sure Want To Delete This Record ? </h3><p></p>                   '
                                + '  </div>                                                                               '
                                + '  <div class="modal-footer">                                                           '
                                + '     <button class="btn btn-primary" ng-click="submit_data(book_category)">Yes</button>'
                                + '     <button class="btn btn-warning" ng-click="cancel()">No</button>                   '
                                + '  </div>                                                                               '

            var modalInstance = $modal.open({
                template: delete_template,
                controller: 'bookCategory_delete',            // No need to specify this controller on HTML page explicitly    
                size: 'sm',
                resolve: {
                    row_index: function () {
                        return RowIndex;               // send value from here to controller as dependency using row_index as fx
                    }
                }
            });
            modalInstance.result.then(function () {
                //on ok button press 
            }, function () {
                //on cancel button press
                console.log("Modal Closed");
            });
        }
      
    }

    // 2nd way > use fx variable and use it with app.Controller and pass dependency arguments
    var bookCategory_edit_Controllerfn = function ($sc, $rsc, $modalInstance, book_categoryModel, notificationService, bookSvc) {

        // edited book category record
        $sc.book_category = book_categoryModel;

        $sc.couponList = {};
        bookSvc.get_CouponName().then(function (d) {
            $sc.couponList = d.result;
        });

        $sc.submit_data = function (bookCategory_model) {
            debugger;
            bookSvc.update_BooksCategory(bookCategory_model).then(function (d) {

                // to show notification
                if (d.result === 'Success') {
                    // get all list
                    $rsc.get_all_bookCategory();

                    notify_fx(notificationService, 'Book category is updated succesfully !', 'warning');

                    // after update record, model is closed
                    $modalInstance.close();
                    console.log('close popup');

                }
                else {
                    notify_fx(notificationService, 'Book category name is already exist.', 'error');
                }

            },
            function () {
                console.log('in error');
            });
        }

        $sc.cancel = function () {
            $modalInstance.dismiss();
            console.log('cancel popup');
        }

    }

    var bookCategory_delete_Controllerfn = function ($sc, $rsc, $modalInstance, index, notificationService, bookSvc) {
        var book_categoryId = $rsc.book_category_list[index].Id;

        $sc.submit_data = function () {
            bookSvc.delete_BooksCategory(book_categoryId).then(function (d) {
                debugger;

                // after update record, model is closed
                $modalInstance.close();
                console.log('close popup');

                // remove client side data > not fetching from server side
                //$rsc.book_category_list.splice(index, 1);

                // getting all list
                $rsc.get_all_bookCategory();

                // show notification             
                notify_fx(notificationService, 'Book category is deleted succesfully !', 'error');

            },
            function () {
                console.log('in error');
            });
        }

        $sc.cancel = function () {
            $modalInstance.dismiss();
            console.log('cancel popup');
        }
    }

    var bookCategory_create_Controllerfn = function ($sc, bookSvc, notificationService) {
        $sc.book_category = {};

        $sc.couponList = {};
        bookSvc.get_CouponName().then(function (d) {
            $sc.couponList = d.result;
        });

        $sc.submit_data = function (form) {
            //debugger;

            if (form.validate()) {
                bookSvc.create_BooksCategory($sc.book_category).then(function (d) {

                    // to show notification
                    if (d.result === 'Success') {
                        // form & field will be reset
                        $sc.book_category = {};

                        notify_fx(notificationService, 'Book category is created succesfully !', 'success');
                    }
                    else {
                        notify_fx(notificationService, 'Book category name is already exist.', 'error');
                    }
                },
                function () {
                    console.log('in error');
                });
            }
        }

        $sc.validationOptions = {
            rules: {
                category_name: {            // use with name attribute in control
                    required: true,
                    maxlength: 100
                },
                category_description: {
                    required: true,
                }
            }
        }

    }


    angular.module('Silverzone_app')
        .controller('bookCategory_list', bookCategory_list_Controllerfn)
        .controller('bookCategory_edit', ['$scope', '$rootScope', '$uibModalInstance', 'book_categoryModel', 'notificationService', 'admin_bookCategory_Service', bookCategory_edit_Controllerfn])
        .controller('bookCategory_delete', ['$scope', '$rootScope', '$uibModalInstance', 'row_index', 'notificationService', 'admin_bookCategory_Service', bookCategory_delete_Controllerfn])
        .controller('bookCategory_create', ['$scope', 'admin_bookCategory_Service', 'notificationService', bookCategory_create_Controllerfn])
    ;

})(angular.module('Silverzone_app'));
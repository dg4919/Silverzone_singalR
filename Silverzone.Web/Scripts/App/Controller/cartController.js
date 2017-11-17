/// <reference path="../../Lib/jquery/jquery-1.10.2.js" />

(function (app) {
    'use strict';

    // global variable use to share value b/w 2 ctrler > these var r use to maintain sessions 
    var country_code = 0;
    var selected_shipping_Adress = {};
    var user_Addres_array = [];
    var orderId = 0;
    var is_paymentDone = false;
    var payment_mode = 0;                   // 1 for offline & 2 for online payment
    var paymentProceed_btn_isClicked = false;
    
    // we can use controller with as keyword when there is ctrler inside another/multiple ctrler
    // we can use as keyword either by setting in URL config (.state) OR on UI page with ng-controller Tag
    var cartDetailsControllerfn = function ($rsc, $modal, $state, $stateParams, svc, $filter) {
        var that = this;            // this object reffers to object of current controller fx like scope      

        //   **************  START > Code to select Country poup  *********************

        that.country_type = country_code;

        // show popup to select country
        that.cart_proceedModal = function () {

            var template = ' <div class="modal-header">                                                              '
                         + ' <h4 class="box-title">Select Your Country</h4>                                          '
                         + '<button type="button" class="close" style="margin-top: -30px !important;">               '
                         + '<span aria-hidden="true" ng-click="cancel()">×</span></button> </div>                    '
                         + ' <div class="modal-body" style="padding: 30px !important;">                              '
                         + ' <label class="radio-inline col-sm-6"> <input type="radio" name="optradio" ng-model="country_type" value="1">India</label> '
                         + ' <label class="radio-inline"><input type="radio" name="optradio" ng-model="country_type" value="2">Outside India</label> </div>'
                         + ' <div class="modal-footer">                                                              '
                         + ' <button class="btn btn-info" ng-click="submit_data(country_type)">OK</button>  </div>   '

            var modalInstance = $modal.open({
                template: template,
                size: 'sm',
                controller: 'cart_ProceedController',     // point to current ctrler
            });
        }

        //   **************  End  *********************


        // this ctrler use on UI with as keyword
        // this points to property/method of current ctrler
        that.removeItem = function (index) {
            // at which position of array, how much element want to remove
            $rsc.buyBook_isDisable[$rsc.cart.Items[index].bookId] = false;

            $rsc.cart.Items.splice(index, 1);
            $rsc.notify_fx('Item is removed from your cart !', 'danger');
        }

        that.goto_bookDetail = function (_bookId) {
            $state.go('book_details', { bookId: _bookId });
        }


        // array > contain new list in rootscope > cartItemsList
        // $watch use to track changes items into Cart List > and update values in total purchase amt
        $rsc.$watch('cart.Items', function (array) {
            //alert('add items to cart');
            var cart_Amount = 0;
            var item_qty = 0;

            angular.forEach($rsc.cart.Items, function (array, key) {
                cart_Amount += parseFloat(array.bookTotalPrice);
                item_qty += parseInt(array.bookQty);
            });

            $rsc.cart.shipping_Amount = cart_Amount;
            $rsc.cart.shipping_Charges = calculate_shipping_charges(item_qty);

            $rsc.cart.total_Amount = $rsc.cart.shipping_Amount + $rsc.cart.shipping_Charges;

        }, true);

        function calculate_shipping_charges(item_qty) {
            if (item_qty !== 0) {
                if (country_code === 1) {
                    return (40 + (item_qty - 1) * 25);
                }
                else {
                    return (1200 + (item_qty - 1) * 200);
                }
            }
            else {
                return 0;
            }
        }

        //   **************  Cart Summary Code start from Here  *********************

        // whenever this ctrl will call 'selected_shipping_Adress' will be undefine aso we will get value rom global variable always
        selected_shipping_Adress = $stateParams.shipping_adress ? $stateParams.shipping_adress : selected_shipping_Adress;      // check $stateParams.shipping_adress contain value or not
        that.selected_shipping_Adress = selected_shipping_Adress;       // reserve previous value while redirects

        that.addCartqty = function (isAdd_qty, index) {
            var book = $rsc.cart.Items[index];

            var qty = parseInt(book.bookQty);

            // true for Add Qty 
            if (isAdd_qty && qty < 99)
                updateCart(book, qty + 1);
            else if (!isAdd_qty && qty > 1)
                updateCart(book, qty - 1);
            else if (isAdd_qty)
                $rsc.notify_fx('You can add only 99 quantity of a product in your cart !', 'warning');
        }

        that.updateQty = function (index) {
            var book = $rsc.cart.Items[index];
            var item_qty = parseInt(book.bookQty);

            if (item_qty < 1 || isNaN(item_qty)) {
                updateCart(book, 1);
            }
            else {
                updateCart(book, item_qty);
            }
        }


        function updateCart(bookInfo, itemQty) {
            bookInfo.bookTotalPrice = bookInfo.bookPrice * itemQty;
            bookInfo.bookQty = itemQty;
        }

        // *********************   add order info to the DB > At payment method page  ******************

        if ($stateParams.paymentProceed_btn_isClicked)
            paymentProceed_btn_isClicked = true;

        // when user click proceed button & redirect to payment method page
        if ($state.is('payment_method') && $stateParams.paymentProceed_btn_isClicked) {
            var model = {           // carete a model same as our view model > pass in ajax
                Shipping_addressId: selected_shipping_Adress.Id,
                Total_Shipping_Amount: $rsc.cart.shipping_Amount,
                Total_Shipping_Charges: $rsc.cart.shipping_Charges,
                bookViewModel: getBooksInfo()           // return array
            }

            svc.insert_orderInfo(model).then(function (data) {

                if (data.result === 'success') {
                    is_paymentDone = false;
                    orderId = data.orderId;
                    $rsc.cart.quizPoints = data.quizPoints;
                }
                else
                    $rsc.notify_fx('Something went wrong.. :(', 'danger');

            }, function () {
                console.log('in error');
            });
        }      // when payment is done & go on 'payment_summary' page > then disable back page navigation of back btn of browser
        // URL state resolver
        else if (($state.is('payment_method') || $state.is('payment_summary')) && is_paymentDone) {
            goTohomeFn();
        }
        else if ($state.is('cart_summary') && paymentProceed_btn_isClicked) {
            $state.go('payment_method');
        }
        else if ($state.is('cart_detail') && $rsc.cart.Items.length === 0) { $state.go('empty_cart'); }

        function getBooksInfo() {
            var books = [];

            angular.forEach($rsc.cart.Items, function (data, key) {
                books.push({ BookId: data.bookId, Quantity: data.bookQty, unitPrice: data.bookPrice, bookType: data.bookType });
            });

            return books;
        }

        // *********************   for order summary page  ******************

        // we r display cart sumry & payment sumry after payment done on a single view of 'order_summary'
        that.Is_paymentDone_online = false;
        that.Is_paymentDone_offline = false;

        // to check if value exist for > order_summary page     >  to ensure user is coming by clik of button
        if ($stateParams.payment_mode !== undefined && $state.is('payment_summary') && !is_paymentDone && paymentProceed_btn_isClicked) {
            payment_mode = $stateParams.payment_mode ? $stateParams.payment_mode : payment_mode;

            if (payment_mode === 1) {      // 1 > if payment choose offline, otherwise online
                that.Is_paymentDone_online = true;
            }
            else if (payment_mode === 2) {
                that.Is_paymentDone_offline = true;
                confirmOrder();
            }
        }

        function confirmOrder() {
            var deduction = $rsc.cart.quizPoint_isChecked ? parseInt($rsc.cart.quizPoints/10) : null;

            svc.confirmOrder(orderId, payment_mode, deduction).then(function (data) {

                if (data.status === 'success') {
                    that.order_confirmData = data.result;

                    var model = {           // create a model same as our view model > pass in ajax
                        cartInfo: $rsc.cart.Items,
                        cart_shipping_Amount: $rsc.cart.shipping_Amount,
                        cart_shipping_Charges: $rsc.cart.shipping_Charges,
                        cart_total_Amount: $rsc.cart.total_Amount,
                        Quiz_Points_Deduction: deduction,
                        shipping_adress: selected_shipping_Adress,
                        order_confirmData: data.result,     // contains orderNumber & date
                        orderType: payment_mode
                    }

                    // send confirmation email to user after order is placed.
                    svc.order_confirmation_Emil(model);
                    $rsc.notify_fx('Your order is sucessfully placed.. :)', 'success');
                }
                else if (data.status === 'notfound' || data.status === 'error') {      // if orderId not found
                    $state.go('shoppingError');       // page will be refresh
                    $rsc.notify_fx('Sorry, we are unable to process this requet :(', 'error');
                }
                else if (data.status === 'notmatched') {
                    $state.go('shoppingError');       // page will be refresh
                    $rsc.notify_fx('Book Prices has changed in the mean time.', 'error');
                }

                // clear CART state
                is_paymentDone = true;
                paymentProceed_btn_isClicked = false;

                $rsc.Newshopping = true;

            }, function (data) {
                console.log('in error');
            });
        }

        that.goTohome = function () {
            goTohomeFn();
        }

        function goTohomeFn() {
            $state.go('book_list');

            that.selected_shipping_Adress = {};
        }

    }

    var cart_ProceedControllerfn = function ($sc, $modalInstance, $state) {
        $sc.submit_data = function (country_type) {
            //alert('data is submit, cntry code is :  ' + country_type);

            if (country_type !== undefined) {
                country_code = parseInt(country_type);      // otherwise country_type = "1" instead of country_type=1

                // redirect to viewCart page
                $state.go('cart_detail');

                // close current opened popup > that is login modal
                $modalInstance.dismiss();
            }
            else {
                alert('Select your country name !')
            }
        }

        $sc.cancel = function () {
            $modalInstance.dismiss();
            console.log('cancel popup');
        }
    }

    var user_addressControllerfn = function ($sc, $rsc, $modal, svc, $filter, $state) {

        $sc.user_adressList = [];  // contains array

        // use to show select button for user adress list while checkOut cart
        $sc.show_selectAdressBtn = $state.is('user_profileAdress') ? false : true;

        $rsc.get_all_Usershipping_Address = function () {
            svc.get_shipping_Address().then(function (data) {
                // order by records in descending
                user_Addres_array = data.result;

                var result = $filter('orderBy')(user_Addres_array, '-create_date');
                $sc.user_adressList = chunk(result, 3);
            }, function () {
                console.log('in error');
            });
        }

        $rsc.get_all_Usershipping_Address();

        // Same popup for both > Add & update Address
        $sc.show_userAdress_modal = function (model) {
            var _model = model ? angular.copy(model) : '';

            var modalInstance = $modal.open({
                templateUrl: 'templates/modal/user_shipping_Addressform.html',
                windowClass: 'shippingAdressModal',
                controller: 'user_adressFormController',            // No need to specify this controller on HTML page explicitly    
                resolve: {
                    user_addressModel: function () {
                        return _model;               // send value from here to controller as dependency
                    }
                }
            });
        }

        $sc.remove_adress = function (adresId) {
            svc.remove_Shipping_adress(adresId).then(function (data) {

                $rsc.get_all_Usershipping_Address();
                $rsc.notify_fx('Shipping adress is removed !', 'warning');

            }, function () {
                console.log('in error');
            });
        }

    }

    var user_adressFormControllerfn = function ($sc, $rsc, $modalInstance, svc, $filter, user_addressModel) {
        $sc.shippingInfo = user_addressModel ? user_addressModel : {};      // check value exist or not for add or Edit
        var Is_Add = user_addressModel ? false : true;

        $sc.shippingInfo.is_disableEmail = false;
        $sc.shippingInfo.is_disableMobile = false;

        // NUll coleasce fx > if $sc.shippingInfo.Country value = 0, null, undefine, '' > then country_code will assign otherwise $sc.shippingInfo.Country
        $sc.shippingInfo.Country = parseInt($sc.shippingInfo.Country || country_code);

        // use to show City & State txtbox
        $sc.is_show = true;

        // for only add adress > check if user is login then fetch record
        if ($rsc.user.currentUser !== '' && Is_Add) {
            $sc.is_show = false;

                // if data is present
                if ($rsc.user.currentUser.EmailID) {
                    $sc.shippingInfo.Email = $rsc.user.currentUser.EmailID;
                    $sc.shippingInfo.is_disableEmail = true;
                }

                if ($rsc.user.currentUser.MobileNumber) {
                    $sc.shippingInfo.Mobile = $rsc.user.currentUser.MobileNumber;
                    $sc.shippingInfo.is_disableMobile = true;
                }
        }

        $sc.close = function () {
            $modalInstance.close();
            console.log('cancel popup');
        }

        $sc.save_user_shipping_Address = function (form) {
            if (form.validate()) {

                if (Is_Add) {       // for Add
                   
                    svc.save_newShipping_adress($sc.shippingInfo).then(function (data) {

                        // retrieve all record again from server
                        $rsc.get_all_Usershipping_Address();
                        $modalInstance.dismiss();

                        $rsc.notify_fx('Shipping adress is saved !', 'info');
                    }, function () {
                        console.log('in error');
                    });
                }
                else {      // for update
                    $sc.shippingInfo.Id = user_addressModel.Id;
                    svc.update_Shipping_adress($sc.shippingInfo).then(function (data) {

                        $rsc.get_all_Usershipping_Address();
                        $modalInstance.dismiss();

                        $rsc.notify_fx('Shipping adress is updated !', 'info');
                    }, function () {
                        console.log('in error');
                    });
                }

            }
        }

        $sc.get_userLocation_by_pinCode = function (pincode) {
            if (pincode) {      // when txtbox is not empty
                svc.get_location($sc.shippingInfo.PinCode).then(function (response) {
                    $sc.shippingInfo.City = response.city;      // wrk when ajax call sucesfuly return data
                    $sc.shippingInfo.State = response.state;
                    $sc.is_show = true;
                }, function () {
                    console.log('in error');
                });
            }
        }

        $sc.change_sts = function (Type) {
            if (Type === 'email')
                $sc.shippingInfo.is_disableEmail = false;
            else if (Type === 'mobile')
                $sc.shippingInfo.is_disableMobile = false;
        }

        // validating rules for registration mmodel
        $sc.validationOptions = {
            rules: {
                userName: {            // use with name attribute in control
                    required: true
                },
                email: {
                    required: true,
                    email: true
                },
                mobile: {
                    required: true,
                    minlength: 10
                },
                shipping_adress: {
                    required: true
                },
                shipping_country: {
                    required: true
                },
                shipping_pincode: {
                    required: true
                },
                shipping_city: {
                    required: true
                },
                shipping_state: {
                    required: true
                }
            }
        }


    }

    // use to divide a list in specified size
    function chunk(arr, size) {
        var newArr = [];
        for (var i = 0; i < arr.length; i += size) {
            newArr.push(arr.slice(i, i + size));
        }
        return newArr;
    }


    app.controller('cartDetailsController', ['$rootScope', '$uibModal', '$state', '$stateParams', 'cartService', '$filter', cartDetailsControllerfn])
       .controller('cart_ProceedController', ['$scope', '$uibModalInstance', '$state', cart_ProceedControllerfn])
       .controller('user_addressController', ['$scope', '$rootScope', '$uibModal', 'cartService', '$filter', '$state', user_addressControllerfn])
       .controller('user_adressFormController', ['$scope', '$rootScope', '$uibModalInstance', 'cartService', '$filter', 'user_addressModel', user_adressFormControllerfn]);

})(angular.module('Silverzone_app'));
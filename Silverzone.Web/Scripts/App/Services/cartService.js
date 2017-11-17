
(function (app) {

   
    var fn = function ($http, $q, $filter, globalConfig) {

        var apiUrl = globalConfig.apiEndPoint + globalConfig.version.Site,
            fac = {};

        //***************** User Shipping Address  **********************

        // we will not pass UserId here, it will be done at server side code
        fac.get_shipping_Address = function () {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Order/get_shipping_Address_byUserId',
                method: 'GET',
                //cache: true      // CACHE WILL WORK > if we redirect 1 page to other using Angular client routing
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        fac.save_newShipping_adress = function (_model) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Order/create_uesr_shipping_Address',
                method: 'POST',
                data: _model
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        fac.update_Shipping_adress = function (_model) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Order/update_uesr_shipping_Address',
                method: 'POST',
                data: _model
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        fac.remove_Shipping_adress = function (Id) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Order/remove_uesr_shipping_Address',
                method: 'POST',
                params: { adresId: Id }
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }


        fac.get_location = function (pincode) {
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Order/get_location',
                method: 'GET',
                params: {
                    pincode: pincode
                }
            })
                .success(function (dt) {
                    var data = jQuery.parseJSON(dt),
                        userLocation = {};

                    if (data.status === "OK") {

                        var list = data.results[0].address_components;

                        for (var i = 0; i < list.length; i++) {

                            angular.forEach(list[i].types, function (data, key) {
                                if (data === "locality" || data === "administrative_area_level_2")
                                    userLocation.city = list[i].long_name;
                                else if (data === "administrative_area_level_1")
                                    userLocation.state = list[i].long_name;
                            });
                        }
                    }
                    defer.resolve(userLocation);
                    //deferred.resolve({
                    //        title: data.title,
                    //        cost: data.price
                    //});
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });

            return defer.promise;
        }


        //******************  for Orders  ********************

        fac.insert_orderInfo = function (_model) {
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Order/create_order',
                method: 'POST',
                data: _model
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        fac.order_confirmation_Emil = function (orderDetail_model) {     // orderType> 1 for offline & 2 for online
            var htmlTemplate = orderDetail_model.orderType === 1 ? 'orderConfirmation_onilne.html' : 'orderConfirmation_offline.html';

            $http({
                url: 'templates/EmailTemplates/' + htmlTemplate,           // get relative path with suffix '/' + url: Path
                method: 'GET',
            })
                .success(function (data) {
                    debugger;
                    console.log(data);

                    var htmlResult = bind_itemList_Html(data, orderDetail_model);
                    var userEmailId = orderDetail_model.shipping_adress.Email;

                    var params = { HtmlTemplate: htmlResult, emailId: userEmailId };

                    // call ajax to send email to user email
                    $http({
                        url: apiUrl + '/Order/sendEmail',
                        method: 'POST',
                        data: params
                    });
                })
                .error(function (e) {
                    console.log('in error');
                });
        }

        fac.confirmOrder = function (Id, orderType, deduction) {
            //orderType = orderType !== 1 ? true : false;

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Order/confirm_order',
                method: 'GET',
                params: {
                    oderId: Id,
                    orderMode: orderType,
                    Quiz_Points_Deduction: deduction
                }
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }


        // we also use service like $filter thats wy its define here 
        function bind_itemList_Html(myHtml, order) {
            var html = $("<div>" + myHtml + "</div>");


            // **************  Item List HTML binding  ********************
            var itemList_html = '';

            angular.forEach(order.cartInfo, function (item, key) {
                var title = item.bookTitle;

                var bookTpe = $filter('split')(title, ':', 1).trim();
                bookTpe = $filter('split')(bookTpe, '-', 0).trim();

                var price = parseFloat(item.bookTotalPrice) / parseFloat(item.bookQty);
                var newTitle = $filter('split')(title, ':', 0).trim();

                itemList_html += ' <tr> <td> <img src="' + item.bookImage + '" style="width: 55px;"> </td>'
                       + ' <td> <p> <strong>' + newTitle + '</strong> </p> '
                       + ' <p> <strong> Publisher: </strong>' + item.bookPublisher + '</p> '
                       + ' <p> <strong> BookType: </strong>' + bookTpe + '</p> '
                       + ' <p> <strong> Class: </strong>' + $filter('split')(title, '-', 1).trim() + '</p>'
                       + ' <p> <strong> Subject: </strong>' + item.Subject + '</p> </td>'
                       + ' <td style="text-align: center;"> ₹ ' + $filter('number')(price, 2) + '</td> '
                       + ' <td style="text-align: center;">' + item.bookQty + '</td>'
                       + ' <td style="text-align: center;"> ₹ ' + $filter('number')(item.bookTotalPrice, 2) + '</td> </tr> '
                       + ' <tr><td colspan="5"> <hr/> </td></tr>'
            });

            var amt = order.Quiz_Points_Deduction !== null ? (order.cart_total_Amount - parseInt(order.Quiz_Points_Deduction)) : order.cart_total_Amount;
            var cart_total_Amount = $filter('number')(amt, 2);
            var list_content = '<tr> <td></td> <td></td> <td></td> <td><strong>Toatal</strong></td> <td style="text-align: center;"> ₹ ' + $filter('number')(order.cart_shipping_Amount, 2) + '</td> </tr>'
                            + ' <tr> <td></td> <td></td> <td></td> <td><strong>Shipping Charges</strong></td> <td style="text-align: center;"> ₹ ' + order.cart_shipping_Charges + ' </td> </tr>'
                            + (order.Quiz_Points_Deduction !== null ? '<tr> <td></td> <td></td> <td></td> <td><strong>Redeem Quiz Points (-)</strong></td> <td style="text-align: center;"> ₹ ' + order.Quiz_Points_Deduction + ' </td> </tr>' : '')
                            + ' <tr> <td></td> <td></td> <td></td> <td><strong>Payable Amount</strong></td> <td style="text-align: center;"> ₹ ' + cart_total_Amount + '</td> </tr>'

            //  binding order details List
            $(html).find('#item_container').append(itemList_html + list_content);

            //  binding panel order details
            $(html).find('#item_panel_container').html('<div> <span> Your Cart Items- ' + order.cartInfo.length + ' Item(s) </span> <span style="float: right;"> Order Total: ' + cart_total_Amount + '</span> </div>');


            // **************  Selected Address HTML binding  ********************
            var shipping_adress = order.shipping_adress;            // get object in a single variable

            var userFull_adres = shipping_adress.Address + '-' + shipping_adress.PinCode + ', ' + shipping_adress.City + '<br />' + shipping_adress.State;

            $(html).find('#address_container').html(' <h4>' + shipping_adress.Username + '</h4> <p style="margin-bottom: 10px;">'
                        + userFull_adres + '</p>                                                               '
                        + ' <p style="color: #565656; padding: 10px 0px 0px 5px; border-top: #e6e6e6 solid 1px;"> '
                        + ' <span>' + shipping_adress.Email + '</span>                                   '
                        + ' <div style="margin-left: 5px; margin-top: 8px;"><strong class="ng-binding">' + shipping_adress.Mobile + '</strong></span> </p> ');

            // **************  order Detail HTML binding  ********************
            
            $(html).find('#orderDetailContainer').html('<h4><strong>Order Number:</strong>'+ order.order_confirmData.OrderNumber + '</h4> '
                        + ' <h4><strong>Order Date:</strong>' + $filter('dateFormat')(order.order_confirmData.OrderDate, 'DD/MM/YYYY') + '</h4>                                     '
                        + ' <h4><strong>Your Status:</strong>Order Placed</h4> '
                        + ' <h4><strong>Payment Method:</strong>Cheque/Draft/Cash Deposit</h4> ');

            // **************  order Detail HTML binding for offline Payment mode  ********************
            if (order.orderType === 2)
                $(html).find('#orderPaymentDetail_container').html('<div style="font-size:16px">'
                        + ' Prepare <strong>At Par Multi-City</strong> Cheque in favor of <strong>3GENX INDIA</strong> for ₹ ' + cart_total_Amount + '</div>'
                        +' <p style="text-align:center;">OR</p>'
                        + ' <div style="font-size:16px;">Prepare a Demand Draft in favor of <strong>3GENX INDIA</strong> payable at New Delhi for the Amount ₹ ' + cart_total_Amount + '</div>'
                        + ' <p style="font-size:16px"> Write Your Postal Address,Phone Number and Order Number <strong>' + order.order_confirmData.OrderNumber + '</strong> at the back side of your Cheque/Draft.</p>');

            return html.html().trim();
        }


        return fac;
    }

  

    app.factory('cartService', ['$http', '$q', '$filter', 'globalConfig', fn]);

})(angular.module('Silverzone_app'));


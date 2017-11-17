(function () {

    var fn = function ($http, $q, globalConfig, $filter, myService) {
        
        console.log('service is initialised only once (singleton), no matter how much controller inject it');

        var apiUrl = globalConfig.apiEndPoint + globalConfig.version.Admin,
             fac = {};

        fac.get_Orders = function (_model) {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Dispatch/getordersInfo',
                method: 'GET',
                params: {
                    date: _model.orderDate,
                    orderType: _model.printType
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

        fac.createLabel = function (_orderId) {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Dispatch/create_orderLabel',
                method: 'GET',
                params: {
                    orderId: _orderId
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

        fac.updateLabel = function (entity) {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Dispatch/update_orderLabael',
                method: 'POST',
                params: {
                    Id: entity.Id,
                    reason: entity.reason
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

        fac.get_labelTemplate = function (model) {     // orderType> 1 for offline & 2 for online
            var htmlTemplate = 'orderLabel.html';

            $http({
                url: 'templates/EmailTemplates/' + htmlTemplate,           // get relative path with suffix '/' + url: Path
                method: 'GET',
            })
                .success(function (data) {
                    debugger;

                    var htmlResult = bind_itemList_Html(data, model);

                    var mywindow = window.open('', 'my div', 'height=800,width=1400');
                    mywindow.document.write(htmlResult);

                    setTimeout(function () {
                        mywindow.document.close();

                        mywindow.focus();
                        mywindow.print();
                        mywindow.close();
                    }, 100);


                })
                .error(function (e) {
                    console.log('in error');
                });
        }

        function bind_itemList_Html(myHtml, model) {

            var html = $("<div>" + myHtml + "</div>"),
                itemList_html = '',
                userAdress = model.orderInfo.shipingAdres,
                userFull_adres = userAdress.Address + '-' + userAdress.PinCode + ', ' + userAdress.City + '<br />' + userAdress.State;

            $(html).find('#AdressInfo').html('<h4 style="margin: 0px !important;">' + userAdress.Username + '</h4>                                                                                                     '
                                + ' <p style="margin-bottom: 10px;" class="ng-binding">' + userFull_adres + '                                                                                         '
                                + ' <p style="color: #565656;margin: 0px !important;border-top: #000 solid 1px;">                            '
                                + ' <i class="fa fa-envelope"></i><span style="margin-left: 5px;">' + userAdress.Email + '</span> <br> '
                                + ' <i class="fa fa-phone"></i><span style="margin-left: 5px;"><strong>' + userAdress.Mobile + '</strong></span> </p>                                   ');

            var imgSrc = 'data:image/png;base64,' + model.packetInfo.imgBase;

            $(html).find('#packetDetail').html('<p> Invoice Number - <b>' + model.packetInfo.Invoice_No + '</b> </p>'
                                           + '<p> Packet ID - <b>' + model.packetInfo.Packet_Id + '</b> </p>'
                                           + '<p> Order ID - <b>' + model.orderInfo.OrderNumber + '</b> </p>'
                                           + '<p>Order Date - <b>' + $filter('dateFormat')(model.orderInfo.OrderDate, 'DD/MM/YYYY hh:mm a') + '</b> </p>'
                                           + '<p>Label Print DateTime - <b>' + $filter('dateFormat')(model.packetInfo.CreationDate, 'DD/MM/YYYY hh:mm a') + '</p>'
                                           + '<p>Bar Code - <img src=' + imgSrc + ' style="margin-left: 7px;"> </p>');

            angular.forEach(model.orderInfo.books, function (item, key) {
                itemList_html += '<tr> <td>' + (key + 1) + '</td>'
                               + '<td> ' + item.subject + ' </td>      '
                               + '<td> ' + item.className + ' </td>    '
                               + '<td> ' + item.bookCategory + ' </td> '
                               + '<td> ' + item.bookQuantity + ' </td> '
                               + '<td> ' + item.bookPrice + '  </td>   '
                               + '<td> ₹ ' + parseFloat(item.bookPrice) * parseFloat(item.bookQuantity) + ' </td> </tr>'

                if (item.bookType === 2) {          // for bundle get name of books inside bundle
                    var chars = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J'];

                    angular.forEach(item.bundle_booksInfo, function (item, key) {
                        itemList_html += '<tr> <td>' + $filter('lowercase')(chars[key]) + '. </td> '
                                       + '<td> ' + item.SubjectName + ' </td>  <td></td>'
                                       + '<td> ' + item.CategoryName + ' </td>  '
                    });
                }
            });

            var cntry = model.orderInfo.shipingAdres.Country;
            var shipcharge = '<td colspan="6" style="text-align: right;"><strong>Shipping Charges</strong> <div> <strong style="font-size: 75% !important;">'
                            + '(for first unit @' + (cntry === 'India' ? 40 : 1200) + ' and rest @' + (cntry === 'India' ? 25 : 200) + ' each)</strong></div> </td>'

            var totl_amt = parseFloat(model.orderInfo.Total_Shipping_Amount) + parseFloat(model.orderInfo.Total_Shipping_Charges);
            itemList_html += '<tr> '
                            + '<td colspan="6" style="text-align: right;"><strong>Shipping Amount</strong></td>'
                            + '<td> ₹ ' + parseFloat(model.orderInfo.Total_Shipping_Amount) + '</td> </tr>'
                            + '<tr>' + shipcharge
                            + '<td> ₹ ' + parseFloat(model.orderInfo.Total_Shipping_Charges) + '</td> </tr>'
                            + '<tr> <td colspan="3"> Counted By ___________ </t> <td colspan="3" style="text-align: right;"><strong>Total Amount</strong></td>'
                            + '<td> ₹ ' + totl_amt + '</td> </tr>'


            $(html).find('#item_container').append(itemList_html);

            return html.html().trim();
        }

        // ***********************  for Dispatch Wheight  *******************

        fac.get_packets = function (_model) {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Dispatch/get_printedOrder',
                method: 'GET',
                params: {
                    date: _model.orderDate,
                    wheightType: _model.type
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

        // ***********************  for Dispatch Consignment  *******************

        fac.get_consignmentPackets = function (_model) {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Dispatch/get_wheightedOrder',
                method: 'GET',
                params: {
                    date: _model.orderDate,
                    consignmentType: _model.type
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

        fac.add_packetConsignment = function (entity) {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Dispatch/add_consignment',
                method: 'PUT',
                params: {
                    dispatchId: entity.dispatchId,
                    consignmentNo: entity.packetConsignment,
                    courierMode: entity.courierMode
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

        fac.get_courierList = function (_model) {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Dispatch/get_couriers',
                method: 'GET'
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        fac.get_courierModes = function (courier_Id) {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Dispatch/get_courierMode_by_courierId',
                method: 'GET',
                params: {
                    courierId: courier_Id
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

        // ***********************  for Tracking order  *******************

        fac.get_trackingOrder = function (_date) {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Dispatch/get_orderTrackList',
                method: 'GET',
                params: {
                    date: _date
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


        return fac;

    }

    angular.module('Silverzone_app')
        .factory('admin_orderDispatch_Service', ['$http', '$q', 'globalConfig', '$filter', 'myService', fn]);

})();


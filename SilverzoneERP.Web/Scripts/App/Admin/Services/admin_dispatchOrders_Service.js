(function () {

    var fn = function ($http, $q, globalConfig, $filter) {
        
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

        fac.createLabel = function (model) {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Dispatch/create_orderLabel',
                method: 'POST',
                data: model
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

        fac.get_otherItemlabelTemplate = function (model) {     // orderType> 1 for offline & 2 for online
            var htmlTemplate = 'otherItem_orderLabel.html';

            $http({
                url: 'templates/EmailTemplates/' + htmlTemplate,           // get relative path with suffix '/' + url: Path
                method: 'GET',
            })
                .success(function (data) {
                    debugger;

                    var htmlResult = bind_otherItemList_Html(data, model);

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

        function bind_otherItemList_Html(myHtml, model) {

            var html = $("<div>" + myHtml + "</div>"),
                itemList_html = '',
                userAdress = model.orderInfo;

            $(html).find('#AdressInfo').html('<h4 style="margin: 0px !important;">' + userAdress.ContactPerson + '</h4>                     '
                                + ' <p style="margin-bottom: 10px;" class="ng-binding">' + userAdress.Name + '                              '
                                + ' <p style="margin-bottom: 10px;" class="ng-binding">' + userAdress.Address + ' - ' + userAdress.PinCode  
                                + ' <p style="color: #565656;margin: 0px !important;border-top: #000 solid 1px;">                            '
                                + ' <i class="fa fa-phone"></i><span style="margin-left: 5px;"><strong>' + userAdress.PhoneNo + '</strong></span> </p>');

            var imgSrc = 'data:image/png;base64,' + model.packetInfo.imgBase;

            $(html).find('#packetDetail').html('<p> Invoice Number - <b>' + model.packetInfo.Invoice_No + '</b> </p>'
                                           + '<p> Packet ID - <b>' + model.packetInfo.Packet_Id + '</b> </p>'
                                           + '<p>Order Date - <b>' + $filter('dateFormat')(model.orderInfo.OrderDate, 'DD/MM/YYYY hh:mm a') + '</b> </p>'
                                           + '<p>Label Print DateTime - <b>' + $filter('dateFormat')(model.packetInfo.CreationDate, 'DD/MM/YYYY hh:mm a') + '</p>'
                                           + '<p>Bar Code - <img src=' + imgSrc + ' style="margin-left: 7px;"> </p>');
            
            $(html).find('#itemContainer').append(userAdress.Items);

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

        fac.add_packetWheight = function (entity) {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Dispatch/add_packetWheight',
                method: 'POST',
                data: entity
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        fac.verify_packetWheight = function (_dispatchId) {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Dispatch/verify_packetWheight',
                method: 'PUT',
                params: {
                    dispatchId: _dispatchId
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

        fac.get_packages = function (_model) {
            debugger;

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Dispatch/get_packages',
                method: 'GET',
                cache: !0
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        fac.printBundle = function (model) {
            $http({
                url: 'templates/EmailTemplates/stockInvoice.html',           // get relative path with suffix '/' + url: Path
                method: 'GET',
            })
                .success(function (data) {
                    debugger;

                    var htmlResult = bind_bundle_Html(data, model);

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

        function bind_bundle_Html(myHtml, model) {

            var html = $("<div>" + myHtml + "</div>");

            var _html = '',
                SZ_userAdress = model.orderInfo.dealerAdress;

            if (model.orderInfo.dealerAdress === undefined)
                var web_userAdress = model.orderInfo.shipingAdres,
                    web_userFull_adres = web_userAdress.Address + '-' + web_userAdress.PinCode + ', ' + web_userAdress.City + '<br />' + web_userAdress.State;

            for (i = 1; i <= model.bundleLength; i++) {
                _html += ' <div style="margin-top: 20px;width: 900px;display: inline-block;">               \
                           <div style="width:485px; float: left;">                                          \
                             <div class="panel panel-default" style="border: 1px solid #333;">              \
                                 <div class="panel-body" style="padding: 5px !Important;">                  \
                                 <div class="col-md-12 col-sm-2" style="padding: 0px;" id="AdressInfo">     \
                                  '+ (SZ_userAdress !== undefined
                                  ? '<h4 style="margin: 0px !important;">' + model.orderInfo.SourceType + '</h4>                                                \
                                     <p style="margin-bottom: 10px;" class="ng-binding">' + SZ_userAdress.SourceAddress + '                                     \
                                     <p style="color: #565656;margin: 0px !important;border-top: #000 solid 1px;">                                              \
                                     <i class="fa fa-envelope"></i><span style="margin-left: 5px;">' + SZ_userAdress.SourceEmail + '</span> <br>                \
                                     <i class="fa fa-phone"></i><span style="margin-left: 5px;"><strong>' + SZ_userAdress.SourceMobile + '</strong></span> </p> '
                                  : '<h4 style="margin: 0px !important;">' + web_userAdress.Username + '</h4>                                              \
                                     <p style="margin-bottom: 10px;" class="ng-binding">' + web_userFull_adres + '                                         \
                                     <p style="color: #565656;margin: 0px !important;border-top: #000 solid 1px;">                                     \
                                     <i class="fa fa-envelope"></i><span style="margin-left: 5px;">' + web_userAdress.Email + '</span> <br>                \
                                     <i class="fa fa-phone"></i><span style="margin-left: 5px;"><strong>' + web_userAdress.Mobile + '</strong></span> </p> ') +
                                     '</div> </div> </div> </div>                                                   \
                                 <div style="width: 385px;float: left;margin-left: 10px" id="packetDetail">        \
                                     <p> Invoice Number - <b>' + model.packetInfo.Invoice_No + '</b> </p>          \
                                     <p> Packet- <b>' + i + '/' + model.bundleLength + '</b> </p>   \
                                     <p> Packet ID - <b>' + model.packetInfo.Packet_Id + '</b> </p> </div></div>   '
            }

            $(html).find('#myId').html(_html);

            return html.html().trim();
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

        fac.get_courierList = function () {
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

        fac.get_orderStatusList = function () {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Dispatch/get_orderStatusList',
                method: 'GET',
                cache: !0
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

        fac.get_orderStatus_Reasons = function (_date) {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Dispatch/get_orderStatus_Reasons',
                method: 'GET',
                cache: true
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }


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

        fac.change_trackingStatus = function (_statusId, _dispatchId, _remarks) {
            debugger;

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Dispatch/changeOrder_TrackStatus',
                method: 'GET',
                params: {
                    statusId: _statusId,
                    dispatchId: _dispatchId,
                    remarks: _remarks
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


        fac.Resend_Order = function (Id) {
            debugger;

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Dispatch/ResendOrder',
                method: 'GET',
                params: {
                    dispatchId: Id,
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

        fac.searchItem = function (model) {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Dispatch/search_dispatch',
                method: 'POST',
                data: model
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

    angular.module('Silverzone_admin_app')
        .factory('admin_orderDispatch_Service', ['$http', '$q', 'globalConfig', '$filter', fn]);

})();


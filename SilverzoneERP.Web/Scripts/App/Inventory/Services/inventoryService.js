(function (app) {

    var fn = function ($http, $q, globalConfig, $filter) {

        var apiUrl = globalConfig.apiEndPoint + globalConfig.version.Inventory,
            fac = {};


        // **************************** For Inventory **************************

        fac.get_inventorySource = function () {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Stock/get_inventorySource',
                method: 'GET',
                async: false,
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

        fac.getBookISBN = function (_bookId) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Stock/get_bookISBN',
                method: 'GET',
                params: {
                    bookId: _bookId
                },
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

        fac.get_bookCategory = function (ISBN) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Stock/get_bookCategory',
                method: 'GET',
                params: {
                    bookISBN: ISBN
                },
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

        fac.getPendingPO = function (_model) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Stock/getAll_pendingPO',
                method: 'POST',
                data: _model,
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        fac.save_stock = function (_model) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Stock/create_inventory',
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

        fac.create_bulkStock = function (_model) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Stock/create_bulkinventory',
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

        fac.create_dealer_stock = function (_model) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Stock/create_delaler_inventory',
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

        fac.edit_stock = function (_model) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Stock/editCreate_inventory',
                method: 'POST',
                data: _model,
                params: {
                    Id: _model.Id
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

        fac.verifyChallan = function (_model) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Stock/verify_challan',
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

        fac.get_dealerAdress = function (srcInfo_Id) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Stock/get_dealerAdressList',
                method: 'GET',
                params: {
                    SourceInfo_Id: srcInfo_Id
                },
                cache: true,
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }


        fac.get_inventoryList = function (_stockId) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Stock/get_Inventory_byStockId',
                method: 'GET',
                //cache: true,
                params: {
                    stockId: _stockId
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

        fac.update_stock = function (_model) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Stock/update_inventory',
                method: 'POST',
                data: _model,
                params: {
                    Id: _model.Id
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

        fac.delete_stock = function (_Id) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Stock/delete_inventory',
                method: 'POST',
                params: {
                    Id: _Id
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

        fac.get_dealerPO_bypoID = function (_srcId) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Stock/get_purchaseOrder_bypo_mId',
                method: 'GET',
                params: {
                    srcId: _srcId
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

        fac.get_inventory_byChallan = function (_challanNo) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Stock/get_Inventory_bychallanNo',
                method: 'GET',
                params: {
                    challanNo: _challanNo
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

        fac.send_Stock_confirmation_Email = function (model, sendEmail) {

            $http({
                url: 'templates/EmailTemplates/Stock_Confirmation.html',           // get relative path with suffix '/' + url: Path
                method: 'GET',
            })
                .success(function (data) {
                    debugger;

                    var htmlResult = bind_stockItemList_Html(data, model);
                    var userEmailId = model.SourceEmail;

                    if (sendEmail) {
                        var _model = {
                            HtmlTemplate: htmlResult,
                            emailId: userEmailId
                        };

                        // call ajax to send email to user email
                        $http({
                            url: apiUrl + '/Stock/send_stockEmail',
                            method: 'POST',
                            data: _model
                        });
                    }
                    else {              // for Print PO
                        var mywindow = window.open('', 'my div', 'height=800,width=1400');
                        mywindow.document.write(htmlResult);

                        setTimeout(function () {
                            mywindow.document.close();

                            mywindow.focus();
                            mywindow.print();
                            mywindow.close();
                        }, 100);
                    }

                })
                .error(function (e) {
                    console.log('in error');
                });
        }

        function bind_stockItemList_Html(myHtml, model) {
            var html = $("<div>" + myHtml + "</div>");


            // **************  Item List HTML binding  ********************
            var itemList_html = '',
                userAdress = model.dealerAdress
                ;

            $(html).find('#companyInfo').html('<h4 style="margin: 0 !important;">' + model.srcFrom_info.SourceName + '</h4>                                 \
                                    <p style="margin: 0 !important;">' + model.srcFrom_info.SourceAddress + '</p>                              \
                                    <p style="margin: 0 !important;"> <i class="fa fa-envelope" style="margin-right: 5px;"></i>' + model.srcFrom_info.SourceEmail + '</p> \
                                    <p style="margin: 0 !important;"> <i class="fa fa-phone" style="margin-right: 5px;"></i>' + model.srcFrom_info.SourceMobile + '</p>   ');


            angular.forEach(model.stockInfo, function (item, key) {
                itemList_html += '<tr> <td>' + parseInt(key + 1) + ' </td>'
                       + ' <td>' + item.Book.bookName + ' </td>'
                       + ' <td>' + item.Quantity + '</td> '
                       + ' <td>' + item.PO_Number + '</td> '
                       + ' </tr>'
            });

            //  binding order details List
            $(html).find('#item_container').append(itemList_html);

            //  binding panel order details
            $(html).find('#item_panel_container').html('<div> <span> Challan cantains ' + model.stockInfo.length + ' Item(s) </span> </div>');

            var remarks = model.Remarks || '';

            $(html).find('#packetDetail').html('<p> Challan Number - <b>' + model.ChallanNo + '</b> </p>'
                                             + '<p> Challan Date - <b>' + $filter('dateFormat')(model.ChallanDate, 'DD/MM/YYYY hh:mm a') + '</b> </p>'
                                             +' <p><strong>Remarks: </strong>' + remarks + '</p>' );


            $(html).find('#AdressInfo').html('<h4 style="margin: 0px !important;">' + model.SourceType + '</h4>                                  '
                                       + ' <p style="margin-bottom: 10px;" class="ng-binding">' + userAdress.SourceAddress + '                                    '
                                       + ' <p style="color: #565656;margin: 0px !important;border-top: #000 solid 1px;">                                          '
                                       + ' <i class="fa fa-envelope"></i><span style="margin-left: 5px;">' + userAdress.SourceEmail + '</span> <br>              '
                                       + ' <i class="fa fa-phone"></i><span style="margin-left: 5px;"><strong>' + userAdress.SourceMobile + '</strong></span> </p>');

            return html.html().trim();
        }

        fac.get_counterSalePO = function (_model) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Stock/getPO_bypoNo',
                method: 'GET',
                params: {
                    srcId: _model.srcId,
                    PoNo: _model.poNo
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


        // **************************** For Source Details **************************

        fac.save_sourceInfo = function (_model) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Stock/create_inventorySource_Detail',
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

        fac.update_sourceInfo = function (_model) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Stock/update_inventorySource_Detail',
                method: 'POST',
                data: _model,
                params: {
                    Id: _model.Id,
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

        fac.save_dealer_sourceInfo = function (_model) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Stock/create_dealer_inventorySource_Detail',
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

        fac.update_dealer_sourceInfo = function (_model) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Stock/update_dealer_inventorySource_Detail',
                method: 'POST',
                data: _model,
                params: {
                    Id: _model.Id,
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


        fac.delete_sourceInfo = function (_model) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Stock/change_inventorySource_Status',
                method: 'POST',
                params: {
                    Id: _model.Id,
                    status: _model.Status
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

        fac.get_sourceInfo = function (_model) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Stock/get_inventorySource_Details',
                method: 'GET',
                params: {
                    sourceId: _model.Id,
                    show_allInfo: _model.Status,
                    type: _model.type

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


        // **************************** For PO **************************

        fac.create_PO = function (_model) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Stock/save_purchaseOrder',
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

        fac.delete_PO = function (_Id) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Stock/delete_purchaseOrder_byId',
                method: 'POST',
                params: {
                    Id: _Id
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

        fac.get_purchaseOrders = function (_Id) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Stock/get_purchaseOrder_byId',
                method: 'GET',
                params: {
                    Id: _Id
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

        fac.send_PO_confirmation_Email = function (model, sendEmail) {

            $http({
                url: 'templates/EmailTemplates/PO_Confirmation.html',           // get relative path with suffix '/' + url: Path
                method: 'GET',
            })
                .success(function (data) {
                    debugger;

                    var htmlResult = bind_itemList_Html(data, model);
                    var userEmailId = model.SourceEmail;

                    if (sendEmail) {
                        var _model = {
                            HtmlTemplate: htmlResult,
                            emailId: userEmailId
                        };

                        // call ajax to send email to user email
                        $http({
                            url: apiUrl + '/Stock/sendEmail',
                            method: 'POST',
                            data: _model
                        });
                    }
                    else {              // for Print PO
                        var mywindow = window.open('', 'my div', 'height=800,width=1400');
                        mywindow.document.write(htmlResult);

                        setTimeout(function () {
                            mywindow.document.close();

                            mywindow.focus();
                            mywindow.print();
                            mywindow.close();
                        }, 100);
                    }

                })
                .error(function (e) {
                    console.log('in error');
                });
        }

        function bind_itemList_Html(myHtml, model) {
            var html = $("<div>" + myHtml + "</div>");


            // **************  Item List HTML binding  ********************
            var itemList_html = '';

            angular.forEach(model.PO_detail, function (item, key) {
                itemList_html += '<tr> <td>' + parseInt(key + 1) + ' </td>'
                       + ' <td>' + item.Book.bookName + ' </td>'
                       + ' <td>' + item.Quantity + '</td> '
                       + ' <td>' + item.Rate + '</td> '
                       + ' </tr>'
            });

            //  binding order details List
            $(html).find('#item_container').append(itemList_html);

            //  binding panel order details
            $(html).find('#item_panel_container').html('<div> <span> Purchase order cantains ' + model.PO_detail.length + ' Item(s) </span> </div>');

            var remarks = model.Remarks || '';
            $(html).find('#source_container').html('<h4><strong>Order Number: </strong>' + model.PO_Number + '</h4> '
                        + ' <h4><strong>Order Date: </strong>' + $filter('dateFormat')(model.PO_Date, 'DD/MM/YYYY hh:mm a') + '</h4>                                     '
                        + ' <h4><strong>From: </strong>' + model.From + '</h4> '
                        + ' <h4><strong>To: </strong>' + model.To + '</h4> '
                        + ' <h4><strong>Source: </strong>' + model.Source + '</h4>'
                        + ' <h4><strong>Remarks: </strong>'+ remarks +'</h4> ');  // if item.Remarks does not value then '' will be pass


            return html.html().trim();
        }

        fac.getPO_byPoNo = function (_poNumber) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Stock/get_POby_poNumber',
                method: 'GET',
                params: {
                    poNumber: _poNumber
                },
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


        //*********************  Search PO  ******************************

        fac.search_pendingPO = function (model) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Stock/search_pendingPO',
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

        fac.getPendingPO_info = function (_srcId, _bookId) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Stock/search_pendingPO_details',
                method: 'GET',
                params: {
                    srcId: _srcId,
                    bookId: _bookId
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

        fac.adjust_pendingPO = function (_poID, _adjustRemarks) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Stock/adjust_pendingPO',
                method: 'POST',
                params: {
                    Id: _poID,
                    adjustRemarks: _adjustRemarks
                },
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


        // *******************  for Orders challan/Invoice  *******************************

        fac.getCustomer_BooksPrice = function (_stockId) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Stock/getCustomer_BooksPrice',
                method: 'GET',
                params: {
                    stockId: _stockId
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

        fac.create_CounterOrder = function (model) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Stock/create_Counterorder',
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

        fac.CounterOrder_template = function (model) {

            $http({
                url: 'templates/EmailTemplates/stockInvoice.html',           // get relative path with suffix '/' + url: Path
                method: 'GET',
            })
                .success(function (data) {
                    debugger;

                    var htmlResult = bind_CounterOrder_Html(data, model);
                    
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

        function bind_CounterOrder_Html(myHtml, model) {
            var html = $("<div>" + myHtml + "</div>");


            // **************  Item List HTML binding  ********************
            var itemList_html = '<tr style="border-bottom: 1px solid;">  '
                                 + ' <th>SNO</th>                        '
                                 + ' <th>Item</th>                       '
                                 + ' <th>Qty</th>                        '
                                 + ' <th>Unit Price (₹)</th>             '
                                 + ' <th>Net Price</th>                  '
                                 + ' </tr>'

            $(html).find('#companyInfo').html('<h4 style="margin: 0 !important;">' + model.source.SourceName + '</h4>                                 \
                                    <p style="margin: 0 !important;">' + model.source.SourceAddress + '</p>                              \
                                    <p style="margin: 0 !important;"> <i class="fa fa-envelope" style="margin-right: 5px;"></i>' + model.source.SourceEmail + '</p> \
                                    <p style="margin: 0 !important;"> <i class="fa fa-phone" style="margin-right: 5px;"></i>' + model.source.SourceMobile + '</p>   ');

            var totalPrice_sum = 0;
            angular.forEach(model.stockInfo, function (item, key) {
                var totalPrice = item.Quantity * item.bookPrice;
                totalPrice_sum += totalPrice;

                itemList_html += '<tr> <td>' + parseInt(key + 1) + ' </td>'
                       + ' <td>' + item.Book.bookName + ' </td>'
                       + ' <td>' + item.Quantity + '</td> '
                       + ' <td>' + item.bookPrice + '</td> '
                       + ' <td>' + totalPrice + '</td> '
                       + ' </tr>'
            });

            itemList_html += '<tr> '
                            + '<td colspan="4"'
                            + 'style="text-align: right;"><strong>Total Amount</strong></td>'
                            + '<td> ₹ ' + totalPrice_sum + '</td> </tr>'

            //  binding order details List
            $(html).find('#item_container').append(itemList_html);

            //  binding panel order details
            $(html).find('#item_panel_container').html('<div> <span> Challan cantains ' + model.stockInfo.length + ' Item(s) </span> </div>');

            $(html).find('#AdressInfo').html('<h4 style="margin: 0px !important;">' + model.customerInfo.name + '</h4>                                  '
                                       + ' <p style="margin-bottom: 10px;" class="ng-binding">' + model.customerInfo.address + '                                    '
                                       + ' <p style="color: #565656;margin: 0px !important;border-top: #000 solid 1px;">                                          '
                                       + ' <i class="fa fa-envelope"></i><span style="margin-left: 5px;">' + model.customerInfo.emailId + '</span> <br>              '
                                       + ' <i class="fa fa-phone"></i><span style="margin-left: 5px;"><strong>' + model.customerInfo.mobile + '</strong></span> </p>');

            return html.html().trim();
        }

        fac.ceateOrder = function (model) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Stock/create_order',
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

        fac.print_challan = function (model, pendingStock) {
            $http({
                url: 'templates/EmailTemplates/stockInvoice.html',           // get relative path with suffix '/' + url: Path
                method: 'GET',
            })
                .success(function (data) {
                    debugger;

                    var htmlResult = bind_invoice_Html(data, model, pendingStock);

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

        function bind_invoice_Html(myHtml, model, pendingStock) {

            var html = $("<div>" + myHtml + "</div>"),
                userAdress = model.orderInfo.dealerAdress,
                itemList_html = '<tr style="border-bottom: 1px solid;">  '
                                 + ' <th>SNO</th>                        '
                                 + ' <th>Item</th>                       '
                                 + ' <th>Qty</th>                        '
                                 + ' <th>Unit Price (₹)</th>             '
                                 +  (model.orderInfo.srcId === 2 && ' <th> Total (₹)</th>'
                                 + ' <th>Dicount (%)</th>                '
                                 + ' <th>Dicount Price (₹)</th>         ') 
                                 + ' <th>Net Price</th>                  '
                                 + ' </tr>'
            ;

           var srcFrom = model.orderInfo.srcFrom_info;
           $(html).find('#companyInfo').html('<h4 style="margin: 0 !important;">' + srcFrom.SourceName + '</h4>                                                \
                                    <p style="margin: 0 !important;">' + srcFrom.SourceAddress + '</p>                                                         \
                                    <p style="margin: 0 !important;"> <i class="fa fa-envelope" style="margin-right: 5px;"></i>' + srcFrom.SourceEmail + '</p> \
                                    <p style="margin: 0 !important;"> <i class="fa fa-phone" style="margin-right: 5px;"></i>' + srcFrom.SourceMobile + '</p>   ');

            $(html).find('#AdressInfo').html('<h4 style="margin: 0px !important;">' + model.orderInfo.SourceType + '</h4>                                  '
                                + ' <p style="margin-bottom: 10px;" class="ng-binding">' + userAdress.SourceAddress + '                                    '
                                + ' <p style="color: #565656;margin: 0px !important;border-top: #000 solid 1px;">                                          '
                                 + ' <i class="fa fa-envelope"></i><span style="margin-left: 5px;">' + userAdress.SourceEmail + '</span> <br>              '
                                + ' <i class="fa fa-phone"></i><span style="margin-left: 5px;"><strong>' + userAdress.SourceMobile + '</strong></span> </p>');

            var imgSrc = 'data:image/png;base64,' + model.packetInfo.imgBase;

            $(html).find('#packetDetail').html('<p> Invoice Number - <b>' + model.packetInfo.Invoice_No + '</b> </p>'
                                           + '<p> Packet ID - <b>' + model.packetInfo.Packet_Id + '</b> </p>'
                                           + '<p> Challan Number - <b>' + model.orderInfo.ChallanNo + '</b> </p>'
                                           + '<p> Challan Date - <b>' + $filter('dateFormat')(model.orderInfo.ChallanDate, 'DD/MM/YYYY hh:mm a') + '</b> </p>'
                                           + '<p>Label Print DateTime - <b>' + $filter('dateFormat')(model.packetInfo.CreationDate, 'DD/MM/YYYY hh:mm a') + '</p>'
                                           + '<p>Bar Code - <img src=' + imgSrc + ' style="margin-left: 7px;"> </p>');
            var totalPrice_sum = 0;
            angular.forEach(model.orderInfo.stockInfo, function (item, key) {
                var bookPrice = parseFloat(item.bookPrice) * parseFloat(item.Quantity),
                    discountPrice = bookPrice * parseFloat(item.DiscountPrice) / 100,
                    totlPrice = bookPrice - discountPrice;

                totalPrice_sum += totlPrice;

                itemList_html += '<tr> <td>' + parseInt(key + 1) + ' </td>'
                     + ' <td>' + item.Book.bookName + ' </td>'
                     + ' <td>' + item.Quantity + '</td> '
                     + ' <td>' + item.bookPrice + '</td> '
                     + (model.orderInfo.srcId === 2 && ' <td>' + bookPrice + '</td> '
                     + ' <td>' + item.DiscountPrice + '</td> '
                     + ' <td>' + discountPrice + '</td> ')
                     + ' <td>₹ ' + totlPrice + ' </td> '
                     + ' </tr>'
            });
         
            itemList_html += '<tr> '
                            + '<td '+ (model.orderInfo.srcId === 2 ? 'colspan="7"' :  'colspan="4"' )
                            + 'style="text-align: right;"><strong>Total Amount</strong></td>'
                            + '<td> ₹ ' + totalPrice_sum + '</td> </tr>'


            $(html).find('#item_container').append(itemList_html);

            // for school > showing pending order :)
            if (model.orderInfo.srcId === 6 &&
                pendingStock) {
                var pending_itemHtml = '',
                    cnt = 1;

                angular.forEach(pendingStock, function (stock, key) {
                    angular.forEach(stock.PO_detail, function (entity, key) {

                        pending_itemHtml += '<tr> <td>' + cnt + ' </td>'
                          + ' <td>' + entity.bookName + ' </td>'
                          + ' <td>' + stock.PO_Number + '</td> '
                          + ' <td>' + entity.Quantity + '</td> '
                          + ' </tr>'

                        cnt++;
                    });
                });
                $(html).find('#pending_item_container').append(pending_itemHtml);
                $(html).find('#div_schoolPending_POs').show();
            }


            return html.html().trim();
        }

        fac.get_allInvoice = function (model) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Stock/get_allInvoice',
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

        fac.get_invoiceDetail = function (model) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Stock/get_InvoiceInfo',
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

        fac.get_invoicePrintInfo = function (_invoiceId) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Stock/get_invoice_printInfo',
                method: 'GET',
                params: {
                    invoiceId: _invoiceId
                },
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


        //****************************  Stock Search *********************************

        fac.searchStock = function (_srcId, _bookId) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Stock/get_allstock',
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

        fac.searchStock_byBookid = function (_bookId, _type) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Stock/searchStock_byBookid',
                method: 'GET',
                params: {
                    bookId: _bookId,
                    type: _type
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

    angular
       .module('SilverzoneERP_invenotry_service', [])       // creating a new module here
       .factory('inventoryService', ['$http', '$q', 'globalConfig', '$filter', fn]);

})();
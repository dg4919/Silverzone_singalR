(function () {
    'use strict';

    angular.module('SilverzoneERP_App')
          .directive('bindBooks', function () {

              bookList_Controllerfn.$inject = ['$scope', '$rootScope'];
              function bookList_Controllerfn($sc, $rsc) {

                  $sc.addtoCart = function (bookInfo) {
                      //var bookInfo = $sc.bookList[index]

                      var qty = parseInt(find_qty_byBookId(bookInfo));

                      if (qty < 99) {
                          $rsc.cart.Items = add_itemInCart(bookInfo, qty + 1);           // fx avilable in bookController.js
                          $rsc.notify_fx('Item is added in your cart !', 'info');

                          // using > $root.is_Disable on Template of directive to access value of it :)
                          // use to show text > Added' OR 'Buy Now' option with Book list
                          $rsc.buyBook_isDisable[bookInfo.BookId] = true;

                      }
                      else
                          $rsc.notify_fx('You can add only 99 quantity of a product in your cart !', 'warning');
                  }

                  function find_qty_byBookId(bookInfo) {
                      var _returnVal = 0;

                      // loop each element & if found then return value
                      angular.forEach(cartItems_array, function (data, key) {
                          if (data.bookId === bookInfo.BookId) {       // means element is found
                              _returnVal = data.bookQty
                              return;         // exit from this loop
                          }
                      });

                      return _returnVal;
                  }
              }

              return {
                  restrict: 'E',
                  replace: true,        // to remove definition of directive from page
                  scope: {
                      book: '='
                  },
                  controller: bookList_Controllerfn,
                  templateUrl: 'templates/customDirective_template/bookList.html'    // work according to parent controller   
              }
          })

          .directive('bindBundles', function () {

              bookbundle_Controllerfn.$inject = ['$scope', '$rootScope'];
              function bookbundle_Controllerfn($sc, $rsc) {
                  var that = this;

                  that.addCartqty = function (isAdd_qty, id) {
                      // this = that will help us to given updated model value
                      that.bookBundle_cart_count[id] = addqty(that.bookBundle_cart_count[id], isAdd_qty);
                  }

                  that.check_cart = function (id) { // calling global fx from bookController.js
                      that.bookBundle_cart_count[id] = checkCart_Value(that.bookBundle_cart_count[id]);
                  }

                  that.addCombo_toCart = function (bookBundleInfo, Id) {
                      var itemQty = that.bookBundle_cart_count[Id];

                      var cartItems = {
                          bookId: bookBundleInfo.Id,
                          bookImage: 'Images/bundle.jpg',
                          bookTitle: bookBundleInfo.Name + ' : ' + 'Bundle' + ' - ' + bookBundleInfo.className,
                          bookTotalPrice: bookBundleInfo.bundle_totalPrice * itemQty,
                          bookQty: itemQty,
                          bookPrice: bookBundleInfo.bundle_totalPrice,
                          bookPublisher: 'Silverzone',
                          Subject: 'Books Bundle',
                          bookType: 2     // for book bundle
                      }

                      $rsc.cart.Items = add_update_Cart(cartItems);
                      $rsc.notify_fx('Item is added in your cart !', 'info');
                  }

              }

              return {
                  restrict: 'E',
                  replace: true,        // to remove definition of directive from page
                  scope: {
                      combo: '='
                  },
                  controller: bookbundle_Controllerfn,
                  controllerAs: 'bookbundle',           // use with page > bookbundle.scope_objectName
                  templateUrl: 'templates/customDirective_template/bundleList.html'    // work according to parent controller   
              }
          })

          .directive('scrollTo', ['$location', '$anchorScroll', function ($location, $anchorScroll) {
              return function (scope, element, attrs) {       // link fx of directive
                  element.bind('click', function (event) {
                      event.stopPropagation();
                      event.preventDefault();

                      var old = $location.hash(location);

                      var location = attrs.scrollTo;
                      $location.hash(location);
                      $anchorScroll();

                      //reset to old to keep any additional routing logic from kicking in
                      $location.hash(old);
                  });
              };
          }])

          .directive('showBundleModal', ['$uibModal', 'bookService', '$filter', function ($modal, svc, $filter) {
              var linkfn = function (scope, element, attrs) {       // link fx of directive
                  element.bind('click', function (event) {
                      svc.get_bookBundleDetail(attrs.bundleid).then(function (data) {
                          var entity = data.result[0];
                          var bookList = '';
                          var lenth = entity.bookInfo.length;

                          angular.forEach(entity.bookInfo, function (book, key) {
                              //var myUrl = '/Site/Book/Info/' + book.BookId + '/' + $filter('stringFilter')(book.title);

                              bookList += ' <div class="col-sm-3" style="padding: 5px;">'
                             + ' <img src="' + book.BookImage + '" class="img-responsive" style="margin: 0 auto; height:170px;">'
                             + ' <div class="text-center"> <div class="name-container"><a href="#">' + book.title + '</a></div>'
                             + ' <div class="price" style="margin: 6px 0px 6px 0px;">'
                             + ' <strong><i class="fa fa-inr"></i> </strong><span>' + book.price + '</span></div>'
                             + ' </div></div>'
                             + (key === lenth - 1 ? '' : ' <div class="col-sm-1" style="width: 25px !important; padding: 0px !important; margin-top: 10%;">'
                             + ' <i class="fa fa-plus fa-2x" style="color: burlywood;"></i></div>')
                          });

                          var template = '<div class="modal-header" style="padding: 8px !important;">                 '
                          + ' <h4 class="box-title">' + entity.bundleInfo.Name + '</h4>                               '
                          + '<button type="button" class="close" style="margin-top: -30px !important;">               '
                          + '<span aria-hidden="true" ng-click="cancel()">×</span></button> </div>                    '
                          + ' <div class="modal-body">                                                                '
                          + ' <div class="row">'
                          + ' <div class="col-sm-12"> <h4> This Combo includes following Items :-</h4> </div> </div>  '
                          + '<div class="col-sm-6 text-danger" style="font-size: larger;"><span class="text-info">'
                          + 'Total Price : </span> <strong><i class="fa fa-inr"></i> </strong>'
                          + '<span style="text-decoration: line-through;">' + entity.bundleInfo.books_totalPrice + '</span> </div>'
                          + ' <div class="col-sm-6 text-warning text-right" style="font-size: larger;">'
                          + ' <span class="text-info">Combo Price :</span> <strong><i class="fa fa-inr"></i> </strong>'
                          + ' <span>' + entity.bundleInfo.bundle_totalPrice + '</span></div> <div class="row" style="margin-top: 50px;">'
                          + bookList + '</div>'
                          + ' <div class="modal-footer">                                                              '
                          + ' <button class="btn btn-info" ng-click="ok()">OK</button>  </div>   '

                          var modalInstance = $modal.open({
                              template: template,
                              size: 'lg',
                              controller: 'modal_Controller',     // point to current ctrler
                          });

                      });
                  });
              };

              return {
                  restrict: 'A',
                  link: linkfn,
              }
          }])

        // shared modal controller > if popup has only close & cancel fxnality
         .controller('modal_Controller', ['$scope', '$uibModalInstance', function ($sc, $modalInstance) {

             $sc.ok = function () {
                 $modalInstance.close();
             }

             $sc.cancel = function () {
                 $modalInstance.dismiss();
             }
         }])

    ;

})();
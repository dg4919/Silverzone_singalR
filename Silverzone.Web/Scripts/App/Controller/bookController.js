/// <reference path="/Lib/angularjs/angular-1.5.0.js" />

// global variable hold its previous values
// now scope of this variable is globally can access in other controllers :)
var cartItems_array = new Array();

//  ------- global fxs ----------
function add_itemInCart(bookInfo, itemQty) {

    // when coupon Info is not null & book_newPrice have some value then it will assign otherwise > bookInfo.Price
    var bookPrice = parseFloat(bookInfo.CouponInfo && bookInfo.CouponInfo.book_newPrice) || parseFloat(bookInfo.Price);

    var cartItems = {
        bookId: bookInfo.BookId,
        bookImage: bookInfo.BookImage,
        bookTitle: bookInfo.book_title + ' : ' + bookInfo.Category + ' - ' + bookInfo.Class,
        bookTotalPrice: bookPrice * itemQty,
        bookQty: itemQty,
        bookPrice: bookPrice,
        bookPublisher: bookInfo.Publisher,
        Subject: bookInfo.Subject,
        bookType: 1     // for books
    }

    // variable initialise in site master js > that is main
    //$sc.cart_detailContainer.cartItemsList.push(cartItems);
    return add_update_Cart(cartItems);
}

// $rsc is rootScope which we can't pass in above self executable fx, its a services which only inject in controller
function add_update_Cart(bookInfo, $rsc) {
    var item_notFound = true;

    angular.forEach(cartItems_array, function (data, key) {

        if (data.bookId === bookInfo.bookId && data.bookType === bookInfo.bookType) {   //  bookType is > type of book or bundle
            // necessary fields will update
            data.bookTotalPrice = bookInfo.bookTotalPrice;
            data.bookQty = bookInfo.bookQty;

            item_notFound = false;
        }
    });

    if (item_notFound) {
        cartItems_array.push(bookInfo);
    }

    return cartItems_array;
}

// to check value is integer or not > while keypress in cart quantity
function checkCart_Value(Qty) {

    // convert a string value too int > like "7f" = 7
    var item_qty = parseInt(Qty);

    // NaN become true if > Qty doesn't have any value or have a string
    return (isNaN(item_qty) || item_qty === 0 ? 1 : item_qty);
}

function addqty(Quantity, isAdd_qty) {
    var itemQty = parseInt(Quantity);

    // true for Add Qty 
    if (isAdd_qty) {
        itemQty < 99 ? itemQty++ : 99;
    }
    else {
        itemQty > 1 ? itemQty-- : 1
    }

    return itemQty;
}

// use app variable as local scope, i.e can access inside this JS only
(function (app) {
    'use strict';

    // show the book details in 2nd column on > /Book/Info
    var bookDetail_Controllerfn = function ($rsc, svc, $stateParams) {
        var that = this;

        //alert('Selected book id is   ' + $stateParams.bookId);
        that.bookInfo = {};
        that.combo = {
            combo_status: {},
            comboNames: {},
            comboInfo: []
        };

        that.book_cart_count = 1;

        that.addCartqty = function (isAdd_qty) {
            that.book_cart_count = addqty(that.book_cart_count, isAdd_qty);
        }

        that.check_cart = function () {
            that.book_cart_count = checkCart_Value(that.book_cart_count);
        }

        svc.get_bookDetail($stateParams.bookId).then(function (data) {
            that.bookInfo = data.result.book_info;
            that.combo.combo_status = data.result.bookCombo_status;  // add a new property

            // making a new Entity for combo :)
            if (that.combo.combo_status) {
                that.combo.comboNames = data.result.comboInfo.map(function (entity) {
                    that.combo.comboInfo.push(
                        angular.extend(entity.bundleInfo, { bookInfo: entity.bookInfo })
                        );

                    // return statement must be at the end bcoz line below it won't be execute
                    return entity.bundleInfo.Name;
                });

            }

            var bookId = data.result.book_info.BookId;
            bookSuggestion(bookId);
            bookRecommend(bookId);
        });

        that.addtoCart = function (bookInfo, itemQty) {
            // use variable to initialise value of it rather than manipulation to direct $scope/$rootScope variable
            $rsc.cart.Items = add_itemInCart(bookInfo, itemQty);
            $rsc.notify_fx('Item is added in your cart !', 'info');
        }

        that.suggestionList = [];
        function bookSuggestion(bookId) {

            svc.get_booksuggestion(bookId).then(function (data) {
                that.suggestionList = data.result;
            });
        }

        that.recommendList = [];
        function bookRecommend(bookId) {
            svc.getbook_recommends(bookId).then(function (data) {
                that.recommendList = data.result;
            });
        }
    }

    var search_model = {
        classId: 1,
        subjectId: '',
        cateogysId: []
    };

    // show the book categories in 1st column on > /Book/Info
    var book_search_Controllerfn = function ($sc, svc, sharedSvc, $state) {

        // contain array of list / a Modal
        $sc.classList = [];
        $sc.subjectList = [];
        $sc.book_categorys = [];

        // contains a single model >  initialise default values
        $sc.search_model = {
            selected_class: search_model.classId,
            selected_subject: search_model.subjectId,
            book_category_listId: search_model.cateogysId
        };

        $sc.get_subjects = function (classId) {
            sharedSvc.get_subject_byClassId(classId).then(function (d) {
                $sc.subjectList = d.result;
            }, function () {
                console.log("error occured");
            });
        }

        $sc.searchBooks = function () {

            search_model.classId = $sc.search_model.selected_class,
            search_model.subjectId = $sc.search_model.selected_subject || null,
            search_model.cateogysId = getCategoriesId($sc.search_model.book_category_listId)

            if (!$state.is('book_list'))
                $state.go('book_list');         // search_model is global var > use to serach book so no need to pass as param
            else
                $sc.$emit('searchBooks');       // send data to parent ctroler
        }

        // class list load at runtime > 1st way
        function getAll_classess() {
            sharedSvc.getAll_class().then(function (data) {
                $sc.classList = data.result;

                // set default class to 1
                $sc.search_model.selected_class = search_model.classId;

                // default load subjet of class 1
                $sc.get_subjects(data.result[0].Id);

            }, function () {
                console.log('in error');
            });
        }
        getAll_classess();

        // Book categories list load at runtime
        function loadBookcategory() {
            sharedSvc.get_bookCategorys().then(function (data) {
                $sc.book_categorys = data.result;

                angular.forEach($sc.book_categorys, function (value, key) {
                    value.ticked = true;                // adding new property to each item from List
                });

            }, function () {
                console.log('in error');
            });
        };
        loadBookcategory();

        function getCategoriesId(book_category_listId) {
            var array = [];
            if (book_category_listId.length > 0) {
                angular.forEach(book_category_listId, function (data, key) {
                    array.push(parseInt(data.id));
                });
            }
            return array;
        }

    }

    // global variable > ajax wont call if its called whil redirect to other pages :)
    var bookList = [],
        bundleList = {
            comboNames: {},
            comboInfo: []
        };

    var book_searchResult_Controllerfn = function ($sc, svc) {
        $sc.bookList = bookList;
        $sc.bundleList = bundleList;

        $sc.$on('searchBooks', function (event, param) {
            $sc.get_books(search_model);
        });

        // Self invoke > anonynous fx with params
        $sc.get_books = function (model) {
            svc.searchBooks(model).then(function (d) {
                bookList = d.result;
                $sc.bookList = bookList;

                get_Bundles(model.classId);

                // use to show text > Added' OR 'Buy Now' option with Book list
                //$sc.bookList.map(function (book) {      // enamurate each items in list
                //    $rsc.buyBook_isDisable[book.BookId] = false;
                //});
            });
        }

        $sc.get_books(search_model);

        // TO DO : change in fx to call bundle list
        function get_Bundles(classId) {

            svc.getbook_bundles(classId).then(function (data) {

                bundleList.comboNames = data.result.map(function (entity) {
                    bundleList.comboInfo.push(
                        angular.extend(entity.bundleInfo, { bookInfo: entity.bookInfo })
                        );

                    // return statement must be at the end bcoz line below it won't be execute
                    return entity.bundleInfo.Name;
                });

                $sc.bundleList = bundleList;
            });
        }

    }

    app.controller('book_Detail_Controller', ['$rootScope', 'bookService', '$stateParams', bookDetail_Controllerfn])
        .controller('book_search_Controller', ['$scope', 'bookService', 'sharedService', '$state', book_search_Controllerfn])
        .controller('book_searchResult_Controller', ['$scope', 'bookService', book_searchResult_Controllerfn])

    ;

})(angular.module('Silverzone_app'));
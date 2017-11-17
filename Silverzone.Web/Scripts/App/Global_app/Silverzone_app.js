/// <reference path="../../Lib/angular-1.5.8.js" />

(function () {

    var configFn = function ($stateProvider, $urlRouterProvider, $httpProvider, ScrollBarsProvider) {

        // scroll config
        ScrollBarsProvider.defaults = {
           setHeight: 350,
            axis: 'y', // enable 2 axis scrollbars by default
            theme: 'dark',
            autoHideScrollbar: true
        };
           
        // Default Url, if any URL not match, redirect to it
        $urlRouterProvider.otherwise('/Site/Home');

        $stateProvider
            .state("silverzone_home", {
                url: '/Site/Home',
                templateUrl: 'templates/views/home.html',
                //controller: 'bookDetailController',
            })
            .state("book_list", {
                url: '/Site/Book/List',            // default URL + Home page is set by server side routing in route.config
                templateUrl: 'templates/views/Book/book_list.html',         // templateUrl must not be start with '/'
                controller: 'book_searchResult_Controller'
            })
            .state("book_details", {
                url: '/Site/Book/Info/{bookId:int}/{bookTitle}',      // url: '/Site/city/{cityId:int}/{cityNameUrl}'  > int use for formating string, otherwise we can do {cityId} only
                templateUrl: 'templates/views/Book/book_detail.html',           //  Note : Don't prefix '/' in Any URL's bcoz we got it from base href 
                controller: 'book_Detail_Controller',
                controllerAs: 'bookDetail'
            })
            .state("cart_detail", {
                url: '/Site/viewcart',
                templateUrl: 'templates/views/cart/cart_detail.html',       // base URL > Set default at the END > "/"
                controller: 'cartDetailsController',
                controllerAs: 'cartDetail',
                data: {                  // using to check user is login or not while come to this page
                    requireCart: true
                }
            })
            .state("cart_address_detail", {
                url: '/Site/checkout',
                templateUrl: 'templates/views/cart/user_address.html',
                controller: 'user_addressController',
                data: {                  // using to check user is login or not while come to this page
                    requireLogin: true,
                    requireCart: true
                }
            })
            .state("cart_summary", {
                url: '/Site/summary',
                templateUrl: 'templates/views/cart/order_summary.html',
                controller: 'cartDetailsController',
                controllerAs: 'cartDetail',
                params: { shipping_adress: null },
                data: {
                    requireLogin: true,
                    requireCart: true
                }
            })
            .state("payment_summary", {       // this will show when user payment is done by using online/offline
                url: '/Site/orderSummary',
                templateUrl: 'templates/views/cart/order_summary.html',
                controller: 'cartDetailsController',
                controllerAs: 'cartDetail',
                params: { payment_mode: null },       // to send these parameter values when redirect to this page
                data: {
                    requireLogin: true,
                    requireCart: true
                }
            })
            .state("payment_method", {
                url: '/Site/paymentMethod',            // default URL + Home page is set by server side routing in route.config
                templateUrl: 'templates/views/cart/paymentMethod.html',
                controller: 'cartDetailsController',
                params: { paymentProceed_btn_isClicked: false },
                data: {
                    requireLogin: true,
                    requireCart: true
                }
            })
            .state("empty_cart", {
                url: '/Site/emptyCart',            // default URL + Home page is set by server side routing in route.config
                templateUrl: 'templates/views/cart/empty_cart.html',
                controller: 'cartDetailsController',
            })

            // User Panel from Area script start from Here :)
            .state("user_profile", {
                url: '/Site/User/Profile',
                templateUrl: 'Areas/User/templates/Views/profile.html',
                controller: 'user_profile_Controller',
                data: {
                    requireLogin: true
                }
            })
            .state("user_order", {
                url: '/Site/User/myOrders',
                templateUrl: 'Areas/User/templates/Views/orders.html',
                controller: 'user_order_Controller',
                data: {         // suppose we r on the page & clear history then to perform any further operation
                    requireLogin: true
                }
            })
            .state("user_orderDetail", {
                url: '/Site/User/Order/Detail',
                templateUrl: 'Areas/User/templates/Views/orderInfo.html',
                controller: 'user_order_Controller',
                params: { orderId: null },
                data: {
                    requireLogin: true
                }
            })
            .state("user_profileAdress", {          // same as checkout page > sharing the page & controller
                url: '/Site/User/Profile/Addressess',
                templateUrl: 'Areas/User/templates/Views/user_shipping_Address.html',
                controller: 'user_addressController',
                data: {                  // using to check user is login or not while come to this page
                    requireLogin: true
                }
            })
            .state("user_profileManagePassword", {
                url: '/Site/User/Profile/changePassword',
                templateUrl: 'Areas/User/templates/Views/changePassword.html',
                controller: 'user_profileManage_Password_Controller',
                data: {
                    requireLogin: true
                }
            })
            .state("user_profile_updateEmailorMobile", {
                url: '/Site/User/Profile/updateEmailorMobile',
                templateUrl: 'Areas/User/templates/Views/change_Mobile_Email.html',
                controller: 'user_profile_change_Mobile_Email_Controller',
                data: {
                    requireLogin: true
                }
            })
             .state("user_quiz", {
                 url: '/Site/User/Profile/Quiz',
                 templateUrl: 'Areas/User/templates/Views/user_quiz.html',
                 controller: 'user_quiz_Controller',
                 data: {
                     requireLogin: true
                 }
             })
              .state("user_mega_quiz", {
                  url: '/Site/User/Profile/MegaQuiz',
                  templateUrl: 'Areas/User/templates/Views/user_quiz.html',
                  controller: 'user_quiz_Controller',
                  data: {
                      requireLogin: true
                  }
              })
            .state("user_quiz_history", {
                url: '/Site/User/Profile/Quiz/History',
                templateUrl: 'Areas/User/templates/Views/user_quiz_history.html',
                controller: 'user_quiz_history_Controller',
                data: {
                    requireLogin: true
                }
            })

            // handling error page > otherwise deafult URL show instead of error URL 
            .state("errorPage", {
                url: '/Site/pageNotFound',
                templateUrl: 'templates/views/errorPages/404_page.html'
            })

            .state("shoppingError", {
                url: '/Site/shoppingError',
                templateUrl: 'templates/views/errorPages/shopping_errorPage.html'
            })

        ;
    }

    // it contains global setting like > global variable used across diffrent ctrlers in this application
    var runFn = function ($rsc, loginModal, $state, $injector, webUrl) {
        webUrl.module = 'Site';         // value provider

        // can change value of it in another ctrl
        // use this here bcoz this ctrler initialise once
        $rsc.cart = {
            Items: new Array(),
            shipping_Amount: 0,
            shipping_Charges: 0,
            total_Amount: 0
        }

        $rsc.Newshopping = false;
        // use to show BuyNow or Added fxnality while add book in cart on Book List Page
        $rsc.buyBook_isDisable = [];

        // use to maintain width of the page while sharing it :)
        $rsc.bindClass = true;

    
        // it will call every time when ever we redirect to other URL/page
        // toState contain state name > means URL to redirect 
        // toParams contains paramerter with that URL 
        $rsc.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {

            if ((toState.data !== undefined && toState.data.requireLogin)
                && $rsc.user.currentUser === '') {         // when user in not logged in & login is required !
                event.preventDefault();

                // server side clear > formauthetiation cookie
                $injector.get('user_Account_Service').Logout();     // calling directly service method

                // if i m on the viewcart page then dont want to move on home page > just show login Pop UP
                if (fromState.name !== "cart_detail") {
                    // if user access a page which needs login then move him to home page & show login POPUP
                    $state.go('book_list');
                }

                // get me a login modal!
                loginModal()
                .then(function () {
                    return $state.go(toState.name, toParams);
                })
                .catch(function () {
                    //return $state.go('welcome');      // we want to stay current page
                });
            }

            // to dynmically bind class for adjust container width on UI
            if (toState.name === 'book_details' || toState.name === 'book_list' || toState.name === 'cart_address_detail')
                $rsc.bindClass = true;
            else
                $rsc.bindClass = false;

            // after payment made
            if ($rsc.Newshopping) {      // it will be true when user confirmed his order by online/offline payment 
                cartItems_array = [];         // empty array >  global variable in bookcontroller.js
                $rsc.cart.Items = [];

                $rsc.buyBook_isDisable = [];
                $rsc.Newshopping = false;
            }

            // when cart is empty while navigations on pages (which required cart items)
            if ((toState.data !== undefined && toState.data.requireCart)
                && $rsc.user.currentUser !== '' && $rsc.cart.Items.length === 0) {
                event.preventDefault();     // if i remove it > it will move to actual redirected page
                $state.go('empty_cart');
            }

        });

    }


    var moduleDependencies =
        [
            'ngScrollbars',               // https://github.com/iominh/ng-scrollbars,
            'Silverzone_app.Common'     // load dependency from a common js file
        ];

    angular.module('Silverzone_app', moduleDependencies)
    .config(['$stateProvider', '$urlRouterProvider', '$httpProvider', 'ScrollBarsProvider', configFn])
    .run(['$rootScope', 'login_modalService', '$state', '$injector', 'webUrl', runFn])
    ;

   

})();
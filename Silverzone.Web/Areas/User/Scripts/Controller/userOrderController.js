
(function () {

    var selected_orderId = '';

    var orderfn = function ($sc, $rsc, svc, $stateParams, $state) {
        $sc.orderList = [];

        svc.get_userOrders().then(function (data) {
            $sc.orderList = data.result;
        }, function () {
            console.log('in error');
        });

        // to retriive order information on page > 'User/Order/Detail'
        if ($state.is('user_orderDetail')) {

            $sc.orderInfo = {};
            selected_orderId = $stateParams.orderId ? $stateParams.orderId : selected_orderId;      // check $stateParams.shipping_adress contain value or not

            if (selected_orderId) {

                svc.get_OrderInfo_byOrderId(selected_orderId).then(function (data) {
                    $sc.orderInfo = data.result;
                }, function () {
                    console.log('in error');
                });
            }
        }

    }

    angular.module('Silverzone_app')
        .controller('user_order_Controller', ['$scope', '$rootScope', 'userProfile_service', '$stateParams', '$state', orderfn]);


})();
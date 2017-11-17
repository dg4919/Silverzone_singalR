/// <reference path="../angular-1.5.8.js" />

// Use to work with Website Layout functionality
(function (app) {

    // when redirect to other URL > then there respective ctrler is called & 'site_MasterController' called once
    // children controller inside this ctrler > can access scope variable of this ctrl
    var site_MasterControllerfn = function ($sc, $rsc, $modal, $state, loginModal, acount_svc, localStorageService, $stateParams) {
        //$sc.user = { mobileNo: "8800110538" };      
      
        $rsc.Show_signInModal = loginModal;         // auto open Login Modal > when we call service

        $rsc.user_logOut = function () {
            acount_svc.Logout();
            $state.go('book_list');
        }

        $sc.show_cartDetail_popUp = function () {
            // return true when URL is matched
            // but i dont want to show popup on these URL so will use (!is_show_cartDetail_popUp)

            return ($state.is("cart_detail") ||
                    $state.is("cart_address_detail") ||
                    $state.is("cart_summary") ||
                    $state.is("payment_summary") ||
                    $state.is("payment_method")
                );
        }             

    }


    app.controller('site_MasterController', ['$scope', '$rootScope', '$uibModal', '$state', 'login_modalService', 'user_Account_Service', 'localStorageService', site_MasterControllerfn]);

})(angular.module('Silverzone_app'));
(function (app) {

    app.service('login_modalService',['$uibModal', function ($modal) {

        return function () {
            var instance = $modal.open({
                templateUrl: 'templates/modal/userLoginModal.html',
                windowClass: 'loginModal',
                controller: 'user_loginController',     // No need to specify this controller on HTML page explicitly    
            });

            // when modal will close
            return instance.result;
        }

    }]);

})(angular.module('Silverzone_app'));

(function (app) {
    var Home_fun = function ($scope ) {        
        $scope.welcome = 'Welcome To Silverzone';
    }
    
    app.controller('homeController', ['$scope', Home_fun]);

})(angular.module('SilverzoneERP_App'));
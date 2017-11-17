/// <reference path="../angular-1.5.8.js" />

// Use to work with Website Layout functionality
(function (app) {

    var masterCtrl_fun = function ($scope, $rootScope, Service, $location, localStorageService, $filter) {
        debugger;
        $rootScope.UserInfo = localStorageService.get('UserInfo');
        $scope.Color = localStorageService.get('BackColor');
        $rootScope.EventInfo = localStorageService.get('EventInfo');
        $rootScope.Permission = localStorageService.get('Permission');
        $rootScope.Title = 'Silverzone';

        $rootScope.SelectedEvent = localStorageService.get('SelectedEvent');
        //$scope.serachText = {};

        $scope.Logout = function () {
            Service.Logout();
        }

        $rootScope.StatusInfo = '';
        if ($rootScope.UserInfo == null)
            $scope.Logout();

        $scope.ChangeEvent = function (EventId, EventColor) {
            debugger;
            var Event_filter = $filter("filter")($rootScope.EventInfo, { EventId: EventId }, true);
            if (Event_filter.length != 0) {
                $rootScope.SelectedEvent = Event_filter[0];
                $rootScope.SelectedEvent.EventColor = EventColor;
            }

            localStorageService.set('SelectedEvent', $rootScope.SelectedEvent);
        }

        $scope.ChangeBackColor = function () {
            if (!$rootScope.UserInfo.show_menu)
                return 'login';
            else if (!angular.isUndefined($scope.Color))
                return $scope.Color;
            else
                return '';
        }
        $scope.MainMenu = function (data) {
            debugger;
            data.Active = !data.Active;
        }

        $scope.SetPermission = function (data) {
            $rootScope.StatusInfo = "";
            $rootScope.Permission = data;
            localStorageService.set('Permission', data);
        }

        $scope.isMenu_srching = false;

    }

    app.controller('masterController', ['$scope', '$rootScope', 'Service', '$location', 'localStorageService', '$filter', masterCtrl_fun]);

})(angular.module('SilverzoneERP_App'));

(function (app) {
    'use strict';

    var Fn = function ($rootScope) {
        return {
            on: function (eventName, callback) {
                var connection = $.connection.chatHub;

                $.connection.hub.url = 'http://localhost:55615/signalr';

                connection.on(eventName, function () {
                    var args = arguments;
                    $rootScope.$apply(function () {
                        callback.apply(connection, args);
                    });
                });

                $.connection.hub.start()
                 .done(function () {
                     console.log('connection has been started with... ' + $.connection.hub.id);
                 })
                 .fail(function (err) {
                     console.log('Could not connect ' + err);
                 });

            }
        };
    }

    app.factory('signalRHubProxy', ['$rootScope', Fn]);

})(angular.module('Silverzone_admin_app'));


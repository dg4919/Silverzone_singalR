/// <reference path="../../Lib/angular-1.5.8.js" />

(function (app) {

    app.filter('dateFormat', function () {

        return function (input, formatType) {
            // using jquery moment.js
            return (moment(input).format(formatType));
        }
    });

})(angular.module('Silverzone_app'));


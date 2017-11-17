(function (app) {
    'use strict';

    app
       .filter('split', function () {
           return function (input, splitChar, splitIndex) {
               // do some bounds checking here to ensure it has that index
               return input.split(splitChar)[splitIndex];
           }
       })
       .filter('dateFormat', function () {

        return function (input, formatType) {
            // using jquery moment.js
            return (moment(input).format(formatType));
        }
       })
       .filter('stringFilter', function () {   // use to make URL for book details

            return function (input) {
                // using jquery moment.js
                return (input.trim().replace(/ /g, '-'));       // replace space with '-' in a string
            }
        })
    ;


})(angular.module('Silverzone_app'));
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
       }).
    filter('groupBy', function () {
        return _.memoize(function (items, field) {
            return _.groupBy(items, field);
        }
            );
    })
    .filter('unique', function () {
         return function (arr, field) {
             return _.uniq(arr, function (a) { return a[field]; });
         };
    })
    .filter('color', function () {
        return function (input, type) {
            // do some bounds checking here to ensure it has that index
            var valu = parseInt(input);
            if (type === 1) {
                if (valu === 0)
                    return '#f25656';
                else if (valu === 1)
                    return '#ffc107';
                else if (valu === 2)
                    return '#00ACED';
            }
            else if (type === 2) {
                if (valu === 0)
                    return 'Book Not Found';
                else if (valu === 1)
                    return 'Quantity Not Matched';
                else if (valu === 2)
                    return 'OK';
            }
        }
    })
    ;


})(angular.module('SilverzoneERP_App'));
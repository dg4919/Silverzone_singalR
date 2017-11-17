(function () {
    'use strict';

    // just add random-backgroundcolor to the element you want to give a random background color
    // autmatically change color in 1 second interval

    angular.module('customDirective_module', [])
        .directive("randomColor", ['$interval', function ($interval) {       // if wee don't provide dependency as array will throw error after minification of JS
            return {
                restrict: 'AE',
                replace: false,
                link: function (scope, element, attr) {
                    $interval(function () {
                        //generate random color
                        var color = '#' + (Math.random() * 0xFFFFFF << 0).toString(16);

                        //Add random background class to selected element
                        element.css('color', color);
                    }, 500);
                }
            }
        }])

        .directive('numberOnly', function () {

            function validate_Number(scope, element, attrs, modelCtrl) {
                modelCtrl.$parsers.push(function (inputValue) {
                    if (inputValue == undefined) return '';         // aftr return blank, below code won't work

                    // allow numeric values only..
                    var transformedInput = inputValue.replace(/[^0-9]/g, '');
                    if (transformedInput != inputValue) {
                        modelCtrl.$setViewValue(transformedInput);
                        modelCtrl.$render();
                    }
                    return transformedInput;
                });
            }

            return {
                restrict: 'A',
                require: 'ngModel',
                link: validate_Number
            };
        })

        // only 2 decimal values
        .directive('decimalOnly', function () {

            function validate_Number(scope, element, attrs, modelCtrl) {
                modelCtrl.$parsers.push(function (val) {

                    if (angular.isUndefined(val)) {
                        var val = '';
                    }

                    var clean = val.replace(/[^-0-9\.]/g, '');
                    var negativeCheck = clean.split('-');
                    var decimalCheck = clean.split('.');
                    if (!angular.isUndefined(negativeCheck[1])) {
                        negativeCheck[1] = negativeCheck[1].slice(0, negativeCheck[1].length);
                        clean = negativeCheck[0] + '-' + negativeCheck[1];
                        if (negativeCheck[0].length > 0)
                            clean = negativeCheck[0];
                    }

                    if (!angular.isUndefined(decimalCheck[1])) {
                        decimalCheck[1] = decimalCheck[1].slice(0, 2);
                        clean = decimalCheck[0] + '.' + decimalCheck[1];
                    }

                    if (val !== clean) {
                        modelCtrl.$setViewValue(clean);
                        modelCtrl.$render();
                    }
                    return clean;
                });
            }

            return {
                restrict: 'A',
                require: 'ngModel',
                link: validate_Number
            };
        })

    ;

})();
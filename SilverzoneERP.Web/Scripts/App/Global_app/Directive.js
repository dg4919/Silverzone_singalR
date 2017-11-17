(function (app) {

    app
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
    .directive('greaterThan', [function () {
        return {
            restrict: 'A',
            require: 'ngModel',
            link: function (scope, elem, attrs, control) {
                debugger;
                // alert('Test');
                var checker = function () {
                    debugger;
                    //get the value of the first number
                    var num1 = scope.$eval(attrs.ngModel);
                    if (angular.isUndefined(num1))
                        num1 = 0;
                    else
                        num1 = parseInt(num1);
                    //get the value of the second number
                    var num2 = scope.$eval(attrs.greaterThan);
                    if (angular.isUndefined(num2))
                        num2 = 0;
                    else
                        num2 = parseInt(num2);
                    if (num1 > num2)
                        return true
                    else
                        return false

                };
                scope.$watch(checker, function (n) {

                    //set the form control to valid if both 
                    //passwords are the same, else invalid
                    control.$setValidity("greater", n);
                });
            }
        };
    }])
    .directive('loading', ['$http', function ($http) {
        return {
            restrict: 'A',
            link: function (scope, elm, attrs) {
                scope.isLoading = function () {
                    return $http.pendingRequests.length > 0;
                };

                scope.$watch(scope.isLoading, function (v) {
                    if (v) {
                        elm.show();
                    } else {
                        elm.hide();
                    }
                });
            }
        };

    }])
    .directive("ngFileSelect", function () {
        return {
            link: function ($scope, el) {

                el.bind("change", function (e) {

                    $scope.file = (e.srcElement || e.target).files[0];
                    $scope.UploadImage($scope.file);
                })
            }
        }
    })
    .directive('passwordMatch', [function () {
        return {
            restrict: 'A',
            scope: true,
            require: 'ngModel',
            link: function (scope, elem, attrs, control) {
                var checker = function () {

                    //get the value of the first password
                    var e1 = scope.$eval(attrs.ngModel);

                    //get the value of the other password  
                    var e2 = scope.$eval(attrs.passwordMatch);
                    return e1 == e2;
                };
                scope.$watch(checker, function (n) {

                    //set the form control to valid if both 
                    //passwords are the same, else invalid
                    control.$setValidity("unique", n);
                });
            }
        };
    }])
    .directive('onlyNumbers', function () {
        debugger;
        return {
            restrict: 'A',
            link: function (scope, elm, attrs, ctrl) {
                debugger;
                elm.on('keydown', function (event) {
                   // if (event.shiftKey) { event.preventDefault(); return false; }
                    //console.log(event.which);
                    if ([8, 13, 27, 37, 38, 39, 40, 9, 123, 12, 18].indexOf(event.which) > -1) {
                        // backspace, enter, escape, arrows
                        return true;
                    } else if (event.which >= 49 && event.which <= 57) {
                        // numbers
                        return true;
                    } else if (event.which >= 96 && event.which <= 105) {
                        // numpad number
                        return true;
                    }
                    else {
                        event.preventDefault();
                        return false;
                    }
                });
            }
        }
    })
    .directive('onlyCaracter', function () {
             debugger;
             return {
                 restrict: 'A',
                 link: function (scope, elm, attrs, ctrl) {
                     debugger;
                     elm.on('keydown', function (event) {
                        // if (event.shiftKey) { event.preventDefault(); return false; }
                         //console.log(event.which);
                         if ([8, 13, 27, 37, 38, 39, 40, 9, 123, 12, 18].indexOf(event.which) > -1) {
                             // backspace, enter, escape, arrows
                             return true;
                         } else if (event.which >= 65 && event.which <= 90) {
                             // numbers
                             return true;
                         } else if (event.which >= 97 && event.which <= 122) {
                             // numpad number
                             return true;
                         }
                         else {
                             event.preventDefault();
                             return false;
                         }
                     });
                 }
             }
    })
    .directive('answerOption', function () {
        debugger;
        return {
            require: 'ngModel',
            restrict: 'A',
            link: function (scope, elm, attrs, ctrl) {
                debugger;
                elm.on('keydown', function (event) {
                    console.log(ctrl);

                    if ([8, 13, 27, 37, 38, 39, 40, 9, 123, 12, 18].indexOf(event.which) > -1) {
                        // backspace, enter, escape, arrows
                        return true;
                    } else if (event.which >= 65 && event.which <= 70) {
                        // Allow capital latter
                        var value = String.fromCharCode(event.which);
                        $(this).val(value);
                        ctrl.$setViewValue(value);

                        return false;
                    } else if (event.which >= 97 && event.which <= 102) {
                        // Allow small latter and convert into capital
                        var value = String.fromCharCode(event.which - 32);
                        $(this).val(value);
                        ctrl.$setViewValue(value);

                        return false;
                    }
                    else if (event.which >= 49 && event.which <= 53) {
                        var value = String.fromCharCode(16 + event.which);
                        $(this).val(value);
                        ctrl.$setViewValue(value);                        
                        return false;
                    }
                    else {
                        event.preventDefault();
                        return false;
                    }
                });
            }
        }
    })
    .directive('examDatePicker', function () {
        debugger;
        return {
            require: 'ngModel',
            restrict: 'A',
            link: function (scope, elm, attrs, ctrl) {
                // to apply bootstarp jqeury Datepicker
                var maxExamDate = new Date();
                maxExamDate.setYear(maxExamDate.getFullYear() + 1);
                elm.datetimepicker({
                    minDate: new Date(),
                    maxDate: maxExamDate,
                    format: 'DD-MMM-YYYY'
                });

                elm.on("dp.change", function () {
                    // contains selected date into txt box
                    //alert('Test');
                    ctrl.$setViewValue(angular.element(elm).val());
                    ctrl.$render();      // save new value
                });
            }
        }
    })
    .directive('paymentDatePicker', function () {
        debugger;
        return {
            require: 'ngModel',
            restrict: 'A',
            link: function (scope, elm, attrs, ctrl) {
                // to apply bootstarp jqeury Datepicker                
                elm.datetimepicker({                  
                    maxDate: 'now',
                    format: 'DD-MMM-YYYY'
                });

                elm.on("dp.change", function () {
                    // contains selected date into txt box
                    //alert('Test');
                    ctrl.$setViewValue(angular.element(elm).val());
                    ctrl.$render();      // save new value
                });
            }
        }
    })
    .directive('showPicker', function () {
        debugger;
        return {
            require: 'ngModel',
            restrict: 'A',
            link: function (scope, elm, attrs, ctrl) {
                // to apply bootstarp jqeury Datepicker
                elm.datetimepicker({
                    maxDate: 'now'
                });

                elm.on("dp.change", function () {
                    // contains selected date into txt box
                    ctrl.$setViewValue(angular.element(elm).val());
                    ctrl.$render();      // save new value
                });
            }
        }
    })
    .directive("digitalClock", function ($timeout, dateFilter) {
        return function (scope, element, attrs) {

            //element.addClass('alert alert-info text-center clock');

            scope.updateClock = function () {
                $timeout(function () {
                    element.text(dateFilter(new Date(), 'dd-MMM-yyyy   hh:mm:ss a'));
                    scope.updateClock();
                }, 1000);
            };
            scope.updateClock();
        };
    })
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
                     decimalCheck[1] = decimalCheck[1].slice(0, 3);
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

})(angular.module('SilverzoneERP_App'));




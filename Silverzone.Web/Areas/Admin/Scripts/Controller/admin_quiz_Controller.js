/// <reference path="C:\dg4919_tfs\silverzoneLatest\Silverzone.Web\Scripts/Lib/angularjs/angular-1.5.0.js" />

(function () {

    'use strict';

    var quiz_createFn = function ($sc, $rsc, svc, $state, $timeout, $stateParams, $filter) {
        $sc.optionArr = new Array(5);
        $sc.quizTypes = ['Normal Quiz', 'Mega Quiz'];

        $sc.quizInfo = {        // we can add dynamic props
            optionModel: {      // we can't add dynamic prop if its not define
                options: new Array(5),          // we need to define dynamic array to initialise it
                options_ImageUrl: new Array(5)  // 5 null object will allocated
            }
        }

        $sc.submit_data = function (form) {

            if (form.validate()) {
                var quizid = $sc.quizInfo.Id;

                if ($sc.quizInfo.AnswerId === undefined || $sc.quizInfo.AnswerId === '') {
                    alert("Select answer of this question !");
                    return false;
                }

                if (quizid === undefined || quizid === '') {
                    svc.createQuiz($sc.quizInfo).then(function (d) {

                        if (d.result === 'success') {
                            $rsc.notify_fx('Quiz Question is created !', 'success');

                            $timeout(function () {
                                $state.go('quiz_list');
                            }, 2000);
                        }
                        else if (d.result === 'exist')
                            $rsc.notify_fx('Quiz is already created on selected date, try another !', 'warning');
                        else
                            $rsc.notify_fx('Oops! Something went wrong, try again :(', 'error');
                    });
                }
                else {
                    angular.forEach($sc.quizInfo.optionModel.Id, function (quiz_OptionId, index) {
                        if (parseInt($sc.quizInfo.AnswerId) === quiz_OptionId) {
                            $sc.quizInfo.AnswerId = index;
                            return true;
                        }
                    });

                    svc.updateQuiz($sc.quizInfo).then(function (d) {
                        if (d.result === 'success') {

                            $rsc.notify_fx('Quiz Question is updated !', 'info');

                            $timeout(function () {
                                $state.go('quiz_list');
                            }, 2000);
                        }
                        else if (d.result === 'exist') 
                            $rsc.notify_fx('Quiz is already created on selected date, try another !', 'warning');
                        else
                            $rsc.notify_fx('Oops! Something went wrong, try again :(', 'error');
                    });
                }
            }
        }

        $sc.validationOptions = {
            rules: {
                quiz_question: {            // use with name attribute in control
                    required: true
                },
                //quiz_option: {
                //    required: true
                //},
                //AnswerTextRadio: {
                //    required: true
                //},
                Points: {
                    required: true
                },
                End_Date: {
                    required: true
                },
                Prize: {
                    required: true
                },
                Active_Date: {
                    required: true
                }
            }
        }

        //****************** Quiz Edit ******************
        $sc.is_Edit = false;
        if ($stateParams.quizId !== undefined) {
            svc.get_QuizbyId($stateParams.quizId).then(function (d) {
                var result = d.result;
                result.Active_Date = $filter('dateFormat')(result.Active_Date, 'MM/DD/YYYY');

                var _model = {
                    Id: [],
                    options: [],
                    options_ImageUrl: [],
                }

                result.optionModel.map(function (entity, key) {
                    _model.Id.push(entity.Id);
                    _model.options.push(entity.options);
                    _model.options_ImageUrl.push(entity.options_ImageUrl);
                });

                $sc.quizInfo = result;
                $sc.quizInfo.optionModel = _model;

                $sc.is_Edit = true;
            });
        }
    }

    var quiz_listFn = function ($sc, $rsc, svc) {
        $sc.quizList = [];

        svc.get_AllQuiz().then(function (d) {
            $sc.quizList = d.result;
        });
    }

    angular.module('Silverzone_app')
        .controller('quiz_create', ['$scope', '$rootScope', 'admin_quiz_Service', '$state', '$timeout', '$stateParams', '$filter', quiz_createFn])
        .controller('quiz_list', ['$scope', '$rootScope', 'admin_quiz_Service', quiz_listFn])

    ;

})();




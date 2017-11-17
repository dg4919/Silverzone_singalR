/// <reference path="C:\dg4919_tfs\silverzoneLatest\Silverzone.Web\Scripts/Lib/angularjs/angular-1.5.0.js" />

(function () {

    var profilefn = function ($sc, $rsc, svc, localStorageService, sharedSvc, accountSvc, $filter) {

        $sc.model = angular.copy($rsc.user.currentUser);

        $sc.uploadImage = function (element) {
            console.log("I hav Changed");

            if (element.files.length > 0) {
                svc.uploadImage(element.files).then(function (data) {
                    //console.log('file saved sucesfully..   ' + data.result);

                    $sc.model.ProfilePic = data.result[0];
                },
                function (e) {
                    console.log('in fail.. ' + e);
                });
            }
        }

        $sc.save_userInfo = function (model) {
            var _model = angular.copy(model);

            //alert('i m in form saved..');
            _model.DOB = new Date(_model.DOB);      // convert date into datetime

            svc.update_userinfo(_model).then(function (data) {
                if (data.result === 'success') {

                    accountSvc.saveUserInfo(
                        data.user,
                        localStorageService.get('authorizeData').tokenInfo
                        );

                    $rsc.notify_fx('Profile is updated !', 'info');
                }

            },
                function (e) {
                    console.log('in fail.. ' + e);
                });
        }

        $sc.classList = [];

        sharedSvc.getAll_class().then(function (data) {
            $sc.classList = data.result;
        }, function () {
            console.log('in error');
        });
    }

    var managePasswordfn = function ($sc, $rsc, svc) {
        $sc.model = {};

        $sc.save_userInfo = function (form) {
            if (form.validate()) {
                //alert('form is validated...');

                svc.change_userPassword($sc.model).then(function (data) {
                    if (data.result === 'success')
                        $rsc.notify_fx('Profile is updated !', 'info');

                    else
                        $rsc.notify_fx('Invalid old password. ', 'danger');
                },
               function (e) {
                   console.log('in fail.. ' + e);
               });
            }
        }

        $sc.validationOptions = {
            rules: {
                newPassword: {            // use with name attribute in control
                    required: true
                },
                oldPassword: {
                    required: true
                },
                oldPassword1: {
                    equalTo: "#oldPassword"
                }
            }
        }

    }

    var change_emailMobile_fn = function ($sc, $rsc, svc) {
        var myModel = {
            EmailID: angular.copy($rsc.user.currentUser.EmailID),
            MobileNumber: angular.copy($rsc.user.currentUser.MobileNumber)
        }

        // 1 way binding > changes in model will not reflect in myModel
        $sc.model = angular.copy(myModel);

        $sc.is_disableBtn = true;           // use to disable/enable 'save changes' btn 

        function intialise_Value(type) {

            if (type === 'email' || type === undefined) {
                $sc.is_emailEdit = false;
                $sc.email_hasTxt = $rsc.user.currentUser.EmailID ? true : false;
                $sc.Is_show_EmailOTP = false;
                $sc.Is_emailOTP_matched = false;
            }

            if (type === 'mobile' || type === undefined) {
                $sc.is_mobileEdit = false;
                $sc.mobile_hasTxt = $rsc.user.currentUser.MobileNumber ? true : false;
                $sc.Is_show_mobileOTP = false;
                $sc.Is_mobileOTP_matched = false;
            }
        }
        intialise_Value();

        $sc.chang_sts = function (type) {
            if (type === 'email' && $sc.model.EmailID) {
                $sc.is_emailEdit = true;
                $sc.Is_show_EmailOTP = false;
            }
            else if (type === 'mobile' && $sc.model.EmailID) {
                $sc.is_mobileEdit = true;
                $sc.Is_show_mobileOTP = false;
            }
        }

        $sc.cancel_sts = function (type) {
            if (type === 'email') {
                $sc.model.EmailID = angular.copy(myModel.EmailID);
                intialise_Value(type);
            }
            else if (type === 'mobile') {
                $sc.model.MobileNumber = angular.copy(myModel.MobileNumber);
                intialise_Value(type);
            }

        }

        $sc.sendOTP = function (type, form) {

            var Is_validate = type === 'email'
                ? form.validate({ ignore: ".userMobile" })
                : form.validate({ ignore: ".userEmail" });

            if (Is_validate) {
                var _model = getModel_byType(type);
                send_OTPfn(_model, type);
            }
        }

        $sc.validate_OTP = function (type) {

            var _model = getModel_byType(type);

            svc.verify_forgetLogin_OTP(_model).then(function (d) {

                if (d.result === 'ok') {
                    $sc.is_disableBtn = false;

                    type === 'email' ? $sc.Is_emailOTP_matched = true : $sc.Is_mobileOTP_matched = true;
                    $rsc.notify_fx('Verification code matched !.', 'success');
                }
                else if (d.result === 'expire') {
                    clearFields(type, false);
                    $rsc.notify_fx('Verification code expired, click resend link', 'info');
                }
                else if (d.result === 'invalid') {
                    clearFields(type, false);
                    $rsc.notify_fx('Invalid verification code.', 'error');
                }

            }, function () {
                console.log('in error');
            });
        }

        $sc.saveInfo = function (form) {
          
                var model = {
                    email: $sc.Is_emailOTP_matched ? $sc.model.EmailID : angular.copy($rsc.user.currentUser.EmailID),       // save value only if OTP is matched otherwise old value will bw=e save
                    mobile: $sc.Is_mobileOTP_matched ? $sc.model.MobileNumber : angular.copy($rsc.user.currentUser.MobileNumber)
                }

                svc.update_user_EmailMobile(model).then(function (d) {

                    if (d.result === 'success') {
                        $rsc.notify_fx('Profile is updated !', 'success');
                        $sc.is_disableBtn = true;
                        intialise_Value();

                        $rsc.user.currentUser.EmailID = angular.copy(model.email);
                        $rsc.user.currentUser.MobileNumber = angular.copy(model.mobile)
                    }
                    else if (d.result === 'notfound') {
                        $rsc.notify_fx('You are not registered user !.', 'warning');
                    }
                },
                 function (e) {
                     console.log('in fail.. ' + e);
                 });
        }

        function clearFields(type, status) {

            if (type === 'email') {
                $sc.Is_emailOTP_matched = status;
                $sc.model.email_OTP = '';
            }
            else {
                $sc.Is_mobileOTP_matched = status;
                $sc.model.mobile_OTP = '';
            }
        }

        function getModel_byType(type) {
            if (type === 'email') {
                return ({
                    userName: $sc.model.EmailID,
                    OTP_code: $sc.model.email_OTP,
                    verificationMode: 0       // for email
                });
            }
            else if (type === 'mobile') {
                return ({
                    userName: $sc.model.MobileNumber,
                    OTP_code: $sc.model.mobile_OTP,
                    verificationMode: 1       // for mobile
                });
            }
        }

        function send_OTPfn(model, type) {
            svc.change_user_emailMobile(model).then(function (d) {

                if (d.result === 'ok') {

                    if (type === 'email') {
                        $sc.Is_show_EmailOTP = true;
                        $sc.is_emailEdit = false;
                        $sc.email_hasTxt = true;
                        $sc.model.email_OTP = '';
                    }
                    else if (type === 'mobile') {
                        $sc.Is_show_mobileOTP = true;
                        $sc.is_mobileEdit = false;
                        $sc.mobile_hasTxt = true;
                        $sc.model.mobile_OTP = '';
                    }

                    if (model.verificationMode === 0)
                        $rsc.notify_fx('Verification code is sent to your email id', 'success');
                    else if (model.verificationMode === 1)
                        $rsc.notify_fx('Verification code is sent to your mobile', 'success');
                }
                else if (d.result === 'Block')
                    $rsc.notify_fx('Maximum attempts reached, Retry tommorrow.', 'warning');
                else if (d.result === 'exist')
                    $rsc.notify_fx('This user is already exist, try another !', 'error');
            },
      function (e) {
          console.log('in fail.. ' + e);
      });
        }

        $sc.validationOptions = {
            rules: {
                email: {
                    email: true,
                    //required: true
                },
                mobile: {
                    minlength: 10
                },
            }
        }
    }

    var user_quiz_fn = function ($sc, $rsc, svc, $state) {
        $sc.quizInfo = {};
        $sc.is_showQuiz = false;
        $sc.disableBtn = false;
        $sc.quizType = 1;
        $sc.charsArr = ['A', 'B', 'C', 'D', 'E'];

        if ($state.is('user_quiz')) {
            svc.get_userQuiz().then(function (d) {
                $sc.quizInfo = d.result;
                $sc.is_showQuiz = d.Is_showQuiz;
            });
        }
        else {
            svc.get_user_megaQuiz().then(function (d) {
                $sc.quizInfo = d.result;
                $sc.is_showQuiz = d.Is_showQuiz;
                $sc.quizType = 2;
                $sc.reason = d.reason;
            });
        }

        $sc.saveInfo = function (quizid, ansId) {
            svc.save_userQuiz({ quizId: quizid, answerId: ansId }).then(function (d) {
                if (d.result === 'ok')
                    $rsc.notify_fx('Congrats, Answer is correct, you earned ' + d.earnPoint + ' Points', "success");
                else
                    $rsc.notify_fx("Answer is not matched, try tomrrow !", "error");

                $sc.disableBtn = true;
            });
        }

    }

    var user_quiz_history_fn = function ($sc, $rsc, svc) {
        $sc.quizList = [];

        $sc.charsArr = ['A', 'B', 'C', 'D', 'E'];

        svc.get_userQuiz_history().then(function (d) {
            $sc.quizList = getData(d.result);
            $sc.totalPoints = d.quizPoints;
        });

        function getData(quizList) {
            var list = [];

            angular.forEach(quizList, function (entity, key) {
                var QuizAnswerId = entity.QuizAnswer,
                    UserAnswerId = entity.UserAnswer;

                angular.forEach(entity.QuizOptions, function (option_entity, key) {
                    if (QuizAnswerId === option_entity.Id)
                        entity.QuizAnswerId_index = key;      // add quiz(correct) answer property to list

                    if (UserAnswerId === option_entity.Id)
                        entity.UserAnswerId_index = key;
                });
                list.push(entity);
            });

            return list;
        }
    }

    angular.module('Silverzone_app')
        .controller('user_profile_Controller', ['$scope', '$rootScope', 'userProfile_service', 'localStorageService', 'sharedService', 'user_Account_Service', '$filter', profilefn])
        .controller('user_profileManage_Password_Controller', ['$scope', '$rootScope', 'userProfile_service', managePasswordfn])
        .controller('user_profile_change_Mobile_Email_Controller', ['$scope', '$rootScope', 'userProfile_service', change_emailMobile_fn])
        .controller('user_quiz_Controller', ['$scope', '$rootScope', 'userProfile_service', '$state', user_quiz_fn])
        .controller('user_quiz_history_Controller', ['$scope', '$rootScope', 'userProfile_service', user_quiz_history_fn])

    ;

})();
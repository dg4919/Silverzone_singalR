/// <reference path="../angular-1.5.8.js" />

// User Account related functionality
(function (app) {

    var user_loginControllerfn = function ($sc, $rsc, $modal, $modalInstance, acount_svc) {

        //  ********** code for Modal POPUP   ********** 
        $sc.Show_signUpModal = function () {

            var modalInstance = $modal.open({
                templateUrl: 'templates/modal/RegistrationModal.html',
                controller: 'user_signUpController',            // No need to specify this controller on HTML page explicitly    
                size: 'md',
                resolve: {
                    //row_index: function () {
                    //    return RowIndex;               // send value from here to controller as dependency using row_index as fx
                    //}
                }
            });
            modalInstance.result.then(function () {
                //on ok button press 
            }, function () {
                //on cancel button press
                console.log("Modal Closed");
            });

            // close current opened popup > that is login modal
            $modalInstance.dismiss();

        }

        $sc.cancel = function () {
            $modalInstance.dismiss();
            console.log('cancel popup');
        }

        //  ********** code for Modal POPUP end  ********** 

        $sc.userInfo = {};

        $sc.user_SignIn = function (form) {
            //alert('register service call');

            if (form.validate()) {
                loginUser();
            }
        }

        // validating rules for registration mmodel
        $sc.validationOptions = {
            rules: {
                userName: {            // use with name attribute in control
                    required: true,
                    validateEmailMobile: true
                },
                password: {            // use with name attribute in control
                    required: true
                }
            }
        }

        function loginUser() {
            // get user type > means whether user enter email/Mobile
            $sc.userInfo.verificationMode = get_userType($sc.userInfo.userName);

            acount_svc.Login($sc.userInfo).then(function (result) {

                if (result.msg === 'ok') {
                    // call global fx define in .run(), bcoz we need notificationService, so inject it in diffrent ctrler
                    // so instead of doing it we inject notificationService at 1 place that is in main app .run() fx
                    $rsc.notify_fx('You have successfully login !', 'success');

                    $modalInstance.close();
                }
                else if (result.msg === 'invalid') {
                    $rsc.notify_fx('username or password is incorrect', 'warning');
                }
                else if (result.msg === 'notfound') {
                    $rsc.notify_fx('You are not registered with us. Please sign up.', 'error');
                }

            }, function () {
                $rsc.user.currentUser = '';
                $rsc.user.is_login = false;
                console.log('in error');
            });
        }

        // **************  For forget password  *****************

        $sc.is_forgetLogin = false;
        $sc.is_enable = false;

        $sc.forgetPassword = function (form) {
            //alert('i call');

            $('.cls_Password').removeClass('has-error');

            var is_validate = form.validate({ ignore: ".cls_Password" });

            if (is_validate) {
                $sc.userInfo.verificationMode = get_userType($sc.userInfo.userName)

                forget_password();
            }
        }

        $sc.forgetLogin_resend = function () {
            forget_password();
        }

        $sc.validate_OTP = function (evnt) {
            var _model = {
                userName: $sc.userInfo.userName,
                OTP_code: $sc.userInfo.sms_OTP,
                verificationMode: $sc.userInfo.verificationMode,
            }
            acount_svc.verify_forgetLogin_OTP(_model).then(function (d) {

                if (d.result === 'ok') {
                    $sc.is_enable = true;
                    $rsc.notify_fx('Verification code matched, enter your password.', 'success');
                }
                else if (d.result === 'expire') {
                    $sc.is_enable = false;
                    $sc.userInfo.sms_OTP = '';
                    $rsc.notify_fx('Verification code expired, click resend link', 'info');
                }
                else if (d.result === 'invalid') {
                    $sc.is_enable = false;
                    $sc.userInfo.sms_OTP = '';
                    $rsc.notify_fx('Invalid verification code.', 'error');
                }

            }, function () {
                console.log('in error');
            });
        }

        $sc.savePassword = function () {
            
            acount_svc.save_newPassword($sc.userInfo).then(function (d) {

                if (d.result === 'success') {
                    loginUser();

                    //$rsc.notify_fx('Password is updated, login again with your new password !.', 'success');
                }
                else if (d.result === 'notfound') {
                    $rsc.notify_fx('User is not found.', 'error');
                }

            }, function () {
                console.log('in error');
            });
        }

        // if i use ng-click to change value > it wrk as 1 way binding
        $sc.change_sts = function () {
            $sc.is_forgetLogin = !($sc.is_forgetLogin);
            $sc.userInfo.Password = '';
            $sc.userInfo.sms_OTP = '';
        }


        function get_userType(inputtxt) {
            var mailFormat = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
            var mobileFormat = /^\d{10}$/;

            if (mailFormat.test(inputtxt))
                return 0;             //  return 'email'; > we r using before
            else if (mobileFormat.test(inputtxt) && inputtxt.length === 10)
                return 1;     //  return 'mobile';
        }

        function forget_password() {

            acount_svc.forgetPassword($sc.userInfo).then(function (d) {

                if (d.result === 'ok') {
                    $sc.is_forgetLogin = true;
                    $sc.is_enable = false;

                    $sc.userInfo.Password = '';
                    $sc.userInfo.sms_OTP = '';

                    if ($sc.userInfo.verificationMode === 0)
                        $rsc.notify_fx('Verification code is sent to your email id', 'success');
                    else if ($sc.userInfo.verificationMode === 1)
                        $rsc.notify_fx('Verification code is sent to your mobile', 'success');
                }
                else if (d.result === 'Block') {
                    $rsc.notify_fx('Maximum attempts reached, Retry tommorrow.', 'warning');
                }
                else if (d.result === "notfound") {
                    $rsc.notify_fx('You are not registered with us. Please sign up.', 'error');
                }

            }, function () {
                console.log('in error');
            });
        }

    }

    var user_signUpControllerfn = function ($sc, $rsc, acount_svc, $modalInstance) {

        // ******** Related to POP up  *********

        $sc.Open_signInModal = function () {

            // close current opened popup > that is Register modal
            // use to close any other opened Modal POP UP
            $modalInstance.dismiss();

            $rsc.Show_signInModal();
        }

        $sc.cancel = function () {
            $modalInstance.dismiss();
            console.log('cancel popup');
        }

        // ******** Related to POP up End  *********

        $sc.userInfo = {};

        // to show diffrent view on register page/Modal POPUP
        $sc.is_sendOTP = false;

        // disable button on register while processing and show loader
        $sc.is_loading = false;

        // use to disable & enable textBox for password in registration form
        $sc.is_enable = false;

        $sc.sendMessage = function (form) {     // pass form name to validate field associate with it, thats it 

            // reset values
            $sc.userInfo.sms_OTP = '';
            $sc.is_enable = false;

            if (form.validate()) {
                $sc.is_loading = true;

                acount_svc.sendSms_onPhone($sc.userInfo.mobileNumber).then(function (d) {
                    $sc.is_loading = false;

                    // sms is 
                    if (d.result === 'ok') {
                        $sc.is_sendOTP = true;
                        $rsc.notify_fx('Verification code is sent to ' + $sc.userInfo.mobileNumber, 'success');
                    }
                    else if (d.result === 'Block') {
                        $rsc.notify_fx('Maximum attempts reached, Retry tommorrow.', 'warning');
                    }
                    else if (d.result === 'exist') {
                        $rsc.notify_fx('You are already registered. Please log in.', 'error');
                    }

                }, function () {
                    console.log('in error');
                });
            }
        }

        $sc.register_User = function () {
            //alert('register service call');
            $sc.userInfo.RoleId = 1;

            acount_svc.Register($sc.userInfo).then(function (d) {
                if (d.result === 'ok') {
                    $modalInstance.dismiss();
                    $rsc.notify_fx('You have successfully registered with us !', 'success');
                }
                else if (d.result === 'invalid_Role') {
                    $rsc.notify_fx('User role is not valid, Try again :(', 'error');
                }
            }, function () {
                console.log('in error');
            });
        }

        $sc.validate_OTP = function (event) {

            //var keycode = event.which;
            //if ((keycode >= 48 && keycode <= 57) || (keycode >= 96 && keycode <= 105)) {

            acount_svc.verify_OTP($sc.userInfo).then(function (d) {

                if (d.result === 'ok') {
                    $sc.is_enable = true;
                    $rsc.notify_fx('Verification code matched, enter your password.', 'success');
                }
                else if (d.result === 'expire') {
                    $sc.is_enable = false;
                    $sc.userInfo.sms_OTP = '';
                    $rsc.notify_fx('Verification code expired, click resend link', 'info');
                }
                else if (d.result === 'invalid') {
                    $sc.is_enable = false;
                    $sc.userInfo.sms_OTP = '';
                    $rsc.notify_fx('Invalid verification code.', 'error');
                }

            }, function () {
                console.log('in error');
            });
        }


        // if i use ng-click to change value > it wrk as 1 way binding
        $sc.change_sts = function () {
            $sc.is_sendOTP = !($sc.is_sendOTP);
        }

        // validating rules for registration model
        $sc.validationOptions = {
            rules: {
                mobileNo: {            // use with name attribute in control
                    required: true,
                    minlength: 10
                },
            },
            messages: {
                mobileNo: {            // use with name attribute in control
                    required: "Mobile number is required !",
                    minlength: "Please enter a valid Mobile number"
                },
            }
        }

    }

    // In new ui-bootstrap-tpls-2.3.0  =>  ($modal changed to > $uibModal)  &&  ('$ModalInstance' changed to > $uibModalInstance)
    app.controller('user_loginController', ['$scope', '$rootScope', '$uibModal', '$uibModalInstance', 'user_Account_Service', user_loginControllerfn])
       .controller('user_signUpController', ['$scope', '$rootScope', 'user_Account_Service', '$uibModalInstance', user_signUpControllerfn]);


})(angular.module('Silverzone_app'));
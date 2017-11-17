(function () {

    var main_ctrlfn = function ($sc, $rsc) {

    }

    //var user_loginfn = function ($sc, $rsc, acount_svc, $state) {
    //    $sc.user = {};

    //    $sc.validationOptions = {
    //        rules: {
    //            user_name: {            // use with name attribute in control
    //                required: true,
    //                validateEmailMobile: true
    //            },
    //            user_password: {
    //                required: true
    //            }
    //        }
    //    }

    //    $sc.submit_data = function (form) {

    //        if (form.validate()) {
    //            $sc.user.verificationMode = get_userType($sc.user.userName);

    //            acount_svc.Login($sc.user).then(function (result) {

    //                if (result.msg === 'ok') {
    //                    if ($rsc.validateUser(result.role)) {
    //                        $rsc.notify_fx('You have successfully login !', 'success');
    //                        $state.go('book_category_list');
    //                    }
    //                    else {
    //                        acount_svc.Logout();
    //                        $rsc.notify_fx('User role is not authorize !', 'warning');
    //                    }
    //                }
    //                else if (result.msg === 'invalid') {
    //                    $rsc.notify_fx('username or password is incorrect', 'warning');
    //                }
    //                else if (result.msg === 'error') {
    //                    $rsc.notify_fx('Something went wrong :(, Try again !', 'error');
    //                }
    //                else if (result.msg === 'notfound') {
    //                    $rsc.notify_fx('You are not registered with us. Please sign up.', 'error');
    //                }

    //            }, function () {
    //                $rsc.user.currentUser = '';
    //                $rsc.user.is_login = false;
    //                console.log('in error');
    //            });
    //        }
    //    }

    //    function get_userType(inputtxt) {
    //        var mailFormat = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
    //        var mobileFormat = /^\d{10}$/;

    //        if (mailFormat.test(inputtxt))
    //            return 0;             //  return 'email'; > we r using before
    //        else if (mobileFormat.test(inputtxt) && inputtxt.length === 10)
    //            return 1;     //  return 'mobile';
    //    }

    //}

    angular.module('Silverzone_admin_app')
       //.controller('user_login', ['$scope', '$rootScope', 'user_Account_Service', '$state', user_loginfn])

       .controller('masterCtrl', ['$scope', main_ctrlfn])

    ;

})();

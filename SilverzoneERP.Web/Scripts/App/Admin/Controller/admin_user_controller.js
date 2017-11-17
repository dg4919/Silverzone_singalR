(function () {

    // use to create Sub user by Admin to provide role
    var user_createfn = function ($sc, $rsc, svc) {
        $sc.user = {
            userName: []
        };

        $sc.roleList = [
           //{ Id: 2, Name: 'Admin' },
           { Id: 3, Name: 'Dispatch' },
           { Id: 4, Name: 'Support' }
        ];

        $sc.validationOptions = {
            rules: {
                user_password: {
                    required: true
                },
                user_role: {
                    required: true
                }
            },
            messages: {
                user_password: {            // use with name attribute in control
                    required: "User Password is required !",
                },
                user_role: {            // use with name attribute in control
                    required: "Select User Role !",
                }
            }
        }

        $sc.submit_data = function (form) {

            if (form.validate()) {

                if ($sc.user.userName.length === 0) {
                    $rsc.notify_fx('Enter atleast one user name to continue !', 'error');
                    return;
                }

                svc.create_user($sc.user).then(function (data) {
                    if (data.result === 'ok') {
                        $rsc.notify_fx('User is created successfully !', 'success');
                    }
                    else if (data.result === 'invalid_Role') {
                        $rsc.notify_fx('Role is not currently activated, Try Again :(', 'warning');
                    }
                    else if (data.result === 'exist') {
                        $rsc.notify_fx(data.msg, 'warning');
                    }
                });
            }
        }

    }

    angular.module('Silverzone_admin_app')
      .controller('user_create', ['$scope', '$rootScope', 'admin_user_Service', user_createfn])

    ;

})();